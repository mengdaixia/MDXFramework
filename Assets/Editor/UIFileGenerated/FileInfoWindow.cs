using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class FileInfoWindow : EditorBaseWindow
{
	private Vector2 srollRect = Vector2.zero;
	private List<UIFileView> filesViewLst = new List<UIFileView>();
	protected override void OnShow()
	{
		EditorEvents.OnUIFileAdd += OnUIFileAddHandler;
	}
	protected override void OnBeginDraw()
	{
		var view = new Rect(0, 0, 900, 1100);
		var realView = new Rect(0, 0, 900, filesViewLst.Count * 310 + 30);
		srollRect = GUI.BeginScrollView(view, srollRect, realView);
		GUILayout.BeginVertical();
	}
	protected override void OnEndDraw()
	{
		GUILayout.EndVertical();
		GUI.EndScrollView();
		if (GUI.Button(new Rect(780, 1080, 100, 20), "生成UI文件"))
		{
			for (int i = 0; i < filesViewLst.Count; i++)
			{
				var file = filesViewLst[i];
				file.Generated();
			}
			FileGenerated.OnGenerated();
		}
	}
	protected override void OnClose()
	{
		EditorEvents.OnUIFileAdd -= OnUIFileAddHandler;
	}
	protected override void OnSelectionChaned(UnityEngine.Object obj)
	{
		for (int i = filesViewLst.Count - 1; i >= 0; i--)
		{
			OnUIFileRemoveHandler(filesViewLst[i]);
		}
	}
	private void OnUIFileAddHandler(PrefabTreeItem item)
	{
		var view = AddView<UIFileView>();
		view.Set(item);
		view.SetIndex(filesViewLst.Count);
		view.OnRemoveFile = OnUIFileRemoveHandler;
		filesViewLst.Add(view);
		ForceRepaint();
	}
	private void OnUIFileRemoveHandler(UIFileView file)
	{
		RemoveView(file);
		filesViewLst.Remove(file);
		for (int i = 0; i < filesViewLst.Count; i++)
		{
			filesViewLst[i].SetIndex(i);
		}
		Changed = true;
	}
}