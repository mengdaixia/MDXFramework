using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class PathDefine
{
	public static readonly string UPDATE_RES_URL;
	public static readonly string TEMP_WRITE_PATH;
	public static readonly string READ_WRITE_PATH;
	public static readonly string LOCAL_PATH;
	public const string AB_FILES_INFO_NAME = "AssetBundlesInfo.bytes";
	public const string AB_FILES_UPDATE_INFO_NAME = "AssetBundlesUpdateInfo.bytes";

	static PathDefine()
	{
		LOCAL_PATH = Application.streamingAssetsPath + "/Data/";
		TEMP_WRITE_PATH = Application.temporaryCachePath + "/Data/";
		READ_WRITE_PATH = Application.persistentDataPath + "/Data/";
		UPDATE_RES_URL = Application.dataPath.Replace("Assets", "Res/ABUpdate/");
	}
}