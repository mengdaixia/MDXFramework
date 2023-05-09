using System;
using System.Collections.Generic;
using UnityEngine;


namespace Mono
{
	public class Slice : MonoBehaviour
	{
		[SerializeField]
		private int initIndex = 0;
		private int currIndex = 0;
		private List<GameObject> goLst = new List<GameObject>();
		private bool isInit = false;
		private void Awake()
		{
			Init();
		}
		private void Init()
		{
			if (isInit)
			{
				return;
			}
			isInit = true;
			for (int i = 0; i < transform.childCount; i++)
			{
				var go = transform.GetChild(i).gameObject;
				Utility.Go.SetActive(go, i == initIndex);
				goLst.Add(go);
			}
			currIndex = initIndex;
		}
		public void SetActive(int index)
		{
			Init();
			if (currIndex == index)
			{
				return;
			}
			if (currIndex < goLst.Count)
			{
				Utility.Go.SetActive(goLst[currIndex], false);
			}
			if (index < goLst.Count)
			{
				Utility.Go.SetActive(goLst[index], true);
			}
			currIndex = index;
		}
	}
}