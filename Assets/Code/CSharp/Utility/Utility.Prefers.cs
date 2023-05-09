using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static partial class Utility
{
	public static class Prefers
	{
		public static void SetInt(string key, int value)
		{
			PlayerPrefs.SetInt(key, value);
		}
		public static void SetFloat(string key, float value)
		{
			PlayerPrefs.SetFloat(key, value);
		}
		public static void SetString(string key, string value)
		{
			PlayerPrefs.SetString(key, value);
		}
		public static int GetInt(string key, int default_value = 0)
		{
			return PlayerPrefs.GetInt(key, default_value);
		}
		public static float GetFloat(string key, float default_value = 0)
		{
			return PlayerPrefs.GetFloat(key, default_value);
		}
		public static string GetString(string key, string default_value = "")
		{
			return PlayerPrefs.GetString(key, default_value);
		}
	}
}