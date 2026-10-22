using System;
using UnityEngine;

[Serializable]
public class InfoMenu : BaseMenu
{
	public GameObject historyButton;

	public GFXToggleButton highVisualButton;

	public GFXToggleButton highSpeedButton;

	public override void activate()
	{
		base.activate();
		Screen.sleepTimeout = 0;
		BaseMenu.mmgr.setStoreButtonVisible(false);
		BaseMenu.mmgr.setTokenCounterVisible(false);
		highSpeedButton.setEnable(PlayerPrefs.GetInt("ANDROID_LOW_DEF") == 1);
		highVisualButton.setEnable(PlayerPrefs.GetInt("ANDROID_LOW_DEF") != 1);
		historyButton.SetActiveRecursively(GameData.isFeatureUnlocked(16384));
		BaseMenu.mmgr.logEvent("Info Menu");
	}

	public override void onBackButton()
	{
		BaseMenuManager.getInstance().onPreviousMenu();
	}

	public virtual void onVisualButtonPressed()
	{
		highSpeedButton.setEnable(false);
		highVisualButton.setEnable(true);
		PlayerPrefs.SetInt("ANDROID_LOW_DEF", 0);
		CommonData.reinit();
	}

	public virtual void onSpeedButtonPressed()
	{
		highSpeedButton.setEnable(true);
		highVisualButton.setEnable(false);
		PlayerPrefs.SetInt("ANDROID_LOW_DEF", 1);
		CommonData.reinit();
	}
}
