using Game.FileSystem;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildAb : EditorWindow
{
	//AB导出路径
	private const string OutPutPath = "Res/ABExport/";
	//AB备份路径
	private const string CopyPath = "Res/ABCopy/";
	//AB更新路径
	private const string UpdatePath = "Res/ABUpdate/";

	private static AssetBundlesInfo bundlesInfo;
	private static AssetBundlesUpdateInfo bundlesUpdateInfo;

	[MenuItem("Tools/Build/ABBase")]
	static void AB()
	{
		bundlesInfo = new AssetBundlesInfo();
		bundlesUpdateInfo = new AssetBundlesUpdateInfo();
		var abPath = Application.dataPath.Replace("Assets", OutPutPath);
		var copyPath = Application.dataPath.Replace("Assets", CopyPath);

		ExportAB(abPath);
		GeneratedABFileInfos(abPath);
		CopyAB(abPath, copyPath);
		bundlesInfo.Write(copyPath + PathDefine.AB_FILES_INFO_NAME);
		bundlesUpdateInfo.Write(copyPath + PathDefine.AB_FILES_UPDATE_INFO_NAME);
		Copy2StreamingAssets(copyPath);

		AssetDatabase.Refresh();
	}

	[MenuItem("Tools/Build/ABUpdate")]
	static void ABUpdate()
	{
		bundlesInfo = new AssetBundlesInfo();
		bundlesUpdateInfo = new AssetBundlesUpdateInfo();
		var abPath = Application.dataPath.Replace("Assets", OutPutPath);
		var copyPath = Application.dataPath.Replace("Assets", UpdatePath);

		ExportAB(abPath);
		GeneratedABFileInfos(abPath);
		CopyAB(abPath, copyPath);
		bundlesInfo.Write(copyPath + PathDefine.AB_FILES_INFO_NAME);
		bundlesUpdateInfo.Write(copyPath + PathDefine.AB_FILES_UPDATE_INFO_NAME);

		AssetDatabase.Refresh();
	}

	private static void GenaratedMD5File(string path)
	{
		var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
		List<(string, string, long)> lst = new List<(string, string, long)>();
		for (int i = 0; i < files.Length; i++)
		{
			if (Path.GetExtension(files[i]).Length == 0)
			{
				var bytes = File.ReadAllBytes(files[i]);
				var fileInfo = new FileInfo(files[i]);
				var bundlePath = files[i].Replace(path, "").Replace("\\", "/");
				var bundleLength = fileInfo.Length;
				var md5 = Md5Helper.GetMd5(bytes);
				bundlesUpdateInfo.Bundle2FileInfoDic[bundlePath] = (md5, bundleLength);
			}
		}
	}
	//导出AB
	private static void ExportAB(string path)
	{
		Utility.FileIO.DeleteDirectory(path);
		Utility.FileIO.CreateDirectory(path);
		BuildAssetBundleOptions options = BuildAssetBundleOptions.DeterministicAssetBundle;
		options |= BuildAssetBundleOptions.ChunkBasedCompression;
		BuildPipeline.BuildAssetBundles(path, options, EditorUserBuildSettings.activeBuildTarget);
	}
	private static void CopyAB(string path, string copy_path)
	{
		Utility.FileIO.DeleteDirectory(copy_path);
		Utility.FileIO.CreateDirectory(copy_path);
		Utility.FileIO.CopyDirectory(path, copy_path, "", (c) => { return c.Contains(".manifest"); });
		GenaratedMD5File(copy_path);

		ReWriteFile(copy_path);
	}
	private static void Copy2StreamingAssets(string path)
	{
		var basePaht = Application.streamingAssetsPath + "/Data";
		Utility.FileIO.DeleteDirectory(basePaht);
		Utility.FileIO.CopyDirectory(path, basePaht, "", (c) => { return c.Contains("manifest"); });
	}
	private static void GeneratedABFileInfos(string path)
	{
		var dirName = path.Split('/');
		var ambPath = path + "/" + dirName[dirName.Length - 2];
		var ab = AssetBundle.LoadFromFile(ambPath);
		var assetbundleMainfest = ab.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
		var arr = assetbundleMainfest.GetAllAssetBundles();
		bundlesInfo.AllAssetBundles = arr;
		for (int i = 0; i < arr.Length; i++)
		{
			bundlesInfo.Bundle2DenpendenceDic[arr[i]] = assetbundleMainfest.GetAllDependencies(arr[i]);
		}
		ab.Unload(true);
		Utility.FileIO.DeleteFile(ambPath);
	}
	private static void ReWriteFile(string path)
	{
		foreach (string file in Directory.GetFileSystemEntries(path))
		{
			if (File.Exists(file))
			{
				var ex = Path.GetExtension(file);
				if (string.IsNullOrEmpty(ex))
				{
					var fs = FileSystemManager.Instance.Get<VFileSystem>(file);
					fs.Write(file);
				}
			}
			if (Directory.Exists(file))
			{
				DirectoryInfo info = new DirectoryInfo(file);
				string destPath = Path.Combine(path, info.Name);
				ReWriteFile(destPath);
			}
		}
	}
}