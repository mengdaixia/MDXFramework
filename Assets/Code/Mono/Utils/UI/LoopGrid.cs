using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Animancer.Examples.Events;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//默认是从左上开始的，暂时不做其他处理了
//GridLayout必须默认enable == false;
public class LoopGrid : MonoBehaviour
{
	private int minCount;
	private ScrollRect scrollRect;
	private GridLayoutGroup gridLayoutGroup;

	private float currValue = 1;
	private bool isInit = false;
	//总共几行几列
	private int lCount = 0;
	//实例的总行/列数
	private int iCount = 0;
	//总的数据量
	private int realCount = 0;
	//视图第一个元素对应的起始行/列数
	private int realIndex = 0;

	private int startGoIndex;
	private int endGoIndex;

	private Dictionary<int, GameObject> index2GoDic = new Dictionary<int, GameObject>();
	private Dictionary<GameObject, int> go2DataIndexDic = new Dictionary<GameObject, int>();

	private RectTransform RootRect => scrollRect.transform as RectTransform;
	private RectTransform GridRect => gridLayoutGroup.transform as RectTransform;
	private int ConstraintCount => gridLayoutGroup.constraintCount;
	private RectOffset Padding => gridLayoutGroup.padding;
	private Vector2 CellSize => gridLayoutGroup.cellSize;
	private Vector2 Spacing => gridLayoutGroup.spacing;
	private float LeftTopSpace => scrollRect.horizontal ? Padding.left : Padding.top;
	private float CellSizeXY => scrollRect.horizontal ? CellSize.x : CellSize.y;
	private float SpacingXY => scrollRect.horizontal ? Spacing.x : Spacing.y;

	public Action<GameObject, int> OnClickFunc;
	public Action<GameObject, int> OnUpdateFunc;
	//private void Awake()
	//{
	//	OnUpdateFunc = (go, index) =>
	//	{
	//		go.GetComponentInChildren<TextMeshProUGUI>().text = index.ToString();
	//	};

