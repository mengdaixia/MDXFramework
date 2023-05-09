using Animancer;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimancerTest : MonoBehaviour
{
	public AnimationClip Clip;

	[SerializeField]
	public AnimationClip[] Clips;

	private AnimancerState currState;
	void Start()
	{
		var ac = GetComponentInChildren<AnimancerComponent>();
		//var clipTrans = new ClipTransition();
		//clipTrans.Clip = Clip;
		//clipTrans.FadeDuration = 0.2f;
		//clipTrans.NormalizedStartTime = 0;
		//clipTrans.Speed = 1;
		//var state = ac.Play(clipTrans);

		//var lmTrans = new LinearMixerTransition();
		//lmTrans.DefaultParameter = 0;
		//lmTrans.FadeDuration = 0.2f;
		//lmTrans.NormalizedStartTime = 0;
		//lmTrans.Speed = 1;
		//lmTrans.Animations = Clips;
		//lmTrans.Thresholds = new float[3] { 0, 0.5f, 1 };
		//currState = ac.Play(lmTrans);

		var lmTrans = new MixerTransition2D();
		lmTrans.DefaultParameter = new Vector2(0, 0);
		lmTrans.FadeDuration = 0.2f;
		lmTrans.NormalizedStartTime = 0;
		lmTrans.Speed = 1;
		lmTrans.Animations = Clips;
		lmTrans.Thresholds = new Vector2[5]
		{
			new Vector2(0,0),
			new Vector2(-1,0),
			new Vector2(0,1),
			new Vector2(1,0),
			new Vector2(0,-1)
		};
		currState = ac.Play(lmTrans);
		var evt = new AnimancerEvent() { normalizedTime = 0.5f, callback = () => { Debug.LogError(1); } };
		currState.Events.Add(evt);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.A))
		{
			(currState as MixerState<Vector2>).Parameter = new Vector2(-1, 0);
		}
		if (Input.GetKeyDown(KeyCode.S))
		{
			(currState as MixerState<Vector2>).Parameter = new Vector2(0, -1);
		}
		if (Input.GetKeyDown(KeyCode.D))
		{
			(currState as MixerState<Vector2>).Parameter = new Vector2(1, 0);
		}
		if (Input.GetKeyDown(KeyCode.W))
		{
			(currState as MixerState<Vector2>).Parameter = new Vector2(0, 1);
		}
	}
}
