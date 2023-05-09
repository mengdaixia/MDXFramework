using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Task
{
	public enum EUpdateResState
	{
		None,
		GetResFilesInfo,
		GetResFiles,
		DecreptResFiles,
		Finish,
	}
	public class UpdateResTask : TaskBase
	{
		private string resUrl;
		private EUpdateResState currState;

		//处理跨版本问题
		private AssetBundlesUpdateInfo tempAbUpdateInfo = null;
		private AssetBundlesUpdateInfo newAbUpdateInfo = null;
		private AssetBundlesUpdateInfo oldAbUpdateInfo = null;

		private Dictionary<string, long> updateFilesDic = new Dictionary<string, long>();
		private Queue<DownloadTask> updateFileTaskQueue = new Queue<DownloadTask>();
		private List<string> faildFiledLst = new List<string>();
		private Dictionary<string, string> tempPathDic = new Dictionary<string, string>();

		private int currGetTaskCount = 0;
		private const int MAX_GET_TASK_COUNT = 5;
		public UpdateResTask()
		{
			TaskState = ETaskState.Start;
		}
		protected override void OnStart()
		{
			resUrl = PathDefine.UPDATE_RES_URL;
			currState = EUpdateResState.GetResFilesInfo;
		}
		protected override void OnUpdate()
		{
			switch (currState)
			{
				case EUpdateResState.GetResFilesInfo:
					GetFilesInfo();
					break;
				case EUpdateResState.GetResFiles:
					GetFiles();
					break;
				case EUpdateResState.DecreptResFiles:
					Decrept();
					break;
				case EUpdateResState.Finish:
					TaskState = ETaskState.Done;
					break;
			}
		}
		private void GetFilesInfo()
		{
			var fileName = PathDefine.AB_FILES_UPDATE_INFO_NAME;
			var tempUpdatePath = PathDefine.TEMP_WRITE_PATH + fileName;
			if (File.Exists(tempUpdatePath))
			{
				tempAbUpdateInfo = new AssetBundlesUpdateInfo();
				tempAbUpdateInfo.Read(tempUpdatePath);
			}
			var task = new DownloadTask();
			task.Set(resUrl + fileName, tempUpdatePath);
			task.OnDownload = OnGetResFilesInfoHandler;
			TaskManager.Instance.AddTask(task);
			currState = EUpdateResState.GetResFiles;
		}
		private void GetFiles()
		{
			if (newAbUpdateInfo == null)
			{
				return;
			}
			if (updateFileTaskQueue.Count > 0)
			{
				if (currGetTaskCount < MAX_GET_TASK_COUNT)
				{
					var task = updateFileTaskQueue.Dequeue();
					TaskManager.Instance.AddTask(task);
					currGetTaskCount++;
				}
				else
				{
					return;
				}
			}
			if (currGetTaskCount > 0)
			{
				return;
			}
			if (updateFilesDic.Count > 0)
			{
				//TODO:下载失败文件
				return;
			}
			Utility.FileIO.CopyDirectory(PathDefine.TEMP_WRITE_PATH, PathDefine.READ_WRITE_PATH);
			Utility.FileIO.DeleteDirectory(PathDefine.TEMP_WRITE_PATH);
			currState = EUpdateResState.DecreptResFiles;
		}
		private void Decrept()
		{
			currState = EUpdateResState.Finish;
		}
		private void OnGetResFilesInfoHandler(EErrorCode code, string msg, string file_path)
		{
			if (code == EErrorCode.SUCCESS)
			{
				currState = EUpdateResState.GetResFiles;
				Debug.LogError("下载更新文件成功");

				currGetTaskCount = 0;
				tempPathDic.Clear();
				updateFileTaskQueue.Clear();
				updateFilesDic.Clear();
				var fileUpdateName = PathDefine.AB_FILES_UPDATE_INFO_NAME;
				newAbUpdateInfo = new AssetBundlesUpdateInfo();
				newAbUpdateInfo.Read(PathDefine.TEMP_WRITE_PATH + fileUpdateName);

				oldAbUpdateInfo = new AssetBundlesUpdateInfo();
				var oldPath = PathDefine.READ_WRITE_PATH + fileUpdateName;
				bool isOldLocal = false;
				if (!File.Exists(oldPath))
				{
					isOldLocal = true;
					oldPath = PathDefine.LOCAL_PATH + fileUpdateName;
				}
				oldAbUpdateInfo.Read(oldPath);

				var newDic = newAbUpdateInfo.Bundle2FileInfoDic;
				var oldDic = oldAbUpdateInfo.Bundle2FileInfoDic;
				var tempDic = tempAbUpdateInfo.Bundle2FileInfoDic;
				foreach (var item in newDic)
				{
					var bPath = item.Key;
					var newMd5 = item.Value.MD5;
					if (oldDic.TryGetValue(bPath, out (string Md5, long Length) result) && result.Md5 != newMd5)
					{
						var tempPath = PathDefine.TEMP_WRITE_PATH + bPath;
						if (!File.Exists(tempPath))
						{
							updateFilesDic[bPath] = item.Value.Length;
						}
						else
						{
							if (tempAbUpdateInfo != null)
							{
								if (tempDic.TryGetValue(bPath, out (string Md5, long Length) tempResult) && tempResult.Md5 != newMd5)
								{
									updateFilesDic[bPath] = item.Value.Length;
								}
							}
						}
					}
				}
				if (updateFilesDic.Count != 0)
				{
					updateFilesDic[PathDefine.AB_FILES_INFO_NAME] = 0;
				}
				if (!isOldLocal)
				{
					foreach (var item in oldDic)
					{
						if (!newDic.TryGetValue(item.Key, out _))
						{
							var path = PathDefine.READ_WRITE_PATH + item.Key;
							Utility.FileIO.DeleteFile(path);
						}
					}
				}
				//删除多余的数据
				if (tempDic.Count > 0)
				{
					foreach (var item in tempDic)
					{
						if (!newDic.TryGetValue(item.Key, out _))
						{
							var path = PathDefine.TEMP_WRITE_PATH + item.Key;
							Utility.FileIO.DeleteFile(path);
						}
					}
				}
				foreach (var item in updateFilesDic)
				{
					var fileName = item.Key;
					var path = PathDefine.TEMP_WRITE_PATH + fileName;
					tempPathDic[path] = fileName;
					Utility.FileIO.DeleteFile(path);
					Debug.LogError("需要更新文件->>>" + fileName);
					var task = new DownloadTask();
					updateFileTaskQueue.Enqueue(task);
					task.Set(resUrl + fileName, path);
					task.OnDownload = OnGetFilesHandler;
				}
			}
			else
			{
				Debug.LogError("发生错误:" + msg + "---" + code);
			}
		}
		private void OnGetFilesHandler(EErrorCode code, string msg, string file_path)
		{
			currGetTaskCount--;
			switch (code)
			{
				case EErrorCode.SUCCESS:
					if (tempPathDic.TryGetValue(file_path, out string file_name))
					{
						tempPathDic.Remove(file_path);
						updateFilesDic.Remove(file_name);
					}
					break;
				case EErrorCode.ERROR:
					faildFiledLst.Add(file_path);
					//弹出提示
					break;
				case EErrorCode.TIMEOUT:
					faildFiledLst.Add(file_path);
					//弹出提示
					break;
				default:
					break;
			}
		}
	}
}