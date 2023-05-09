using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SetAbName : EditorWindow
{
	[MenuItem("Assets/设置AB包")]
	private static void SetName()
	{
		var select = Selection.activeObject;
		if (select != null)
		{
			var path = AssetDatabase.GetAssetPath(select);
			if (Directory.Exists(path))
			{
				var importer = AssetImporter.GetAtPath(path);
				if (importer != null)
				{
					var index = path.IndexOf("Res");
					if (index > 0)
					{
						importer.assetBundleName = path.Substring(index);
					}
				}
			}
		}
	}
}