	//	SetAmount(testCount, 29);
	//}
	public void Init()
	{
		if (isInit)
		{
			return;
		}
		isInit = true;
		scrollRect = transform.parent.GetComponent<ScrollRect>();
		gridLayoutGroup = GetComponent<GridLayoutGroup>();
		if (gridLayoutGroup.enabled)
		{
			Debug.LogError("LoopGrid的GridLayout必须默认为False");
		}
		gridLayoutGroup.enabled = false;
		scrollRect.onValueChanged.AddListener(OnScrollHandler);

		var rootWidth = RootRect.Width();
		var rootHeight = RootRect.Height();
		int hCount = 0;
		int vCount = 0;
		if (scrollRect.horizontal)
		{
			for (float i = Padding.left; i < rootWidth - 0.1f;)
			{
				hCount++;
				i += CellSize.x + Spacing.x;
			}
			vCount = gridLayoutGroup.constraintCount;
			minCount = vCount * (hCount + 1);
		}
		else
		{
			for (float i = Padding.top; i < rootHeight - 0.1f;)
			{
				vCount++;
				i += CellSize.y + Spacing.y;
			}
			hCount = gridLayoutGroup.constraintCount;
			minCount = hCount * (vCount + 1);
		}
		//if (gridLayoutGroup.constraintCount <= 0)
		//{
		//	gridLayoutGroup.constraintCount = !scrollRect.horizontal ? hCount : vCount;
		//}


		if (transform.childCount < minCount)
		{
			for (int i = transform.childCount; i < minCount; i++)
			{
				var go = GameObject.Instantiate(transform.GetChild(0));
				go.transform.parent = transform;
				go.transform.localScale = Vector3.one;
				go.transform.localEulerAngles = Vector3.zero;
			}
		}
		for (int i = 0; i < minCount; i++)
		{
			var rt = transform.GetChild(i) as RectTransform;
			rt.pivot = new Vector2(0, 1);
			rt.anchorMin = new Vector2(0, 1);
			rt.anchorMax = new Vector2(0, 1);
			var rc = i / ConstraintCount;
			var index = i % ConstraintCount;
			if (scrollRect.horizontal)
			{
				rc = rc ^ index;
				index = rc ^ index;
				rc = rc ^ index;
			}
			var x = Padding.left + index * CellSize.x + index * Spacing.x;
			var y = Padding.top + rc * CellSize.y + rc * Spacing.y;
			rt.anchoredPosition = new Vector2(x, -y);
			index2GoDic[i] = rt.gameObject;

			var btn = rt.gameObject.GetComponentInChildren<Button>();
			if (btn != null)
			{
				btn.onClick.AddListener(() => { OnClickFunc(rt.gameObject, go2DataIndexDic[rt.gameObject]); });
			}
		}
	}
	public void SetAmount(int count, int index = 0)
	{
		Init();
		realCount = count;
		UpdateRect();
		var content = transform as RectTransform;
		realIndex = Mathf.Clamp(index, 0, int.MaxValue) / 6;

		//-1把最下面隐藏的一些提上来
		var maxIndex = Mathf.Max(0, lCount - (iCount - 1));
		realIndex = Mathf.Min(maxIndex, realIndex);
		var startIndex = realIndex * ConstraintCount;
		startGoIndex = startIndex % minCount;
		endGoIndex = (startGoIndex + (iCount - 1) * ConstraintCount) % minCount;

		if (scrollRect.horizontal)
		{
			var pos = realIndex * (CellSize.x + Spacing.x);
			content.SetPositionX(pos);
		}
		else
		{
			var pos = realIndex * (CellSize.y + Spacing.y);
			content.SetPositionY(pos);
		}
		for (int i = startGoIndex; i < startGoIndex + minCount; i++)
		{
			var goIndex = i % minCount;
			var go = index2GoDic[goIndex];
			var rt = go.transform as RectTransform;
			var dataIndex = realIndex * ConstraintCount + i - startGoIndex;

			var h = i;
			var v = dataIndex;
			var posX = 0f;
			var posY = 0f;
			if (scrollRect.horizontal)
			{
				h = h ^ v;
				v = h ^ v;
				h = h ^ v;

				posX = Padding.left + h / ConstraintCount * (CellSize.x + Spacing.x);
				posY = -Padding.top - v % ConstraintCount * (CellSize.y + Spacing.y);
			}
			else
			{
				posX = Padding.left + h % ConstraintCount * (CellSize.x + Spacing.x);
				posY = -Padding.top - v / ConstraintCount * (CellSize.y + Spacing.y);
			}
			rt.SetPositionX(posX);
			rt.SetPositionY(posY);
			rt.SetPositionZ();
			UpdateGoData(go, dataIndex);
		}
	}
	public void UpdateCurr(int count)
	{
		realCount = count;
		UpdateRect();
		foreach (var item in go2DataIndexDic)
		{
			var go = item.Key;
			var data_index = item.Value;
			if (data_index < realCount)
			{
				Utility.Go.SetActive(go, true);
				OnUpdateFunc?.Invoke(go, data_index);
			}
			else
			{
				Utility.Go.SetActive(go, false);
			}
		}
	}
	private void UpdateRect()
	{
		var content = transform as RectTransform;
		lCount = realCount / ConstraintCount + (realCount % ConstraintCount == 0 ? 0 : 1);
		iCount = minCount / ConstraintCount;
		var length = LeftTopSpace + lCount * CellSizeXY + (lCount - 1 >= 0 ? lCount - 1 : 0) * SpacingXY;
		length = Mathf.Max(length, scrollRect.horizontal ? RootRect.Width() : RootRect.Height());
		var width = scrollRect.horizontal ? length : content.Width();
		var height = !scrollRect.horizontal ? length : content.Height();
		content.SetWidthNHeight(width, height);
	}
	private void UpdateGoData(GameObject go, int data_index)
	{
		go2DataIndexDic[go] = data_index;
		if (data_index < realCount)
		{
			Utility.Go.SetActive(go, true);
			OnUpdateFunc?.Invoke(go, data_index);
		}
		else
		{
			Utility.Go.SetActive(go, false);
		}
	}
	public void Select(int id)
	{
		foreach (var item in go2DataIndexDic)
		{
			if (item.Value == id && id < realCount)
			{
				OnClickFunc(item.Key, id);
			}
		}
	}
	private void OnScrollHandler(Vector2 value)
	{
		var checkValue = scrollRect.horizontal ? value.x : value.y;
		if (currValue != checkValue)
		{
			var isUp = currValue < checkValue;
			currValue = checkValue;
			if (realCount < minCount)
			{
				return;
			}
			if (isUp)
			{
				if (scrollRect.horizontal)
				{
					if (realIndex + iCount < lCount)
					{
						while (true)
						{
							var startRect = index2GoDic[startGoIndex].transform as RectTransform;
							var startPos = startRect.anchoredPosition.x + CellSize.x;
							var topPos = GridRect.anchoredPosition.x;
							var deltaPos = startPos + topPos;
							if (deltaPos < 0)
							{
								var endRect = index2GoDic[endGoIndex].transform as RectTransform;
								for (int i = startGoIndex; i < startGoIndex + ConstraintCount; i++)
								{
									var go = index2GoDic[i];
									var dataIndex = (realIndex + iCount) * ConstraintCount + i - startGoIndex;
									var rt = go.transform as RectTransform;
									rt.SetPositionX(endRect.anchoredPosition.x + CellSizeXY + SpacingXY);
									UpdateGoData(go, dataIndex);
								}
								startGoIndex = (startGoIndex + ConstraintCount) % minCount;
								endGoIndex = (endGoIndex + ConstraintCount) % minCount;
								realIndex++;
								if (realIndex + iCount >= lCount)
								{
									break;
								}
							}
							else
							{
								break;
							}
						}
					}
				}
				else
				{
					if (realIndex > 0)
					{
						while (true)
						{
							var startRect = index2GoDic[endGoIndex].transform as RectTransform;
							var startPos = startRect.anchoredPosition.y;
							var topPos = GridRect.anchoredPosition.y;
							var deltaPos = topPos + startPos + RootRect.Height();
							if (deltaPos < 0)
							{
								var endRect = index2GoDic[startGoIndex].transform as RectTransform;
								for (int i = endGoIndex; i < endGoIndex + ConstraintCount; i++)
								{
									var go = index2GoDic[i];
									var dataIndex = (realIndex - 1) * ConstraintCount + i - endGoIndex;
									var rt = go.transform as RectTransform;
									rt.SetPositionY(endRect.anchoredPosition.y + CellSizeXY + SpacingXY);
									UpdateGoData(go, dataIndex);
								}
								startGoIndex = (startGoIndex - ConstraintCount + minCount) % minCount;
								endGoIndex = (endGoIndex - ConstraintCount + minCount) % minCount;
								realIndex--;
								if (realIndex == 0)
								{
									break;
								}
							}
							else
							{
								break;
							}
						}
					}
				}
			}
			else
			{
				if (scrollRect.horizontal)
				{
					if (realIndex > 0)
					{
						while (true)
						{
							var endRect = index2GoDic[endGoIndex].transform as RectTransform;
							var startPos = endRect.anchoredPosition.x;
							var topPos = GridRect.anchoredPosition.x;
							var deltaPos = startPos + topPos - RootRect.Width();
							if (deltaPos > 0)
							{
								var startRect = index2GoDic[startGoIndex].transform as RectTransform;
								for (int i = endGoIndex; i < endGoIndex + ConstraintCount; i++)
								{
									var go = index2GoDic[i];
									var dataIndex = (realIndex - 1) * ConstraintCount + i - endGoIndex;
									var rt = go.transform as RectTransform;
									rt.SetPositionX(startRect.anchoredPosition.x - CellSizeXY - SpacingXY);
									UpdateGoData(go, dataIndex);
								}
								startGoIndex = (startGoIndex - ConstraintCount + minCount) % minCount;
								endGoIndex = (endGoIndex - ConstraintCount + minCount) % minCount;
								realIndex--;
								if (realIndex == 0)
								{
									break;
								}
							}
							else
							{
								break;
							}
						}
					}
				}
				else
				{
					if (realIndex + iCount < lCount)
					{
						while (true)
						{
							var startRect = index2GoDic[startGoIndex].transform as RectTransform;
							var startPos = startRect.anchoredPosition.y;
							var topPos = GridRect.anchoredPosition.y;
							var deltaPos = topPos + startPos;
							if (deltaPos > CellSizeXY)
							{
								var endRect = index2GoDic[endGoIndex].transform as RectTransform;
								for (int i = startGoIndex; i < startGoIndex + ConstraintCount; i++)
								{
									var go = index2GoDic[i];
									var dataIndex = (realIndex + iCount) * ConstraintCount + i - startGoIndex;
									var rt = go.transform as RectTransform;
									rt.SetPositionY(endRect.anchoredPosition.y - CellSizeXY - SpacingXY);
									UpdateGoData(go, dataIndex);
								}
								startGoIndex = (startGoIndex + ConstraintCount) % minCount;
								endGoIndex = (endGoIndex + ConstraintCount) % minCount;
								realIndex++;
								if (realIndex + iCount >= lCount)
								{
									return;
								}
							}
							else
							{
								break;
							}
						}
					}
				}
			}
		}
	}
#if UNITY_EDITOR
	private void Reset()
	{
		(transform as RectTransform).pivot = new Vector2(0, 1);
		for (int i = 0; i < transform.childCount; i++)
		{
			var child = transform.GetChild(i);
			var rt = child as RectTransform;
			rt.pivot = new Vector2(0, 1);
			rt.pivot = new Vector2(0, 1);
			rt.anchorMin = new Vector2(0, 1);
			rt.anchorMax = new Vector2(0, 1);
		}
	}
#endif
}