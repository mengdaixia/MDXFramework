using ET;
using Game.FileSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Loader
{
	public class BundleVO : IAssetVO
	{
		private AssetBundleLoader loader;
		private VFileSystem vfile;
		private AssetBundle bundle;
		private AssetBundleCreateRequest bundleRequest;
		private HashSet<string> dependenceBundleHash = new HashSet<string>();
		private HashSet<string> removedHash = new HashSet<string>();
		private Dictionary<string, AssetVO> assetName2AssetDic = new Dictionary<string, AssetVO>();
		public int RefCount { get; set; }
		public EAssetLoadState State { get; private set; }
		public AssetBundle Bundle => bundle;

		public BundleVO(AssetBundleLoader la)
		{
			loader = la;
		}
		public void Set(VFileSystem vfs, string[] dependences)
		{
			vfile = vfs;
			dependenceBundleHash.UnionWith(dependences);
			State = EAssetLoadState.Start;
		}
		public bool IsDependenceLoaded()
		{
			foreach (var item in dependenceBundleHash)
			{
				if (loader.BundleDic[item].State == EAssetLoadState.Finish)
				{
					removedHash.Add(item);
				}
			}
			if (removedHash.Count > 0)
			{
				foreach (var item in removedHash)
				{
					dependenceBundleHash.Remove(item);
				}
				removedHash.Clear();
			}
			return dependenceBundleHash.Count == 0;
		}
		public void Start()
		{
			bundleRequest = AssetBundle.LoadFromFileAsync(vfile.FilePath, 0, (ulong)vfile.DataOffset);
			State = EAssetLoadState.Loading;
		}
		public void Update()
		{
			if (bundleRequest != null && bundleRequest.isDone)
			{
				bundle = bundleRequest.assetBundle;
				State = EAssetLoadState.Finish;
			}
		}
		public void Finish()
		{
			bundleRequest = null;
			dependenceBundleHash.Clear();
		}
		public void LoadBundle()
		{
			if (bundle == null)
			{
				foreach (var item in dependenceBundleHash)
				{
					loader.BundleDic[item].LoadBundle();
				}
				bundle = AssetBundle.LoadFromFile(vfile.FilePath, 0, (ulong)vfile.DataOffset);
				State = EAssetLoadState.Finish;
			}
		}
		public Object Load(string asset_name)
		{
			LoadBundle();
			var vo = GetAssetVO(asset_name);
			return vo.Load(asset_name);
		}
		public async ETTask<Object> LoadAsync(string asset_name)
		{
			var vo = GetAssetVO(asset_name);
			return await vo.LoadAsync(asset_name);
		}
		private AssetVO GetAssetVO(string asset_name)
		{
			if (!assetName2AssetDic.TryGetValue(asset_name, out AssetVO vo))
			{
				vo = new AssetVO(loader, this);
				vo.Set(asset_name);
				assetName2AssetDic[asset_name] = vo;
			}
			return vo;
		}
		public void UnLoad()
		{
			if (bundle == null)
			{
				return;
			}
			dependenceBundleHash.Clear();
			assetName2AssetDic.Clear();
			bundle.Unload(true);
			bundle = null;
		}
	}
}