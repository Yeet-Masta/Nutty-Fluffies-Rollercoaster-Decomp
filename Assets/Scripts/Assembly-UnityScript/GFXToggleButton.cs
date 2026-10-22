using System;

[Serializable]
public class GFXToggleButton : MenuButton
{
	public VGUIImagesTK2D frames;

	public override void OnEnable()
	{
	}

	public virtual void setEnable(bool enable)
	{
		if (enable)
		{
			frames.setImage(2);
		}
		else
		{
			frames.setImage(1);
		}
	}
}
