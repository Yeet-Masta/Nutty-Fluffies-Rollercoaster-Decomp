using System;
using UnityEngine;

[Serializable]
public class SlideInAnimator : VGUIAnimator
{
	public Vector3 from;

	private Vector3 to;

	private Vector3 animFrom;

	private bool wasActive;

	protected override void activateInternal()
	{
		to = target.localPosition;
		animFrom = to;
		animFrom += from;
		wasActive = true;
		target.localPosition = animFrom;
	}

	public override void Update()
	{
		float timeInterpolator = getTimeInterpolator();
		if (target != null)
		{
			target.localPosition = animFrom + (to - animFrom) * timeInterpolator;
		}
		checkComplete(timeInterpolator);
	}

	public override void deactivate()
	{
		if (target != null && wasActive)
		{
			target.localPosition = to;
			wasActive = false;
		}
		base.deactivate();
	}
}
