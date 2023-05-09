using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Utils
{
	public class ListDic<T, W>
	{
		private List<T> keys;
		private List<W> values;

		public int Count => keys.Count;
		public (T Key, W Value) this[int index]
		{
			get
			{
				if (index >= keys.Count)
				{
					throw new IndexOutOfRangeException();
				}
				return (keys[index], values[index]);
			}
		}
		public ListDic()
		{
			keys = new List<T>();
			values = new List<W>();
		}
		public ListDic(int capcity)
		{
			keys = new List<T>(capcity);
			values = new List<W>(capcity);
		}
		public bool ContainsKey(T item)
		{
			return keys.Contains(item);
		}
		public void RemoveAt(int index)
		{
			keys.RemoveAt(index);
			values.RemoveAt(index);
		}
		public void Add(T key, W value)
		{
			keys.Add(key);
			values.Add(value);
		}
		public void Clear()
		{
			keys.Clear();
			values.Clear();
		}
	}
}