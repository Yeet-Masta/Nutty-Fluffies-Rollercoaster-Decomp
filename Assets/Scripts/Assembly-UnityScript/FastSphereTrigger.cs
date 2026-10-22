using System;
using UnityEngine;

[Serializable]
public class FastSphereTrigger : FastSphereTriggerListener
{
	public float size;

	public bool reactivates;

	public float reactivateTime;

	public float lastTriggerTime;

	public bool activated;

	public FastSphereTrigger()
	{
		activated = true;
	}

	public virtual void init()
	{
		lastTriggerTime = 0f - reactivateTime;
		size = (GetComponent<Collider>() as SphereCollider).radius;
		size *= size;
		UnityEngine.Object.Destroy(GetComponent<Collider>());
	}

	public virtual void trigger()
	{
		lastTriggerTime = Time.time;
		activated = false;
	}

	public virtual void checkForReactivate()
	{
		if (!activated && reactivates && !(Time.time <= lastTriggerTime + reactivateTime))
		{
			reactivate();
		}
	}

	public virtual void reactivate()
	{
		activated = true;
		reactivated();
	}

	public virtual void reactivated()
	{
	}
}
