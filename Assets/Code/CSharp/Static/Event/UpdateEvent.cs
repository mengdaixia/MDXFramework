using System;
using System.Collections.Generic;
using UnityEngine;

public static class UpdateEvent
{
	public static Action OnFixedUpdate;
	public static Action OnUpdate;
	public static Action OnLateUpdate;
}