using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Code.Loader.Resource;
using Object = UnityEngine.Object;
using ET;

namespace Code.Loader
{
	public class AssetDataBaseLoader : IResLoader
	{
		private Dictionary<string, AssetDataBaseVO> cacheDic = new Dictionary<string, AssetDataBaseVO>();
		private Dictionary<int, AssetDataBaseVO> id2VoDic = new Dictionary<int, AssetDataBaseVO>();
		public void Update()
		{

		}
		public T Load<T>(string path) where T : Object
		{
			if (!cacheDic.TryGetValue(path, out AssetDataBaseVO result))
			{
				var realPath = "Assets/" + path;
				result = new AssetDataBaseVO();
				result.Load(realPath);
				cacheDic[path] = result;
			}
			return result.Asset as T;
		}
		public void LoadAsync(string path, Action<Object> loaded = null)
		{
			loaded?.Invoke(Load<Object>(path));
		}
		public void Recycle(GameObject go)
		{
			if (id2VoDic.TryGetValue(go.GetInstanceID(), out AssetDataBaseVO vo))
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