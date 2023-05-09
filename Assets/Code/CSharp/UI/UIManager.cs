using Code.Mgr;
using Code.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>, IRunningMgr, IUpdate, ILateUpdate
{
	private List<UIBasePanel> panelLst = new List<UIBasePanel>();
	private Dictionary<Type, UIBasePanel> type2PanelDic = new Dictionary<Type, UIBasePanel>();
	private Dictionary<EPanelId, UIBasePanel> id2PanelDic = new Dictionary<EPanelId, UIBasePanel>(new EPanelIdCompare());
	private Queue<IOperateNode> operateNodes = new Queue<IOperateNode>();
	private List<UIBasePanel> showLst = new List<UIBasePanel>();
	private List<UIBasePanel> closeLst = new List<UIBasePanel>();

	public GameObject UIRoot { get; private set; }
	public GameObject ScreenRoot { get; private set; }
	private UIManager() { }
	public void Init()
	{
		var env = typeof(UIBasePanel).Assembly;
		var types = env.GetTypes();
		foreach (var type in types)
		{
			var attrType = typeof(UIIDAttribute);
			if (Attribute.IsDefined(type, attrType))
			{
				var attr = Attribute.GetCustomAttribute(type, attrType) as UIIDAttribute;
				var panel = Activator.CreateInstance(type) as UIBasePanel;
				panel.UIConf = CSVUIPanel.Get((int)attr.PanelId);
				type2PanelDic[type] = panel;
				id2PanelDic[attr.PanelId] = panel;
			}
		}

		InitUIRoot();
	}
	public void Update()
	{
		while (operateNodes.Count > 0)
		{
			var node = operateNodes.Dequeue();
			var panel = id2PanelDic[node.PanelId];
			switch (node.OperateType)
			{
				case EOperateType.Show:
					var param = (node as PanelOperatrShowNode).Param;
					ShowPanel(panel, param);
					break;
				case EOperateType.Close:
					ClosePanel(panel);
					break;
			}
			ObjectPool.Release(node);
		}
		for (int i = showLst.Count - 1; i >= 0; i--)
		{
			showLst[i].Update();
		}
	}
	public void LateUpdate()
	{
		for (int i = showLst.Count - 1; i >= 0; i--)
		{
			showLst[i].LateUpdate();
		}
	}
	public void Destroy()
	{

	}
	private void InitUIRoot()
	{
		UIRoot = GameObject.Find("UIRoot");
		ScreenRoot = UIRoot;
		Utility.Go.DontDestroy(UIRoot);
	}
	public void Show<T>(UIParam param = null) where T : UIBasePanel
	{
		var type = typeof(T);
		var panel = GetPanel(type);
		ShowNode(panel, param);
	}
	public void Close<T>() where T : UIBasePanel
	{
		var type = typeof(T);
		var panel = GetPanel(type);
		CloseNode(panel);
	}
	public void Close(UIBasePanel panel)
	{
		CloseNode(panel);
	}
	private void ShowNode(UIBasePanel panel, UIParam param = null)
	{
		if (panel.State != EPanelState.Show)
		{
			var node = ObjectPool.Get<PanelOperatrShowNode>();
			node.OperateType = EOperateType.Show;
			node.PanelId = panel.UIConf.PanelId;
			node.Param = param;
			operateNodes.Enqueue(node);
		}
	}
	private void CloseNode(UIBasePanel panel)
	{
		if (panel.State == EPanelState.Show)
		{
			var node = ObjectPool.Get<PanelOperatrCloseNode>();
			node.OperateType = EOperateType.Close;
			node.PanelId = panel.UIConf.PanelId;
			operateNodes.Enqueue(node);
		}
	}
	private void ShowPanel(UIBasePanel panel, UIParam param)
	{
		panel.UIParam = param;
		panel.Show();
		showLst.Add(panel);
		closeLst.Remove(panel);
	}
	private void ClosePanel(UIBasePanel panel)
	{
		panel.Close();
		showLst.Remove(panel);
		closeLst.Add(panel);
	}
	private UIBasePanel GetPanel(Type type)
	{
		if (!type2PanelDic.TryGetValue(type, out UIBasePanel panel))
		{
			panel = Activator.CreateInstance(type) as UIBasePanel;
			type2PanelDic[type] = panel;
			id2PanelDic[panel.UIConf.PanelId] = panel;
			panelLst.Add(panel);
		}
		return panel;
	}
}