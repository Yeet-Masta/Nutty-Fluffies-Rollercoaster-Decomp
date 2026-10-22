using System;
using System.Collections;
using UnityEngine;

public class FlurryManager : MonoBehaviour
{
	public static event Action<Hashtable> videoDidFinishEvent;

	public static event Action videoAvailableEvent;

	public static event Action videoUnavailableEvent;

	private void Awake()
	{
		gameObject.name = GetType().ToString();
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	public void videoDidFinish(string param)
	{
		if (videoDidFinishEvent != null)
		{
			videoDidFinishEvent(param.hashtableFromJson());
		}
	}

	public void videoAvailable(string empty)
	{
		if (videoAvailableEvent != null)
		{
			videoAvailableEvent();
		}
	}

	public void videoUnavailable(string empty)
	{
		if (videoUnavailableEvent != null)
		{
			videoUnavailableEvent();
		}
	}
}
