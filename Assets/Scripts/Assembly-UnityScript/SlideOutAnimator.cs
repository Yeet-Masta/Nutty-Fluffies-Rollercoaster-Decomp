using System;
using UnityEngine;

[Serializable]
public class SlideOutAnimator : VGUIAnimator
{
	private Vector3 from;

	public Vector3 to;

	private Vector3 animTo;

	protected override void activateInternal()
	{
		from = target.localPosition;
		animTo = from;
		animTo += to;
	}

	public override void Update()
	{
		float timeInterpolator = getTimeInterpolator();
		if (target != null)
		{
			target.localPosition = from + (animTo - from) * timeInterpolator;
		}
		checkComplete(timeInterpolator);
	}
}
