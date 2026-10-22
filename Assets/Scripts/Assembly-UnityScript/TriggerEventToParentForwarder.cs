using System;
using UnityEngine;

[Serializable]
public class TriggerEventToParentForwarder : MonoBehaviour
{
	public virtual void OnTriggerEnter(Collider other)
	{
		transform.parent.gameObject.SendMessage("OnTriggerEnter", GetComponent<Collider>());
	}

	public virtual void OnFastTrigger(object @object)
	{
		transform.parent.gameObject.SendMessage("OnFastTrigger", gameObject);
	}

	public virtual void Main()
	{
	}
}
