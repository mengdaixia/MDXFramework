using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Fight
{
	public enum EAttrUpdateType
	{
		Base,
		TempAdd,
		TempMul
	}
	//三个属性对应计算方法---(基础值 + 加值) * (1 + 乘值)
	//任何一个属性变更都要重新计算属性
	public class UnitAttr
	{
		//当前实际值
		private Dictionary<int, float> realAttrDic = new Dictionary<int, float>();
		//基础值
		private Dictionary<int, float> attrDic = new Dictionary<int, float>();
		//加值
		private Dictionary<int, float> attrAddDic = new Dictionary<int, float>();
		//乘值
		private Dictionary<int, float> attrMulDic = new Dictionary<int, float>();
		private static Dictionary<int, (int Normal, int Max)> normalMaxDic;

		public Action<int, float> OnAttrChanged;
		public float this[EAttrType type]
		{
			get
			{
				return GetAttrValue(realAttrDic, (int)type);
			}
		}
		public float this[int id]
		{
			get
			{
				return GetAttrValue(realAttrDic, id);
			}
		}
		public float this[EAttrType type1, EAttrType type2]
		{
			get
			{
				return this[(int)type1, (int)type2];
			}
		}
		public float this[int type1, int type2]
		{
			get
			{
				var value1 = GetAttrValue(realAttrDic, type1);
				var value2 = GetAttrValue(realAttrDic, type2);

				if (value2 <= 0)
				{
					return 0;
				}
				return value1 / value2;
			}
		}

		public void Init(Dictionary<int, float> base_attr)
		{
			OnAttrChanged = null;
			attrDic.Clear();
			realAttrDic.Clear();
			attrAddDic.Clear();
			attrMulDic.Clear();

			if (base_attr != null)
			{
				foreach (var item in base_attr)
				{
					attrDic[item.Key] = item.Value;
				}
			}

			//var lst = CSVCharacterAttr.GetAllLst();
			//foreach (var attr in lst)
			//{
			//	if (attr.iDefaultValue != 0 && !attrDic.ContainsKey(attr.iAttrId))
			//	{
			//		attrDic[attr.iAttrId] = attr.iDefaultValue;
			//	}
			//}

			//if (normalMaxDic == null)
			//{
			//	normalMaxDic = new Dictionary<int, (int Normal, int Max)>();
			//	foreach (var item in lst)
			//	{
			//		if (item.iAttrMaxId > 0)
			//		{
			//			AddNormal2MaxDic(item.iAttrId, item.iAttrMaxId);
			//		}
			//	}
			//}

			SyncAllAttr();
		}

		private void AddNormal2MaxDic(int attr_normal, int attr_max)
		{
			normalMaxDic[attr_normal] = (attr_normal, attr_max);
			normalMaxDic[attr_max] = (attr_normal, attr_max);
		}

		public void SyncAllAttr()
		{
			foreach (var item in attrDic)
			{
				SyncAttrValue(item.Key);
			}
		}
		public bool IsMax(int type)
		{
			if (type > 1000)
			{
				type = type - 1000;
			}
			if (normalMaxDic.TryGetValue(type, out (int Normal, int Max) result))
			{
				if (this[result.Max] == this[result.Normal])
				{
					return true;
				}
			}
			return false;
		}
		public void UpdateValue(EAttrType type, float delta, EAttrUpdateType attr_update_type)
		{
			UpdateValue((int)type, delta, attr_update_type);
		}
		public void UpdateValue(int type, float delta, EAttrUpdateType attr_update_type)
		{
			switch (attr_update_type)
			{
				case EAttrUpdateType.Base:
					UpdateValue(type, delta);
					break;
				case EAttrUpdateType.TempAdd:
					UpdateAddValue(type, delta);
					break;
				case EAttrUpdateType.TempMul:
					UpdateMulValue(type, delta);
					break;
			}
		}
		public void UpdateValue(int type, float delta)
		{
			if ((int)type > 1000)
			{
				type = type - 1000;
				if (normalMaxDic.TryGetValue(type, out (int Normal, int Max) result))
				{
					delta = realAttrDic[result.Max] * delta;
				}
				else
				{
					delta = realAttrDic[type] * delta;
				}
			}

			if (attrDic.TryGetValue(type, out float value))
			{
				var newValue = value + delta;
				attrDic[type] = newValue;
				if (normalMaxDic.TryGetValue(type, out (int Normal, int Max) target))
				{
					attrDic[target.Normal] = Mathf.Clamp(GetAttrValue(attrDic, target.Normal), 0, GetAttrValue(attrDic, target.Max));
				}
				else
				{
					attrDic[type] = Mathf.Clamp(newValue, 0, float.MaxValue);
				}
			}
			else
			{
				attrDic[type] = delta;
			}
			if (value != attrDic[type])
			{
				SyncAttrValue(type);
			}
		}

		public void UpdateAddValue(int type, float delta)
		{
			float newValue = delta;

			if (attrAddDic.TryGetValue(type, out float value))
				newValue = value + delta;

			attrAddDic[type] = newValue;

			if (value != newValue)
			{
				SyncAttrValue(type);
			}
		}

		public void UpdateMulValue(int type, float delta)
		{
			if (attrMulDic.TryGetValue(type, out float value))
			{
				var newValue = value + delta;
				attrMulDic[type] = newValue;
				if (value != newValue)
				{
					SyncAttrValue(type);
				}
			}
			else
			{
				attrMulDic[type] = delta;
				SyncAttrValue(type);
			}
		}

		private void SyncAttrValue(int type)
		{
			var baseValue = GetAttrValue(attrDic, type);
			var addValue = GetAttrValue(attrAddDic, type);
			var mulValue = GetAttrValue(attrMulDic, type);
			var realValue = (baseValue + addValue) * (1 + mulValue);
			realAttrDic[type] = realValue;
			OnAttrChanged?.Invoke(type, realValue);
		}

		private float GetAttrValue(Dictionary<int, float> dic, int type)
		{
			if (dic == null)
			{
				return 0;
			}
			if (dic.TryGetValue(type, out float value))
			{
				return value;
			}
			return 0;
		}
	}
}