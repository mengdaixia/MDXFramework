using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static partial class Utility
{
	public static class Time
	{
		private static StringBuilder sbHelper = new StringBuilder();
		public static float deltaTime { get; private set; }
		public static float timeScale { get; private set; } = 1;

		public static void SetDeltaTime(float delta_time)
		{
			deltaTime = delta_time * timeScale;
		}
		public static void SetTimeScale(float time_scale)
		{
			timeScale = time_scale;
		}
		public static string GetCTime(float time)
		{
			sbHelper.Clear();
			var ts = TimeSpan.FromSeconds(time);
			if (ts.Hours > 0)
			{
				sbHelper.Append(ts.Hours).Append("时");
			}
			sbHelper.Append(ts.Minutes).Append("分");
			sbHelper.Append(ts.Seconds).Append("秒");
			return sbHelper.ToString();
		}
	}
}