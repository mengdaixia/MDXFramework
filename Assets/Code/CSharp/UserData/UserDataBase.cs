using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class PreferKeyAttribute : Attribute
{
	public string PreferKey;
	public PreferKeyAttribute(string key)
	{
		PreferKey = key;
	}
}
public abstract class UserDataBase
{
	public bool IsDirty { get; set; }
}