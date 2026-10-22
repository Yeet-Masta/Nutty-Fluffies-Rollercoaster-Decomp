using System;
using UnityEngine;

[Serializable]
public class ScreenContentScaleUtility
{
	public static float getContentScaleFactor()
	{
		if (GameData.LOG)
		{
			Debug.Log("Screen size: " + Screen.width + " x " + Screen.height);
		}
		return 1f;
	}
}
