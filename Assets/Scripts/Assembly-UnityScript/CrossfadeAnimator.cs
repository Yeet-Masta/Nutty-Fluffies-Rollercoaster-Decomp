using System;
using UnityEngine;

[Serializable]
public class CrossfadeAnimator : VGUIAnimator
{
	public Vector2 from;

	public Vector2 to;

	protected override void activateInternal()
	{
	}

	public override void Update()
	{
		float timeInterpolator = getTimeInterpolator();
		Vector2 vector = from + (to - from) * timeInterpolator;
		float x = vector.x;
		Vector3 localScale = target.localScale;
		float num = (localScale.x = x);
		Vector3 vector2 = (target.localScale = localScale);
		float y = vector.y;
		Vector3 localScale2 = target.localScale;
		float num2 = (localScale2.y = y);
		Vector3 vector4 = (target.localScale = localScale2);
		checkComplete(timeInterpolator);
	}
}
