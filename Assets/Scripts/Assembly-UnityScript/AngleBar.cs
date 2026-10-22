using System;
using UnityEngine;

[Serializable]
public class AngleBar : MonoBehaviour
{
	private float angle;

	private bool angleChangePending;

	public AngleBar()
	{
		angleChangePending = true;
	}

	public virtual void applyAngle(float angleFloat)
	{
		MonoBehaviour.print("apply angle : " + angleFloat);
		angle = angleFloat;
		angleChangePending = true;
	}

	public virtual void Update()
	{
		if (angleChangePending)
		{
			float z = 0f - angle;
			Quaternion localRotation = gameObject.transform.localRotation;
			float num = (localRotation.z = z);
			Quaternion quaternion = (gameObject.transform.localRotation = localRotation);
			angleChangePending = false;
		}
	}
}
