using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static partial class Utility
{
	public static class Json
	{
		public static JsonSerializerSettings IgnoreLoopSetting = new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore, TypeNameHandling = TypeNameHandling.All };
		public static T Deserialize<T>(string data, JsonSerializerSettings settings = null)
		{
			return JsonConvert.DeserializeObject<T>(data, settings);
		}
		public static string Serialize(object data, JsonSerializerSettings settings = null)
		{
			return JsonConvert.SerializeObject(data, settings);
		}
	}
}