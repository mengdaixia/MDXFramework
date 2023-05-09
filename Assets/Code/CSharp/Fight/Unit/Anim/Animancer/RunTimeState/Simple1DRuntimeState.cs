using Animancer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Fight.Animancer
{
	public class Simple1DRuntimeState : MultipleRuntimeState
	{
		private AnimancerBlendTreeInfo btInfo;
		private LinearMixerTransition lmTrans;
		public override ITransition Transition => lmTrans;

		public override void Init(AnimancerManager mgr, AnimancerMotionInfo motion_info)
		{
			base.Init(mgr, motion_info);
			btInfo = motion_info as AnimancerBlendTreeInfo;
			lmTrans = new LinearMixerTransition();
			lmTrans.DefaultParameter = 0;
			lmTrans.NormalizedStartTime = 0;
			var childs = btInfo.ChildMotionLst;
			var count = childs.Count;
			lmTrans.Animations = new System.Object[count]; ;
			lmTrans.Thresholds = new float[count];
			for (int i = 0; i < count; i++)
			{
				AddChildState(i, childs[i]);
			}
		}
		public void AddChildState(int index, AnimancerChildMotionInfo motion_info)
		{
			var child = AnimancerManager.CreateState(animancer, motion_info.Motion);
			lmTrans.Animations[index] = child.Transition;
			lmTrans.Thresholds[index] = motion_info.Threshold;
			lmTrans.Speed = motion_info.TimeScale;
			ChildRuntimeStateLst.Add(child);
		}
		public override void Update()
		{
			base.Update();
			lmTrans.State.Parameter = animancer.GetParam(btInfo.BlendParameter);
		}
	}
}