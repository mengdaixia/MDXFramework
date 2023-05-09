using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.Task
{
	public class MyDownloader : DownloadHandlerScript
	{
		private FileStream fileStream;
		private string targetFilePath;
		private string tempFilePath;
		public long DownloadLength { get; private set; } = 0;
		public long TotolLength { get; private set; }
		public MyDownloader(string path) : base(new byte[1024 * 4])
		{
			var dirName = Path.GetDirectoryName(path);
			if (!Directory.Exists(dirName))
			{
				Directory.CreateDirectory(dirName);
			}
			targetFilePath = path;
			tempFilePath = path + ".temp";
			fileStream = new FileStream(tempFilePath, FileMode.OpenOrCreate);
			DownloadLength += fileStream.Length;
		}
		protected override void ReceiveContentLengthHeader(ulong contentLength)
		{
			TotolLength = (long)contentLength;
		}
		protected override bool ReceiveData(byte[] data, int dataLength)
		{
			if (data == null || data.Length == 0)
			{
				Debug.LogError("下载失败->>>" + targetFilePath);
				return false;
			}
			fileStream.Write(data, 0, dataLength);
			DownloadLength += dataLength;
			return true;
		}
		protected override void CompleteContent()
		{
			fileStream.Close();
			fileStream.Dispose();
			Utility.FileIO.DeleteFile(targetFilePath);
			File.Move(tempFilePath, targetFilePath);
			Utility.FileIO.DeleteFile(tempFilePath);

			Debug.LogError("下载成功->>>" + targetFilePath);
		}
	}
}