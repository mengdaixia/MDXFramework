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
	public class Climb : HFStateBase, IStateCondition
	{
		private ISceneUnit owner;
		public Climb(ISceneUnit unit)
		{
			owner = unit;
		}
		public bool CheckConditon()
		{
			return false;
		}
	}
}