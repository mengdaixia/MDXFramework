using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PrefabTreeItem : ITreeViewItem
{
	private bool isSelect = false;
	private List<ITreeViewItem> childItemLst = new List<ITreeViewItem>();
	public List<ITreeViewItem> ChildItemLst => childItemLst;
	public string Name { get; set; }
	public GameObject Go { get; set; }
	public Action<PrefabTreeItem> OnExDraw;

	public virtual void Draw()
	{
		GUILayout.BeginHorizontal();
		var styleState = GUI.skin.label.normal;
		styleState.background = isSelect ? EUtility.GUI.BlueTex2D : null;
		styleState.textColor = isSelect ? Color.white : Color.black; ;
		GUILayout.Label(Name);
		styleState.textColor = Color.black;
		styleState.background = null;
		OnExDraw?.Invoke(this);
		GUILayout.EndHorizontal();
	}
	public void SetSelect(bool is_select)
	{
		if (isSelect != is_select)
		{
			isSelect = is_select;
		}
	}
}