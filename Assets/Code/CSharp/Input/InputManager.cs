using Code.Mgr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Fight
{
	public enum EBtnStateType
	{
		None = -1,
		Down = 0,
		Up = 1,
		Click = 2,
	}
	public enum EBtnType
	{
		WASD = 0,
		LeftShift = 1,
	}
	public struct EBtnTypeCompare : IEqualityComparer<EBtnType>
	{
		public bool Equals(EBtnType x, EBtnType y)
		{
			return ((int)x) == ((int)y);
		}

		public int GetHashCode(EBtnType obj)
		{
			return (int)obj;
		}
	}
	public class InputManager : Singleton<InputManager>, IRunningMgr, IUpdate
	{
		private Dictionary<EBtnType, EBtnStateType> btnStateDic = new Dictionary<EBtnType, EBtnStateType>(new EBtnTypeCompare());
		public bool IsDirty { get; private set; }
		public void SetState(int id, EBtnStateType type)
		{
			btnStateDic[(EBtnType)id] = type;
			IsDirty = true;
		}
		public Dictionary<EBtnType, EBtnStateType> GetBtnStates()
		{
			return btnStateDic;
		}
		public void Init()
		{

		}
		public void Update()
		{
			if (IsKey(KeyCode.A) || IsKey(KeyCode.S) || IsKey(KeyCode.D) || IsKey(KeyCode.W))
			{
				IsDirty = true;
				btnStateDic[EBtnType.WASD] = EBtnStateType.Down;
			}
			else
			{
				IsDirty = true;
				btnStateDic[EBtnType.WASD] = EBtnStateType.Up;
			}
			if (IsKey(KeyCode.LeftShift))
			{
				IsDirty = true;
				btnStateDic[EBtnType.LeftShift] = EBtnStateType.Down;
			}
			if (IsKeyUp(KeyCode.LeftShift))
			{
				IsDirty = true;
				btnStateDic[EBtnType.LeftShift] = EBtnStateType.Up;
			}
		}
		public void Destroy()
		{

		}
		private bool IsKey(KeyCode keycode)
		{
			return Input.GetKey(keycode);
		}
		private bool IsKeyUp(KeyCode keycode)
		{
			return Input.GetKeyUp(keycode);
		}
		public unsafe void ResetState()
		{
			Span<EBtnType> btnArr = stackalloc EBtnType[5];
			int index = 0;
			foreach (var item in btnStateDic)
			{
				if (item.Value == EBtnStateType.Click)
				{
					btnArr[index++] = item.Key;
				}
			}
			for (int i = 0; i < index; i++)
			{
				btnStateDic[btnArr[i]] = EBtnStateType.Up;
			}
		}
	}
}