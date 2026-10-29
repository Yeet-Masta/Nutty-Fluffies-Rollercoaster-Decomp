using UnityEngine;

public class LevelPackButton : MenuButton
{
	public VGUIText levelName;

	public VGUIText progress;

	public GameObject @lock;

	public bool extraPack;

	public override void setLocked(bool @lock)
	{
		base.setLocked(@lock);
		this.@lock.GetComponent<Renderer>().enabled = @lock;
	}

	public override void clickButton()
	{
		base.clickButton();
		(MenuButton.clickAnimator).restore();
		if (isLocked() && !pressed && !VGUIElement.blockingButtonPressed)
		{
			string message = TextData.LEVELPACK_TEXT_LOCKED.Replace("%d", string.Empty + GameData.CARTS_TO_UNLOCK_NEXT_LEVEL);
			if (extraPack)
			{
				message = TextData.LEVELPACK_EXTRA_TEXT_LOCKED;
			}
			MenuManager.getInstance().showErrorMessage(TextData.LEVELPACK_TITLE_LOCKED, message, MenuManager.getInstance().featureImages[0]);
		}
	}
}
