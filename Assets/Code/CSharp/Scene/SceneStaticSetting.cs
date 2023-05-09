using System;
using System.Collections.Generic;
using UnityEngine;

public static class SceneStaticSetting
{
	public static readonly Transform SceneRoot;
	public static readonly Transform CharacterRoot;
	public static readonly Transform BulletRoot;
	public static readonly Transform LevelRoot;
	public static readonly Transform EffecRoot;
	public static readonly Transform DisableRoot;


	static SceneStaticSetting()
	{
		SceneRoot = new GameObject("SceneRoot").transform;
		CharacterRoot = new GameObject("CharacterRoot").transform;
		BulletRoot = new GameObject("BulletRoot").transform;
		LevelRoot = new GameObject("LevelRoot").transform;
		DisableRoot = new GameObject("DisableRoot").transform;
		EffecRoot = new GameObject("EffecRoot").transform;

		Utility.Trans.SetParent(CharacterRoot, SceneRoot);
		Utility.Trans.SetParent(BulletRoot, SceneRoot);
		Utility.Trans.SetParent(LevelRoot, SceneRoot);
		Utility.Trans.SetParent(DisableRoot, SceneRoot);
		Utility.Trans.SetParent(EffecRoot, SceneRoot);

		Utility.Go.SetActive(DisableRoot.gameObject, false);

		Utility.Trans.SetPosition(SceneRoot, Vector3.zero);
		Utility.Go.DontDestroy(SceneRoot.gameObject);
	}
}