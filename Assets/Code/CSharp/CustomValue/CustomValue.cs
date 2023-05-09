using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.CustomValue
{
	public enum EValueType
	{
		Int,
		Float,
	}
	public class CustomValue
	{
		private int value;
		private EValueType valueType;

		public void Init(string data, EValueType type)
		{
			valueType = type;
			switch (valueType)
			{
				case EValueType.Int:
					SetValue(int.Parse(data));
					break;
				case EValueType.Float:
					SetValue(float.Parse(data));
					break;
			}
		}
		public void SetValue(int val)
		{
			value = val;
		}
		public unsafe void SetValue(float val)
		{
			value = *(int*)&val;
		}
		public void AddValue(int val, out int result)
		{
			result = val + value;
		}
		public unsafe void AddValue(float val, out float result)
		{
			fixed (int* ptr = &value)
			{
				result = val + (*(float*)ptr);
			}
		}
		public void GetValue(out int result)
		{
			result = value;
		}
		public unsafe void GetValue(out float result)
		{
			fixed (int* ptr = &value)
			{
				result = *(float*)ptr;
			}
		}
	}
}