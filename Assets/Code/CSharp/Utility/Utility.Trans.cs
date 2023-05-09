using System;
using System.Collections.Generic;
using UnityEngine;

public static partial class Utility
{
	public static class Trans
	{
		public static void SetParent(Transform trans, Transform parent, bool is_2_local = true)
		{
			if (trans == null)
			{
				return;
			}
			if (trans.parent != parent)
			{
				trans.SetParent(parent);
			}
			if (is_2_local && parent != null)
			{
				trans.localPosition = Vector3.zero;
				trans.localRotation = Quaternion.identity;
				trans.localScale = Vector3.one;
			}
		}
		public static void SetForward(Transform trans, Vector3 forward)
		{
			if (trans == null)
			{
				return;
			}
			if (forward != trans.forward && forward != Vector3.zero)
			{
				trans.forward = forward;
			}
		}
		public static void SetLocalDefaultPSQ(Transform trans)
		{
			if (trans == null)
			{
				return;
			}
			SetPosition(trans, Vector3.zero, true);
			SetScale(trans, Vector3.one);
			SetRotation(trans, Quaternion.identity, true);
		}
		public static void SetPosAndDir(Transform trans, Transform target)
		{
			if (trans == null || target == null)
			{
				return;
			}
			SetPosition(trans, target.position);
			SetForward(trans, target.forward);
		}
		public static void SetPosition(Transform trans, Vector3 position, bool isloca = false)
		{
			if (trans == null)
			{
				return;
			}
			if (isloca)
			{
				trans.localPosition = position;
			}
			else
			{
				trans.position = position;
			}
		}
		public static void SetRotation(Transform trans, Quaternion rot, bool isloca = false)
		{
			if (trans == null)
			{
				return;
			}
			if (isloca)
			{
				trans.localRotation = rot;
			}
			else
			{
				trans.rotation = rot;
			}
		}
		public static void SetScale(Transform trans, Vector3 scale)
		{
			if (trans == null)
			{
				return;
			}
			trans.localScale = scale;
		}
	}
}