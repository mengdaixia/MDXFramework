using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class PrefabPreviewWindow : EditorBaseWindow
{
	private EditorTreeView prefabView;
	private PrefabTreeItem currItem;
	protected override void OnShow()
	{
		prefabView = AddView<EditorTreeView>();
		OnSelectionChaned(Selection.activeObject);
	}
	protected override void OnClose()
	{

	}
	private void BuildRoot(GameObject go)
	{
		var root = EUtility.Go.BuildPrefabRoot(go, OnExDrawHandler);
		prefabView.SetItems(root, EUtility.GUI.PrefabIcon);
	}
	protected override void OnSelectionChaned(Object obj)
	{
		if (obj != null && obj is GameObject go)
		{
			currItem = null;
			BuildRoot(go);
		}
	}
	private void OnExDrawHandler(PrefabTreeItem item)
	{
		if (GUILayout.Button("添加UIFile", EUtility.GUI.ExpandWidthFalse))
		{
			EditorEvents.OnUIFileAdd?.Invoke(item);
		}
	}
}