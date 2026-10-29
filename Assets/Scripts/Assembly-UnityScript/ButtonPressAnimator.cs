using System;
using UnityEngine;

public class ButtonPressAnimator : VGUIAnimator
{
	private float origScale;

	private int origScaleStorePoint = -1;

	private Transform origScaleSource;

	public float factor = 1f;

	public AudioClip pressClick;

	public AudioClip releaseClick;

	protected override void activateInternal()
	{
		if (!reverse)
		{
			origScale = target.localScale.x;
			origScaleSource = target;
			origScaleStorePoint = 0;
		}
		playSound(!reverse);
	}

	public virtual void playSound(bool down)
	{
		if (!down)
		{
			if (releaseClick != null)
			{
				GeneralSoundEffects.playSound(releaseClick);
			}
		}
		else if (pressClick != null)
		{
			GeneralSoundEffects.playSound(pressClick);
		}
	}

	public override void Update()
	{
		float timeInterpolator = getTimeInterpolator();
		if (target != null)
		{
			float num = (1f - Mathf.Sin(timeInterpolator * (float)Math.PI * 0.5f) * factor) * origScale;
			float x = num;
			Vector3 localScale = target.localScale;
			localScale.x = x;
			target.localScale = localScale;
			float y = num;
			Vector3 localScale2 = target.localScale;
			localScale2.y = y;
			target.localScale = localScale2;
		}
		checkComplete(timeInterpolator);
	}

	public override void deactivate()
	{
		base.deactivate();
		if (reverse)
		{
			restore();
		}
	}

	public virtual void restore()
	{
		if (target != null && origScale != 0f)
		{
			float x = origScale;
			Vector3 localScale = target.localScale;
			localScale.x = x;
			target.localScale = localScale;
			float y = origScale;
			Vector3 localScale2 = target.localScale;
			localScale2.y = y;
			target.localScale = localScale2;
			if (target != origScaleSource)
			{
				print("RESTORING ORIG SCALE TO OBJECT " + target.name + " WHICH WAS ORIGINALLY STORED FROM " + origScaleSource.name + ", STOREPOINT: " + origScaleStorePoint);
			}
			else if (origScaleSource == null)
			{
				print("TARGET WAS SETTED FROM OUTSIDE: " + target.name);
			}
		}
	}

	public virtual void setTarget(Transform target)
	{
		base.target = target;
		origScale = target.localScale.x;
		origScaleSource = target;
		origScaleStorePoint = 1;
	}
}
