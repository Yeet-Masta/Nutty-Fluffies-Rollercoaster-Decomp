using System;
using UnityEngine;

[Serializable]
public class ScaleAnimator : VGUIAnimator
{
	public Vector2 from;

	public Vector2 to;

	public override void activate()
	{
		base.activate();
		float x = from.x;
		Vector3 localScale = target.localScale;
		float num = (localScale.x = x);
		Vector3 vector = (target.localScale = localScale);
		float y = from.y;
		Vector3 localScale2 = target.localScale;
		float num2 = (localScale2.y = y);
		Vector3 vector3 = (target.localScale = localScale2);
	}

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

	public override void deactivate()
	{
		if (!enabled)
		{
			return;
		}
		if (target != null)
		{
			if (reverse)
			{
				float x = from.x;
				Vector3 localScale = target.localScale;
				float num = (localScale.x = x);
				Vector3 vector = (target.localScale = localScale);
				float y = from.y;
				Vector3 localScale2 = target.localScale;
				float num2 = (localScale2.y = y);
				Vector3 vector3 = (target.localScale = localScale2);
			}
			else
			{
				float x2 = to.x;
				Vector3 localScale3 = target.localScale;
				float num3 = (localScale3.x = x2);
				Vector3 vector5 = (target.localScale = localScale3);
				float y2 = to.y;
				Vector3 localScale4 = target.localScale;
				float num4 = (localScale4.y = y2);
				Vector3 vector7 = (target.localScale = localScale4);
			}
		}
		base.deactivate();
	}
}
