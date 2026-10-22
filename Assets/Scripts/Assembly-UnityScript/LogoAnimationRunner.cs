using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class LogoAnimationRunner : MonoBehaviour
{
	public VGUIAnimator[] redlynxAnimators;

	public VGUIAnimator[] ubisoftAnimators;

	public BaseMenu loadingMenu;

	public virtual void Start()
	{
		Application.targetFrameRate = GameData.targetFPS;
		int num = default(int);
		loadingMenu.gameObject.SetActiveRecursively(false);
		for (num = 0; num < Extensions.get_length((System.Array)redlynxAnimators); num++)
		{
			redlynxAnimators[num].gameObject.active = false;
		}
		for (num = 0; num < Extensions.get_length((System.Array)ubisoftAnimators); num++)
		{
			ubisoftAnimators[num].gameObject.active = false;
		}
		Invoke("startRedlynxAnim", 2.45f);
	}

	public virtual void startRedlynxAnim()
	{
		for (int i = 0; i < Extensions.get_length((System.Array)redlynxAnimators); i++)
		{
			redlynxAnimators[i].gameObject.active = true;
			redlynxAnimators[i].activate();
		}
		Invoke("endRedlynxAnim", 1.75f);
	}

	public virtual void endRedlynxAnim()
	{
		int num = default(int);
		for (num = 0; num < Extensions.get_length((System.Array)redlynxAnimators); num++)
		{
			redlynxAnimators[num].deactivate();
			redlynxAnimators[num].reverse = true;
			redlynxAnimators[num].activate();
		}
		Invoke("startUbisoftAnim", 0.35f);
	}

	public virtual void startUbisoftAnim()
	{
		int num = default(int);
		for (num = 0; num < Extensions.get_length((System.Array)redlynxAnimators); num++)
		{
			redlynxAnimators[num].gameObject.active = false;
		}
		for (num = 0; num < Extensions.get_length((System.Array)ubisoftAnimators); num++)
		{
			ubisoftAnimators[num].gameObject.active = true;
			ubisoftAnimators[num].activate();
		}
		Invoke("endUbisoftAnim", 1.75f);
	}

	public virtual void endUbisoftAnim()
	{
		for (int i = 0; i < Extensions.get_length((System.Array)ubisoftAnimators); i++)
		{
			ubisoftAnimators[i].deactivate();
			ubisoftAnimators[i].reverse = true;
			ubisoftAnimators[i].activate();
		}
		Invoke("showLoading", 0.35f);
	}

	public virtual void showLoading()
	{
		int num = default(int);
		for (num = 0; num < Extensions.get_length((System.Array)ubisoftAnimators); num++)
		{
			ubisoftAnimators[num].gameObject.active = false;
		}
		loadingMenu.activate();
	}

	public virtual void continueToMenu()
	{
		if (PlayerPrefs.GetInt("DisableTapjoy") == 1)
		{
			GameData.IS_TAPJOY_ENABLED = false;
		}
		else if (GameData.USE_AMAZON_IAP)
		{
			GameData.IS_TAPJOY_ENABLED = false;
		}
		else
		{
			GameData.IS_TAPJOY_ENABLED = false;
		}
		if (PlayerPrefs.GetInt("STORY_DISPLAYED", 0) != 0)
		{
			Application.LoadLevel("MenuScene");
		}
		else
		{
			Application.LoadLevel("StoryScene");
		}
	}

	public virtual void onLoadingFadeIn()
	{
		continueToMenu();
	}

	public virtual void Main()
	{
	}
}
