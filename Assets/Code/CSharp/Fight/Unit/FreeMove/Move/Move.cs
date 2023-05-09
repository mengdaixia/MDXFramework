using Code.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Fight.FreeMove
{
	public class Move : HFStateBase
	{
		private ISceneUnit owner;
		public Move(ISceneUnit unit)
		{
			owner = unit;
		}
		protected override void OnUpdate()
		{
			if (true)
			{

			}
		}
	}
}