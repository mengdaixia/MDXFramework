using System;
using System.Collections.Generic;
using UnityEngine;

namespace Code.Msg
{
	public delegate void Callback();
	public delegate void Callback<T>(T t);
	public delegate void Callback<T, U>(T t, U u);
	public delegate void Callback<T, U, W>(T t, U u, W w);
	
	//TODO:Delegate.Combine多个委托
	public static class Messenger
	{
		private static Dictionary<string, Delegate> callbackDic = new Dictionary<string, Delegate>();

		private static void OnListenerAdding(string msg)
		{
			if (!callbackDic.ContainsKey(msg))
			{
				callbackDic.Add(msg, null);
			}
		}
		public static void AddListener(string msg, Callback handler)
		{
			OnListenerAdding(msg);
			callbackDic[msg] = (Callback)callbackDic[msg] + handler;
		}
		public static void AddListener<T>(string msg, Callback<T> handler)
		{
			OnListenerAdding(msg);
			callbackDic[msg] = (Callback<T>)callbackDic[msg] + handler;
		}
		public static void AddListener<T, U>(string msg, Callback<T, U> handler)
		{
			OnListenerAdding(msg);
			callbackDic[msg] = (Callback<T, U>)callbackDic[msg] + handler;
		}
		public static void AddListener<T, U, W>(string msg, Callback<T, U, W> handler)
		{
			OnListenerAdding(msg);
			callbackDic[msg] = (Callback<T, U, W>)callbackDic[msg] + handler;
		}
		public static void RemoveListener(string msg, Callback handler)
		{
			if (callbackDic.TryGetValue(msg, out Delegate del))
			{
				var callback = (del as Callback);
				callbackDic[msg] = callback - handler;
			}
		}
		public static void RemoveListener<T>(string msg, Callback<T> handler)
		{
			if (callbackDic.TryGetValue(msg, out Delegate del))
			{
				var callback = (del as Callback<T>);
				callbackDic[msg] = callback - handler;
			}
		}
		public static void RemoveListener<T, U>(string msg, Callback<T, U> handler)
		{
			if (callbackDic.TryGetValue(msg, out Delegate del))
			{
				var callback = (del as Callback<T, U>);
				callbackDic[msg] = callback - handler;
			}
		}
		public static void RemoveListener<T, U, W>(string msg, Callback<T, U, W> handler)
		{
			if (callbackDic.TryGetValue(msg, out Delegate del))
			{
				var callback = (del as Callback<T, U, W>);
				callbackDic[msg] = callback - handler;
			}
		}
		public static void Post(string msg)
		{
			Delegate d;
			if (callbackDic.TryGetValue(msg, out d))
			{
				Callback callback = d as Callback;
				callback?.Invoke();
			}
		}
		public static void Post<T>(string msg, T t)
		{
			Delegate d;
			if (callbackDic.TryGetValue(msg, out d))
			{
				Callback<T> callback = d as Callback<T>;
				callback?.Invoke(t);
			}
		}
		public static void Post<T, U>(string msg, T t, U u)
		{
			Delegate d;
			if (callbackDic.TryGetValue(msg, out d))
			{
				Callback<T, U> callback = d as Callback<T, U>;
				callback?.Invoke(t, u);
			}
		}
		public static void Post<T, U, W>(string msg, T t, U u, W w)
		{
			Delegate d;
			if (callbackDic.TryGetValue(msg, out d))
			{
				Callback<T, U, W> callback = d as Callback<T, U, W>;
				callback?.Invoke(t, u, w);
			}
		}
		public static void AddListeners(Dictionary<string, Delegate> msg_dic)
		{
			foreach (var item in msg_dic)
			{
				var msg = item.Key;
				var handler = item.Value;
				OnListenerAdding(msg);
				callbackDic[msg] = Delegate.Combine(callbackDic[msg], handler);
			}
		}
		public static void RemoveListeners(Dictionary<string, Delegate> msg_dic)
		{
			foreach (var item in msg_dic)
			{
				var msg = item.Key;
				var handler = item.Value;
				OnListenerAdding(msg);
				callbackDic[msg] = Delegate.Remove(callbackDic[msg], handler);
			}
		}
	}
}