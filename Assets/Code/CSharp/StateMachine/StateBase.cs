using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.StateMachine
{
	public abstract class HFStateBase
	{
		private HFStateBase currState;
		private List<HFStateBase> childState = new List<HFStateBase>();
		public HFStateBase Root;
		public void Enter()
		{
			OnEnter();
		}
		public void Update()
		{
			OnUpdate();
			currState?.Update();
			for (int i = 0; i < childState.Count; i++)
			{
				var child = childState[i];
				if (child is IStateCondition condition)
				{
					if (condition.CheckConditon())
					{
						Switch(child);
						return;
					}
				}
			}
		}
		public void End()
		{
			OnEnd();
		}
		public void Add(HFStateBase child)
		{
			child.Root = this;
			childState.Add(this);
		}
		public void Switch(HFStateBase state)
		{
			currState?.End();
			currState = state;
			currState.Enter();
		}
		protected virtual void OnEnter() { }
		protected virtual void OnUpdate() { }
		protected virtual void OnEnd() { }
	}
}