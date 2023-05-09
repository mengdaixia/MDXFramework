using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

public class AssetBundlesInfo
{
	public string[] AllAssetBundles;
	public Dictionary<string, string[]> Bundle2DenpendenceDic = new Dictionary<string, string[]>();

	public string[] GetAllDependencies(string bundle_name)
	{
		return Bundle2DenpendenceDic[bundle_name];
	}

	public void Read(string path)
	{
		using (var fs = File.Open(path, FileMode.Open))
		{
			using (var br = new BinaryReader(fs))
			{
				var length = br.ReadInt32();
				AllAssetBundles = new string[length];
				for (int i = 0; i < length; i++)
				{
					AllAssetBundles[i] = br.ReadString();
					var dpLength = br.ReadInt32();
					var dpArr = new string[dpLength];
					Bundle2DenpendenceDic[AllAssetBundles[i]] = dpArr;
					for (int j = 0; j < dpLength; j++)
					{
						dpArr[j] = br.ReadString();
					}
				}
			}
		}
	}
	public void Write(string path)
	{
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		using (var fs = File.Create(path))
		{
			using (var bw = new BinaryWriter(fs))
			{
				bw.Write(AllAssetBundles.Length);
				foreach (var item in Bundle2DenpendenceDic)
				{
					bw.Write(item.Key);
					var arr = item.Value;
					bw.Write(arr.Length);
					for (int i = 0; i < arr.Length; i++)
					{
						bw.Write(arr[i]);
					}
				}
			}
		}
	}
}
public class AssetBundlesUpdateInfo
{
	public string CurrPath;
	public Dictionary<string, (string MD5, long Length)> Bundle2FileInfoDic = new Dictionary<string, (string MD5, long Length)>();
	public void Read(string path)
	{
		CurrPath = path;
		using (var fs = File.Open(path, FileMode.Open))
		{
			using (var br = new BinaryReader(fs))
			{
				var count = br.ReadInt32();
				for (int i = 0; i < count; i++)
				{
					var bundleName = br.ReadString();
					var bundleMd5 = br.ReadString();
					var bundleLength = br.ReadInt64();
					Bundle2FileInfoDic[bundleName] = (bundleMd5, bundleLength);
				}
			}
		}
	}
	public void Write(string path)
	{
		if (File.Exists(path))
		{
			File.Delete(path);
		}
		using (var fs = File.Create(path))
		{
			using (var bw = new BinaryWriter(fs))
			{
				bw.Write(Bundle2FileInfoDic.Count);
				foreach (var item in Bundle2FileInfoDic)
				{
					bw.Write(item.Key);
					bw.Write(item.Value.MD5);
					bw.Write(item.Value.Length);
				}
			}
		}
	}
}