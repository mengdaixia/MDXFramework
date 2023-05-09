using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Code.OcTree
{
	//要视情况看需不需要改递归为广度的遍历
	public class OcTreeNode
	{
		private OcTree ocTree;
		private OcTreeNodeArea area;
		private OcTreeNode[] childArr = null;
		private List<ISceneObject> objLst = new List<ISceneObject>();
		public OcTreeNodeArea Area => area;
		public OcTreeNode((Vector3, Vector3) area_pos, OcTree tree)
		{
			area = new OcTreeNodeArea(area_pos.Item1, area_pos.Item2);
			ocTree = tree;
		}
		//找出不在此空间的节点
		public void FindOutNodes(List<ISceneObject> obj_lst)
		{
			if (childArr != null)
			{
				var len = childArr.Length;
				for (int i = 0; i < len; i++)
				{
					var child = childArr[i];
					child.FindOutNodes(obj_lst);
				}
			}
			else
			{
				FindOutObjects(obj_lst);
			}
		}
		private void FindOutObjects(List<ISceneObject> obj_lst)
		{
			var count = objLst.Count;
			for (int i = 0; i < count; i++)
			{
				var obj = objLst[i];
				if (!area.Contains(obj.Position))
				{
					obj_lst.Add(obj);
				}
			}
		}
		public void AddObj(ISceneObject obj)
		{
			objLst.Add(obj);
		}
		public void RemoveObject(ISceneObject obj)
		{
			objLst.Remove(obj);
		}
		public OcTreeNode GetContainsNode(Vector3 pos)
		{
			if (childArr == null)
			{
				if (area.Contains(pos))
				{
					return this;
				}
			}
			else
			{
				for (int i = 0; i < childArr.Length; i++)
				{
					var node = childArr[i];

					bool nodeOnSameXSide = !((area.Center.x <= node.area.Center.x) ^ (area.Center.x <= pos.x));
					bool nodeOnSameYSide = !((area.Center.y >= node.area.Center.y) ^ (area.Center.y <= pos.y));
					bool nodeOnSameZSide = !((area.Center.z >= node.area.Center.z) ^ (area.Center.z <= pos.z));

					if (nodeOnSameXSide && nodeOnSameYSide && nodeOnSameZSide)
					{
						return node;
					}
				}
			}
			return null;
		}
		public void SpaceCheck()
		{
			if (objLst.Count > OcTreeDefine.MAX_OBJ_COUNT)
			{
				if (area.CanSlit())
				{
					Split();
				}
			}
			var objCount = GetChildObjCount();
			if (objCount <= OcTreeDefine.MAX_OBJ_COUNT)
			{
				Merge();
			}
			if (childArr != null)
			{
				var len = childArr.Length;
				for (int i = 0; i < len; i++)
				{
					var child = childArr[i];
					child.SpaceCheck();
				}
			}
		}
		private void Split()
		{
			var center = area.Center;
			var min = area.MinPos;
			var max = area.MaxPos;
			var halfDelta = center - min;
			var halfX = Vector3.right * halfDelta.x;
			var halfY = Vector3.up * halfDelta.y;
			var halfZ = Vector3.forward * halfDelta.z;


			childArr = new OcTreeNode[8];
			childArr[0] = new OcTreeNode((min, center), ocTree);
			childArr[1] = new OcTreeNode((min + halfX, center + halfX), ocTree);
			childArr[2] = new OcTreeNode((min + halfZ, center + halfZ), ocTree);
			childArr[3] = new OcTreeNode((min + halfX + halfZ, center + halfX + halfZ), ocTree);
			childArr[4] = new OcTreeNode((min + halfY, center + halfY), ocTree);
			childArr[5] = new OcTreeNode((min + halfX + halfY, center + halfX + halfY), ocTree);
			childArr[6] = new OcTreeNode((min + halfZ + halfY, center + halfZ + halfY), ocTree);
			childArr[7] = new OcTreeNode((min + halfX + halfZ + halfY, center + halfX + halfZ + halfY), ocTree);

			for (int i = 0; i < objLst.Count; i++)
			{
				var obj = objLst[i];
				var pos = obj.Position;
				var node = GetContainsNode(pos);
				ocTree.AddObj2Node(obj, node);
			}
			objLst.Clear();
		}
		private void Merge()
		{
			var len = childArr.Length;
			for (int i = 0; i < len; i++)
			{
				var child = childArr[i];
				child.MergeObjs(this);
			}
			childArr = null;
		}
		public void MergeObjs(OcTreeNode merge_node)
		{
			foreach (var item in objLst)
			{
				ocTree.AddObj2Node(item, merge_node);
			}
			if (childArr != null)
			{
				var len = childArr.Length;
				for (int i = 0; i < len; i++)
				{
					var child = childArr[i];
					child.MergeObjs(merge_node);
				}
			}
		}
		private int GetChildObjCount()
		{
			if (childArr == null)
			{
				return 0;
			}
			var count = 0;
			foreach (var node in childArr )
			{
				count += node.GetChildObjCount();
			}
			return count;
		}
	}
}