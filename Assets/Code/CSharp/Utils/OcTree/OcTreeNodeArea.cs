using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.OcTree
{
	public struct OcTreeNodeArea
	{
		public Vector3 MinPos;
		public Vector3 MaxPos;
		public Vector3 Center => MinPos + MaxPos / 2;
		public OcTreeNodeArea(Vector3 min, Vector3 max)
		{
			MinPos = min;
			MaxPos = max;
		}
		public bool Contains(Vector3 pos)
		{
			return pos.x >= MinPos.x && pos.y >= MinPos.y && pos.z >= MinPos.z && pos.x < MaxPos.x && pos.y < MaxPos.y && pos.z < MaxPos.z;
		}
		public int GetIndex(Vector3 pos)
		{
			var xLength = (MaxPos.x - MinPos.x);
			var yLength = (MaxPos.y - MinPos.y);
			var zLength = (MaxPos.z - MinPos.z);

			var xPercent = (pos.x - MinPos.x) / xLength;
			var yPercent = (pos.y - MinPos.y) / yLength;
			var zPercent = (pos.z - MinPos.z) / zLength;

			var index = 0;
			index += xPercent >= 0.5f ? 1 : 0;
			index += yPercent >= 0.5f ? 2 : 0;
			index += zPercent >= 0.5f ? 4 : 0;
			return index;
		}
		public bool CanSlit()
		{
			var xLength = (MaxPos.x - MinPos.x);
			var yLength = (MaxPos.y - MinPos.y);
			var zLength = (MaxPos.z - MinPos.z);
			return xLength >= OcTreeDefine.MAX_SIDE_LENGTH && yLength >= OcTreeDefine.MAX_SIDE_LENGTH && zLength >= OcTreeDefine.MAX_SIDE_LENGTH;
		}
	}
}