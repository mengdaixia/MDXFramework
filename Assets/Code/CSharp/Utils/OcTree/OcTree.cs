using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Code.OcTree
{
	public interface ISceneObject
	{
		public Vector3 Position { get; }
	}
	public class OcTree
	{
		private Dictionary<ISceneObject, OcTreeNode> obj2NodeDic = new Dictionary<ISceneObject, OcTreeNode>();
		private OcTreeNode root;
		private List<ISceneObject> tempObjLst = new List<ISceneObject>();
		private List<ISceneObject> addObjLst = new List<ISceneObject>();
		private List<ISceneObject> disableObjLst = new List<ISceneObject>();
		private HashSet<ISceneObject> outOcObjLst = new HashSet<ISceneObject>();

		public Action OnUpdateTickAc;
		public OcTree(Vector3 min_pos, Vector3 max_pos)
		{
			root = new OcTreeNode((min_pos, max_pos), this);
		}
		public void Update()
		{
			DisableNodes();
			FindOutNodes();
			AddNodes();
			UpdateOcSpace();
			OnUpdateTick();
		}
		private void AddNodes()
		{
			if (addObjLst.Count > 0)
			{
				tempObjLst.AddRange(addObjLst);
			}
			if (outOcObjLst.Count > 0)
			{
				tempObjLst.AddRange(outOcObjLst);
			}
			if (tempObjLst.Count > 0)
			{
				for (int i = 0; i < tempObjLst.Count; i++)
				{
					var obj = tempObjLst[i];
					var pos = obj.Position;
					var node = root;
					if (node.Area.Contains(pos))
					{
						while (node != null)
						{
							var nextNode = node.GetContainsNode(pos);
							if (nextNode == null)
							{
								AddObj2Node(obj, node);
								break;
							}
							node = nextNode;
						}
						outOcObjLst.Remove(obj);
					}
					else
					{
						outOcObjLst.Add(obj);
					}
				}
			}
		}
		private void UpdateOcSpace()
		{
			root.SpaceCheck();
		}
		private void FindOutNodes()
		{
			root.FindOutNodes(tempObjLst);
			RemoveNode2Obj(tempObjLst, false);
		}
		private void DisableNodes()
		{
			RemoveNode2Obj(disableObjLst);
		}
		private void RemoveNode2Obj(List<ISceneObject> obj_lst, bool clear = true)
		{
			var count = obj_lst.Count;
			if (count > 0)
			{
				for (int i = 0; i < count; i++)
				{
					var obj = tempObjLst[i];
					if (obj2NodeDic.TryGetValue(obj, out OcTreeNode node))
					{
						node.RemoveObject(obj);
						obj2NodeDic.Remove(obj);
					}
				}
				if (clear)
				{
					obj_lst.Clear();
				}
			}
		}
		private void OnUpdateTick()
		{
			OnUpdateTickAc?.Invoke();
		}
		public void AddObj2Node(ISceneObject obj, OcTreeNode node)
		{
			node.AddObj(obj);
			obj2NodeDic[obj] = node;
		}
	}
}