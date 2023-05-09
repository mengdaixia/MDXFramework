using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class EditorBaseWindow : EditorWindow
{
	private List<EditorBaseView> viewLst = new List<EditorBaseView>();
	private List<EditorBaseView> removeLst = new List<EditorBaseView>();
	private bool isfocus = true;

	public bool Changed = false;
	public Action<Object> OnSelectChanged;
	private void Awake()
	{
		OnShow();
	}
	private void OnFocus()
	{
		isfocus = true;
	}
	private void Update()
	{
		OnUpdate();
	}
	private void OnGUI()
	{
		OnBeginDraw();
		OnDraw();
		for (int i = 0; i < removeLst.Count; i++)
		{
			var view = removeLst[i];
			view.Close();
			viewLst.Remove(view);
		}
		for (int i = 0; i < viewLst.Count; i++)
		{
			var view = viewLst[i];
			if (view.IsBaseDraw)
			{
				view.Draw();
			}
		}
		OnEndDraw();
		CheckRepaint();
	}
	private void OnLostFocus()
	{
		isfocus = false;
	}
	private void OnDestroy()
	{
		OnClose();
	}
	private void OnSelectionChange()
	{
		OnSelectionChaned(Selection.activeObject);
		OnSelectChanged?.Invoke(Selection.activeObject);
		CheckRepaint();
	}
	protected virtual void OnShow() { }
	protected virtual void OnUpdate() { }
	protected virtual void OnClose() { }
	protected virtual void OnDraw() { }
	protected virtual void OnBeginDraw() { }
	protected virtual void OnEndDraw() { }
	protected virtual void OnSelectionChaned(Object obj) { }

	protected T AddView<T>(bool is_base_draw = true) where T : EditorBaseView, new()
	{
		var view = new T();
		viewLst.Add(view);
		view.Root = this;
		view.IsBaseDraw = is_base_draw;
		view.Show();
		Changed = true;
		return view;
	}
	protected void RemoveView(EditorBaseView view)
	{
		removeLst.Add(view);
		Changed = true;
	}
	public void ForceRepaint()
	{
		Repaint();
	}
	private void CheckRepaint()
	{
		if (Changed)
		{
			Changed = false;
			Repaint();
		}
	}
}