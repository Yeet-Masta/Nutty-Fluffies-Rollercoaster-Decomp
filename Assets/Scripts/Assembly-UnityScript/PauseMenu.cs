using UnityEngine;

public class PauseMenu : VGUIElement
{
	public Transform pauseButton;

	private bool shown;

	public static bool handlebackKey = true;

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
		// No Tapjoy offer wall: hide its button and close the gap.
		transform.Find("TapjoyButton").gameObject.SetActive(false);
		Vector3 position = transform.Find("MissionButton").gameObject.transform.position;
		position.x = -0.1536663f;
		transform.Find("MissionButton").gameObject.transform.position = position;
		Vector3 position2 = transform.Find("HelpButton").gameObject.transform.position;
		position2.x = 0.1463337f;
		transform.Find("HelpButton").gameObject.transform.position = position2;
	}

	public override void deactivate()
	{
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
		base.deactivate();
	}

	private void setPauseDisable()
	{
		isPauseMenuEnable = false;
	}

	public virtual void onResumeGame()
	{
		if (shown)
		{
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
		GameManager.getInstance().endGame();
	}

	// Only printed a debug line; kept because the volume button calls it by name.
	public virtual void onAdjustVolume()
	{
	}

	public virtual void onRestartGame()
	{
		if (shown)
		{
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
			pauseButton.gameObject.SetActiveRecursively(true);
			TutorialManager.isTutorialFinished = false;
			TutorialManager.getInstance().showPage("controls", true);
			GameManager.isInPauseMenu = false;
		}
	}

	// The Tapjoy offer wall is gone (its button is hidden in activate); kept
	// because the scene's button calls it by name.
	public virtual void onShowOfferWall()
	{
	}

	public override void Update()
	{
		if (CustomInput.getKeyDown(27))
		{
			Input.ResetInputAxes();
			onResumeGame();
		}
	}
}
