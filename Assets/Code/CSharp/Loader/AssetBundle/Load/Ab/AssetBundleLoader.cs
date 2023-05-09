using ET;
using Game.FileSystem;
using Game.Task;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;
using TaskManager = Game.Task.TaskManager;

namespace Code.Loader
{
	//TODO1:把表中的路径列标记到导出PathHash和AssetName，在打AssetBundle时为每一个Bundle计算路径Hash，回来加载时使用PathHash和Asset加载，应该会节省不少字符串的内存（路径不多，冲突概率不高）
	//TODO2:把导表文件也打成Bundle，每次更新时写出去
	public class AssetBundleLoader : IResLoader
	{
		private LoadQueue loadQueue = new LoadQueue();
		private HashSet<string> dependenceHash = new HashSet<string>();
		private Dictionary<string, (string BunldName, string AssetName)> path2AssetDic = new Dictionary<string, (string BunldName, string AssetName)>();
		private Dictionary<string, VFileSystem> fsDic = new Dictionary<string, VFileSystem>();
		public Dictionary<string, BundleVO> BundleDic { get; private set; } = new Dictionary<string, BundleVO>();
		public AssetBundlesInfo ABFileInfos { get; private set; }

		public AssetBundleLoader()
		{
			OnUpdateResHandler();
		}
		public void Update()
		{
			loadQueue.Update();
		}
		public T Load<T>(string path) where T : Object
		{
			var asset = GetAssetInfo(path);
			if (!BundleDic.TryGetValue(asset.BundleName, out BundleVO vo))
			{
				LoadBundle(asset.BundleName, false);
				vo = BundleDic[asset.BundleName];
				dependenceHash.Clear();
			}
			return vo.Load(asset.AssetName) as T;
		}
		public async void LoadAsync(string path, Action<Object> on_loaded_func)
		{
			var obj = await LoadAsync(path);
			on_loaded_func?.Invoke(obj);
		}
		private ETTask<Object> LoadAsync(string path)
		{
			var asset = GetAssetInfo(path);
			if (!BundleDic.TryGetValue(asset.BundleName, out BundleVO bvo))
			{
				LoadBundle(asset.BundleName);
				bvo = BundleDic[asset.BundleName];
				dependenceHash.Clear();
			}
			return bvo.LoadAsync(asset.AssetName);
		}
		private void LoadBundle(string bunlde_name, bool is_async = true)
		{
			var dependence = ABFileInfos.GetAllDependencies(bunlde_name);
			for (int i = 0; i < dependence.Length; i++)
			{
				var bName = dependence[i];
				if (!BundleDic.ContainsKey(bName) && dependenceHash.Add(bName))
				{
					LoadBundle(bName, is_async);
				}
			}
			var bvo = new BundleVO(this);
			var vfs = GetFileSystem(bunlde_name);
			bvo.Set(vfs, dependence);
			BundleDic[bunlde_name] = bvo;
			if (is_async)
			{
				EnqueueJob(bvo);
			}
		}
		public void EnqueueJob(IAssetVO vo)
		{
			loadQueue.EnqueueJob(vo);
		}
		public GameObject GetInstance(string path, Transform parent)
		{
			var go = Load<GameObject>(path);
			var result = GameObject.Instantiate(go, parent);
			return result;
		}
		public void Recycle(GameObject instance)
		{
			Utility.Go.DestroyImmediate(instance);
		}
		private VFileSystem GetFileSystem(string bundl_name)
		{
			var path = PathDefine.READ_WRITE_PATH + bundl_name;
			if (!File.Exists(path))
			{
				path = PathDefine.LOCAL_PATH + bundl_name;
			}
			VFileSystem vfs = FileSystemManager.Instance.Get<VFileSystem>(path);
			fsDic[bundl_name] = vfs;
			return vfs;
		}
		private (string BundleName, string AssetName) GetAssetInfo(string path)
		{
			if (!path2AssetDic.TryGetValue(path, out (string BundleName, string AssetName) asset))
			{
				var index = path.LastIndexOf('/');
				var bundleName = path.Substring(0, index);
				var assetName = path.Substring(index + 1, path.Length - index - 1);
				asset = (bundleName.ToLower(), assetName);
			}
			return asset;
		}
		#region 热更新
		//应该放到外面
		public void CheckResUpdate(Action suc_ac)
		{
			TaskManager.Instance.Start();
			var task = new UpdateResTask();
			task.OnTaskDone += OnUpdateResHandler;
			task.OnTaskDone += suc_ac;
			TaskManager.Instance.AddTask(task);
		}
		private void OnUpdateResHandler()
		{
			ABFileInfos = new AssetBundlesInfo();
			var path = PathDefine.READ_WRITE_PATH + "AssetBundlesInfo.bytes";
			if (!File.Exists(path))
			{
				path = PathDefine.LOCAL_PATH + "AssetBundlesInfo.bytes";
			}
			ABFileInfos.Read(path);
		}
		#endregion
	}
}