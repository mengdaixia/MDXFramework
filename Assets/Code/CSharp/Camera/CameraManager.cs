using Code.Mgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code
{
	public class CameraManager : Singleton<CameraManager>, IRunningMgr, ILateUpdate
	{
		public Transform Target { get; private set; }
		public void Init()
		{
			
		}
		public void LateUpdate()
		{
			if (Target != null)
			{
				var camera = Utility.CameraX.main;
				Utility.Trans.SetForward(camera.transform, Target.transform.forward);
				Utility.Trans.SetPosition(camera.transform, Target.position + Target.forward * -2 + Target.up * 2);
			}
		}
		public void Destroy()
		{

		}
		public void SetTarget(Transform target)
		{
			Target = target;
		}
	}
}