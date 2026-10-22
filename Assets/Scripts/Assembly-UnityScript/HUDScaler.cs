using System;
using UnityEngine;

[Serializable]
public class HUDScaler : MonoBehaviour
{
	public float currentAspectRatio;

	public float scaleMinimumLimit;

	public virtual void Awake()
	{
		reset();
	}

	public virtual void reset()
	{
		currentAspectRatio = (float)Screen.width / ((float)Screen.height + 0f);
		if (GameData.LOG)
		{
			Debug.Log("Screen: " + Screen.width + " x " + Screen.height + " aspect " + currentAspectRatio);
		}
		if (!(currentAspectRatio >= scaleMinimumLimit))
		{
			float x = currentAspectRatio / scaleMinimumLimit;
			Vector3 localScale = transform.localScale;
			float num = (localScale.x = x);
			Vector3 vector = (transform.localScale = localScale);
			if (GameData.LOG)
			{
				Debug.Log("Screen is under minimum aspect, scaling hud...");
			}
		}
	}
}
