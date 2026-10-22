using System;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class CheckPointManager : CheckPointListener
{
	public CheckPoint[] checkPoints;

	public int lapAmount;

	public GameObject listenerRef;

	public bool autoSearchCheckpointsFromChilds;

	public string autoSearchCheckpointNamePrefix;

	private int nextCheckPointIndex;

	private int currentLap;

	public bool ignoreFirstLapCheckpoint;

	public CheckPointManager()
	{
		autoSearchCheckpointNamePrefix = "CheckPoint";
		ignoreFirstLapCheckpoint = true;
	}

	public virtual void Awake()
	{
		CheckPoint checkPoint = null;
		if (autoSearchCheckpointsFromChilds)
		{
			int num = 1;
			UnityScript.Lang.Array array = new UnityScript.Lang.Array();
			checkPoint = findCheckPointWithName(autoSearchCheckpointNamePrefix + num);
			while (checkPoint != null)
			{
				array.Push(checkPoint);
				num++;
				checkPoint = findCheckPointWithName(autoSearchCheckpointNamePrefix + num);
			}
			checkPoints = new CheckPoint[array.length];
			for (int i = 0; i < Extensions.get_length((System.Array)checkPoints); i++)
			{
				CheckPoint[] array2 = checkPoints;
				int num2 = i;
				object obj = array[i];
				if (!(obj is CheckPoint))
				{
					obj = RuntimeServices.Coerce(obj, typeof(CheckPoint));
				}
				array2[num2] = (CheckPoint)obj;
			}
		}
		for (int j = 0; j < Extensions.get_length((System.Array)checkPoints); j++)
		{
			checkPoint = checkPoints[j];
			checkPoint.setIndex(j);
			checkPoint.setListener(this);
		}
		reset();
	}

	private CheckPoint findCheckPointWithName(string name)
	{
		GameObject gameObject = GameObject.Find(name);
		object result;
		if (gameObject == null)
		{
			result = null;
		}
		else
		{
			CheckPoint checkPoint = (CheckPoint)gameObject.GetComponent(typeof(CheckPoint));
			result = checkPoint;
		}
		return (CheckPoint)result;
	}

	public virtual int getCheckpointAmountLeft()
	{
		return (lapAmount - currentLap) * Extensions.get_length((System.Array)checkPoints) - nextCheckPointIndex;
	}

	public virtual void fireWithLine(Vector3 from, Vector3 to, int layerMask)
	{
		Vector3 direction = to - from;
		RayCastTools.ifHitInvoke(new Ray(from, direction), direction.magnitude, "OnManualFire", layerMask);
	}

	public virtual void setListener(GameObject listener)
	{
		listenerRef = listener;
	}

	public virtual void setTriggerer(GameObject @object)
	{
		for (int i = 0; i < Extensions.get_length((System.Array)checkPoints); i++)
		{
			checkPoints[i].setTriggerer(@object);
		}
	}

	public virtual bool reset()
	{
		bool result = false;
		if (currentLap == 0 && nextCheckPointIndex == 0)
		{
			result = true;
		}
		resetForLap();
		currentLap = 0;
		return result;
	}

	public virtual void activateAll()
	{
		for (int i = 0; i < Extensions.get_length((System.Array)checkPoints); i++)
		{
			checkPoints[i].gameObject.active = true;
		}
	}

	public virtual void deactivateAll()
	{
		for (int i = 0; i < Extensions.get_length((System.Array)checkPoints); i++)
		{
			checkPoints[i].gameObject.active = false;
		}
	}

	public virtual void deactivateAllButNextFirstAndLast()
	{
		activateAll();
		for (int i = 1; i < Extensions.get_length((System.Array)checkPoints) - 1; i++)
		{
			if (i != nextCheckPointIndex)
			{
				checkPoints[i].gameObject.active = false;
			}
		}
	}

	public virtual void resetForLap()
	{
		for (int i = 0; i < Extensions.get_length((System.Array)checkPoints); i++)
		{
			checkPoints[i].reset();
		}
		nextCheckPointIndex = 0;
		deactivateAllButNextFirstAndLast();
	}

	public override void checkPointTriggered(CheckPoint cp)
	{
		if (nextCheckPointIndex == cp.getIndex())
		{
			if (listenerRef != null)
			{
				listenerRef.SendMessage("checkPointTriggered", cp, SendMessageOptions.DontRequireReceiver);
			}
			nextCheckPointIndex++;
			if (nextCheckPointIndex == Extensions.get_length((System.Array)checkPoints))
			{
				currentLap++;
				listenerRef.SendMessage("lapComplete", currentLap, SendMessageOptions.DontRequireReceiver);
				if (lapAmount > currentLap)
				{
					resetForLap();
				}
				else if (listenerRef != null)
				{
					listenerRef.SendMessage("lastCheckPoint", cp, SendMessageOptions.DontRequireReceiver);
				}
			}
			deactivateAllButNextFirstAndLast();
		}
		else
		{
			cp.reset();
			listenerRef.SendMessage("checkPointNonTriggerVisit", cp, SendMessageOptions.DontRequireReceiver);
			if (cp == checkPoints[Extensions.get_length((System.Array)checkPoints) - 1] && (!ignoreFirstLapCheckpoint || nextCheckPointIndex != 0 || currentLap != 0) && nextCheckPointIndex != 1)
			{
				listenerRef.SendMessage("lapNotAllCheckpointsVisited", currentLap, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
