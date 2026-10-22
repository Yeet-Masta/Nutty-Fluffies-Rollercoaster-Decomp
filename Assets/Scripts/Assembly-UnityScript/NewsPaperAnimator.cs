using System;
using UnityEngine;

[Serializable]
public class NewsPaperAnimator : VGUIAnimator
{
	private Vector3 to;

	private Vector3 animFrom;

	public float startAngle;

	public NewsPaperAnimator()
	{
		startAngle = 2400f;
	}

	public override void activate()
	{
		base.activate();
		target.localScale = new Vector3(0f, 0f, 1f);
	}

	protected override void activateInternal()
	{
		target.gameObject.SetActiveRecursively(true);
	}

	public override void Update()
	{
		float timeInterpolator = getTimeInterpolator();
		Vector2 vector = Vector3.one * timeInterpolator;
		float x = vector.x * 1f;
		Vector3 localScale = target.localScale;
		float num = (localScale.x = x);
		Vector3 vector2 = (target.localScale = localScale);
		float y = vector.y * 1f;
		Vector3 localScale2 = target.localScale;
		float num2 = (localScale2.y = y);
		Vector3 vector4 = (target.localScale = localScale2);
		float z = startAngle * (1f - timeInterpolator);
		Vector3 localEulerAngles = target.localEulerAngles;
		float num3 = (localEulerAngles.z = z);
		Vector3 vector6 = (target.localEulerAngles = localEulerAngles);
		checkComplete(timeInterpolator);
	}
}
