using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Utils
{
	public struct FrameEvent
	{
		public float Time;
		public Action Event;
	}
	public class EventsRunningCtr
	{
		private List<FrameEvent> eventsLst = new List<FrameEvent>();

		public void AddEvents(List<FrameEvent> evts)
		{
			if (evts != null && evts.Count > 0)
			{
				eventsLst.AddRange(evts);
			}
		}
		public void AddEvent(FrameEvent evt)
		{
			eventsLst.Add(evt);
		}
		public void Update(float time)
		{
			for (int i = eventsLst.Count - 1; i >= 0; i--)
			{
				var evt = eventsLst[i];
				if (time > evt.Time)
				{
					eventsLst.RemoveAt(i);
					evt.Event?.Invoke();
				}
			}
		}
		public void ClearEvt()
		{
			if (eventsLst.Count > 0)
			{
				eventsLst.Clear();
			}
		}
	}
}