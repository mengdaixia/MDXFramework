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
	//     The type of blending algorithm that the blend tree uses.
	public enum EBlendTreeType
	{
		//
		// 摘要:
		//     Basic blending using a single parameter.
		Simple1D,
		//
		// 摘要:
		//     Best used when your motions represent different directions, such as "walk forward",
		//     "walk backward", "walk left", and "walk right", or "aim up", "aim down", "aim
		//     left", and "aim right".
		SimpleDirectional2D,
		//
		// 摘要:
		//     This blend type is used when your motions represent different directions, however
		//     you can have multiple motions in the same direction, for example "walk forward"
		//     and "run forward".
		FreeformDirectional2D,
		//
		// 摘要:
		//     Best used when your motions do not represent different directions.
		FreeformCartesian2D,
		//
		// 摘要:
		//     Direct control of blending weight for each node.
		Direct
	}
	public class AnimancerBlendTreeInfo : AnimancerMotionInfo
	{
		public override EMotionType MotionType => EMotionType.BlendTree;
		public string BlendParameter;
		public string BlendParameterY;
		public EBlendTreeType Type;
		public List<AnimancerChildMotionInfo> ChildMotionLst = new();

#if UNITY_EDITOR
		public void AddChildMotion(UnityEditor.Animations.ChildMotion cmotion)
		{
			var acmInfo = new AnimancerChildMotionInfo();
			acmInfo.Mirror = cmotion.mirror;
			acmInfo.Position = cmotion.position;
			acmInfo.Threshold = cmotion.threshold;
			acmInfo.TimeScale = cmotion.timeScale;

			if (cmotion.motion is AnimationClip)
			{
				var clipInfo = new AnimancerClipInfo();
				var clip = cmotion.motion as AnimationClip;
				if (clip != null)
				{
					var assetPath = UnityEditor.AssetDatabase.GetAssetPath(clip);
					clipInfo.ClipPath = assetPath.Replace("Assets/", "");
				}
				acmInfo.Motion = clipInfo;
			}
			else
			{
				var btInfo = new AnimancerBlendTreeInfo();
				var bt = cmotion.motion as UnityEditor.Animations.BlendTree;
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
				acmInfo.Motion = btInfo;
			}
			ChildMotionLst.Add(acmInfo);
		}
#endif
	}
}