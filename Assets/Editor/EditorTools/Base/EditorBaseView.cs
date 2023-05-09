using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public abstract class EditorBaseView
{
	private List<EditorBaseView> viewLst = new List<EditorBaseView>();
	private List<EditorBaseView> removeLst = new List<EditorBaseView>();

	public EditorBaseWindow Root;
	public bool IsBaseDraw = true;
	public void Show()
	{
		Root.OnSelectChanged += OnSelectionChanged;
		OnShow();
	}
	public void Draw()
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
	}
	public void Close()
	{
		Root.OnSelectChanged -= OnSelectionChanged;
		OnClose();
	}
	protected virtual void OnShow() { }
	protected virtual void OnUpdate() { }
	protected virtual void OnClose() { }
	protected virtual void OnDraw() { }
	protected virtual void OnBeginDraw() { }
	protected virtual void OnEndDraw() { }
	protected virtual void OnSelectionChanged(Object go) { }

	public T AddView<T>(bool is_base_draw = true) where T : EditorBaseView, new()
	{
		var view = new T();
		viewLst.Add(view);
		view.Root = Root;
		view.IsBaseDraw = is_base_draw;
		view.Show();
		Root.Changed = true;
		return view;
	}
	public void RemoveView(EditorBaseView view)
	{
		removeLst.Add(view);
		Root.Changed = true;
	}
}