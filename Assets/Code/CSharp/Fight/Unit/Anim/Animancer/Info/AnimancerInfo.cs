using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Fight.Animancer
{
	public class AnimancerInfo
	{
		public List<AnimancerLayerInfo> LayerInfoLst = new List<AnimancerLayerInfo>();
		public Dictionary<string, float> ParamDic = new Dictionary<string, float>();
		public AnimancerStateInfo GetState(int name_hash, int layer)
		{
			if (LayerInfoLst.Count > layer)
			{
				if (LayerInfoLst[layer].StateDic.TryGetValue(name_hash, out AnimancerStateInfo state))
				{
					return state;
				}
			}
			return null;
		}
	}
}