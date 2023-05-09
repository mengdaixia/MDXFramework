using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class EditorTreeView : EditorBaseView
{
	private Vector2 scrollPos = Vector2.zero;
	private GUIContent InfoContent;
	private ITreeViewItem rootItem;
	private Dictionary<ITreeViewItem, bool> foldOutDic = new Dictionary<ITreeViewItem, bool>();

	protected override void OnDraw()
	{
		if (rootItem != null)
		{
			scrollPos = GUILayout.BeginScrollView(scrollPos);
			GUILayout.BeginVertical();
			DrawItem(rootItem, 0);
			GUILayout.EndVertical();
			GUILayout.EndScrollView();
		}
	}
	public void SetItems(ITreeViewItem item, GUIContent content = null)
	{
		if (rootItem != item)
		{
			rootItem = item;
			InfoContent = content;
			foldOutDic.Clear();
			Root.Changed = true;
		}
	}
	private void DrawItem(ITreeViewItem item, int deep)
	{
		var childs = item.ChildItemLst;
		GUILayout.BeginHorizontal();
		//需要区分InfoContent的情况、、、
		GUILayout.Space(deep * 20);
		var value = FoldOut(item);
		if (childs.Count > 0)
		{
			value = EUtility.GUI.FoldOutBtn(value);
			foldOutDic[item] = value;
		}
		else
		{
			GUILayout.Space(17);
		}
		if (InfoContent != null)
		{
			GUILayout.Label(InfoContent, EUtility.GUI.WHOptions(17, 17));
		}
		item.Draw();
		GUILayout.EndHorizontal();
		if (value)
		{
			for (int i = 0; i < childs.Count; i++)
			{
				var child = childs[i];
				DrawItem(child, deep + 1);
			}
		}
	}
	private bool FoldOut(ITreeViewItem item)
	{
		if (!foldOutDic.TryGetValue(item, out bool result))
		{
			result = false;
			foldOutDic[item] = result;
		}
		return result;
	}
}