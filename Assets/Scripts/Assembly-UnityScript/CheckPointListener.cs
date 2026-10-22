using System;
using UnityEngine;

[Serializable]
public class CheckPointListener : MonoBehaviour
{
	public virtual void checkPointTriggered(CheckPoint @object)
	{
	}

	public virtual void checkPointNonTriggerVisit(CheckPoint @object)
	{
	}

	public virtual void lapComplete(int lapNumber)
	{
	}

	public virtual void lapNotAllCheckpointsVisited(int lapNumber)
	{
	}

	public virtual void lastCheckPoint(int completeLaps)
	{
	}
}
