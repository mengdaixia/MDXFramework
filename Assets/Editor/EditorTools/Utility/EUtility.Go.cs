using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public static partial class EUtility
{
	public static class Go
	{
		private static StringBuilder tempSb = new StringBuilder();
		private static Dictionary<GameObject, string> go2NameDic = new Dictionary<GameObject, string>();

		public static string GetName(GameObject go)
		{
			if (!go2NameDic.TryGetValue(go, out string name))
			{
				name = go.name;
				go2NameDic[go] = name;
			}
			return name;
		}
		public static string GetRelativePath(GameObject root, GameObject target)
		{
			if (target == root || target == null)
			{
				return string.Empty;
			}
			tempSb.Clear();
			tempSb.Append(GetName(target));
			var parent = target.transform.parent;
			while (parent != root.transform)
			{
				if (parent != null)
				{
					tempSb.Insert(0, "/");
					tempSb.Insert(0, GetName(parent.gameObject));
				}
				else
				{
					break;
				}
			}
			var str = tempSb.ToString();
			tempSb.Clear();
			return str;
		}
		public static ITreeViewItem BuildPrefabRoot(GameObject go, Action<PrefabTreeItem> on_draw = null)
		{
			return BuildItem(go, on_draw);
		}
		private static PrefabTreeItem BuildItem(GameObject go, Action<PrefabTreeItem> on_draw = null)
		{
			var root = new PrefabTreeItem();
			root.Name = GetName(go);
			root.Go = go;
			root.OnExDraw = on_draw;
			for (int i = 0; i < go.transform.childCount; i++)
			{
				var childGo = go.transform.GetChild(i).gameObject;
				var childItem = BuildItem(childGo, on_draw);
				root.ChildItemLst.Add(childItem);
			}
			return root;
		}
	}
}