using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

//基本生成文件的功能都实现了，但是如果涉及到修改预制体名字或者结构的操作，则实体绑定则会失效，后续可以在实际情况中再改进
public class UIFileGeneratedWindow : EditorBaseWindow
{
	[MenuItem("Tools/UIFileGenerated")]
	static void OpenWindow()
	{
		ShowView();
	}
	private static void ShowView()
	{
		Type prefabPreType = typeof(PrefabPreviewWindow);
		Type fileInfoType = typeof(FileInfoWindow);
		//创建最外层容器
		object containerInstance = EditorContainerWindow.CreateInstance();
		//创建分屏容器
		object splitViewInstance = EditorSplitView.CreateInstance();
		//设置根容器
		EditorContainerWindow.SetRootView(containerInstance, splitViewInstance);
		//添加menu容器和工具容器
		object prefabDockAreaInstance = EditorDockArea.CreateInstance();

		EditorDockArea.SetPosition(prefabDockAreaInstance, new Rect(0, 0, 200, 800));
		EditorWindow prefabPreWindow = (EditorWindow)ScriptableObject.CreateInstance(prefabPreType);
		EditorDockArea.AddTab(prefabDockAreaInstance, prefabPreWindow);
		EditorSplitView.AddChild(splitViewInstance, prefabDockAreaInstance);

		object fileDockAreaInstance = EditorDockArea.CreateInstance();

		EditorDockArea.SetPosition(fileDockAreaInstance, new Rect(200, 0, 600, 800));
		EditorWindow fileInfoWindow = (EditorWindow)ScriptableObject.CreateInstance(fileInfoType);
		EditorDockArea.AddTab(fileDockAreaInstance, fileInfoWindow);
		EditorSplitView.AddChild(splitViewInstance, fileDockAreaInstance);

		EditorEditorWindow.MakeParentsSettingsMatchMe(prefabPreWindow);
		EditorEditorWindow.MakeParentsSettingsMatchMe(fileInfoWindow);

		EditorContainerWindow.SetPosition(containerInstance, new Rect(100, 100, 1200, 1200));
		EditorSplitView.SetPosition(splitViewInstance, new Rect(0, 0, 800, 800));
		EditorContainerWindow.Show(containerInstance, 0, true, false, true);
		EditorContainerWindow.OnResize(containerInstance);

		//var arr = Resources.FindObjectsOfTypeAll<Texture>();
		//for (int i = 0; i < arr.Length; i++)
		//{
		//	var tex = arr[i];
		//	if (tex.name.Contains("int") || tex.name.Contains("out"))
		//	{
		//		//Debug.LogError(tex.name);
		//	}
		//}
	}
}