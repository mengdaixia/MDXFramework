using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FileBindingView : EditorBaseView
{
	private List<FileBindItem> itemLst = new List<FileBindItem>();
	protected override void OnBeginDraw()
	{
		GUILayout.BeginVertical();
	}
	protected override void OnEndDraw()
	{
		GUILayout.EndVertical();
	}
	public void Generated(int id)
	{
		for (int i = 0; i < itemLst.Count; i++)
		{
			itemLst[i].Generated(id);
		}
	}
	public void Bind(GameObject root, Component comp)
	{
		var item = AddView<FileBindItem>();
		item.Set(root, comp);
		item.OnDelete = OnDeleteHandler;
		itemLst.Add(item);
	}
	private void OnDeleteHandler(FileBindItem item)
	{
		RemoveView(item);
		itemLst.Remove(item);
	}
}