using System;
using System.Collections.Generic;
using UnityEngine;

public static class SplitExtensions
{
	public static Dictionary<int, float> ToIFDic(this string data, char s1 = ';', char s2 = ':')
	{
		var datas = data.Split(s1);
		Dictionary<int, float> result = new Dictionary<int, float>();
		for (int i = 0; i < datas.Length; i++)
		{
			var param = datas[i].Split(s2);
			result[int.Parse(param[0])] = float.Parse(param[1]);
		}
		return result;
	}
	public static Dictionary<int, int> ToIIDic(this string data, char s1 = ';', char s2 = ':')
	{
		var datas = data.Split(s1);
		Dictionary<int, int> result = new Dictionary<int, int>();
		for (int i = 0; i < datas.Length; i++)
		{
			var param = datas[i].Split(s2);
			result[int.Parse(param[0])] = int.Parse(param[1]);
		}
		return result;
	}
	public static List<int> ToIList(this string data, char s1 = ';')
	{
		var datas = data.Split(s1);
		List<int> result = new List<int>();
		for (int i = 0; i < datas.Length; i++)
		{
			result.Add(int.Parse(datas[i]));
		}
		return result;
	}
	public static List<(int, int)> ToIIList(this string data, char s1 = ';', char s2 = ':')
	{
		var datas = data.Split(s1);
		List<(int, int)> result = new List<(int, int)>();
		for (int i = 0; i < datas.Length; i++)
		{
			result.Add(datas[i].ToIITuple(s2));
		}
		return result;
	}
	public static List<(int, int)> ToIIList(this List<string> data, char s1 = ':')
	{
		List<(int, int)> result = new List<(int, int)>();
		for (int i = 0; i < data.Count; i++)
		{
			result.Add(data[i].ToIITuple(s1));
		}
		return result;
	}
	public static (int, int) ToIITuple(this string data, char s1 = ';')
	{
		if (data.Length == 0)
		{
			return (0, 0);
		}
		var datas = data.Split(s1);
		var item1 = int.Parse(datas[0]);
		var item2 = int.Parse(datas[1]);
		return (item1, item2);
	}
	public static (int, float) ToIFTuple(this string data, char s1 = ';')
	{
		if (data.Length == 0)
		{
			return (0, 0);
		}
		var datas = data.Split(s1);
		var item1 = int.Parse(datas[0]);
		var item2 = float.Parse(datas[1]);
		return (item1, item2);
	}
	public static List<(int, float)> ToIFTupleList(this List<string> data, char s1 = ':')
	{
		List<(int, float)> result = new List<(int, float)>();
		for (int i = 0; i < data.Count; i++)
		{
			result.Add(data[i].ToIFTuple(s1));
		}
		return result;
	}
	public static Dictionary<int, float> ToIFDic(this List<string> data, char s1 = ':')
	{
		Dictionary<int, float> result = new Dictionary<int, float>();
		for (int i = 0; i < data.Count; i++)
		{
			var ifTuple = data[i].ToIFTuple(s1);
			result[ifTuple.Item1] = ifTuple.Item2;
		}
		return result;
	}
}