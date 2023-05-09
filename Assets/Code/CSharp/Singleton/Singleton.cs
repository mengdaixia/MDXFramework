using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> where T : class
{
	private static T instance;
	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = (T)Activator.CreateInstance(typeof(T), true);
				SingletonEvent.OnSingletonInit?.Invoke(instance);
			}
			return instance;
		}
	}
}