using DMTimeArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.Skill
{
	public class SkillTrackItem
	{
		private const int TRACK_HEIGHT = 35;
		private float dragDelta;
		private bool isDraging = false;

		protected float startTime = 0;
		protected float durationTime = 1;

		protected SkillEditor timeArea;
		protected bool isSelect = false;

		public virtual bool isCanDragTrack { get; } = true;
		public virtual string HeadName { get; }
		public virtual string TrackName { get; protected set; }
		public Action<SkillTrackItem> OnSelect;
		public void Set(SkillEditor area)
		{
			timeArea = area;
		}
		public virtual void DrawInfo(ref float z, float width)
		{
			var rect = new Rect(4, z, width - 5, TRACK_HEIGHT);
			var col = isSelect ? Color.blue : Color.white;
			col.a = isSelect ? 0.5f : 0.6f;
			EditorGUI.DrawRect(rect, col);
			EUtility.GUI.EditorGUILable(rect, HeadName, 25, Color.red);
			z += rect.height;

			switch (Event.current.type)
			{
				case EventType.MouseDown:
					if (rect.Contains(Event.current.mousePosition))
					{
						OnSelect(this);
					}
					break;
			}
		}
		public virtual void DrawTrack(ref float z, Rect area)
		{
			var start = timeArea.TimeToPixel(startTime) - area.x;
			var end = timeArea.TimeToPixel(startTime + durationTime) - area.x;

			var rect = new Rect(start, z, end - start, TRACK_HEIGHT);
			var borderCol = Color.white;
			borderCol.a = isSelect ? 1 : 0.5f;
			EUtility.GUI.DrawBorderRect(rect, Color.grey, borderCol);

			var lbRect = EUtility.GUI.HalfRect(rect);
			EUtility.GUI.EditorGUILable(lbRect, TrackName, 25, Color.white, true);
			z += rect.height;

			switch (Event.current.type)
			{
				case EventType.MouseDown:
					if (rect.Contains(Event.current.mousePosition))
					{
						OnSelect(this);
					}
					dragDelta = 0;
					break;
				case EventType.MouseDrag:
					if (rect.Contains(Event.current.mousePosition))
					{
						if (isCanDragTrack)
						{
							isDraging = true;
						}
					}
					if (isDraging)
					{
						dragDelta += Event.current.delta.x;
						var x = area.x + rect.x + dragDelta;
						var lastTime = startTime;
						var newTime = timeArea.PixelToTime(x);
						var frame = timeArea._frameRate;
						if (timeArea._timeInFrames)
						{
							var deltaTime = 1f / frame;
							var moveFrameCount = (int)((newTime - lastTime) / deltaTime);
							if (Mathf.Abs(moveFrameCount) >= 1)
							{
								newTime = startTime + moveFrameCount * deltaTime;
								startTime = newTime;
								var frame1Pos = timeArea.TimeToPixel(deltaTime) - area.x;
								var frame2Pos = timeArea.TimeToPixel(deltaTime * 2) - area.x;
								var deltaDis = frame2Pos - frame1Pos;
								dragDelta -= deltaDis * moveFrameCount;
							}
						}
						else
						{
							startTime = newTime;
						}
						startTime = Mathf.Max(startTime, 0);
						GUI.changed = true;
					}
					break;
				case EventType.MouseUp:
					dragDelta = 0;
					isDraging = false;
					break;
			}
		}
		public void SetSelect(bool is_select)
		{
			isSelect = is_select;
		}
		public virtual void OnRunTimeChanged(double time, bool is_play)
		{

		}
	}
}