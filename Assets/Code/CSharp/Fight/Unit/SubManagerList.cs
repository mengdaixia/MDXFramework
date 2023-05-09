using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Fight
{
	public interface ISubManagerList
	{
		T Add<T>() where T : SubManager, new();
		T AddTemp<T>() where T : SubManager, new();
		void Remove<T>() where T : SubManager;
		T Get<T>() where T : SubManager;
	}
	public class SubManagerList : ISubManagerList
	{
		private Action updateFunc;
		private ISceneUnit owner;
		private Dictionary<Type, SubManager> mgrDic = new Dictionary<Type, SubManager>();
		private List<SubManager> mgrLst = new List<SubManager>();
		private List<SubManager> tempMgrLst = new List<SubManager>();
		private bool isEnable = false;
		public void Init(ISceneUnit unit)
		{
			isEnable = true;
			owner = unit;
			foreach (var mgr in mgrLst)
			{
				mgr.Set(owner);
				mgr.Init();
			}
			updateFunc = updateFunc == null ? Update : updateFunc;
			UpdateEvent.OnUpdate += updateFunc;
		}
		public void Update()
		{
			if (!isEnable)
			{
				return;
			}
			for (int i = mgrLst.Count - 1; i >= 0; i--)
			{
				mgrLst[i].Update();
			}
		}
		public void Destroy(bool is_dead)
		{
			isEnable = false;
			foreach (var item in mgrDic)
			{
				item.Value.Destroy(is_dead);
			}
			for (int i = tempMgrLst.Count - 1; i >= 0; i--)
			{
				Remove(tempMgrLst[i].GetType(), true);
			}
			UpdateEvent.OnUpdate -= updateFunc;
		}
		public T AddTemp<T>() where T : SubManager, new()
		{
			var result = Add<T>();
			tempMgrLst.Add(result);
			return result;
		}
		public T Add<T>() where T : SubManager, new()
		{
			var type = typeof(T);
			if (!mgrDic.TryGetValue(type, out SubManager mgr))
			{
				mgr = ObjectPool.Get<T>();
				if (owner != null)
				{
					mgr.Set(owner);
					mgr.Init();
				}
				mgrDic[type] = mgr;
				mgrLst.Add(mgr);
			}
			return mgr as T;
		}
		public void Remove<T>() where T : SubManager
		{
			var type = typeof(T);
			Remove(type, true);
		}
		public T Get<T>() where T : SubManager
		{
			if (mgrDic.TryGetValue(typeof(T), out SubManager result))
			{
				return result as T;
			}
			return null;
		}
		private void Remove(Type type, bool destroy = false)
		{
			if (mgrDic.TryGetValue(type, out SubManager mgr))
			{
				if (destroy)
				{
					mgr.Destroy(false);
				}
				mgrDic.Remove(type);
				mgrLst.Remove(mgr);
				tempMgrLst.Remove(mgr);
				ObjectPool.Release(mgr);
			}
		}
		public void RebindModel()
		{
			foreach (var mgr in mgrLst)
			{
				mgr.RebindModel();
			}
		}
	}
}