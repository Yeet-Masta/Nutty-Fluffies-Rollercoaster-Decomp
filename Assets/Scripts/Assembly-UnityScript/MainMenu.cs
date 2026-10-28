using System;
using UnityEngine;

[Serializable]
public class MainMenu : BaseMenu
{
	[NonSerialized]
	private static bool loginCalled;

	public VGUIButton newsButton;

	public VGUIButton qualityToggleButton;

	public override void activate()
	{
		base.activate();
		Screen.sleepTimeout = 0;
		MenuManager.getInstance().versionString.gameObject.SetActiveRecursively(true);
		qualityToggleButton.gameObject.SetActive(false);
		newsButton.gameObject.SetActive(GameData.hasInternet());
		if (!loginCalled)
		{
			OnlineServiceManager.getInstance().login();
			loginCalled = true;
		}
	}

	public override void deactivate()
	{
		base.deactivate();
		MenuManager.getInstance().versionString.gameObject.SetActiveRecursively(false);
	}

	// NFPort: Esc used to be read here, in ExitMenu.Update and in
	// BaseMenuManager.Update in the same frame, so the outcome depended on
	// script update order (the exit dialog could open and close in one
	// frame, or stay open invisibly). BaseMenuManager now routes Esc here.
	public override void onBackButton()
	{
		ExitMenu.getInstance().ToggleExitDialog();
	}
}
