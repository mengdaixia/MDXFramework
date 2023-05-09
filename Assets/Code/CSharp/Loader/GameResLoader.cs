using Code.Loader;
using Code.Mgr;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameResLoader : Singleton<GameResLoader>, IRunningMgr, IUpdate
{
	private IResLoader resLoader;
	private GameResLoader() { }

	public void Init()
	{
		switch (GameSetting.Instance.GameType)
		{
			case EGameType.Editor:
				resLoader = new AssetDataBaseLoader();
				break;
			case EGameType.Normal:
				resLoader = new AssetBundleLoader();
				break;
			case EGameType.Update:
				resLoader = new AssetBundleLoader();
				break;
		}
	}
	public void Update()
	{
		resLoader.Update();
	}
	public void LateUpdate()
	{

	}
	public void Destroy()
	{

	}
	public T Load<T>(string path) where T : UnityEngine.Object
	{
		return resLoader.Load<T>(path);
	}
	public GameObject GetInstance(string path, Transform parent = null)
	{
		return resLoader.GetInstance(path, parent);
	}
	//待定
	public void Recycle(GameObject go)
	{
		if (go != null)
		{
			resLoader.Recycle(go);
		}
	}
}