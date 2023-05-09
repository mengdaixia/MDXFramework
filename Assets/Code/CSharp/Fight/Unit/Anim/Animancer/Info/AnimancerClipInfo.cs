using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Fight.Animancer
{
	public class AnimancerClipInfo : AnimancerMotionInfo
	{
		public override EMotionType MotionType => EMotionType.Clip;
		public string ClipPath;
	}
}