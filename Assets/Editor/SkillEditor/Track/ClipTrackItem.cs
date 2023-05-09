using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.Skill
{
	public class ClipTrackItem : SkillTrackItem
	{
		private string animName;
		private Animator animator;
		private AnimationClip clip;
		public override bool isCanDragTrack => false;
		public override string HeadName => "Animation";
		public void Set(string anim_name)
		{
			animName = anim_name;
			TrackName = animName;
			var go = Selection.activeGameObject;
			animator = go.GetComponent<Animator>();
			var actr = animator.runtimeAnimatorController as AnimatorController;
			foreach (var state in actr.layers[0].stateMachine.states)
			{
				if (state.state.name == animName)
				{
					clip = state.state.motion as AnimationClip;
					break;
				}
			}
			durationTime = clip.length;
		}
		public override void DrawInfo(ref float z, float width)
		{
			base.DrawInfo(ref z, width);
		}
		public override void DrawTrack(ref float z, Rect area)
		{
			base.DrawTrack(ref z, area);
		}
		public override void OnRunTimeChanged(double time, bool is_play)
		{
			clip?.SampleAnimation(animator.gameObject, (float)time);
		}
	}
}