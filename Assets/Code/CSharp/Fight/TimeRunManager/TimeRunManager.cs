using Code.Mgr;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Fight
{
	public class TimeRunManager : Singleton<TimeRunManager>, IRunningMgr
	{

		public float CurrTimeSpeed { get; private set; } = 1;
		private TimeRunManager() { }
		public void Init()
		{

		}
		public void Destroy()
		{

		}
		public void PauseGame()
		{
			SetTimeRunSpeed(CurrTimeSpeed != 0 ? 0 : 1);
		}
		public void SetTimeRunSpeed(float speed)
		{
			if (CurrTimeSpeed != speed)
			{
				Utility.Time.SetTimeScale(speed);
				CurrTimeSpeed = speed;
			}
		}
	}
}