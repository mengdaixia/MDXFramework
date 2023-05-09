using Code.Fight;
using System;
using System.Collections.Generic;
using UnityEngine;

public static class ISceneUnitExtionsion
{
	public static float DeltaTime(this ISceneUnit unit)
	{
		var trSpeed = unit.Attr[EAttrType.TimeRunningSpeed];
		var deltaTime = trSpeed * Utility.Time.deltaTime;
		return deltaTime;
	}
}