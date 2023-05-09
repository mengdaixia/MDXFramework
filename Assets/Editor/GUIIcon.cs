using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public sealed class GUIIcon : EditorWindow
{
	[MenuItem("Tools/Open Built In Icons")]
	private static void OpenGUIIcon()
	{
		GetWindow<GUIIcon>().Show();
	}
	private List<Texture> icons = new List<Texture>();
	private Vector2 scrollPosition;
	private string[] names;
	private string searchContent = "";
	private const float width = 50f;

	private void OnGUI()
	{
		if (icons.Count == 0)
		{
			icons.AddRange(Resources.FindObjectsOfTypeAll<Texture>());
		}
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);
		{
			int count = Mathf.RoundToInt(position.width / (width + 3f));
			for (int i = 0; i < icons.Count; i += count)
			{
				GUILayout.BeginHorizontal();
				for (int j = 0; j < count; j++)
				{
					int index = i + j;
					if (index < icons.Count)
					{
						if (GUILayout.Button(icons[index], GUILayout.Width(width), GUILayout.Height(30)))
						{
							Debug.LogError(icons[index].name);
						}
					}
				}
				GUILayout.EndHorizontal();
			}
		}
		GUILayout.EndScrollView();
	}
}