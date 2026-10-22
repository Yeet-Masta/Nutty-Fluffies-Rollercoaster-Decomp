using System;
using UnityEngine;

[Serializable]
public class DeactivateWhenInvisible : MonoBehaviour
{
	private Rigidbody rb;

	private bool wasSleeping;

	public DeactivateWhenInvisible()
	{
		wasSleeping = true;
	}

	public virtual void Awake()
	{
		rb = GetComponent<Rigidbody>();
		if (rb == null)
		{
			rb = transform.parent.gameObject.GetComponent<Rigidbody>();
		}
		if (rb == null)
		{
			Debug.Log("NO BODY! " + name);
		}
	}

	public virtual void OnBecameVisible()
	{
		if ((bool)rb)
		{
			rb.isKinematic = false;
			rb.detectCollisions = true;
			if (!wasSleeping)
			{
				rb.WakeUp();
			}
			else
			{
				rb.Sleep();
			}
		}
	}

	public virtual void OnBecameInvisible()
	{
		if (rb != null)
		{
			rb.isKinematic = true;
			rb.detectCollisions = false;
			wasSleeping = rb.IsSleeping();
		}
	}

	public virtual void stateResetted()
	{
		wasSleeping = true;
	}
}
