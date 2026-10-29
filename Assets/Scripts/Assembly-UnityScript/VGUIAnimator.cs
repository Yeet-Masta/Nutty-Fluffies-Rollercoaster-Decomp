using System;
using UnityEngine;

public class VGUIAnimator : MonoBehaviour
{
	[Serializable]
	public enum Interpolator
	{
		LINEAR = 0,
		OVERSHOOT = 1,
		EASE_IN_OUT = 2
	}

	public GameObject invokeRef;

	public string completeInvoke = "onAnimationComplete";

	public float activationDelay;

	[HideInInspector]
	public float startTime;

	public float duration = 1f;

	public Transform target;

	public bool reverse;

	public Interpolator interpolator = Interpolator.LINEAR;

	public AudioClip activationSound;

	private bool firstUpdate;

	public virtual void Awake()
	{
		useGUILayout = false;
		enabled = false;
	}

	public virtual void activate()
	{
		if (enabled)
		{
			deactivate();
		}
		if (target == null)
		{
			target = transform;
		}
		startTime = Time.realtimeSinceStartup;
		firstUpdate = true;
		if (!(activationDelay <= 0f))
		{
			Invoke("invokeActivation", activationDelay);
		}
		else
		{
			invokeActivation();
		}
	}

	protected virtual void invokeActivation()
	{
		startTime = Time.realtimeSinceStartup;
		if (target == null)
		{
			Debug.LogWarning("ANIMATION WITH NULL TARGET");
			enabled = false;
			return;
		}
		enabled = true;
		activateInternal();
		if (activationSound != null)
		{
			GeneralSoundEffects.playSound(activationSound);
		}
		Update();
	}

	public virtual void activate(Transform target)
	{
		this.target = target;
		activate();
	}

	protected virtual void activateInternal()
	{
		deactivate();
	}

	public virtual void deactivate()
	{
		CancelInvoke();
		if (enabled)
		{
			enabled = false;
			if (invokeRef != null)
			{
				invokeRef.SendMessage(completeInvoke);
			}
		}
	}

	protected virtual float getOvershootEaseOutInterpolator(float interpolator)
	{
		return interpolator + (1f - interpolator) * 2f * interpolator;
	}

	public virtual float getTimeInterpolator()
	{
		if (firstUpdate)
		{
			startTime = Time.realtimeSinceStartup;
			firstUpdate = false;
		}
		float t = (Time.realtimeSinceStartup - startTime) / duration;
		if (reverse)
		{
			t = 1f - t;
			if (!(t > 0f))
			{
				t = 0f;
			}
		}
		else if (!(t < 1f))
		{
			t = 1f;
		}
		switch (interpolator)
		{
		case Interpolator.LINEAR:
			return t;
		case Interpolator.OVERSHOOT:
			return getOvershootEaseOutInterpolator(t);
		case Interpolator.EASE_IN_OUT:
			return 0.5f - Mathf.Cos(t * (float)Math.PI) * 0.5f;
		default:
			return t;
		}
	}

	public virtual void checkComplete(float x)
	{
		float t = (Time.realtimeSinceStartup - startTime) / duration;
		if ((!(t < 1f) && !reverse) || (!(1f - t > 0f) && reverse))
		{
			deactivate();
		}
	}

	public virtual void Update()
	{
	}
}
