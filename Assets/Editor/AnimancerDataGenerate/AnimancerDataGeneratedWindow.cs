using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using Code.Fight.Animancer;
using Newtonsoft.Json;

public class AnimancerDataGeneratedWindow : EditorBaseWindow
{
	[MenuItem("Tools/Animancer Generated")]
	private static void Generated()
	{
		GetWindow<AnimancerDataGeneratedWindow>();
	}
	private AnimatorController animator;
	private void OnGUI()
	{
		GUILayout.BeginVertical();
		animator = EditorGUILayout.ObjectField(animator, typeof(AnimatorController), true) as AnimatorController;
		if (GUILayout.Button("生成状态机数据"))
		{
			StartG();
		}
		GUILayout.EndVertical();
	}
	private AnimancerInfo animancerInfo;
	private void StartG()
	{
		if (animator == null)
		{
			return;
		}
		var layers = animator.layers;
		animancerInfo = new AnimancerInfo();
		var layerInfos = new List<AnimancerLayerInfo>();
		animancerInfo.LayerInfoLst = layerInfos;

		for (int i = 0; i < animator.parameters.Length; i++)
		{
			var param = animator.parameters[i];
			switch (param.type)
			{
				case AnimatorControllerParameterType.Float:
					animancerInfo.ParamDic[param.name] = param.defaultFloat;
					break;
				case AnimatorControllerParameterType.Int:
					animancerInfo.ParamDic[param.name] = param.defaultInt;
					break;
				case AnimatorControllerParameterType.Bool:
					animancerInfo.ParamDic[param.name] = param.defaultBool ? 1 : 0;
					break;
				case AnimatorControllerParameterType.Trigger:
					break;
			}
		}

		for (int i = 0; i < layers.Length; i++)
		{
			var layerInfo = new AnimancerLayerInfo();
			layerInfos.Add(layerInfo);
			var layer = layers[i];
			var avatar = layer.avatarMask;
			layerInfo.IkPass = layer.iKPass;
			layerInfo.Mode = (EAnimatorLayerBlendingMode)(int)layer.blendingMode;
			layerInfo.AvatarMask = EUtility.Asset.GetAssetPathNoAsset(avatar);
			CollectState(layerInfo, layer.stateMachine);
		}
		var data = Utility.Json.Serialize(animancerInfo, Utility.Json.IgnoreLoopSetting);
		Utility.FileIO.Write(Application.dataPath + "/Res/AnimData/" + animator.name + ".bytes", data);
		AssetDatabase.Refresh();
	}
	private void CollectState(AnimancerLayerInfo layer, AnimatorStateMachine machine)
	{
		foreach (var item in machine.anyStateTransitions)
		{
			layer.AddStateTrans(0, item);
		}
		foreach (var childStates in machine.states)
		{
			var state = childStates.state;
			layer.AddState(state);
			foreach (var item in state.transitions)
			{
				layer.AddStateTrans(state.nameHash, item);
			}
		}
		foreach (var childMachine in machine.stateMachines)
		{
			CollectState(layer, childMachine.stateMachine);
		}
	}
}