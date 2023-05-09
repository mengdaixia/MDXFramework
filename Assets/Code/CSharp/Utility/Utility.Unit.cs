using Code.Fight;
using Code.Fight.Operate;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static partial class Utility
{
	public static class Unit
	{
		public static PlayerUnit CreatePlayerUnit(int id, Vector3 pos, Quaternion rot)
		{
			var player = new PlayerUnit();
			player.Set(id);
			SceneUnitManager.Instance.CreateUnit(player);
			Utility.Trans.SetPosition(player.Root, pos);
			Utility.Trans.SetRotation(player.Root, rot);

			player.SubMgrList.AddTemp<AnimancerManager>();
			player.SubMgrList.AddTemp<SkillManager>();
			player.SubMgrList.AddTemp<OperateManager>();
			player.SubMgrList.AddTemp<AnimParamManager>();
			return player;
		}
	}
}