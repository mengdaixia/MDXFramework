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
	public class ClipRuntimeState : AnimancerRuntimeState
	{
		private ClipTransition clipTrans;
		public override ITransition Transition => clipTrans;

		public override void Init(AnimancerManager mgr, AnimancerMotionInfo motion_info)
		{
			base.Init(mgr, motion_info);
			var clipInfo = motion_info as AnimancerClipInfo;
			clipTrans = new ClipTransition();
			clipTrans.Clip = GameResLoader.Instance.Load<AnimationClip>(clipInfo.ClipPath);
		}
	}
}