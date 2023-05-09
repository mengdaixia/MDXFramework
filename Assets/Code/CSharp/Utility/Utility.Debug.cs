using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

public static partial class Utility
{
	public static class DebugX
	{
		//[Conditional("Debug")]
		public static void LogError(object obj)
		{
			UnityEngine.Debug.LogError(obj);
		}
	}
}