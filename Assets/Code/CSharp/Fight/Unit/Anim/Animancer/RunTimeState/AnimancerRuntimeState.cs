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
	public abstract class AnimancerRuntimeState
	{
		protected AnimancerManager animancer;
		protected AnimancerState animancerState;
		public AnimancerMotionInfo MotionInfo { get; private set; }
		public abstract ITransition Transition { get; }

		public virtual void Init(AnimancerManager mgr, AnimancerMotionInfo motion_info)
		{
			animancer = mgr;
			MotionInfo = motion_info;
		}
		public virtual void Set(AnimancerState state)
		{
			animancerState = state;
		}
		public virtual void Update()
		{

		}
	}
}