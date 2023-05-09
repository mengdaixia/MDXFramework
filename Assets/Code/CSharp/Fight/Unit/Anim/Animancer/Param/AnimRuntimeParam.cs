using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Fight
{
	public enum EState
	{

	}
	public class AnimRuntimeParam
	{
		private ISceneUnit owner;
		private float time;
		private CSVAnimParam paramConf;
		private float duration => paramConf.fLifeTime;
		private List<float> startValueLst = new List<float>();
		private List<float> targetValueLst => paramConf.fListValue;

		public bool IsEnd => time > duration;
		public void Set(ISceneUnit unit, CSVAnimParam conf)
		{
			owner = unit;
			paramConf = conf;
			for (int i = 0; i < targetValueLst.Count; i++)
			{
				var animancer = owner.SubMgrList.Get<AnimancerManager>();
				var value = animancer.GetParam(paramConf.sListParam[i]);
				startValueLst.Add(value);
			}
			if (duration <= 0)
			{
				SetParam(1);
			}
		}
		public void Update()
		{
			if (duration <= 0)
			{
				return;
			}
			time += owner.DeltaTime();
			var percent = time / duration;
			SetParam(percent);
		}
		private void SetParam(float percent)
		{
			percent = Mathf.Clamp01(percent);
			for (int i = 0; i < startValueLst.Count; i++)
			{
				var start = startValueLst[i];
				var end = targetValueLst[i];
				var value = Mathf.Lerp(start, end, percent);
				var animancer = owner.SubMgrList.Get<AnimancerManager>();
				animancer.SetParam(paramConf.sListParam[i], value);
			}
		}
	}
}