using Code.Fight;
using Code.Mgr;
using Code.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneUnitManager : Singleton<SceneUnitManager>, IRunningMgr
{
	private Dictionary<ESceneUnitType, IndexedSet<ISceneUnit>> type2UnitDic = new Dictionary<ESceneUnitType, IndexedSet<ISceneUnit>>(new SceneUnitTypeComparer());
	private Dictionary<EUnitCampType, IndexedSet<ISceneUnit>> camp2UnitDic = new Dictionary<EUnitCampType, IndexedSet<ISceneUnit>>(new UnitCampTypeComparer());
	private Dictionary<long, ISceneUnit> id2UnitDic = new Dictionary<long, ISceneUnit>();
	private IndexedSet<ISceneUnit> unitSet = new IndexedSet<ISceneUnit>();
	private List<ISceneUnit> removeLst = new List<ISceneUnit>();

	private Queue<GameObject> unitRootPool = new Queue<GameObject>();

	private long unitId = 1;

	private SceneUnitManager() { }
	public void Init()
	{

	}
	public void Destroy()
	{
		unitSet.Clear();
		type2UnitDic.Clear();
		camp2UnitDic.Clear();
		id2UnitDic.Clear();
	}
	private GameObject GetRoot()
	{
		if (unitRootPool.Count > 0)
		{
			var result = unitRootPool.Dequeue();
			Utility.Trans.SetParent(result.transform, null);
			return result;
		}
		return new GameObject();
	}
	private void RecycleRoot(GameObject go)
	{
		unitRootPool.Enqueue(go);
		Utility.Go.Hide(go);
	}
	public ISceneUnit Get(long uid)
	{
		if (id2UnitDic.TryGetValue(uid, out ISceneUnit unit))
		{
			return unit;
		}
		return null;
	}
	public void Get(ESceneUnitType unit_type, EUnitCampType unit_camp, List<ISceneUnit> unit_lst)
	{
		unit_lst.Clear();
		for (int i = 0; i < unitSet.Count; i++)
		{
			var unit = unitSet[i];
			if ((unit.UnitType & unit_type) == unit.UnitType && (unit.UnitCamp & unit_camp) == unit.UnitCamp)
			{
				unit_lst.Add(unit);
			}
		}
	}
	private void RegisterUnit(ISceneUnit unit)
	{
		IndexedSet<ISceneUnit> unitLst = null;
		if (!type2UnitDic.TryGetValue(unit.UnitType, out unitLst))
		{
			unitLst = new IndexedSet<ISceneUnit>();
			type2UnitDic[unit.UnitType] = unitLst;
		}

		IndexedSet<ISceneUnit> campLst = null;
		if (!camp2UnitDic.TryGetValue(unit.UnitCamp, out campLst))
		{
			campLst = new IndexedSet<ISceneUnit>();
			camp2UnitDic[unit.UnitCamp] = campLst;
		}
		unit.UnitId = unitId++;
		unit.Root = GetRoot().transform;

#if UNITY_EDITOR
		unit.Root.name = unit.UnitId.ToString();
#endif

		if (!unitSet.Contains(unit))
			unitSet.Add(unit);
		if (!campLst.Contains(unit))
			campLst.Add(unit);
		if (!unitLst.Contains(unit))
			unitLst.Add(unit);
		id2UnitDic[unit.UnitId] = unit;
	}
	private void UnRegisterUnit(ISceneUnit unit)
	{
		if (type2UnitDic.TryGetValue(unit.UnitType, out IndexedSet<ISceneUnit> unitLst))
		{
			if (camp2UnitDic.TryGetValue(unit.UnitCamp, out IndexedSet<ISceneUnit> campLst))
			{
				campLst.Remove(unit);
			}
			id2UnitDic.Remove(unit.UnitId);
			unitSet.Remove(unit);
			unitLst.Remove(unit);
			SceneUnitEvent.OnUnRegisterUnit?.Invoke(unit);
		}
	}
	public void CreateUnit(ISceneUnit unit)
	{
		if (unitSet.Contains(unit))
		{
			return;
		}
		RegisterUnit(unit);
		unit.Init();
	}
	public void DestroyUnit(ISceneUnit unit, bool is_dead = true)
	{
		if (!unitSet.Contains(unit))
		{
			return;
		}
		UnRegisterUnit(unit);
		unit.DestroySelf(is_dead);
		RecycleRoot(unit.Root.gameObject);
		unit.Root = null;
	}
	public void DestroyAllUnit(bool destroy_mainrole = false)
	{
		for (int i = 0; i < unitSet.Count; i++)
		{
			removeLst.Add(unitSet[i]);
		}
		foreach (var item in removeLst)
		{
			DestroyUnit(item, false);
		}
		removeLst.Clear();
	}
	public IList<ISceneUnit> Get(ESceneUnitType type)
	{
		if (!type2UnitDic.TryGetValue(type, out IndexedSet<ISceneUnit> lst))
		{
			lst = new IndexedSet<ISceneUnit>();
			type2UnitDic[type] = lst;
		}
		return lst;
	}
	public IList<ISceneUnit> Get(EUnitCampType type)
	{
		if (!camp2UnitDic.TryGetValue(type, out IndexedSet<ISceneUnit> lst))
		{
			lst = new IndexedSet<ISceneUnit>();
			camp2UnitDic[type] = lst;
		}
		return lst;
	}
}