using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.CustomValue
{
	public class CustomValueMgr : Singleton<CustomValueMgr>
	{
		private Dictionary<int, CustomValue> valueDic = new Dictionary<int, CustomValue>();
		private CustomValueMgr() { }
		public void SetValue(int id, int value)
		{
			if (valueDic.TryGetValue(id, out CustomValue target))
			{
				target.SetValue(value);
			}
		}
		public void SetValue(int id, float value)
		{
			if (valueDic.TryGetValue(id, out CustomValue target))
			{
				target.SetValue(value);
			}
		}
		public bool TryGetValue(int id, out int value)
		{
			value = 0;
			if (valueDic.TryGetValue(id, out CustomValue result))
			{
				result.GetValue(out value);
				return true;
			}
			return false;
		}
		public bool TryGetValue(int id, out float value)
		{
			value = 0f;
			if (valueDic.TryGetValue(id, out CustomValue result))
			{
				result.GetValue(out value);
				return true;
			}
			return false;
		}
	}
}