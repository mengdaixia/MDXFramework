using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Fight.Animancer
{
	public class AnimancerTransitionInfo
	{
		public float Duration;
		public int DestNameHash;
		public List<AnimancerConditionInfo> ConditionLst = new();
#if UNITY_EDITOR
		public void AddCondition(UnityEditor.Animations.AnimatorCondition conditon)
		{
			var info = new AnimancerConditionInfo();
			info.ParamHash = Animator.StringToHash(conditon.parameter);
			info.ConditionMode = (EAnimatorConditionMode)(int)conditon.mode;
			info.Value = conditon.threshold;
			ConditionLst.Add(info);
		}
#endif
	}
}