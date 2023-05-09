using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildWindow : EditorWindow
{
	[MenuItem("Tools/Build/Build")]
	static void Build()
	{
		List<string> names = new List<string>();

		foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
		{
			if (e == null)
				continue;

			if (e.enabled)
				names.Add(e.path);
		}
		BuildPipeline.BuildPlayer(names.ToArray(), "E:/Work/Build/Test.apk", BuildTarget.Android, BuildOptions.None);
	}
}
public  class BuildTools
{
	static string[] GetBuildScenes()
	{
		List<string> names = new List<string>();

		foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
		{
			if (e == null)
				continue;

			if (e.enabled)
				names.Add(e.path);
		}
		return names.ToArray();
	}
	public static void Build()
	{
		Debug.Log("Command line build\n------------------\n------------------");
		//string path = @"E:\Unity游戏包\Android\消消乐游戏";//这里的路径是打包的路径， 定义
		string path = GetJenkinsParameter("BuildPath");
		Debug.Log("Starting Build!");
		Debug.Log(GetJenkinsParameter("Platform"));

		string platform = GetJenkinsParameter("Platform");
		BuildPipeline.BuildPlayer(GetBuildScenes(), path + ".apk", BuildTarget.Android, BuildOptions.None);
	}
	static string GetJenkinsParameter(string name)
	{
		foreach (string arg in Environment.GetCommandLineArgs())
		{
			Debug.Log("arg:" + arg);
			if (arg.StartsWith(name))
			{
				return arg.Split("-"[0])[1];
			}
		}
		return null;
	}
}
