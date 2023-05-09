using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class IrregularLoopGrid : MonoBehaviour
{
	private HorizontalOrVerticalLayoutGroup layoutGroup;



#if UNITY_EDITOR
	private void Reset()
	{
		(transform as RectTransform).pivot = new Vector2(0, 1);
		for (int i = 0; i < transform.childCount; i++)
		{
			var child = transform.GetChild(i);
			var rt = child as RectTransform;
			rt.pivot = new Vector2(0, 1);
			rt.pivot = new Vector2(0, 1);
			rt.anchorMin = new Vector2(0, 1);
			rt.anchorMax = new Vector2(0, 1);
		}
	}
#endif
}