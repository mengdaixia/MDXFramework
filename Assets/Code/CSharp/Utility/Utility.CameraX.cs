using System;
using System.Collections.Generic;
using UnityEngine;

public static partial class Utility
{
	public static class CameraX
	{
		private static Camera mainCamera = null;
		private static Vector3[] PosAreaArr = new Vector3[2];

		public static float MinZ => PosAreaArr[0].z;
		public static Camera main
		{
			get
			{
				if (mainCamera == null)
				{
					mainCamera = Camera.main;
				}
				return mainCamera;
			}
		}
		public static bool IsInView(Vector3 worldPosition)
		{
			if (main == null)
			{
				return false;
			}
			Transform cameraTransform = main.transform;
			Vector2 viewPos = main.WorldToViewportPoint(worldPosition);
			Vector3 dir = (worldPosition - cameraTransform.position).normalized;
			float dot = Vector3.Dot(cameraTransform.forward, dir);
			if (dot > 0 && viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1)
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		public static void PreCameraViewPos()
		{
			var camera = main;

			var leftButtom = new Vector2(0, 0);
			var leftTop = new Vector3(0, Screen.height);
			var rightTop = new Vector3(Screen.width, Screen.height);
			var rightButtom = new Vector3(Screen.width, 0);

			var ray_leftButtom = camera.ScreenPointToRay(leftButtom);
			var ray_leftTop = camera.ScreenPointToRay(leftTop);
			var ray_rightTop = camera.ScreenPointToRay(rightTop);
			var ray_rightButtom = camera.ScreenPointToRay(rightButtom);

			var dist = GameObject.Find("MainBornPos").transform.position.y - camera.transform.position.y;
			var ray1L = dist / ray_leftButtom.direction.y;
			var ray3L = dist / ray_rightTop.direction.y;

			PosAreaArr[0] = ray_leftButtom.origin + ray1L * ray_leftButtom.direction;
			PosAreaArr[1] = ray_rightTop.origin + ray3L * ray_rightTop.direction;
		}
		public static void CheckIsInCameraView(ref Vector3 pos)
		{
			var x = pos.x < PosAreaArr[0].x ? PosAreaArr[0].x : pos.x;
			x = pos.x > PosAreaArr[1].x ? PosAreaArr[1].x : x;
			var z = pos.z < PosAreaArr[0].z ? PosAreaArr[0].z : pos.z;
			z = pos.z > PosAreaArr[1].z ? PosAreaArr[1].z : z;
			pos = new Vector3(x, pos.y, z);
		}
	}
}