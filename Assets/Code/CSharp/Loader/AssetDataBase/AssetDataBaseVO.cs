using ET;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Loader.Resource
{
	public class AssetDataBaseVO
	{
		public Object Asset { get; private set; }
		private Queue<GameObject> goPool = new Queue<GameObject>();
		public void Load(string path)
		{
#if UNITY_EDITOR
			if (Asset == null)
			{
				Asset = AssetDatabase.LoadAssetAtPath(path, typeof(Object));
			}
#endif
		}
		public void Recycle(GameObject go)
		{
			goPool.Enqueue(go);
			Utility.Go.Hide(go);
		}
		public GameObject GetInstance(Transform parent)
		{
			GameObject result = null;
			if (goPool.Count > 0)
			{
				result = goPool.Dequeue();
				Utility.Trans.SetParent(result.transform, parent);
			}
			else
			{
				result = GameObject.Instantiate(Asset as GameObject, parent);
			}
			return result;
		}
	}
}