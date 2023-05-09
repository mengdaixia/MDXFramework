using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
	public class UILoopGrid<T> : UIComponent where T : UIComponent, new()
	{
		private LoopGrid loopGrid;
		private Dictionary<GameObject, T> go2ItemDic = new Dictionary<GameObject, T>();
		private Action<T, int> OnUpdateFunc;
		private Action<T, int> OnClickFunc;

		protected override void OnInit()
		{
			loopGrid = uiObj.GetComponent<LoopGrid>();
			loopGrid.OnUpdateFunc += OnUpdateHandler;
			loopGrid.OnClickFunc += OnClickHandler;
		}
		protected override void OnClose()
		{
			loopGrid.OnUpdateFunc -= OnUpdateHandler;
			loopGrid.OnClickFunc -= OnClickHandler;
		}
		public void SetUpdateFunc(Action<T, int> func)
		{
			OnUpdateFunc = func;
		}
		public void SetClickFunc(Action<T, int> func)
		{
			OnClickFunc = func;
		}
		private void OnUpdateHandler(GameObject go, int index)
		{
			if (!go2ItemDic.TryGetValue(go, out T item))
			{
				item = CreateUI<T>(go);
				go2ItemDic[go] = item;
			}
			OnUpdateFunc?.Invoke(item, index);
		}
		private void OnClickHandler(GameObject go, int index)
		{
			if (!go2ItemDic.TryGetValue(go, out T item))
			{
				item = CreateUI<T>(go);
				go2ItemDic[go] = item;
			}
			OnClickFunc?.Invoke(item, index);
		}
	}
}