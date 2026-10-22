using System;

[Serializable]
public class AppearAnimator : VGUIAnimator
{
	public bool setVisible;

	public AppearAnimator()
	{
		setVisible = true;
	}

	protected override void activateInternal()
	{
		target.gameObject.SetActiveRecursively(setVisible);
		deactivate();
	}
}
