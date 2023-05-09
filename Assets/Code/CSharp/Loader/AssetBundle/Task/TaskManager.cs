using Code.Mgr;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Task
{
	public class TaskManager : Singleton<TaskManager>, IRunningMgr, IUpdate
	{
		private List<TaskBase> workTaskLst = new List<TaskBase>();

		public bool IsRunning { get; private set; } = false;
		public bool HasTask => workTaskLst.Count > 0;
		private TaskManager() { }
		public void Init()
		{

		}
		public void Destroy()
		{

		}
		public void Start()
		{
			IsRunning = true;
		}
		public void End()
		{
			IsRunning = false;
		}
		public void AddTask(TaskBase task)
		{
			workTaskLst.Add(task);
		}
		public void Update()
		{
			if (!IsRunning)
			{
				return;
			}
			for (int i = workTaskLst.Count - 1; i >= 0; i--)
			{
				var task = workTaskLst[i];
				switch (task.TaskState)
				{
					case ETaskState.Start:
						task.Start();
						break;
					case ETaskState.Update:
						task.Update();
						break;
					case Task.ETaskState.Done:
						task.Done();
						workTaskLst.RemoveAt(i);
						break;
					default:
						break;
				}
			}
		}
	}
}