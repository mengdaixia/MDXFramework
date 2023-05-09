using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Mgr
{
	public interface IRunningMgr
	{
		void Init();
		void Destroy();
	}
	public interface IFixedUpdate
	{
		void FixedUpdate();
	}
	public interface IUpdate
	{
		void Update();
	}
	public interface ILateUpdate
	{
		void LateUpdate();
	}
}