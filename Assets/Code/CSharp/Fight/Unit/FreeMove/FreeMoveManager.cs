using Code.Fight.FreeMove;
using Code.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Fight
{
	public class FreeMoveManager : SubManager
	{
		private HFStateMachine root;
		protected override void OnInit()
		{
			var climb = new Climb(owner);
			var move = new Move(owner);
			root = new HFStateMachine();
			root.Add(move);
			root.Add(climb);
			root.Switch(move);
		}
		protected override void OnUpdate()
		{
			root.Update();
		}
		protected override void OnDestroy(bool is_dead)
		{
			
		}
	}
}