/************************************************* 
  *Author: 作者 
  *Date: 日期 
  *Description: 説明
**************************************************/
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TriggerTest : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		Debug.LogError("OnTriggerEnter->>>" + other.name);
	}
}