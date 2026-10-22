using System;
using UnityEngine;

[Serializable]
public class PauseMenu : VGUIElement
{
	public Transform pauseButton;

	private bool shown;

	private bool tapjoyOWOpened;

	[NonSerialized]
	public static bool handlebackKey = true;

	[NonSerialized]
	public static bool isPauseMenuEnable;

	public override void activate()
	{
		base.activate();
		VGUIElement.blockingButtonPressed = false;
		handlebackKey = true;
		shown = true;
		isPauseMenuEnable = true;
		AnimationManager.getInstance().fadeIn(transform);
		pauseButton.gameObject.SetActiveRecursively(false);
		CrossPromotionAndroid.setInfoBadgeVisibility(true);
		if (BaseMenuManager.getInstance() != null)
		{
			BaseMenuManager.getInstance().logEvent("Pause Menu");
		}
		if (GameData.IS_TAPJOY_ENABLED)
		{
			if (GameData.LOG)
			{
				Debug.Log("Anji display ad is called from activate pause menu......");
			}
			TapjoyAdController.getInstance().HideAd();
			TapjoyAdController.getInstance().DisplayAd(false, "RC_PAUSEMENU_AD");
		}
		else
		{
			transform.Find("TapjoyButton").gameObject.SetActive(false);
			float x = -0.1536663f;
			Vector3 position = transform.Find("MissionButton").gameObject.transform.position;
			float num = (position.x = x);
			Vector3 vector = (transform.Find("MissionButton").gameObject.transform.position = position);
			float x2 = 0.1463337f;
			Vector3 position2 = transform.Find("HelpButton").gameObject.transform.position;
			float num2 = (position2.x = x2);
			Vector3 vector3 = (transform.Find("HelpButton").gameObject.transform.position = position2);
		}
	}

	public override void deactivate()
	{
		CrossPromotionAndroid.setInfoBadgeVisibility(false);
		if (GameData.IS_TAPJOY_ENABLED)
		{
			if (GameData.LOG)
			{
				Debug.Log("Anji hide tapjoy ad is called in deactivate pause menu....");
			}
			TapjoyAdController.getInstance().HideAd();
		}
		Invoke("setPauseDisable", 0.25f);
		if (shown)
		{
			AnimationManager.getInstance().fadeOut(transform);
			pauseButton.gameObject.SetActiveRecursively(true);
			Invoke("deactiveInvoke", 0.2f);
			shown = false;
		}
		else
		{
			base.deactivate();
		}
		shown = false;
	}

	public virtual void deactiveInvoke()
	{
		if (GameData.IS_TAPJOY_ENABLED)
		{
			if (GameData.LOG)
			{
				Debug.Log("Hide Ad will be called from deactivate pausemenu....");
			}
			TapjoyAdController.getInstance().HideAd();
		}
		base.deactivate();
	}

	private void setPauseDisable()
	{
		if (GameData.LOG)
		{
			Debug.Log("ting  SetPause disabled called ****************************");
		}
		isPauseMenuEnable = false;
	}

	public virtual void onResumeGame()
	{
		if (shown)
		{
			MonoBehaviour.print("onResume");
			deactivate();
			GameManager.getInstance().resumeGame();
		}
	}

	public virtual void onEndGame()
	{
		if (!shown)
		{
			return;
		}
		if (GameData.IS_TAPJOY_ENABLED)
		{
			TapjoyAdController.getInstance().HideAd();
			if (!CrossPromotionAndroid.canShowUrgentNews())
			{
				TapjoyAdController.getInstance().DisplayFullScreenAd("RC_BACK_TO_MENU_FROM_GAME");
			}
		}
		MonoBehaviour.print("onEnd");
		GameManager.getInstance().endGame();
	}

	public virtual void onAdjustVolume()
	{
		MonoBehaviour.print("onAdjustVolume");
	}

	public virtual void onRestartGame()
	{
		if (shown)
		{
			MonoBehaviour.print("onRestart");
			GameManager.getInstance().restart();
		}
	}

	public virtual void onMissions()
	{
		if (shown)
		{
			deactiveInvoke();
			shown = false;
			handlebackKey = false;
			CrossPromotionAndroid.setInfoBadgeVisibility(false);
			GameMenuManager.getInstance().displayMissionsIngame();
		}
	}

	public virtual void onHelp()
	{
		if (shown)
		{
			deactiveInvoke();
			handlebackKey = false;
			TutorialManager.fromIGM = true;
			CrossPromotionAndroid.setInfoBadgeVisibility(false);
			pauseButton.gameObject.SetActiveRecursively(true);
			TutorialManager.isTutorialFinished = false;
			TutorialManager.getInstance().showPage("controls", true);
			GameManager.isInPauseMenu = false;
		}
	}

	public virtual void onShowOfferWall()
	{
		if (GameData.IS_TAPJOY_ENABLED)
		{
			TapjoyPlugin.ShowOffers();
			Invoke("enableTapjoyOfferwall", 2f);
		}
	}

	private void enableTapjoyOfferwall()
	{
		tapjoyOWOpened = true;
	}

	public override void Update()
	{
		if (CustomInput.getKeyDown(27))
		{
			Input.ResetInputAxes();
			onResumeGame();
		}
		else if (tapjoyOWOpened)
		{
			BaseMenuManager.getInstance().showLoadingScreen(false);
			tapjoyOWOpened = false;
			TapjoyAdController.getInstance().GetTapPoints();
		}
	}
}
