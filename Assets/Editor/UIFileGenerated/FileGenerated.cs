using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class FileGenerated
{
	private static Dictionary<Type, string> type2NamePre = new Dictionary<Type, string>();
	private static string fileTemplate;
	private static Dictionary<int, string> replaceStrDic = new Dictionary<int, string>();
	private static StringBuilder writeTempSb = new StringBuilder();
	private static Dictionary<int, Dictionary<EFileWriteType, StringBuilder>> writeCacheDic = new Dictionary<int, Dictionary<EFileWriteType, StringBuilder>>();

	static FileGenerated()
	{
		type2NamePre[typeof(GameObject)] = "go_";
		type2NamePre[typeof(Transform)] = "trans_";
		type2NamePre[typeof(Camera)] = "camera_";

		type2NamePre[typeof(Image)] = "img_";
		type2NamePre[typeof(Text)] = "lbl_";
		type2NamePre[typeof(Image)] = "img_";
		type2NamePre[typeof(ScrollRect)] = "scrollrect_";
		type2NamePre[typeof(Slider)] = "slider_";
		type2NamePre[typeof(RectTransform)] = "rtrans_";
		type2NamePre[typeof(Canvas)] = "canvas_";
		type2NamePre[typeof(CanvasGroup)] = "canvasgroup_";
		type2NamePre[typeof(GraphicRaycaster)] = "graphicray";

		fileTemplate = File.ReadAllText(Application.dataPath + "/Editor/UIFileGenerated/UIFileTemplate.txt");
	}
	private static StringBuilder GetFileCahche(int id, EFileWriteType type)
	{
		Dictionary<EFileWriteType, StringBuilder> dic;
		if (!writeCacheDic.TryGetValue(id, out dic))
		{
			dic = new Dictionary<EFileWriteType, StringBuilder>(new EFileWriteTypeComparer());
			writeCacheDic[id] = dic;
		}
		if (!dic.TryGetValue(type, out StringBuilder cache))
		{
			cache = new StringBuilder();
			dic[type] = cache;
		}
		return cache;
	}
	private static string GetReplaceStr(EFileWriteType type)
	{
		if (!replaceStrDic.TryGetValue((int)type, out string replaceStr))
		{
			replaceStr = "/*" + type.ToString() + "*/";
			replaceStrDic[(int)type] = replaceStr;
		}
		return replaceStr;
	}
	public static void WirteCache(int id, EFileWriteType type, string value)
	{
		var replaceStr = GetReplaceStr(type);
		var cache = GetFileCahche(id, type);
		cache.Append(value);
		switch (type)
		{
			case EFileWriteType.Field:
			case EFileWriteType.Property:
				cache.Append("\n		");
				break;
		}
	}
	public static string GetFieldName(string field_name)
	{
		writeTempSb.Append("m_").Append(field_name);
		var result = writeTempSb.ToString();
		writeTempSb.Clear();
		return result;
	}
	public static string GetPropertyName(string field_name, Type type)
	{
		writeTempSb.Append(GetPreName(type)).Append(field_name);
		var result = writeTempSb.ToString();
		writeTempSb.Clear();
		return result;
	}
	public static string GetPreName(Type type)
	{
		if (type2NamePre.TryGetValue(type, out string preName))
		{
			return preName;
		}
		return "un_";
	}
	public static void OnGenerated()
	{
		foreach (var file in writeCacheDic)
		{
			writeTempSb.Clear();
			var fileInfo = file.Value;
			var fileName = fileInfo[EFileWriteType.Class];
			writeTempSb.Append(fileTemplate);
			foreach (var item in fileInfo)
			{
				var type = item.Key;
				var content = item.Value;
				var replaceStr = GetReplaceStr(type);
				writeTempSb.Replace(replaceStr, content.ToString());
			}

			var filePath = Application.dataPath + "/Code/CSharp/UI/Gnerated/" + fileName + ".cs";
			Utility.FileIO.Write(filePath, writeTempSb.ToString());
		}

		writeCacheDic.Clear();
		AssetDatabase.Refresh();
	}
}
public enum EFileWriteType
{
	Class,
	BaseClass,
	Field,
	Property
}
public struct EFileWriteTypeComparer : IEqualityComparer<EFileWriteType>
{
	public bool Equals(EFileWriteType x, EFileWriteType y)
	{
		return (int)x == (int)y;
	}

	public int GetHashCode(EFileWriteType obj)
	{
		return (int)obj;
	}
}