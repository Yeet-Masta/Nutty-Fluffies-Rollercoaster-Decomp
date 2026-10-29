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

	public string completeInvoke;

	public float activationDelay;

	[HideInInspector]
	public float startTime;

	public float duration;

	public Transform target;

	public bool reverse;

	public Interpolator interpolator;

	public AudioClip activationSound;

	private bool firstUpdate;

	public VGUIAnimator()
	{
		completeInvoke = "onAnimationComplete";
		duration = 1f;
		interpolator = Interpolator.LINEAR;
	}

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
		float num = (Time.realtimeSinceStartup - startTime) / duration;
		if (reverse)
		{
			num = 1f - num;
			if (!(num > 0f))
			{
				num = 0f;
			}
		}
		else if (!(num < 1f))
		{
			num = 1f;
		}
		float result;
		switch (interpolator)
		{
		case Interpolator.LINEAR:
			result = num;
			break;
		case Interpolator.OVERSHOOT:
			result = getOvershootEaseOutInterpolator(num);
			break;
		case Interpolator.EASE_IN_OUT:
			result = 0.5f - Mathf.Cos(num * (float)Math.PI) * 0.5f;
			break;
		default:
			result = num;
			break;
		}
		return result;
	}

	public virtual void checkComplete(float x)
	{
		float num = (Time.realtimeSinceStartup - startTime) / duration;
		if ((!(num < 1f) && !reverse) || (!(1f - num > 0f) && reverse))
		{
			deactivate();
		}
	}

	public virtual void Update()
	{
	}
}
