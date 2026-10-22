using System;
using UnityEngine;

[Serializable]
public class LockPosition : CachedMonoBehaviour
{
	public bool lockX;

	public bool lockY;

	public bool lockZ;

	public bool lockXRot;

	public bool lockYRot;

	public bool lockZRot;

	private Vector3 initialPosition;

	private Quaternion initialRotation;

	public virtual void Start()
	{
		initialPosition = cRigidbody.position;
		initialRotation = cTransform.rotation;
	}

	public virtual void FixedUpdate()
	{
		if (!cRigidbody.isKinematic && !cRigidbody.IsSleeping())
		{
			if (lockX)
			{
				float x = initialPosition.x;
				Vector3 position = cRigidbody.position;
				float num = (position.x = x);
				Vector3 vector = (cRigidbody.position = position);
			}
			if (lockY)
			{
				float y = initialPosition.y;
				Vector3 position2 = cRigidbody.position;
				float num2 = (position2.y = y);
				Vector3 vector3 = (cRigidbody.position = position2);
			}
			if (lockZ)
			{
				float z = initialPosition.z;
				Vector3 position3 = cRigidbody.position;
				float num3 = (position3.z = z);
				Vector3 vector5 = (cRigidbody.position = position3);
			}
			if (lockXRot)
			{
				float x2 = initialRotation.x;
				Quaternion rotation = cTransform.rotation;
				float num4 = (rotation.x = x2);
				Quaternion quaternion = (cTransform.rotation = rotation);
				int num5 = 0;
				Vector3 angularVelocity = cRigidbody.angularVelocity;
				float num6 = (angularVelocity.x = num5);
				Vector3 vector7 = (cRigidbody.angularVelocity = angularVelocity);
			}
			if (lockYRot)
			{
				float y2 = initialRotation.y;
				Quaternion rotation2 = cTransform.rotation;
				float num7 = (rotation2.y = y2);
				Quaternion quaternion3 = (cTransform.rotation = rotation2);
				int num8 = 0;
				Vector3 angularVelocity2 = cRigidbody.angularVelocity;
				float num9 = (angularVelocity2.y = num8);
				Vector3 vector9 = (cRigidbody.angularVelocity = angularVelocity2);
			}
			if (lockZRot)
			{
				float z2 = initialRotation.z;
				Quaternion rotation3 = cTransform.rotation;
				float num10 = (rotation3.z = z2);
				Quaternion quaternion5 = (cTransform.rotation = rotation3);
				int num11 = 0;
				Vector3 angularVelocity3 = cRigidbody.angularVelocity;
				float num12 = (angularVelocity3.z = num11);
				Vector3 vector11 = (cRigidbody.angularVelocity = angularVelocity3);
			}
		}
	}
}
