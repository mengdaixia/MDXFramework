using Code.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.UI
{
	public struct EPanelIdCompare : IEqualityComparer<EPanelId>
	{
		public bool Equals(EPanelId x, EPanelId y)
		{
			return x == y;
		}

		public int GetHashCode(EPanelId obj)
		{
			return (int)obj;
		}
	}
}