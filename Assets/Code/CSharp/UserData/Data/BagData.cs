using System;
using System.Collections.Generic;
using UnityEngine;

public class BagData : UserDataBase
{
	private Dictionary<int, long> itemsDic = new Dictionary<int, long>();

	public void AddItem(int id, long add_count)
	{
		if (itemsDic.TryGetValue(id, out long count))
		{
			add_count += count;
		}
		itemsDic[id] = add_count;
		UseDataEvent.OnBagItemsChanged?.Invoke(id, add_count);
	}
	public long this[int id]
	{
		get
		{
			if (itemsDic.TryGetValue(id, out long count))
			{
				return count;
			}
			return 0;
		}
	}
}