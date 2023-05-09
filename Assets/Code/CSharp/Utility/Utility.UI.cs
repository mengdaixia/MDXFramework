using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class UIIDAttribute : Attribute
{
	public EPanelId PanelId { get; }
	public UIIDAttribute(EPanelId id)
	{
		PanelId = id;
	}
}
public class UIWidgetAttribute : Attribute
{
	public string Path { get; }
	public UIWidgetAttribute(string path)
	{
		Path = path;
	}
}
public static partial class Utility
{
	public static class UI
	{
		private static string[] timeStrArr = new string[11];
		private static Dictionary<Type, FieldInfo[]> type2FieldsDic = new Dictionary<Type, FieldInfo[]>();
		public static void OnUIInit()
		{
			type2FieldsDic.Clear();
		}
		public static void InitUIWidgets<T>(T ui_component, GameObject go_ui) where T : class
		{
			var type = ui_component.GetType();
			if (!type2FieldsDic.TryGetValue(type, out FieldInfo[] fields))
			{
				fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
				type2FieldsDic[type] = fields;
			}
			for (int i = 0; i < fields.Length; i++)
			{
				var field = fields[i];
				var tWidget = typeof(UIWidgetAttribute);
				if (field.IsDefined(tWidget))
				{
					var widget = field.GetCustomAttribute<UIWidgetAttribute>();
					if (widget != null)
					{
						var path = widget.Path;
						var trans = go_ui.transform.Find(path);
						if (trans == null)
						{
							Debug.LogError("没有找到物体" + go_ui.name + "---" + path);
							continue;
						}
						var targetGo = trans.gameObject;
						var fType = field.FieldType;
						if (fType == typeof(GameObject))
						{
							Unsafe.SetValue(ui_component, field, targetGo);
							//field.SetValue(ui_component, targetGo);
						}
						else
						{
							var component = targetGo.GetComponent(fType);
							Unsafe.SetValue(ui_component, field, component);
							//field.SetValue(ui_component, component);
						}
					}
				}
			}
		}
	}
}