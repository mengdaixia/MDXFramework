using Animancer;
using Code.Fight.Animancer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Fight
{
	public class AnimancerManager : SubManager
	{
		private Animator animator;
		private AnimancerInfo animancerInfo;
		private AnimancerComponent animancer;

		private static Dictionary<string, float> param2ValueDic = new();
		private static Dictionary<string, int> animName2HashDic = new();
		private Dictionary<AnimancerStateInfo, AnimancerRuntimeState> state2TransitionDic = new();
		private Dictionary<int, AnimancerRuntimeState> layer2RuntimeStateDic = new();
		protected override void OnRebindModel()
		{
			InitAnimator();
		}
		protected override void OnInit()
		{
			InitAnimator();
		}
		protected override void OnDestroy(bool is_dead)
		{
			Clear();
		}
		private void InitAnimator()
		{
			Clear();
			var model = owner.ModelGo;
			animator = model.GetComponent<Animator>();
			animancer = model.GetComponent<AnimancerComponent>() ?? model.AddComponent<AnimancerComponent>();
			animancer.Animator = animator;

			//TODO:等AB加载改成Hash后这些字符串拼接消耗也能省下
			var aName = CSVModel.Get((owner as IModelConf).ModelId).sAnimatorName;
			var path = "Res/AnimData/" + aName + ".bytes";
			var ta = GameResLoader.Instance.Load<TextAsset>(path);
			var data = ta.text;
			animancerInfo = Utility.Json.Deserialize<AnimancerInfo>(data, Utility.Json.IgnoreLoopSetting);

			foreach (var item in animancerInfo.ParamDic)
			{
				param2ValueDic[item.Key] = item.Value;
			}
		}
		public override void Clear()
		{
			animator = null;
			animancerInfo = null;
			animancer = null;
			state2TransitionDic.Clear();
			layer2RuntimeStateDic.Clear();
		}
		protected override void OnUpdate()
		{
			foreach (var item in layer2RuntimeStateDic)
			{
				if (item.Value != null)
				{
					item.Value.Update();
				}
			}
		}
		public float GetParam(string name)
		{
			if (param2ValueDic.TryGetValue(name, out float result))
			{
				return result;
			}
			return 0;
		}
		public void SetParam(string name, float value)
		{
			if (param2ValueDic.TryGetValue(name, out float result))
			{
				if (result == value)
				{
					return;
				}
			}
			param2ValueDic[name] = value;
		}
		public bool IsPlaying(string name, int layer)
		{
			var stateInfo = GetStateInfo(name, layer);
			var runtimeState = GetRuntimeState(stateInfo);
			return animancer.IsPlaying(runtimeState.Transition);
		}
		public void CrossFade(string name, int layer, float fade_duration = 0.2f, bool is_playing_restart = false)
		{
			var isPlaying = IsPlaying(name, layer);
			var stateInfo = GetStateInfo(name, layer);
			var runtimeState = GetRuntimeState(stateInfo);
			if (!isPlaying || !is_playing_restart)
			{
				var aState = animancer.Layers[layer].Play(runtimeState.Transition, fade_duration);
				aState.Speed = stateInfo.Speed;
				runtimeState.Set(aState);
			}
			layer2RuntimeStateDic[layer] = runtimeState;
		}
		//暂时不暂停AnimancerComponent
		public void Stop(int layer)
		{
			layer2RuntimeStateDic[layer] = null;
		}
		private AnimancerRuntimeState GetRuntimeState(AnimancerStateInfo state_info)
		{
			if (!state2TransitionDic.TryGetValue(state_info, out AnimancerRuntimeState runtimeState))
			{
				runtimeState = CreateState(this, state_info.Motion);
				state2TransitionDic[state_info] = runtimeState;
			}
			return runtimeState;
		}
		private AnimancerStateInfo GetStateInfo(string name, int layer)
		{
			if (!animName2HashDic.TryGetValue(name, out int hash))
			{
				hash = Animator.StringToHash(name);
				animName2HashDic[name] = hash;
			}
			return animancerInfo.GetState(hash, layer);
		}

		public static AnimancerRuntimeState CreateState(AnimancerManager mgr, AnimancerMotionInfo motion_info)
		{
			AnimancerRuntimeState runtimeState = null;
			switch (motion_info)
			{
				case var c when c is AnimancerClipInfo clipInfo:
					runtimeState = new ClipRuntimeState();
					break;
				case var c when c is AnimancerBlendTreeInfo btInfo:
					if (btInfo.Type == EBlendTreeType.Simple1D)
					{
						runtimeState = new Simple1DRuntimeState();
					}
					else
					{
						runtimeState = new Free2DRuntimeState();
					}
					break;
			}
			runtimeState.Init(mgr, motion_info);
			return runtimeState;
		}
	}
}