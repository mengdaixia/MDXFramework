using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.FileSystem
{
	public class FileSystemManager
	{
		public static FileSystemManager Instance { get; private set; } = new FileSystemManager();
		private FileSystemManager() { }

		private Dictionary<string, FileSystem> fileDic = new Dictionary<string, FileSystem>();

		public T Get<T>(string path) where T : FileSystem, new()
		{
			FileSystem result = null;
			if (!fileDic.TryGetValue(path, out result))
			{
				result = new T();
				fileDic[path] = result;
				result.Read(path);
			}
			return result as T;
		}
	}
}