using System;
using System.Collections.Generic;
using UnityEngine;

public static partial class Utility
{
	public static class Go
	{
		public static void SetLayer(GameObject go, int layer, bool is_all = true)
		{
			if (go != null)
			{
				go.layer = layer;
				if (is_all)
				{
					for (int i = 0; i < go.transform.childCount; i++)
					{
						var child = go.transform.GetChild(i).gameObject;
						SetLayer(child, layer, true); ;
					}
				}
			}
		}
		public static void SetActive(GameObject go, bool active)
		{
			if (go != null && go.activeSelf != active)
			{
				go.SetActive(active);
			}
		}
		public static void Hide(GameObject go)
		{
			Utility.Trans.SetParent(go.transform, SceneStaticSetting.DisableRoot, false);
		}
		public static void Destroy(GameObject go)
		{
			if (go != null)
			{
				GameObject.Destroy(go);
			}
		}
		public static void DestroyImmediate(GameObject go, bool allowDestroyingAssets = false)
		{
			if (go != null)
			{
				GameObject.DestroyImmediate(go, allowDestroyingAssets);
			}
		}
		public static void DontDestroy(GameObject go)
		{
			if (go != null)
			{
				GameObject.DontDestroyOnLoad(go);
			}
		}
	}
}