using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static partial class EUtility
{
	public static class GUI
	{
		public static Texture2D WhiteTex2D;
		public static Texture2D YellowTex2D;
		public static Texture2D BlueTex2D;
		public static Texture2D BlackTex2D;
		public static Texture2D AlphaTex2D;

		public static GUIStyle NormalLabel => UnityEngine.GUI.skin.label;
		public static GUIStyle SelectLabel;

		public static GUISkin CusSkin;

		public static GUIContent PrefabIcon = new GUIContent(EditorGUIUtility.LoadRequired("Prefab Icon") as Texture);
		public static GUIContent FoldInIcon = new GUIContent(EditorGUIUtility.LoadRequired("IN_foldout_on") as Texture);
		public static GUIContent FoldOutIcon = new GUIContent(EditorGUIUtility.LoadRequired("IN foldout focus") as Texture);

		private static GUILayoutOption[] FoldOut;

		public static GUILayoutOption[] ExpandWidthFalse;

		//也可以用数组，但是可能会浪费空间，可视情况而定
		private static Dictionary<int, GUILayoutOption> widthDic = new Dictionary<int, GUILayoutOption>();
		private static Dictionary<int, GUILayoutOption> heightDic = new Dictionary<int, GUILayoutOption>();
		private static Dictionary<int, Dictionary<int, GUILayoutOption[]>> optionsDic = new Dictionary<int, Dictionary<int, GUILayoutOption[]>>();

		static GUI()
		{
			WhiteTex2D = Tex2D(Color.white);
			YellowTex2D = Tex2D(Color.yellow);
			BlueTex2D = Tex2D(Color.blue);
			BlackTex2D = Tex2D(Color.black);
			var white = Color.white;
			white.a = 0;
			AlphaTex2D = Tex2D(white);

			CusSkin = AssetDatabase.LoadAssetAtPath<GUISkin>("Assets/Editor/CusSkin.guiskin");
			CusSkin.label.normal.background = BlueTex2D;

			SelectLabel = CusSkin.label;

			FoldOut = new GUILayoutOption[2] { Width(17), Height(17) };
			ExpandWidthFalse = new GUILayoutOption[1] { GUILayout.ExpandWidth(false) };
		}
		public static Texture2D Tex2D(Color color)
		{
			var tex = new Texture2D(1, 1);
			for (int w = 0; w < 1; w++)
			{
				for (int h = 0; h < 1; h++)
				{
					tex.SetPixel(w, h, color);
				}
			}
			tex.Apply();
			return tex;
		}
		public static GUILayoutOption Width(int width)
		{
			if (!widthDic.TryGetValue(width, out GUILayoutOption option))
			{
				option = GUILayout.Width(width);
				widthDic[width] = option;
			}
			return option;
		}
		public static GUILayoutOption Height(int height)
		{
			if (!heightDic.TryGetValue(height, out GUILayoutOption option))
			{
				if (height == 0)
				{
					option = GUILayout.MinHeight(15);
				}
				else
				{
					option = GUILayout.Height(height);
				}
				heightDic[height] = option;
			}
			return option;
		}
		public static GUILayoutOption[] WHOptions(int width, int height = 0)
		{
			if (!optionsDic.TryGetValue(width, out Dictionary<int, GUILayoutOption[]> dic))
			{
				dic = new Dictionary<int, GUILayoutOption[]>();
				optionsDic[width] = dic;
			}
			if (!dic.TryGetValue(height, out GUILayoutOption[] options))
			{
				options = new GUILayoutOption[2];
				options[0] = Width(width);
				options[1] = Height(height);
				dic[height] = options;
			}
			return options;
		}
		public static bool FoldOutBtn(bool value)
		{
			var content = value ? FoldInIcon : FoldOutIcon;
			if (GUILayout.Button(content, NormalLabel, FoldOut))
			{
				value = !value;
			}
			return value;
		}
		public static string TextField(string name, string field, int width_name = 0, int width_field = 0)
		{
			GUILayout.Label(name, WHOptions(width_name, 0));
			field = GUILayout.TextField(field, WHOptions(width_field, 0));
			return field;
		}
		public static void DrawRectBorder(Rect rect, Color color)
		{
			EditorGUI.DrawRect(new Rect(rect.x, rect.y, 1, rect.height), color);
			EditorGUI.DrawRect(new Rect(rect.x + rect.width, rect.y, 1, rect.height), color);
			EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, 1), color);
			EditorGUI.DrawRect(new Rect(rect.x, rect.y + rect.height, rect.width, 1), color);
		}
		public static void DrawBorderRect(Rect rect, Color col, Color border_col)
		{
			EditorGUI.DrawRect(rect, col);
			DrawRectBorder(rect, border_col);
		}
		public static Rect HalfRect(Rect rect)
		{
			rect.x = rect.x + rect.width / 4;
			rect.y = rect.y + rect.height / 4;
			rect.width /= 2;
			rect.height /= 2;
			return rect;
		}
		public static void EditorGUILable(Rect rect, string text, int size, Color color, bool is_center = false)
		{
			var lable = EditorStyles.label;
			TextAnchor anchor = lable.alignment;
			if (is_center)
			{
				lable.alignment = TextAnchor.MiddleCenter;
			}
			var styleState = lable.normal;
			var col = styleState.textColor;
			styleState.textColor = color;
			var fontSize = lable.fontSize;
			lable.fontSize = size;
			EditorGUI.LabelField(rect, text);
			lable.fontSize = fontSize;
			styleState.textColor = col;
			if (is_center)
			{
				lable.alignment = anchor;
			}
		}
	}
}