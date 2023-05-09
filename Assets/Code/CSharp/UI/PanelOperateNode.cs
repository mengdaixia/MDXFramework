using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.UI
{
	public interface IOperateNode : IPoolObj
	{
		public EPanelId PanelId { get; set; }
		public EOperateType OperateType { get; set; }
	}
	public enum EOperateType
	{
		Show,
		Close
	}
	public class PanelOperatrShowNode : IOperateNode
	{
		public EPanelId PanelId { get; set; }
		public EOperateType OperateType { get; set; }
		public UIParam Param;
		public void Clear()
		{
			Param = null;
		}
	}
	public class PanelOperatrCloseNode : IOperateNode
	{
		public EPanelId PanelId { get; set; }
		public EOperateType OperateType { get; set; }
		public void Clear()
		{

		}
	}
	public abstract class UIParam
	{

	}
}