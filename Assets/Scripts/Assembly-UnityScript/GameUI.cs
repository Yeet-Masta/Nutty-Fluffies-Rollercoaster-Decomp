using System;
using UnityEngine;

[Serializable]
public class GameUI : MonoBehaviour
{
	public ResultScreen resultScreen;

	public VGUIElement gameHud;

	public PauseMenu pauseMenu;

	public CrashScreen crashScreen;

	public AlphaAnimator fadeIn;

	public VGUIRunningValue coinCounter;

	public GameObject[] specialIcon;

	public Transform coinCounterPosition;

	public VGUIAnimator buttonAnimator;

	public Camera uiCamera;

	public ScaleAnimator specialItemAnimator;

	public GameObject finishBonusBadge;

	public MenuButton pauseButton;

	public int visibleScoreValue;

	public virtual void Awake()
	{
		MenuButton.clickAnimator = (ButtonPressAnimator)buttonAnimator;
		useGUILayout = false;
		resultScreen.deactivate();
		crashScreen.deactivate();
		finishBonusBadge.SetActiveRecursively(false);
	}

	public virtual void showResults()
	{
		pauseMenu.deactivate();
		resultScreen.activate(0f);
		gameHud.deactivate();
		finishBonusBadge.SetActiveRecursively(false);
		int i = 0;
		GameObject[] array = specialIcon;
		for (int length = array.Length; i < length; i++)
		{
			array[i].SetActiveRecursively(false);
		}
	}

	public virtual void showCrash()
	{
		crashScreen.activate();
	}

	public virtual void startGame()
	{
		resultScreen.deactivate();
		pauseMenu.deactivate();
		gameHud.activate();
		coinCounter.setValueImmediately(0);
		crashScreen.deactivate();
		fadeIn.reverse = false;
		fadeIn.duration = 0.4f;
		visibleScoreValue = 0;
		int i = 0;
		GameObject[] array = specialIcon;
		for (int length = array.Length; i < length; i++)
		{
			array[i].SetActiveRecursively(false);
		}
		finishBonusBadge.SetActiveRecursively(false);
		activatePauseButton(false);
	}

	public virtual void activatePauseButton(bool _active)
	{
		pauseButton.transform.gameObject.SetActive(_active);
	}

	public virtual void hideAll()
	{
		resultScreen.deactivate();
		pauseMenu.deactivate();
		gameHud.deactivate();
		crashScreen.deactivate();
		finishBonusBadge.SetActiveRecursively(false);
		int i = 0;
		GameObject[] array = specialIcon;
		for (int length = array.Length; i < length; i++)
		{
			array[i].SetActiveRecursively(false);
		}
	}

	public virtual void pauseGame()
	{
		fadeIn.gameObject.SetActiveRecursively(true);
		if (!pauseMenu.isActive())
		{
			pauseMenu.activate();
			crashScreen.deactivate();
		}
	}

	public virtual void resumeGame()
	{
	}

	public virtual void endGame()
	{
		fadeIn.gameObject.SetActiveRecursively(true);
		fadeIn.reverse = true;
		fadeIn.duration = 0.1f;
		fadeIn.activate();
		hideAll();
	}

	public virtual void updateScore(int score)
	{
		coinCounter.setText(string.Empty + score);
	}

	public virtual void addScore(int amount)
	{
		visibleScoreValue += amount;
		coinCounter.setText(string.Empty + visibleScoreValue);
	}

	public virtual void showSpecialActive(int specialType, int mode)
	{
		if (specialType > -1)
		{
			specialIcon[specialType].SetActiveRecursively(true);
			specialItemAnimator.target = specialIcon[specialType].transform;
			specialItemAnimator.reverse = mode == 0;
			specialItemAnimator.activate();
		}
	}

	public virtual void onRestartGame()
	{
		endGame();
		GameManager.getInstance().restart();
	}

	public virtual void onPauseGame()
	{
		if (!pauseMenu.isActive())
		{
			GameManager.getInstance().pauseGame();
		}
	}

	public virtual void showFinishBadge()
	{
		finishBonusBadge.SetActiveRecursively(true);
		pauseButton.deactivate();
	}
}
