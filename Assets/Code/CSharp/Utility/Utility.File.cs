using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public static partial class Utility
{
	public unsafe static class FileIO
	{
		public static void CopyDirectory(string sourcePath, string destinationPath, string suffix = "", Func<string, bool> onFilter = null)
		{
			if (onFilter != null && onFilter(sourcePath))
			{
				return;
			}

			if (!Directory.Exists(destinationPath))
			{
				Directory.CreateDirectory(destinationPath);
			}

			foreach (string file in Directory.GetFileSystemEntries(sourcePath))
			{
				if (File.Exists(file))
				{
					FileInfo info = new FileInfo(file);
					if (string.IsNullOrEmpty(suffix) || file.ToLower().EndsWith(suffix.ToLower()))
					{
						string destName = Path.Combine(destinationPath, info.Name);
						if (onFilter == null || !onFilter(file))
						{
							File.Copy(file, destName, true);
						}
					}
				}

				if (Directory.Exists(file))
				{
					DirectoryInfo info = new DirectoryInfo(file);
					string destName = Path.Combine(destinationPath, info.Name);
					CopyDirectory(file, destName, suffix, onFilter);
				}
			}
		}
		public static void DeleteFile(string path)
		{
			try
			{
				if (File.Exists(path))
				{
					File.Delete(path);
				}
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message);
			}
		}
		public static void CreateDirectory(string path)
		{
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}
		public static void DeleteDirectory(string path)
		{
			try
			{
				if (Directory.Exists(path))
				{
					DirectoryInfo di = new DirectoryInfo(path);
					di.Delete(true);
				}
			}
			catch (Exception e)
			{
				Debug.LogError(e.Message);
			}
		}
		public static void Write(string path, string text)
		{
			var dir = Path.GetDirectoryName(path);
			if (!Directory.Exists(dir))
			{
				Directory.CreateDirectory(dir);
			}
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			using (var fs = File.Create(path))
			{
				using (var sw = new StreamWriter(fs))
				{
					sw.Write(text);
				}
			}
		}
	}
}