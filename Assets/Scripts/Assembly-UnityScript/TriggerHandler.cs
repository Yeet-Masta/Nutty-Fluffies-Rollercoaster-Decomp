using System;
using UnityEngine;

[Serializable]
public class TriggerHandler
{
	public bool playerTrigger;

	public bool onceTrigger;

	public bool hasTriggered;

	public virtual bool isVehicle(Collider collider)
	{
		return collider.gameObject.tag == "Player";
	}

	public virtual bool canTrigger(Collider collider)
	{
		return !CommonData.EDITOR_ACTIVE && (!onceTrigger || !hasTriggered) && (!playerTrigger || isVehicle(collider));
	}
}
