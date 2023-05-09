using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static partial class EUtility
{
	public static class Asset
	{
		public static string GetAssetPathNoAsset(UnityEngine.Object obj)
		{
			return AssetDatabase.GetAssetPath(obj).Replace("Assets/", "");
		}
	}
}