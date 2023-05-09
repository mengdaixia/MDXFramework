using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PassUIEvent : MonoBehaviour, IPointerClickHandler, IDragHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
	public void OnPointerDown(PointerEventData eventData)
	{
		PassEvent(eventData, ExecuteEvents.pointerDownHandler);
	}
	public void OnPointerUp(PointerEventData eventData)
	{
		PassEvent(eventData, ExecuteEvents.pointerUpHandler);
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		PassEvent(eventData, ExecuteEvents.pointerExitHandler);
	}
	public void OnPointerClick(PointerEventData eventData)
	{
		PassEvent(eventData, ExecuteEvents.submitHandler);
		PassEvent(eventData, ExecuteEvents.pointerClickHandler);
	}
	public void OnDrag(PointerEventData eventData)
	{
		PassEvent(eventData, ExecuteEvents.dragHandler);
	}
	/// <summary>
	/// 把鼠标点击事件传递到下层 UI 及 GameObject
	/// </summary>
	private void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function)
		where T : IEventSystemHandler
	{
		var results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(data, results);
		var current = gameObject;
		foreach (var t in results)
		{
			if (t.gameObject == current)
			{
				continue;
			}
			ExecuteEvents.Execute(t.gameObject, data, function);
			break;
			//RaycastAll后ugui会自己排序，如果你只想响应透下去的最近的一个响应，这里ExecuteEvents.Execute后直接break就行。
		}
	}
}