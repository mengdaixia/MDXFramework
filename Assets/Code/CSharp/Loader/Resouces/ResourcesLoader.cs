using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Code.Loader.Resource;
using Object = UnityEngine.Object;

namespace Code.Loader
{
	public class ResourcesLoader : IResLoader
	{
		private Dictionary<string, ResourceVO> cacheDic = new Dictionary<string, ResourceVO>();
		private Dictionary<int, ResourceVO> id2VoDic = new Dictionary<int, ResourceVO>();
		public void Update()
		{

		}
		public T Load<T>(string path) where T : Object
		{
			if (!cacheDic.TryGetValue(path, out ResourceVO result))
			{
				var realPath = path.Split('.')[0];
				result = new ResourceVO();
				result.Load(realPath);
				cacheDic[path] = result;
			}
			return result.Asset as T;
		}
		public void LoadAsync(string path, Action<Object> on_loaded_func = null)
		{
			on_loaded_func?.Invoke(Load<Object>(path));
		}
		public void Recycle(GameObject go)
		{
			if (id2VoDic.TryGetValue(go.GetInstanceID(), out ResourceVO vo))
			{
				vo.Recycle(go);
			}
			else
			{
				Utility.Go.DestroyImmediate(go);
			}
		}
		public GameObject GetInstance(string path, Transform parent)
		{
			Load<Object>(path);
			var vo = cacheDic[path];
			var result = vo.GetInstance(parent);
			id2VoDic[result.GetInstanceID()] = vo;
			return result;
		}
	}
}