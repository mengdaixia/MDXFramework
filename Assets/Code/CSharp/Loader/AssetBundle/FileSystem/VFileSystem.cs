using ImportTables.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.FileSystem
{
	public class VFileSystem : FileSystem
	{
		public int DataOffset { get; private set; } = 0;
		public string FilePath { get; private set; }
		public override void Read(string path)
		{
			FilePath = path;
			using (var fs = new FileStream(path, FileMode.Open))
			{
				for (int i = 0; i < 4; i++)
				{
					DataOffset += fs.ReadByte() << (8 * i);
				}
			}
			DataOffset += 4;
		}
		public override void Write(string path)
		{
			var offset = 10;
			byte[] bytes = File.ReadAllBytes(path);
			var newBytes = new byte[4 + offset + bytes.Length];
			int pos = 0;
			FastBitConvert.GetBytes(newBytes, ref pos, offset);
			pos += offset;
			Buffer.BlockCopy(bytes, 0, newBytes, pos, bytes.Length);
			File.WriteAllBytes(path, newBytes);
		}
	}
}