using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

/*
 * Introduction:Custom UGUI`s button
 * Creator:Xiaohei Wang
 */
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(CanvasRenderer))]
public class GameButton : Selectable, IPointerClickHandler, ISubmitHandler, IDragHandler
{
	protected GameButton() { }

	public class ButtonClickedPosEvent : UnityEvent<PointerEventData> { }

	[Serializable]
	public class ButtonClickedEvent : UnityEvent
	{

	}
	public class ButtonDragedEvent : UnityEvent<PointerEventData> { }

	[FormerlySerializedAs("onClick")]
	[SerializeField]
	private ButtonClickedEvent m_OnClick = new ButtonClickedEvent();
	[SerializeField]
	private ButtonClickedPosEvent m_OnClickPos = new ButtonClickedPosEvent();
	[FormerlySerializedAs("onLongPress")]
	[SerializeField]
	private ButtonClickedEvent m_onLongPress = new ButtonClickedEvent();

	[FormerlySerializedAs("onDoubleClick")]
	[SerializeField]
	private ButtonClickedEvent m_onDoubleClick = new ButtonClickedEvent();

	[FormerlySerializedAs("onKeepPress")]
	[SerializeField]
	private ButtonClickedEvent m_onKeepPress = new ButtonClickedEvent();
	private ButtonClickedEvent m_onPress = new ButtonClickedEvent();
	private ButtonClickedEvent m_onUp = new ButtonClickedEvent();
	private ButtonDragedEvent m_drag = new ButtonDragedEvent();
	public ButtonClickedEvent onClick
	{
		get { return m_OnClick; }
	}
	public ButtonClickedPosEvent onClickPos
	{
		get { return m_OnClickPos; }
	}
	public ButtonClickedEvent onDoubleClick
	{
		get { return m_onDoubleClick; }
	}
	public ButtonClickedEvent onLongPress
	{
		get { return m_onLongPress; }
	}
	public ButtonClickedEvent onKeepPress
	{
		get { return m_onKeepPress; }
	}
	public ButtonClickedEvent onPress
	{
		get { return m_onPress; }
	}
	public ButtonClickedEvent onUp
	{
		get { return m_onUp; }
	}
	public ButtonDragedEvent onDrag
	{
		get { return m_drag; }
	}

	private bool m_isPress = false;
	private bool m_longPress = false;
	private bool m_isKeepPress = false;
	private DateTime m_currentStartTime;


	private void Press()
	{
		if (!IsActive() || !IsInteractable())
			return;

		UISystemProfilerApi.AddMarker("Button.onClick", this);
		m_OnClick.Invoke();
	}

	private void Update()
	{
		if (!this.interactable) return;
		CheckForLongPress();

		if (m_isKeepPress) onKeepPress?.Invoke();
	}

	private void CheckForLongPress()
	{
		if (m_isPress && !m_longPress)
		{
			if ((DateTime.Now - m_currentStartTime).TotalMilliseconds >= 500)
			{
				m_isPress = false;
				m_longPress = true;
				m_onLongPress?.Invoke();
			}
		}
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		m_isPress = true;
		m_longPress = false;
		m_isKeepPress = true;
		m_currentStartTime = DateTime.Now;
		m_onPress?.Invoke();
		base.OnPointerDown(eventData);
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		m_isPress = false;
		m_isKeepPress = false;
		onUp?.Invoke();
		base.OnPointerUp(eventData);
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		m_isPress = false;
		m_isKeepPress = false;

		base.OnPointerExit(eventData);
	}

	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (this.interactable && !m_longPress)
		{
			if (eventData.clickCount == 2)
			{
				m_onDoubleClick?.Invoke();
			}
			else if (eventData.clickCount == 1)
			{
				onClick?.Invoke();
				m_OnClickPos?.Invoke(eventData);
			}
		}
	}

	public virtual void OnSubmit(BaseEventData eventData)
	{
		Press();

		if (!IsActive() || !IsInteractable())
			return;

		DoStateTransition(SelectionState.Pressed, false);
		StartCoroutine(OnFinishSubmit());
	}

	private IEnumerator OnFinishSubmit()
	{
		var fadeTime = colors.fadeDuration;
		var elapsedTime = 0f;

		while (elapsedTime < fadeTime)
		{
			elapsedTime += Time.unscaledDeltaTime;
			yield return null;
		}

		DoStateTransition(currentSelectionState, false);
	}

	public void OnDrag(PointerEventData eventData)
	{
		onDrag?.Invoke(eventData);
	}
}