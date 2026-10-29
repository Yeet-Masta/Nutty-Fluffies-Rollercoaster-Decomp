using UnityEngine;
using UnityScript.Lang;

public class StoryMenu : BaseMenu
{
	public BaseMenu loadingMenu;

	public Transform scroller;

	public GameObject[] cameraFlash;

	public GameObject newsPaper;

	public ScaleAnimator flashAnimator;

	public SlideInAnimator translateAnimator;

	public NewsPaperAnimator newsPaperAnimator;

	public SlideInAnimator reporterScrollAnimator;

	public GameObject clickBlocker;

	public GameObject[] storyPanels;

	public AudioClip[] storyPanelAudios;

	public AudioClip storyBackgroundAudio;

	private int currentPanel;

	private float originalMusicVolume;

	private Vector3 originalPosition;

	private Vector3 originalScale;

	public virtual void Start()
	{
		loadingMenu.deactivate();
		activate();
	}

	public override void activate()
	{
		base.activate();
		Screen.sleepTimeout = -1;
		gameObject.SetActiveRecursively(false);
		gameObject.active = true;
		clickBlocker.SetActiveRecursively(true);
		currentPanel = -1;
		showNextPanel();
		PlayerPrefs.SetInt("STORY_DISPLAYED", 1);
		PlayerPrefs.Save();
		GetComponent<AudioSource>().clip = storyBackgroundAudio;
		GetComponent<AudioSource>().loop = true;
		GetComponent<AudioSource>().Play();
		loadingMenu.deactivate();
	}

	public virtual void showNextPanel()
	{
		ScaleAnimator scaleAnimator = null;
		SlideOutAnimator slideOutAnimator = null;
		if (currentPanel > -1)
		{
			storyPanels[currentPanel].transform.position = originalPosition;
			storyPanels[currentPanel].transform.localScale = originalScale;
			storyPanels[currentPanel].SetActiveRecursively(false);
			scaleAnimator = (ScaleAnimator)storyPanels[currentPanel].GetComponent(typeof(ScaleAnimator));
			if (scaleAnimator != null)
			{
				scaleAnimator.deactivate();
			}
			slideOutAnimator = (SlideOutAnimator)storyPanels[currentPanel].GetComponent(typeof(SlideOutAnimator));
			if (slideOutAnimator != null)
			{
				slideOutAnimator.deactivate();
			}
		}
		if (currentPanel == storyPanels.Length - 1)
		{
			showLoading();
			return;
		}
		currentPanel++;
		storyPanels[currentPanel].SetActiveRecursively(true);
		originalScale = storyPanels[currentPanel].transform.localScale;
		originalPosition = storyPanels[currentPanel].transform.position;
		scaleAnimator = (ScaleAnimator)storyPanels[currentPanel].GetComponent(typeof(ScaleAnimator));
		if (scaleAnimator != null)
		{
			scaleAnimator.activate();
		}
		slideOutAnimator = (SlideOutAnimator)storyPanels[currentPanel].GetComponent(typeof(SlideOutAnimator));
		if (slideOutAnimator != null)
		{
			slideOutAnimator.activate();
		}
		if (storyPanelAudios[currentPanel] != null)
		{
			GeneralSoundEffects.playSound(storyPanelAudios[currentPanel]);
		}
		float time = 3.25f;
		if (currentPanel == 0)
		{
			time = 5.25f;
		}
		if (currentPanel == 2)
		{
			time = 3.25f;
		}
		if (currentPanel == 3)
		{
			time = 4.25f;
		}
		Invoke("showNextPanel", time);
	}

	public override void deactivate()
	{
		CancelInvoke();
		base.deactivate();
		GetComponent<AudioSource>().Stop();
		if (originalMusicVolume != 0f)
		{
			BaseMenu.mmgr.setMusicVolume(originalMusicVolume);
		}
	}

	public virtual void showLoading()
	{
		loadingMenu.activate();
	}

	public virtual void continueToMenu()
	{
		deactivate();
		Application.LoadLevel("MenuScene");
	}

	public override void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) || NFPortInput.TapEnded())
		{
			showLoading();
			Invoke("continueToMenu", 0.5f);
		}
	}

	public virtual void onLoadingFadeIn()
	{
		if (currentPanel > 0)
		{
			continueToMenu();
		}
	}
}
