using Code.Mgr;
using Code.Msg;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.UI
{
	public enum EPanelLayer
	{
		Bottom,
		Middle,
		Top,
	}
	public enum EPanelState
	{
		Invaild,
		Loading,
		Show,
		Close,
		Destroy
	}
	public abstract class UIBasePanel
	{
		private List<UIComponent> uiComponentLst = new List<UIComponent>();
		private List<UIComponent> initInActiveComponentLst = new List<UIComponent>();

		protected GameObject uiObj;
		protected bool isActive;
		protected Dictionary<string, Delegate> msgDic = new Dictionary<string, Delegate>();

		public UIParam UIParam { get; set; }
		public CSVUIPanel UIConf { get; set; }
		public EPanelState State { get; private set; } = EPanelState.Invaild;
		public GameObject UIObj => uiObj;

		public void Load()
		{
			State = EPanelState.Loading;
			var uiRoot = UIManager.Instance.ScreenRoot;
			var layer = uiRoot.transform.GetChild(UIConf.iLayer);
			uiObj = GameResLoader.Instance.GetInstance(UIConf.sPath, layer);
			Utility.Trans.SetLocalDefaultPSQ(uiObj.transform);

			var rectTrans = uiObj.transform as RectTransform;
			rectTrans.offsetMin = Vector2.zero;
			rectTrans.offsetMax = Vector2.zero;
		}
		public void Init()
		{
			State = EPanelState.Close;
			Utility.UI.InitUIWidgets(this, uiObj);
			OnInit();
			isActive = false;
		}
		public void Show()
		{
			if (State == EPanelState.Invaild)
			{
				Load();
				Init();
			}
			uiObj.transform.SetAsLastSibling();
			for (int i = 0; i < uiComponentLst.Count; i++)
			{
				var comp = uiComponentLst[i];
				if (!initInActiveComponentLst.Contains(comp))
				{
					uiComponentLst[i].Show();
				}
			}
			SetActive(true);
		}
		public void Update()
		{
			OnUpdate();
			for (int i = 0; i < uiComponentLst.Count; i++)
			{
				if (uiComponentLst[i].IsActive)
				{
					uiComponentLst[i].Update();
				}
			}
		}
		public void LateUpdate()
		{
			OnLateUpdate();
			for (int i = 0; i < uiComponentLst.Count; i++)
			{
				if (uiComponentLst[i].IsActive)
				{
					uiComponentLst[i].LateUpdate();
				}
			}
		}
		public void Close()
		{
			for (int i = 0; i < uiComponentLst.Count; i++)
			{
				uiComponentLst[i].Close();
			}
			SetActive(false);
		}
		public void Destroy()
		{
			GameResLoader.Instance.Recycle(uiObj);
			State = EPanelState.Destroy;
			OnDestroy();
			for (int i = 0; i < uiComponentLst.Count; i++)
			{
				uiComponentLst[i].Destroy();
			}
		}
		private void AddListeners()
		{
			OnInitListeners();
			Messenger.AddListeners(msgDic);
		}
		private void RemoveListeners()
		{
			Messenger.RemoveListeners(msgDic);
		}
		protected virtual void OnInit() { }
		protected virtual void OnInitListeners() { }
		protected virtual void OnShow() { }
		protected virtual void OnUpdate() { }
		protected virtual void OnLateUpdate() { }
		protected virtual void OnClose() { }
		protected virtual void OnDestroy() { }

		public void SetActive(bool active)
		{
			if (active != isActive)
			{
				isActive = active;
				Utility.Go.SetActive(uiObj, isActive);
				if (isActive)
				{
					AddListeners();
					State = EPanelState.Show;
					OnShow();
				}
				else
				{
					RemoveListeners();
					State = EPanelState.Close;
					OnClose();
				}
			}
		}

		protected T CreateUI<T>(GameObject go, bool init_active = true) where T : UIComponent, new()
		{
			var ui = new T();
			ui.Init(go, this, init_active);
			uiComponentLst.Add(ui);
			if (!init_active)
			{
				initInActiveComponentLst.Add(ui);
			}
			return ui;
		}
		protected T GetParam<T>() where T : UIParam
		{
			return UIParam as T;
		}
	}
}