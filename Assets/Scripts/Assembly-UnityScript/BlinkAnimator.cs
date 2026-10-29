using System;
using UnityEngine;

public class BlinkAnimator : VGUIAnimator
{
	public float blinkRate;

	private float initialScale;

	public float scaleFactor;

	public bool selfActivate = true;

	private bool activated;

	public override void Awake()
	{
		base.Awake();
		enabled = true;
		if (target != null)
		{
			initialScale = target.localScale.x;
		}
		else
		{
			initialScale = 1f;
		}
		activated = false;
		if (selfActivate)
		{
			Invoke("activate", blinkRate + activationDelay);
		}
	}

	public override void activate()
	{
		startTime = Time.realtimeSinceStartup;
		activated = true;
		enabled = true;
	}

	public override void deactivate()
	{
		base.deactivate();
		if (target != null)
		{
			float x = initialScale;
			Vector3 localScale = target.localScale;
			localScale.x = x;
			target.localScale = localScale;
			float y = initialScale;
			Vector3 localScale2 = target.localScale;
			localScale2.y = y;
			target.localScale = localScale2;
		}
	}

	public override void Update()
	{
		if (activated)
		{
			float progress = (Time.realtimeSinceStartup - startTime) / duration;
			if (!(progress <= 1f))
			{
				Invoke("activate", blinkRate);
				activated = false;
				progress = 1f;
			}
			float scale = initialScale + (0.5f - Mathf.Cos(progress * (float)Math.PI * 2f) * 0.5f) * (scaleFactor - initialScale);
			if (target != null)
			{
				float x = scale;
				Vector3 localScale = target.localScale;
				localScale.x = x;
				target.localScale = localScale;
				float y = scale;
				Vector3 localScale2 = target.localScale;
				localScale2.y = y;
				target.localScale = localScale2;
			}
		}
	}
}
