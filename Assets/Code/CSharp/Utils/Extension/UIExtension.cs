using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public static class UIExtension
{
	public static void SetWidth(this RectTransform rect, float width)
	{
		var delta = rect.sizeDelta;
		rect.sizeDelta = new Vector2(width, delta.y);
	}
	public static void SetHeight(this RectTransform rect, float height)
	{
		var delta = rect.sizeDelta;
		rect.sizeDelta = new Vector2(delta.x, height);
	}
	public static void SetWidthNHeight(this RectTransform rect, float width, float height)
	{
		SetWidth(rect, width);
		SetHeight(rect, height);
	}
	public static float Width(this RectTransform rect)
	{
		return rect.sizeDelta.x;
	}
	public static float Height(this RectTransform rect)
	{
		return rect.sizeDelta.y;
	}
	public static void SetPositionX(this RectTransform rect, float x)
	{
		var pos = rect.anchoredPosition;
		rect.anchoredPosition = new Vector2(x, pos.y);
	}
	public static void SetPositionY(this RectTransform rect, float y)
	{
		var pos = rect.anchoredPosition;
		rect.anchoredPosition = new Vector2(pos.x, y);
	}
}