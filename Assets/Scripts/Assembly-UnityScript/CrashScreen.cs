using UnityEngine;

public class CrashScreen : VGUIElement
{
	public ScaleAnimator scaleAnimator;

	public AlphaAnimator alphaAnimator;

	public override void activate()
	{
		Invoke("screenInvoke", 1f);
	}

	public override void deactivate()
	{
		base.deactivate();
		CancelInvoke();
	}

	public virtual void screenInvoke()
	{
		base.activate();
		scaleAnimator.activate();
		alphaAnimator.activate();
	}

	public virtual void onRestart()
	{
		MonoBehaviour.print("RESTARTING");
		GameManager.getInstance().restart();
	}

	public override void Update()
	{
		if (Input.anyKey)
		{
			GameManager.getInstance().restart();
		}
	}
}
