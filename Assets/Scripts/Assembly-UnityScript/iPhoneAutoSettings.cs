using System;
using UnityEngine;

[Serializable]
public class iPhoneAutoSettings : MonoBehaviour
{
	public ScreenOrientation screenOrientation;

	public bool canDarken;

	public iPhoneAutoSettings()
	{
		screenOrientation = ScreenOrientation.LandscapeRight;
		canDarken = true;
	}

	public virtual void Awake()
	{
		Screen.orientation = screenOrientation;
		if (!canDarken)
		{
			Screen.sleepTimeout = 0;
		}
		Screen.autorotateToPortrait = false;
		Screen.autorotateToPortraitUpsideDown = false;
		Screen.autorotateToLandscapeRight = false;
		Screen.autorotateToLandscapeLeft = false;
	}

	public virtual void Main()
	{
	}
}
