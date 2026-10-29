using UnityEngine;

public class HeartAnimator : VGUIAnimator
{
	public Vector3 from;

	public Vector3 to;

	public int heartValue;

	protected override void activateInternal()
	{
		enabled = true;
		from = Camera.main.WorldToViewportPoint(target.position);
		from = GameManager.getInstance().gameUI.uiCamera.ViewportToWorldPoint(from);
		to = GameManager.getInstance().gameUI.coinCounterPosition.position;
	}

	public override void Update()
	{
		float timeInterpolator = getTimeInterpolator();
		float num = 1f - timeInterpolator;
		num = 1f - num * num;
		float x = from.x + (to.x - from.x) * timeInterpolator;
		Vector3 position = target.position;
		position.x = x;
		target.position = position;
		float y = from.y + (to.y - from.y) * num;
		Vector3 position2 = target.position;
		position2.y = y;
		target.position = position2;
		float num4 = 0.75f + timeInterpolator * 0.75f;
		target.localScale = new Vector3(num4, num4, 1f);
		if (!(timeInterpolator < 1f))
		{
			deactivate();
			gameObject.SetActiveRecursively(false);
			GameManager.getInstance().gameUI.addScore(heartValue);
		}
	}
}
