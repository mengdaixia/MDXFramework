using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum EUIFileType
{
	UIBasePanel,
	UIComponent,
}
public class UIFileView : EditorBaseView
{
	private int id = 0;
	private PrefabTreeItem targetPrefab;
	private PrefabTreeItem currItem;

	private Vector2 scrollPos1 = Vector2.zero;
	private Vector2 scrollPos2 = Vector2.zero;
	private Vector2 scrollPos3 = Vector2.zero;


	private EditorTreeView prefabView;
	private ComponentView compView;
	private FileBindingView bindView;

	private string fileName;
	private EUIFileType fileType;

	public Action<UIFileView> OnRemoveFile;

	public void SetIndex(int index)
	{
		id = index;
	}
	public void Set(PrefabTreeItem item)
	{
		targetPrefab = item;
		prefabView = AddView<EditorTreeView>(false);
		var root = EUtility.Go.BuildPrefabRoot(targetPrefab.Go, OnExDrawHandler);
		prefabView.SetItems(root, EUtility.GUI.PrefabIcon);

		compView = AddView<ComponentView>(false);
		bindView = AddView<FileBindingView>(false);

		compView.OnCompBind += OnCompBindHandler;
	}
	protected override void OnDraw()
	{
		var style = GUI.skin.textArea;
		var rect = new Rect(10, 10, 867, 300);
		rect.y = rect.y + (rect.height * id) + id * 20;
		GUILayout.BeginArea(rect, style);
		GUILayout.BeginVertical();

		GUILayout.BeginHorizontal();
		fileName = EUtility.GUI.TextField("文件名:", fileName, 50, 150);

		//应该会装箱插箱，后面再改
		fileType = (EUIFileType)EditorGUILayout.EnumPopup(fileType, EUtility.GUI.WHOptions(120, 0));
		if (GUILayout.Button("删除", EUtility.GUI.WHOptions(50, 0)))
		{
			OnRemoveFile?.Invoke(this);
		}
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();

		GUILayout.BeginArea(new Rect(10, 30, 220, 260), style);
		scrollPos1 = GUILayout.BeginScrollView(scrollPos1);
		prefabView.Draw();
		GUILayout.EndScrollView();
		GUILayout.EndArea();

		GUILayout.BeginArea(new Rect(220 + 10 + 10, 30, 280, 260), style);
		scrollPos2 = GUILayout.BeginScrollView(scrollPos2);
		compView.Draw();
		GUILayout.EndScrollView();
		GUILayout.EndArea();

		GUILayout.BeginArea(new Rect(220 + 280 + 10 + 20, 30, 325, 260), style);
		scrollPos3 = GUILayout.BeginScrollView(scrollPos3);
		bindView.Draw();
		GUILayout.EndScrollView();
		GUILayout.EndArea();

		GUILayout.EndHorizontal();

		GUILayout.EndVertical();
		GUILayout.EndArea();
	}
	public bool Generated()
	{
		if (string.IsNullOrEmpty(fileName))
		{
			return false;
		}
		FileGenerated.WirteCache(id, EFileWriteType.BaseClass, fileType.ToString());
		FileGenerated.WirteCache(id, EFileWriteType.Class, fileName);
		bindView.Generated(id);
		return true;
	}
	private void OnExDrawHandler(PrefabTreeItem item)
	{
		if (GUILayout.Button("检索组件", EUtility.GUI.ExpandWidthFalse))
		{
			currItem?.SetSelect(false);
			currItem = item;
			currItem?.SetSelect(true);
			compView.Set(item.Go);
			Root.Changed = true;
		}
	}
	private void OnCompBindHandler(Component comp)
	{
		bindView.Bind(targetPrefab.Go, comp);
	}
}