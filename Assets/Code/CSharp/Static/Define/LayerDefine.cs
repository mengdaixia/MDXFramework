using System;
using System.Collections.Generic;
using UnityEngine;

public class LayerDefine
{
	public static int MainRoleLayer = LayerMask.NameToLayer("MainRole");
	public static int EnemyLayer = LayerMask.NameToLayer("Enemy");
	public static int BulletLayer = LayerMask.NameToLayer("Bullet");

	public static int MainRoleLayerMask = 1 << MainRoleLayer;
	public static int EnemyLayerMask = 1 << EnemyLayer;
	public static int BulletLayerMask = 1 << BulletLayer;

	public static int ColliderLayerMask = MainRoleLayerMask | EnemyLayerMask | BulletLayerMask;

	public static bool IsColliderLayer(GameObject go)
	{
		var layerMask = 1 << go.layer;
		if ((layerMask & ColliderLayerMask) == layerMask)
		{
			return true;
		}
		return false;
	}
}