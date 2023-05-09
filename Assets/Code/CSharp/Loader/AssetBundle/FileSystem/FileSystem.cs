using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.FileSystem
{
	public abstract class FileSystem
	{
		public abstract void Read(string path);
		public abstract void Write(string path);
	}
}