using System;
using UnityEngine;

[Serializable]
public class StickyObject : MonoBehaviour
{
	public bool returnCenterToSurface;

	public bool stopObjectWhenStuck;

	public bool stuck;

	public GameObject stuckTo;

	public Collision stuckCollision;

	public StickyObject()
	{
		stopObjectWhenStuck = true;
	}

	public virtual void unStuck()
	{
		stuck = false;
		stuckTo = null;
	}

	public virtual void OnCollisionEnter(Collision collision)
	{
		if (!stuck)
		{
			if (GetComponent<Rigidbody>() != null && stopObjectWhenStuck)
			{
				int num = 0;
				Vector3 velocity = GetComponent<Rigidbody>().velocity;
				float num2 = (velocity.x = num);
				Vector3 vector = (GetComponent<Rigidbody>().velocity = velocity);
				int num3 = 0;
				Vector3 velocity2 = GetComponent<Rigidbody>().velocity;
				float num4 = (velocity2.y = num3);
				Vector3 vector3 = (GetComponent<Rigidbody>().velocity = velocity2);
				int num5 = 0;
				Vector3 velocity3 = GetComponent<Rigidbody>().velocity;
				float num6 = (velocity3.z = num5);
				Vector3 vector5 = (GetComponent<Rigidbody>().velocity = velocity3);
				GetComponent<Rigidbody>().isKinematic = true;
			}
			if (returnCenterToSurface)
			{
				transform.position = collision.contacts[0].point;
			}
			stuck = true;
			stuckTo = collision.gameObject;
			stuckCollision = collision;
		}
	}
}
