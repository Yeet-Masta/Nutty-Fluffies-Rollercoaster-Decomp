using System;
using UnityEngine;

[Serializable]
public class HeartCounter : MonoBehaviour
{
	public VGUITextTK2D scoreCount;

	public int currentValue;

	private int targetValue;

	public virtual void Awake()
	{
		reset();
	}

	public virtual void setValue(int value)
	{
		targetValue = value;
		valueInvoke();
	}

	private void valueInvoke()
	{
		currentValue++;
		scoreCount.setText(string.Empty + currentValue);
		if (currentValue < targetValue)
		{
			Invoke("valueInvoke", UnityEngine.Random.value * 0.1f);
		}
	}

	public virtual void reset()
	{
		currentValue = 0;
		scoreCount.setText(string.Empty + currentValue);
	}
}
