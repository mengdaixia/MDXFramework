/************************************************* 
  *Author: 作者 
  *Date: 日期 
  *Description: 説明
**************************************************/
using System.Collections.Generic;
using UnityEngine;

namespace Code.Mgr
{
	public class RunningMgrList
	{
		private List<IRunningMgr> mgrLst = new List<IRunningMgr>();

		public void AddMgr(IRunningMgr mgr)
		{
			mgr.Init();
			mgrLst.Add(mgr);
		}
		public void FixedUpdate()
		{
			for (int i = 0; i < mgrLst.Count; i++)
			{
				var mgr = mgrLst[i];
				if (mgr is IFixedUpdate update)
				{
					update.FixedUpdate();
				}
			}
		}
		public void Update()
		{
			for (int i = 0; i < mgrLst.Count; i++)
			{
				var mgr = mgrLst[i];
				if (mgr is IUpdate update)
				{
					update.Update();
				}
			}
		}

		public void LateUpdate()
		{
			for (int i = 0; i < mgrLst.Count; i++)
			{
				var mgr = mgrLst[i];
				if (mgr is ILateUpdate update)
				{
					update.LateUpdate();
				}
			}
		}
		public void Destroy()
		{
			for (int i = 0; i < mgrLst.Count; i++)
			{
				mgrLst[i].Destroy();
			}
		}
	}
}