using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreferdShowUtils : MonoBehaviour
{
	public float PreferedHeight;
	private ILayoutElement element;

	private void Awake()
	{
		element = transform.GetComponent<ILayoutElement>();
	}
	void Update()
    {
		if (element != null)
		{
			PreferedHeight = element.preferredHeight;
		}
    }
}
