using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Fight.Animancer
{
	public enum EAnimatorConditionMode
	{
		//
		// 摘要:
		//     The condition is true when the parameter value is true.
		If = 1,
		//
		// 摘要:
		//     The condition is true when the parameter value is false.
		IfNot = 2,
		//
		// 摘要:
		//     The condition is true when parameter value is greater than the threshold.
		Greater = 3,
		//
		// 摘要:
		//     The condition is true when the parameter value is less than the threshold.
		Less = 4,
		//
		// 摘要:
		//     The condition is true when parameter value is equal to the threshold.
		Equals = 6,
		//
		// 摘要:
		//     The condition is true when the parameter value is not equal to the threshold.
		NotEqual = 7
	}
	public class AnimancerConditionInfo
	{
		public EAnimatorConditionMode ConditionMode;
		public int ParamHash;
		public float Value;
	}
}