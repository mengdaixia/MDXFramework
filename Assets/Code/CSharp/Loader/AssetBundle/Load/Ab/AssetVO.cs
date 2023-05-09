using ET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Loader
{
	public class AssetVO : IAssetVO
	{
		private AssetBundleLoader loader;
		private BundleVO bundlVO;
		private string assetName;
		private AssetBundleRequest request;
		private ETTask<Object> assetTask;
		private Object asset;
		public int RefCount { get; set; }

		public EAssetLoadState State { get; private set; }

		public AssetVO(AssetBundleLoader la, BundleVO bundle)
		{
			loader = la;
			bundlVO = bundle;
		}
		public void Set(string asset_name)
		{
			assetName = asset_name;
			assetTask = ETTask<Object>.Create();
			State = EAssetLoadState.None;
		}
		public bool IsDependenceLoaded()
		{
			return bundlVO.Bundle != null;
		}
		public void Start()
		{
			request = bundlVO.Bundle.LoadAssetAsync(assetName);
			State = EAssetLoadState.Loading;
		}
		public void Update()
		{
			if (request != null && request.isDone)
			{
				State = EAssetLoadState.Finish;
				asset = request.asset;
			}
		}
		public void Finish()
		{
			assetTask.SetResult(asset);
			request = null;
		}
		public Object Load(string asset_name)
		{
			if (asset != null)
			{
				return asset;
			}
			asset = bundlVO.Bundle.LoadAsset(asset_name);
			State = EAssetLoadState.Finish;
			return asset;
		}
		public ETTask<Object> LoadAsync(string asset_name)
		{
			if (State < EAssetLoadState.Start)
			{
				loader.EnqueueJob(this);
				State = EAssetLoadState.Start;
			}
			return assetTask;
		}
		public void UnLoad()
		{

		}
	}
}