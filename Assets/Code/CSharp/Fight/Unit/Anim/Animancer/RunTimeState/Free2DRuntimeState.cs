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
	public class Free2DRuntimeState : MultipleRuntimeState
	{
		private AnimancerBlendTreeInfo btInfo;
		private MixerTransition2D mtTrans;
		public override ITransition Transition => mtTrans;

		public override void Init(AnimancerManager mgr, AnimancerMotionInfo motion_info)
		{
			base.Init(mgr, motion_info);
			btInfo = motion_info as AnimancerBlendTreeInfo;
			mtTrans = new MixerTransition2D();
			mtTrans.DefaultParameter = Vector2.zero;
			mtTrans.NormalizedStartTime = 0;
			var childs = btInfo.ChildMotionLst;
			var count = childs.Count;
			mtTrans.Animations = new System.Object[count];
			mtTrans.Thresholds = new Vector2[count];
			for (int i = 0; i < count; i++)
			{
				AddChildState(i, childs[i]);
			}
		}
		public void AddChildState(int index, AnimancerChildMotionInfo motion_info)
		{
			var child = AnimancerManager.CreateState(animancer, motion_info.Motion);
			mtTrans.Animations[index] = child.Transition;
			mtTrans.Thresholds[index] = motion_info.Position;
			mtTrans.Speed = motion_info.TimeScale;
			ChildRuntimeStateLst.Add(child);
		}
		public override void Update()
		{
			base.Update();
			var x = animancer.GetParam(btInfo.BlendParameter);
			var y = animancer.GetParam(btInfo.BlendParameterY);
			mtTrans.State.Parameter = new Vector2(x, y);
		}
	}
}