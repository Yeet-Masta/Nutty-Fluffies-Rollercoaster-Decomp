using System;
using UnityEngine;

public class Bouncer : MonoBehaviour
{
	private Transform tr;

	public float animationFactor;

	public float duration;

	private bool triggered;

	private float phase;

	private Vector3 iconPosition;

	public virtual void Start()
	{
		triggered = false;
		tr = transform;
		iconPosition = tr.localPosition;
	}

	public virtual void bounce()
	{
		triggered = true;
		phase = 0f;
	}

	public virtual void Update()
	{
		if (triggered)
		{
			float y = tr.localPosition.y + Mathf.Sin(phase) * animationFactor;
			Vector3 localPosition = tr.localPosition;
			localPosition.y = y;
			tr.localPosition = localPosition;
			phase += Time.deltaTime * (float)Math.PI / duration;
			if (!(phase <= (float)Math.PI))
			{
				triggered = false;
				tr.localPosition = iconPosition;
			}
		}
	}
}
