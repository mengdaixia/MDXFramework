using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Fight.Animancer
{
	public enum EMotionType
	{
		Clip,
		BlendTree,
	}
	public abstract class AnimancerMotionInfo
	{
		public abstract EMotionType MotionType { get; }
	}
}