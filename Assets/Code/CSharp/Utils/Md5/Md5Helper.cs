using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using MD5 = XguanjiaMsg.MD5;
using MD5CryptoServiceProvider = XguanjiaMsg.MD5CryptoServiceProvider;

public class Md5Helper
{
	static string CalcMd5StringFromHash(byte[] bytes)
	{
		string ret = "";
		foreach (byte b in bytes) {
			ret += Convert.ToString (b, 16);
		}

		return ret;
	}

	public static string GetFileMd5 (string path)
	{
		if (!File.Exists (path)) {
			return "";
		}

		FileStream stream = File.OpenRead (path);

		MD5 md5 = new MD5CryptoServiceProvider ();
		byte[] result = md5.ComputeHash (stream);
		stream.Close ();

		return CalcMd5StringFromHash (result);
	}

	public static string GetMd5 (byte[] bytes)
	{
		MD5 md5 = new MD5CryptoServiceProvider ();
		byte[] result = md5.ComputeHash (bytes);

		return CalcMd5StringFromHash (result);
	}

	public static string GetMd5 (string source)
	{
		return MD5.GetMd5String (source);
	}
}