using Code.Mgr;
using System;
using System.Collections.Generic;
using UnityEngine;

public class UserDataManager : Singleton<UserDataManager>, IRunningMgr, ILateUpdate
{
	private Dictionary<Type, string> userKeyDic = new Dictionary<Type, string>();
	private Dictionary<Type, UserDataBase> userDic = new Dictionary<Type, UserDataBase>();
	private UserDataManager() { }
	public void Init()
	{
		userKeyDic[typeof(BagData)] = "UseData_Bag_Key";
		userKeyDic[typeof(UserData)] = "UseData_User_Key";
	}
	public void LateUpdate()
	{
		foreach (var item in userDic)
		{
			var obj = item.Value;
			if (obj.IsDirty)
			{
				var datas = Utility.Json.Serialize(obj);
				Utility.Prefers.SetString(userKeyDic[obj.GetType()], datas);
				obj.IsDirty = false;
			}
		}
	}
	public void Destroy()
	{

	}
	public T Get<T>() where T : UserDataBase, new()
	{
		var type = typeof(T);
		if (!userDic.TryGetValue(type, out UserDataBase result))
		{
			var key = userKeyDic[type];
			var datas = Utility.Prefers.GetString(key);
			result = Utility.Json.Deserialize<T>(datas);
			if (result == null)
			{
				result = new T();
			}
			userDic[type] = result;
		}
		return result as T;
	}
}