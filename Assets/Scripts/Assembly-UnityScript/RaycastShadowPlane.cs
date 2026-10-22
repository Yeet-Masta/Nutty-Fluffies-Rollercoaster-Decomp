using System;
using UnityEngine;

[Serializable]
[ExecuteInEditMode]
public class RaycastShadowPlane : MonoBehaviour
{
	public Transform shadowObject;

	public Transform castingObject;

	public Transform castingDirection;

	public float rayLength;

	public float surfaceBias;

	public int ignoreLayers;

	private Vector3 direction;

	public RaycastShadowPlane()
	{
		direction = default(Vector3);
	}

	public virtual void Update()
	{
		RaycastHit hitInfo = default(RaycastHit);
		if (Physics.Raycast(castingObject.position, -Vector3.up, out hitInfo, rayLength, ~ignoreLayers))
		{
			float num = Vector3.Angle(-hitInfo.normal, Vector3.up);
			shadowObject.position = hitInfo.point;
			shadowObject.position += hitInfo.normal * surfaceBias;
			shadowObject.localRotation = Quaternion.Euler(270f + num, 270f, 180f);
		}
	}

	public virtual void Main()
	{
	}
}
