using Code.Fight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

//单位类型
[Flags]
public enum ESceneUnitType
{
	None = 0,
	Character = 1 << 0,
	Bullte = 1 << 1,
	Summoned = 1 << 2,
	Destructable = 1 << 3,          //可破坏物
	DropItem = 1 << 4,
}
public interface ISceneUnit
{
	long UnitId { get; set; }
	Transform Root { get; set; }
	void Init();
	void DestroySelf(bool is_dead);
	ISubManagerList SubMgrList { get; }
	UnitAttr Attr { get; }              //可以放到上面，但为了方便吧
	bool IsAlive { get; }
	EUnitCampType UnitCamp { get; }
	ESceneUnitType UnitType { get; }
	GameObject ModelGo { get; }
}

public struct SceneUnitTypeComparer : IEqualityComparer<ESceneUnitType>
{
	public bool Equals(ESceneUnitType x, ESceneUnitType y)
	{
		return (int)x == (int)y;
	}
	public int GetHashCode(ESceneUnitType obj)
	{
		return (int)obj;
	}
}

//阵营
[Flags]
public enum EUnitCampType
{
	None = 0,
	Character = 1 << 0,                              //玩家
	Enemy = 1 << 1,                                  //敌人
	Destructable = 1 << 2,                           //可破坏物
	All = Character | Enemy | Destructable,          //所有
}

public struct UnitCampTypeComparer : IEqualityComparer<EUnitCampType>
{
	public bool Equals(EUnitCampType x, EUnitCampType y)
	{
		return (int)x == (int)y;
	}
	public int GetHashCode(EUnitCampType obj)
	{
		return (int)obj;
	}
}