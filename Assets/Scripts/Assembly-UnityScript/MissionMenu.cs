using System.Collections.Generic;
using UnityEngine;

public class MissionMenu : BaseMenu
{
	public static bool WAS_FORCED_TO_WATCH;

	public static bool ENTERED_FROM_PAUSE_MENU;

	public static bool GOING_TO_MENU;

	public MissionList missionList;

	private bool describingFeatures;

	private bool wasActive;

	private bool handleBackKey = true;

	public override void activate()
	{
		base.activate();
		handleBackKey = true;
		List<Mission> missionsForMenu = RollerCoasterMissions.getInstance().getMissionsForMenu(GameData.MAX_ACTIVE_MISSIONS);
		if (!WAS_FORCED_TO_WATCH)
		{
			nextMenu = MenuManager.getInstance().mainMenu;
			WAS_FORCED_TO_WATCH = false;
		}
		if (missionsForMenu.Count > 0)
		{
			missionList.activate();
		}
		BaseMenu.mmgr.setTokenCounterVisible(true);
		BaseMenu.mmgr.setStoreButtonVisible(true);
		wasActive = true;
	}

	public override void deactivate()
	{
		if (wasActive)
		{
			missionList.deactivate();
		}
		base.deactivate();
		ENTERED_FROM_PAUSE_MENU = false;
		wasActive = false;
	}

	public virtual void onContinue()
	{
		if (GameData.LOG)
		{
			Debug.Log(" MissionMenu ");
		}
		if ((GameData.getNewFeaturesList() & -8193) != 0 && !describingFeatures)
		{
			BaseMenuManager.getInstance().descriptNewFeatures();
			describingFeatures = true;
			return;
		}
		describingFeatures = false;
		if (ENTERED_FROM_PAUSE_MENU)
		{
			GameMenuManager.getInstance().continueFromMissions();
			BaseMenuManager.getInstance().setMenuTitle(null);
			ENTERED_FROM_PAUSE_MENU = false;
		}
		else if (GOING_TO_MENU)
		{
			GameManager.getInstance().endGame();
		}
		else
		{
			BaseMenuManager.getInstance().onNextMenu();
		}
	}

	public override void onMessageBoxClosed(bool dismissed)
	{
		if (describingFeatures)
		{
			onContinue();
		}
	}

	public override void Update()
	{
		if (CustomInput.getKeyDown(27) && handleBackKey)
		{
			onContinue();
		}
	}
}
