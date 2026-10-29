using UnityEngine;

public class VGUIRunningValue : VGUIText
{
	public float duration = 0.5f;

	public float stepDuration = 0.03f;

	public float initialDelay = 1f;

	public AudioClip stepSoundUp;

	public AudioClip stepSoundDown;

	public bool changingSoundPitch;

	private int valueFrom;

	private int valueTo = 1;

	private float currentValue;

	private int steps;

	private int totalSteps;

	private float valueStep;

	private float soundPitch = 1f;

	public virtual float setValue(int from, int to)
	{
		valueFrom = from;
		valueTo = to;
		currentValue = from;
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
