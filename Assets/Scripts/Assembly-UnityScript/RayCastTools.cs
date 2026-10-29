using System;
using UnityEngine;

[Serializable]
public class RayCastTools
{
	public static void ifHitInvoke(Ray ray, float depth, string methodName, int layerMask)
	{
		RaycastHit[] hits = null;
		hits = layerMask != -1 ? Physics.RaycastAll(ray, depth, layerMask) : Physics.RaycastAll(ray, depth);
		for (int i = hits.Length - 1; i >= 0; i--)
		{
			RaycastHit raycastHit = hits[i];
			if (raycastHit.collider != null)
			{
				raycastHit.collider.gameObject.SendMessage(methodName, raycastHit.point, SendMessageOptions.DontRequireReceiver);
			}
		}
	}

	public static GameObject ifHitInvokeNearest(Ray ray, float depth, string methodName, int layerMask)
	{
		RaycastHit hitInfo = default(RaycastHit);
		bool hit = false;
		hit = layerMask != -1 ? Physics.Raycast(ray, out hitInfo, depth, layerMask) : Physics.Raycast(ray, out hitInfo, depth);
		if (hit && hitInfo.collider != null)
		{
			hitInfo.collider.gameObject.SendMessage(methodName, hitInfo.point, SendMessageOptions.DontRequireReceiver);
			return hitInfo.collider.gameObject;
		}
		return null;
	}
}
