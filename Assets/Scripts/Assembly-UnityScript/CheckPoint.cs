using System;
using UnityEngine;

[Serializable]
public class CheckPoint : MonoBehaviour
{
	private int pointIndex;

	private CheckPointListener listenerRef;

	private bool triggered;

	private Vector3 lastTriggerPosition;

	private GameObject triggererRef;

	public bool allowMouseDownTriggering;

	public bool allowMouseDragTriggering;

	public virtual void Awake()
	{
		triggered = false;
	}

	public virtual void setIndex(int index)
	{
		pointIndex = index;
	}

	public virtual void setTriggerer(GameObject trigger)
	{
		triggererRef = trigger;
	}

	public virtual int getIndex()
	{
		return pointIndex;
	}

	public virtual Vector3 getLastTriggerPosition()
	{
		return lastTriggerPosition;
	}

	public virtual void setListener(CheckPointListener listener)
	{
		listenerRef = listener;
	}

	public virtual void OnMouseDown(Vector3 at)
	{
		if (!triggered && allowMouseDownTriggering)
		{
			trigger(at);
		}
	}

	public virtual void OnMouseDrag(Vector3 at)
	{
		if (!triggered && allowMouseDragTriggering)
		{
			trigger(at);
		}
	}

	public virtual void OnManualFire(Vector3 at)
	{
		if (!triggered)
		{
			trigger(at);
		}
	}

	public virtual void OnTriggerEnter(Collider other)
	{
		if (triggererRef == null || other == triggererRef.gameObject)
		{
			trigger(other.transform.position);
		}
	}

	public virtual void trigger(Vector3 atPos)
	{
		lastTriggerPosition = atPos;
		if (!triggered)
		{
			triggered = true;
			listenerRef.checkPointTriggered(this);
		}
		else
		{
			listenerRef.checkPointNonTriggerVisit(this);
		}
	}

	public virtual void reset()
	{
		triggered = false;
	}

	public virtual void Main()
	{
	}
}
