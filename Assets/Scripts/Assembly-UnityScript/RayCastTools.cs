using System;
using UnityEngine;

[Serializable]
public class RayCastTools
{
	public static void ifHitInvoke(Ray ray, float depth, string methodName, int layerMask)
	{
		RaycastHit[] array = null;
		array = layerMask != -1 ? Physics.RaycastAll(ray, depth, layerMask) : Physics.RaycastAll(ray, depth);
		for (int num = array.Length - 1; num >= 0; num--)
		{
			RaycastHit raycastHit = array[num];
			if (raycastHit.collider != null)
			{
				raycastHit.collider.gameObject.SendMessage(methodName, raycastHit.point, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public static GameObject ifHitInvokeNearest(Ray ray, float depth, string methodName, int layerMask)
	{
		RaycastHit hitInfo = default(RaycastHit);
		bool flag = false;
		flag = layerMask != -1 ? Physics.Raycast(ray, out hitInfo, depth, layerMask) : Physics.Raycast(ray, out hitInfo, depth);
		if (flag && hitInfo.collider != null)
		{
			hitInfo.collider.gameObject.SendMessage(methodName, hitInfo.point, SendMessageOptions.DontRequireReceiver);
			return hitInfo.collider.gameObject;
		}
		else
		{
			return null;
		}
	}
}
