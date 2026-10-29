using UnityEngine;

public class NewsPaperAnimator : VGUIAnimator
{
	public float startAngle = 2400f;

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
		Vector2 scale = Vector3.one * timeInterpolator;
		float x = scale.x * 1f;
		Vector3 localScale = target.localScale;
		localScale.x = x;
		target.localScale = localScale;
		float y = scale.y * 1f;
		Vector3 localScale2 = target.localScale;
		localScale2.y = y;
		target.localScale = localScale2;
		float z = startAngle * (1f - timeInterpolator);
		Vector3 localEulerAngles = target.localEulerAngles;
		localEulerAngles.z = z;
		target.localEulerAngles = localEulerAngles;
		checkComplete(timeInterpolator);
	}
}
