using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ComponentView : EditorBaseView
{
	private List<Component> compLst = new List<Component>();

	public Action<Component> OnCompBind;
	public void Set(GameObject go)
	{
		compLst.Clear();
		go.GetComponents(compLst);
	}
	protected override void OnDraw()
	{
		GUILayout.BeginVertical();
		for (int i = 0; i < compLst.Count; i++)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label(compLst[i].GetType().Name);
			if (GUILayout.Button("添加绑定", EUtility.GUI.ExpandWidthFalse))
			{
				OnCompBind?.Invoke(compLst[i]);
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndVertical();
	}
}