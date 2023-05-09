using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Task
{
	public enum ETaskState
	{
		None,
		Start,
		Update,
		Done,
	}
	public abstract class TaskBase
	{
		public Action OnTaskDone;
		public ETaskState TaskState { get; protected set; } = ETaskState.None;
		public void Start()
		{
			OnStart();
			TaskState = ETaskState.Update;
		}
		public void Update()
		{
			OnUpdate();
		}
		public void Done()
		{
			OnDone();
			OnTaskDone?.Invoke();
		}
		protected virtual void OnStart() { }
		protected virtual void OnUpdate() { }
		protected virtual void OnDone() { }
	}
}