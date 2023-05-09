using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Fight
{
	//部分使用回收较多的需要使用池子
	public abstract class SubManager : IPoolObj
	{
		protected bool enable;
		protected ISceneUnit owner;

		public void Set(ISceneUnit unit)
		{
			owner = unit;
		}
		public void RebindModel()
		{
			OnRebindModel();
		}
		public void Init()
		{
			OnInit();
		}
		public void Destroy(bool is_dead)
		{
			OnDestroy(is_dead);
		}
		public void Update()
		{
			OnUpdate();
		}
		protected virtual void OnRebindModel() { }
		protected virtual void OnInit() { }
		protected virtual void OnUpdate() { }
		protected virtual void OnDestroy(bool is_dead) { }

		public virtual void Clear()
		{

		}
	}
}