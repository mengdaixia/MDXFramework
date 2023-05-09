using System;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IPoolObj
{
	void Clear();
}
public static class ObjectPool
{
	private static Dictionary<Type, Stack<IPoolObj>> poolDic = new Dictionary<Type, Stack<IPoolObj>>();
	public static T Get<T>() where T : class, IPoolObj, new()
	{
		var type = typeof(T);
		if (!poolDic.TryGetValue(type, out Stack<IPoolObj> stack))
		{
			stack = new Stack<IPoolObj>();
			poolDic[type] = stack;
		}
		T obj = null;
		if (stack.Count > 0)
		{
			obj = stack.Pop() as T;
		}
		else
		{
			obj = new T();
		}
		return obj;
	}
	public static void Release(IPoolObj obj)
	{
		var type = obj.GetType();
		var stack = poolDic[type];
		if (stack.Count > 0 && ReferenceEquals(stack.Peek(), obj))
		{
			return;
		}
		obj.Clear();
		stack.Push(obj);
	}
	public static void Clear()
	{
		poolDic.Clear();
	}
}