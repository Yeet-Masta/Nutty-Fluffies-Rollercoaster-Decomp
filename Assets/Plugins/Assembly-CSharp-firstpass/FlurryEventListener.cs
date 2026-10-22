using System.Collections;
using UnityEngine;

public class FlurryEventListener : MonoBehaviour
{
	private void OnEnable()
	{
		FlurryManager.videoDidFinishEvent += videoDidFinishEvent;
		FlurryManager.videoAvailableEvent += videoAvailableEvent;
		FlurryManager.videoUnavailableEvent += videoUnavailableEvent;
	}

	private void OnDisable()
	{
		FlurryManager.videoDidFinishEvent -= videoDidFinishEvent;
		FlurryManager.videoAvailableEvent -= videoAvailableEvent;
		FlurryManager.videoUnavailableEvent -= videoUnavailableEvent;
	}

	private void videoDidFinishEvent(Hashtable ht)
	{
		if (ht != null)
		{
			Debug.Log("videoDidFinishEvent. total keys in hashtable: " + ht.Keys.Count);
		}
	}

	private void videoAvailableEvent()
	{
		Debug.Log("videoAvailableEvent");
	}

	private void videoUnavailableEvent()
	{
		Debug.Log("videoUnavailableEvent");
	}
}
