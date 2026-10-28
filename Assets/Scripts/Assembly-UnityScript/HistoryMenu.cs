using System;

[Serializable]
public class HistoryMenu : BaseMenu
{
	public override void activate()
	{
		base.activate();
		BaseMenu.mmgr.setStoreButtonVisible(false);
		BaseMenu.mmgr.setTokenCounterVisible(false);
	}

	public override void onBackButton()
	{
		BaseMenuManager.getInstance().onPreviousMenu();
	}
}
