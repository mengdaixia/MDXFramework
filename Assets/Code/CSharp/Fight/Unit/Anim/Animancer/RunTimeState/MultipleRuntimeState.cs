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
	public class MultipleRuntimeState : AnimancerRuntimeState
	{
		public override ITransition Transition => null;

		protected List<AnimancerRuntimeState> ChildRuntimeStateLst = new();

		public override void Set(AnimancerState state)
		{
			base.Set(state);
			for (int i = 0; i < ChildRuntimeStateLst.Count; i++)
			{
				ChildRuntimeStateLst[i].Set(animancerState.GetChild(i));
			}
		}
		public override void Update()
		{
			foreach (var item in ChildRuntimeStateLst)
			{
				item.Update();
			}
		}
	}
}