using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImportTables.Utils
{
	public sealed class BytesWrite
	{
		private byte[] buffer;
		private int position;

		public int Capcity => buffer.Length;
		public BytesWrite(int capcity)
		{
			buffer = new byte[capcity];
		}
		public void SetPosition(int pos)
		{
			position = pos;
		}
		public void Write(short value)
		{
			EnsureCapcity(2);
			FastBitConvert.GetBytes(buffer, ref position, value);
		}
		public void Write(int value)
		{
			EnsureCapcity(4);
			FastBitConvert.GetBytes(buffer, ref position, value);
		}
		public void Write(float value)
		{
			EnsureCapcity(4);
			FastBitConvert.GetBytes(buffer, ref position, value);
		}
		public void Write(double value)
		{
			EnsureCapcity(8);
		}
		public void Write(string value)
		{
			//UTF8Encoding会直接操作指针，并不会申请Char[],可以直接用
			var bytesCount = Encoding.UTF8.GetByteCount(value);
			EnsureCapcity(4 + bytesCount);
			FastBitConvert.GetBytes(buffer, ref position, value);
		}
		public byte[] GetBuffer()
		{
			if (position == 0)
			{
				return new byte[0];
			}
			var realLen = position;
			//可用对象池
			var bytes = new byte[realLen];
			Buffer.BlockCopy(buffer, 0, bytes, 0, realLen);
			return bytes;
		}
		private void EnsureCapcity(int cap)
		{
			if (position + cap > Capcity)
			{
				byte[] newBytes = null;
				if (position + cap < Capcity * 2)
				{
					newBytes = new byte[Capcity * 2];
				}
				else
				{
					newBytes = new byte[Capcity + cap];
				}
				Buffer.BlockCopy(buffer, 0, newBytes, 0, buffer.Length);
				buffer = newBytes;
			}
		}
	}
}