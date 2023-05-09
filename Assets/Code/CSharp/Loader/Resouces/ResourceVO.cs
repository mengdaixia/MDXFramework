using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Loader.Resource
{
	public class ResourceVO
	{
		public Object Asset { get; private set; }
		private Queue<GameObject> goPool = new Queue<GameObject>();
		public void Load(string path)
		{
			if (Asset == null)
			{
				Asset = Resources.Load(path);
			}
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