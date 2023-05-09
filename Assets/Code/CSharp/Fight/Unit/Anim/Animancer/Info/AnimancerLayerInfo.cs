using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Fight.Animancer
{   //
	// 摘要:
	//     Specifies how the layer is blended with the previous layers.
	public enum EAnimatorLayerBlendingMode
	{
		//
		// 摘要:
		//     Animations overrides to the previous layers.
		Override,
		//
		// 摘要:
		//     Animations are added to the previous layers.
		Additive
	}
	public class AnimancerLayerInfo
	{
		public bool IkPass;
		public EAnimatorLayerBlendingMode Mode;
		public string AvatarMask;
		public Dictionary<int, AnimancerStateInfo> StateDic = new();
		public List<AnimancerTransitionInfo> AnyStateTransitionLst = new();
		public Dictionary<int, List<AnimancerTransitionInfo>> StateTransitionDic = new();

#if UNITY_EDITOR
		public void AddState(UnityEditor.Animations.AnimatorState state)
		{
			var stateInfo = new AnimancerStateInfo();
			stateInfo.Speed = state.speed;
			stateInfo.NameHash = state.nameHash;
			if (state.motion is AnimationClip)
			{
				var clipInfo = new AnimancerClipInfo();
				var clip = state.motion as AnimationClip;
				if (clip != null)
				{
					clipInfo.ClipPath = UnityEditor.AssetDatabase.GetAssetPath(clip).Replace("Assets/", "");
				}
				stateInfo.Motion = clipInfo;
			}
			else
			{
				var btInfo = new AnimancerBlendTreeInfo();
				var bt = state.motion as UnityEditor.Animations.BlendTree;
				btInfo.BlendParameter = bt.blendParameter;
				btInfo.BlendParameterY = bt.blendParameterY;
				btInfo.Type = (EBlendTreeType)(int)bt.blendType;
				if (bt != null)
				{
					foreach (var cm in bt.children)
					{
						btInfo.AddChildMotion(cm);
					}
				}
				stateInfo.Motion = btInfo;
			}
			StateDic.Add(stateInfo.NameHash, stateInfo);
		}
		public void AddStateTrans(int source, UnityEditor.Animations.AnimatorStateTransition transition)
		{
			var info = new AnimancerTransitionInfo();
			info.DestNameHash = transition.destinationState.nameHash;
			info.Duration = transition.duration;
			foreach (var condition in transition.conditions)
			{
				info.AddCondition(condition);
			}
			if (source == 0)
			{
				AnyStateTransitionLst.Add(info);
			}
			else
			{
				if (!StateTransitionDic.TryGetValue(source, out List<AnimancerTransitionInfo> lst))
				{
					lst = new List<AnimancerTransitionInfo>();
					StateTransitionDic[source] = lst;
				}
				lst.Add(info);
			}
		}
#endif
	}
}