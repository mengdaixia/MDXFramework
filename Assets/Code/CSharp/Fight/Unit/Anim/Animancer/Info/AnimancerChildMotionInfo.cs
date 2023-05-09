using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Fight.Animancer
{
	public class AnimancerChildMotionInfo
	{
		public AnimancerMotionInfo Motion;
		public float Threshold;
		public Vector2 Position;
		public float TimeScale;
		public bool Mirror;
	}
}