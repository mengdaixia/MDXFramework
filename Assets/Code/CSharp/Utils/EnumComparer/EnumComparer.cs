using System;
using System.Collections.Generic;
using UnityEngine;

public struct EGlobalParamTypeComparer : IEqualityComparer<EGlobalParamType>
{
	public bool Equals(EGlobalParamType x, EGlobalParamType y)
	{
		return (int)x == (int)y;
	}
	public int GetHashCode(EGlobalParamType obj)
	{
		return (int)obj;
	}
}