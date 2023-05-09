using Code.Msg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Code.UI
{
	public abstract class UIComponent
	{
		protected UIBasePanel rootPanel;
		protected GameObject uiObj;
		protected bool isActive;
		protected Dictionary<string, Delegate> msgDic = new Dictionary<string, Delegate>();

		private List<UIComponent> uiComponentLst = new List<UIComponent>();

		public bool IsActive => isActive;
		public GameObject UIObj => uiObj;
		public void Init(GameObject go, UIBasePanel logic, bool init_active = true)
		{
			uiObj = go;
			rootPanel = logic;
			Utility.UI.InitUIWidgets(this, uiObj);
			isActive = init_active;
			OnInit();
			if (init_active)
			{
				AddListeners();
				OnShow();
			}
			Utility.Go.SetActive(uiObj, init_active);
		}
		public void Show()
		{
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
			OnUpdate();
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
			SetActive(false);
		}
		public void Destroy()
		{
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
				Utility.Go.SetActive(uiObj, active);
				if (isActive)
				{
					AddListeners();
					OnShow();
				}
				else
				{
					RemoveListeners();
					OnClose();
				}
			}
		}
		protected T CreateUI<T>(GameObject go, bool init_active = true) where T : UIComponent, new()
		{
			var ui = new T();
			ui.Init(go, rootPanel, init_active);
			uiComponentLst.Add(ui);
			return ui;
		}
	}
}