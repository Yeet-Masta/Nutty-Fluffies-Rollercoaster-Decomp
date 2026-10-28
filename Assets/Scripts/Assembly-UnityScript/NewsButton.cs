using System;

[Serializable]
public class NewsButton : VGUIButton
{
	public VGUIImage notifyImage;

	// The "new news" badge was lit by the Ubisoft cross-promotion service,
	// which is gone, so it always stays hidden. The HD/SD toggle in MenuScene
	// reuses this script without a notifyImage.
	public override void OnEnable()
	{
		if (notifyImage != null)
		{
			notifyImage.deactivate();
		}
	}
}
