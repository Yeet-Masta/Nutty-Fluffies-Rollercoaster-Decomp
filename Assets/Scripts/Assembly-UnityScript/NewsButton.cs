using System;

[Serializable]
public class NewsButton : VGUIButton
{
	public VGUIImage notifyImage;

	private bool hasNews;

	private bool viewed;

	private int frame;

	// NFPort: the HD/SD toggle in MenuScene reuses this script without a
	// notifyImage, which threw a NullReferenceException on every activation.
	public override void OnEnable()
	{
		viewed = false;
		if (notifyImage == null)
		{
			return;
		}
		if (!hasNews)
		{
			notifyImage.deactivate();
		}
		else
		{
			notifyImage.activate();
		}
	}

	public override void Update()
	{
		frame++;
		if (notifyImage != null && frame % 30 == 0 && !hasNews && !viewed && CrossPromotionAndroid.hasWebNews())
		{
			notifyImage.activate();
			hasNews = true;
		}
	}

	public override void OnMouseDown()
	{
		base.OnMouseDown();
		if (notifyImage != null)
		{
			notifyImage.deactivate();
		}
		hasNews = false;
		viewed = true;
	}
}
