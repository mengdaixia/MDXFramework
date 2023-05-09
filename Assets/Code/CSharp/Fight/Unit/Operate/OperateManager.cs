using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Fight.Operate
{
	public class OperateManager : SubManager
	{
		private Dictionary<EBtnType, EBtnStateType> btnStateDic = new Dictionary<EBtnType, EBtnStateType>(new EBtnTypeCompare());
		private Action updateFunc;

		public Dictionary<EBtnType, EBtnStateType> CurrStateDic => btnStateDic;
		protected override void OnInit()
		{
			updateFunc = updateFunc == null ? UpdateOperate : updateFunc;
			UpdateEvent.OnLateUpdate += updateFunc;
		}
		protected override void OnDestroy(bool is_dead)
		{
			UpdateEvent.OnLateUpdate -= updateFunc;
		}
		//可能需要考虑时序问题(input->operate=>skill)
		private void UpdateOperate()
		{
			if (InputManager.Instance.IsDirty)
			{
				btnStateDic.Clear();
				var states = InputManager.Instance.GetBtnStates();
				foreach (var item in states)
				{
					btnStateDic[item.Key] = item.Value;
				}
			}
		}
		public Vector2 GetInputXY()
		{
			return Vector2.zero;
		}
	}
}