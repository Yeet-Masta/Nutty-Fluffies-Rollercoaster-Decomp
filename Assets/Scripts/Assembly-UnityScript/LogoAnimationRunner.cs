using UnityEngine;

public class LogoAnimationRunner : MonoBehaviour
{
	public VGUIAnimator[] redlynxAnimators;

	public VGUIAnimator[] ubisoftAnimators;

	public BaseMenu loadingMenu;

	public virtual void Start()
	{
		Application.targetFrameRate = GameData.targetFPS;
		int i = 0;
		loadingMenu.gameObject.SetActiveRecursively(false);
		for (i = 0; i < redlynxAnimators.Length; i++)
		{
			redlynxAnimators[i].gameObject.active = false;
		}
		for (i = 0; i < ubisoftAnimators.Length; i++)
		{
			ubisoftAnimators[i].gameObject.active = false;
		}
		Invoke("startRedlynxAnim", 2.45f);
	}

	public virtual void startRedlynxAnim()
	{
		for (int i = 0; i < redlynxAnimators.Length; i++)
		{
			redlynxAnimators[i].gameObject.active = true;
			redlynxAnimators[i].activate();
		}
		Invoke("endRedlynxAnim", 1.75f);
	}

	public virtual void endRedlynxAnim()
	{
		int i = 0;
		for (i = 0; i < redlynxAnimators.Length; i++)
		{
			redlynxAnimators[i].deactivate();
			redlynxAnimators[i].reverse = true;
			redlynxAnimators[i].activate();
		}
		Invoke("startUbisoftAnim", 0.35f);
	}

	public virtual void startUbisoftAnim()
	{
		int i = 0;
		for (i = 0; i < redlynxAnimators.Length; i++)
		{
			redlynxAnimators[i].gameObject.active = false;
		}
		for (i = 0; i < ubisoftAnimators.Length; i++)
		{
			ubisoftAnimators[i].gameObject.active = true;
			ubisoftAnimators[i].activate();
		}
		Invoke("endUbisoftAnim", 1.75f);
	}

	public virtual void endUbisoftAnim()
	{
		for (int i = 0; i < ubisoftAnimators.Length; i++)
		{
			ubisoftAnimators[i].deactivate();
			ubisoftAnimators[i].reverse = true;
			ubisoftAnimators[i].activate();
		}
		Invoke("showLoading", 0.35f);
	}

	public virtual void showLoading()
	{
		int i = 0;
		for (i = 0; i < ubisoftAnimators.Length; i++)
		{
			ubisoftAnimators[i].gameObject.active = false;
		}
		loadingMenu.activate();
	}

	public virtual void continueToMenu()
	{
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
}
