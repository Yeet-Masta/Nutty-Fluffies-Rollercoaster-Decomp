using System;
using UnityEngine;

[Serializable]
public class VGUIRunningValue : VGUIText
{
	public float duration;

	public float stepDuration;

	public float initialDelay;

	public AudioClip stepSoundUp;

	public AudioClip stepSoundDown;

	public bool changingSoundPitch;

	private float startTime;

	private int valueFrom;

	private int valueTo;

	private float currentValue;

	private int direction;

	private int steps;

	private int totalSteps;

	private int stepTime;

	private float valueStep;

	private float soundPitch;

	public VGUIRunningValue()
	{
		duration = 0.5f;
		stepDuration = 0.03f;
		initialDelay = 1f;
		valueTo = 1;
		soundPitch = 1f;
	}

	public virtual float setValue(int from, int to)
	{
		valueFrom = from;
		valueTo = to;
		currentValue = from;
		if (to - from > 0)
		{
			direction = 1;
		}
		else
		{
			direction = -1;
		}
		steps = 0;
		totalSteps = Mathf.Abs(to - from);
		totalSteps = (int)Mathf.Min(totalSteps, Mathf.Floor(duration / stepDuration));
		if (totalSteps > 0)
		{
			valueStep = (to - from) / totalSteps;
		}
		setText(string.Empty + valueFrom);
		CancelInvoke();
		if (totalSteps > 0)
		{
			Invoke("updateValue", initialDelay);
		}
		soundPitch = 1f;
		return (float)totalSteps * stepDuration;
	}

	public virtual float setValue(int to)
	{
		return setValue((int)currentValue, to);
	}

	public virtual void setValueImmediately(int value)
	{
		setText(string.Empty + value);
		valueFrom = value;
		currentValue = value;
	}

	public virtual void updateValue()
	{
		steps++;
		float f = (float)valueFrom + (float)steps * valueStep;
		currentValue = Mathf.Round(f);
		if (steps <= totalSteps)
		{
			Invoke("updateValue", stepDuration);
		}
		else
		{
			currentValue = valueTo;
		}
		setText(string.Empty + currentValue);
		if (valueTo > valueFrom)
		{
			if (stepSoundUp != null)
			{
				GeneralSoundEffects.playSound(stepSoundUp, 1f, soundPitch);
			}
			if (changingSoundPitch && !(soundPitch >= 1.4f))
			{
				soundPitch += 0.05f;
			}
		}
		else if (stepSoundDown != null)
		{
			GeneralSoundEffects.playSound(stepSoundDown, 1f, soundPitch);
			if (changingSoundPitch && !(soundPitch <= 0.7f))
			{
				soundPitch -= 0.05f;
			}
		}
	}
}
