using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Networking;

namespace Game.Task
{
	public class DownloadTask : TaskBase
	{
		private string getUrl;
		private string filePath;
		private UnityWebRequestAsyncOperation requestOp;
		private UnityWebRequest request;
		private DateTime startTime;

		public Action<EErrorCode, string, string> OnDownload;

		public void Set(string url, string file_path)
		{
			getUrl = url;
			filePath = file_path;
			TaskState = ETaskState.Start;
		}
		protected override void OnStart()
		{
			request = UnityWebRequest.Get(getUrl);
			var downLoader = new MyDownloader(filePath);
			request.disposeDownloadHandlerOnDispose = true;
			request.downloadHandler = downLoader;
			request.SetRequestHeader("Range", "bytes=" + downLoader.DownloadLength + "-");
			requestOp = request.SendWebRequest();
			startTime = DateTime.Now;
		}
		protected override void OnUpdate()
		{
			if (requestOp.isDone)
			{
				TaskState = ETaskState.Done;
				if (request.responseCode == 200)
				{
					OnDownload?.Invoke(EErrorCode.SUCCESS, "", filePath);
				}
				else
				{
					OnDownload?.Invoke(EErrorCode.ERROR, "", filePath);
				}
			}
			else
			{
				var now = DateTime.Now;
				var ts = now - startTime;
				if (ts.TotalMilliseconds > 5000)
				{
					TaskState = ETaskState.Done;
					OnDownload?.Invoke(EErrorCode.TIMEOUT, "", filePath);
				}
			}
		}
		protected override void OnDone()
		{

		}
	}
}