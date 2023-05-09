using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Fight
{
	public enum EAnimParamType
	{
		Float,
		Bool,
	}
	public enum EAnimPlayType
	{
		StateName,
		Trigger,
		Bool,
	}
	public class AnimManager : SubManager
	{
		private Animator animator;
		private Dictionary<string, int> stateName2HashDic = new Dictionary<string, int>();
		private Dictionary<int, float> layer2WeightDic = new Dictionary<int, float>();
		private Dictionary<string, (int Id, float Value)> animParamDic = new Dictionary<string, (int Id, float Value)>();

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
			animator = owner.ModelGo.GetComponent<Animator>();
			if (animator != null)
			{
				for (int i = 0; i < animator.parameterCount; i++)
				{
					var param = animator.GetParameter(i);
					switch (param.type)
					{
						case AnimatorControllerParameterType.Float:
							animParamDic[param.name] = (param.nameHash, param.defaultFloat);
							break;
						case AnimatorControllerParameterType.Int:
							animParamDic[param.name] = (param.nameHash, param.defaultInt);
							break;
						case AnimatorControllerParameterType.Bool:
							animParamDic[param.name] = (param.nameHash, param.defaultBool ? 1 : 0);
							break;
						case AnimatorControllerParameterType.Trigger:
							break;
					}
				}
			}
		}
		public override void Clear()
		{
			stateName2HashDic.Clear();
			layer2WeightDic.Clear();
			animParamDic.Clear();
		}
		public void SetAnimParam(string name, EAnimParamType type, float value)
		{
			var result = GetAnimatorParamValue(name);
			if (result.Value == value || result.Id == -1)
			{
				return;
			}
			switch (type)
			{
				case EAnimParamType.Float:
					animator.SetFloat(result.Id, value);
					break;
				case EAnimParamType.Bool:
					animator.SetBool(result.Id, value == 1);
					break;
			}
			animParamDic[name] = (result.Id, value);
		}
		public void SetTrigger(string name)
		{
			var paramHash = GetAnimatorParamHash(name);
			animator.SetTrigger(paramHash);
		}
		public void ResetTrigger(string name)
		{
			var paramHash = GetAnimatorParamHash(name);
			animator.ResetTrigger(paramHash);
		}
		public void SetBool(string name, bool value)
		{
			SetAnimParam(name, EAnimParamType.Bool, value ? 1 : 0);
		}
		public void SetFloat(string name, float value)
		{
			SetAnimParam(name, EAnimParamType.Float, value);
		}
		public void CrossFade(string name, float transition_time = 0)
		{
			var paramHash = GetAnimatorParamHash(name);
			animator.CrossFade(paramHash, transition_time);
		}
		private int GetLayerIndexByAnimationName(string animationName)
		{
			var paramHash = GetAnimatorParamHash(animationName);
			int layerCount = animator.layerCount;
			int layerIndex = 0;
			for (int i = 0; i < layerCount; i++)
			{
				if (animator.HasState(i, paramHash))
				{
					layerIndex = i;
					break;
				}
			}

			return layerIndex;
		}

		public bool IsPlaySameAnimation(string animationName)
		{
			int layerIndex = GetLayerIndexByAnimationName(animationName);
			AnimatorStateInfo animatorInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
			if (animatorInfo.IsName(animationName))
			{
				return true;
			}
			return false;
		}
		private (int Id, float Value) GetAnimatorParamValue(string name)
		{
			if (animParamDic.TryGetValue(name, out (int Id, float Value) result))
			{
				return result;
			}
			return (-1, 0);
		}
		private int GetAnimatorParamHash(string name)
		{
			if (!stateName2HashDic.TryGetValue(name, out int hash))
			{
				hash = Animator.StringToHash(name);
				stateName2HashDic[name] = hash;
			}
			return hash;
		}
		public void SetLayerWeight(int layer_index, float weight)
		{
			var index = layer_index;
			if (!layer2WeightDic.TryGetValue(index, out float curr_weight))
			{
				curr_weight = -1;
				layer2WeightDic[index] = curr_weight;
			}
			if (curr_weight != weight)
			{
				animator.SetLayerWeight(index, weight);
				layer2WeightDic[index] = weight;
			}
		}
		public bool TryGetStateTime(string state, int layer_index, out float time)
		{
			time = -1;
			var stateHash = GetAnimatorParamHash(state);
			var stateInfo = animator.GetCurrentAnimatorStateInfo(layer_index);
			if (stateInfo.shortNameHash == stateHash)
			{
				time = stateInfo.normalizedTime * stateInfo.length;
				return true;
			}
			stateInfo = animator.GetNextAnimatorStateInfo(layer_index);
			if (stateInfo.shortNameHash == stateHash)
			{
				time = stateInfo.normalizedTime * stateInfo.length;
				return true;
			}
			return false;
		}
	}
}