using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static partial class Utility
{
	public static class Random
	{
		public static int Range(int min, int max)
		{
			return UnityEngine.Random.Range(min, max);
		}
	}
}