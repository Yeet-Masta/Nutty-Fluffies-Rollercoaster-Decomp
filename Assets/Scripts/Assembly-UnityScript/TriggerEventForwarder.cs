using System;
using UnityEngine;

[Serializable]
public class TriggerEventForwarder : MonoBehaviour
{
	public GameObject forwardTo;

	public virtual void OnTriggerEnter(Collider other)
	{
		if (forwardTo != null)
		{
			forwardTo.SendMessage("OnTriggerEnter", GetComponent<Collider>());
		}
	}

	public virtual void OnFastTrigger(GameObject @object)
	{
		if (forwardTo != null)
		{
			forwardTo.SendMessage("OnFastTrigger", gameObject);
		}
	}

	public virtual void Main()
	{
	}
}
