using ET;
using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Loader
{
	public interface IResLoader
	{
		void Update();
		T Load<T>(string path) where T : Object;
		void LoadAsync(string path, Action<Object> on_loaded_func);
		GameObject GetInstance(string path, Transform parent);
		void Recycle(GameObject instance);
	}
}