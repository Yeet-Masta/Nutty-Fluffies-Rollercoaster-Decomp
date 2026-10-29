using UnityEngine;

public class TutorialManager : MonoBehaviour
{
	public static bool SHOW_TUTORIAL = true;

	public static bool TUTORIAL_DEBUG;

	public static float MINIMUM_PAGE_SHOWTIME = 1f;

	public static bool isTutorialFinished;

	public TutorialPage[] tutorialPages;

	public AlphaAnimator animator;

	public AlphaAnimator bganimator;

	public VGUIButton continueButton;

	public GameObject background;

	public static bool fromIGM;

	private static TutorialManager instance;

	private TutorialPage currentPage;

	private float currentPageStartShowTime;

	private string nextPage;

	private bool animating;

	private bool showAlways;

	public static bool isTutorialEnable;

	public virtual void Awake()
	{
		instance = this;
		gameObject.SetActiveRecursively(false);
		currentPageStartShowTime = Time.realtimeSinceStartup - MINIMUM_PAGE_SHOWTIME * 2f;
	}

	public virtual void Start()
	{
		isTutorialFinished = false;
		if (!SHOW_TUTORIAL)
		{
			foreach (TutorialPage tutorialPage in tutorialPages)
			{
				UnityEngine.Object.Destroy(tutorialPage.gameObject);
			}
		}
	}

	public static TutorialManager getInstance()
	{
		return instance;
	}

	public virtual bool showPage(string key)
	{
		return showPage(key, false);
	}

	public virtual bool showPage(string key, bool always)
	{
		int result;
		if (!SHOW_TUTORIAL)
		{
			return false;
		}
		else
		{
			currentPageStartShowTime = Time.realtimeSinceStartup;
			showAlways = always;
			if (!hasAllPagesShown(key) || TUTORIAL_DEBUG || always)
			{
				isTutorialEnable = true;
				currentPage = null;
				int num = 0;
				TutorialPage[] array = tutorialPages;
				int length = array.Length;
				while (true)
				{
					if (num < length)
					{
						if (array[num].key != key)
						{
							num++;
							continue;
						}
						currentPage = array[num];
						if (array[num].shown)
						{
							result = 0;
							break;
						}
					}
					if (currentPage != null)
					{
						currentPage.gameObject.SetActiveRecursively(true);
						gameObject.active = true;
						background.SetActiveRecursively(true);
						continueButton.activate();
						currentPage.activate();
						if (currentPage.pauseGame)
						{
							Time.timeScale = 0f;
						}
						animator.reverse = false;
						animator.activate();
						bganimator.reverse = false;
						bganimator.activate();
						animating = true;
						gameObject.active = true;
						if (GameData.LOG)
						{
							print("DISPLAYING TUTORIAL : " + key);
						}
					}
					else
					{
						Debug.LogError("UNKONWN TUTORIAL: " + key);
					}
					result = 1;
					break;
				}
			}
			else
			{
				onContinue();
				return false;
			}
		}
		return (byte)result != 0;
	}

	public virtual bool hasAllPagesShown(string key)
	{
		if (GameData.getActiveLevelPackIndex() == 0 && GameData.SELECTED_LEVEL_INDEX < 4)
		{
			return false;
		}
		else
		{
			int intValue = GameData.getSelectedLevel().settings.getIntValue("specialcart", Vehicle.CART_NORMAL);
			if (intValue != Vehicle.CART_NORMAL && PlayerPrefs.GetInt("TUTORIAL_SHOWN_PAGE_SPECIAL_" + intValue, 0) <= 2)
			{
				return false;
			}
			else
			{
				bool flag = PlayerPrefs.GetInt("TUTORIAL_SHOWN_PAGE_" + key, 0) > 0;
				return (flag && flag);
			}
		}
	}

	public virtual void onContinue()
	{
		if (!(Time.realtimeSinceStartup - currentPageStartShowTime >= MINIMUM_PAGE_SHOWTIME))
		{
			return;
		}
		if (GameData.LOG)
		{
			print("TUTORIAL - onContinue");
		}
		if (currentPage != null)
		{
			PlayerPrefs.SetInt("TUTORIAL_SHOWN_PAGE_" + currentPage.key, 1);
			int intValue = GameData.getSelectedLevel().settings.getIntValue("specialcart", Vehicle.CART_NORMAL);
			if (intValue != Vehicle.CART_NORMAL)
			{
				int num = PlayerPrefs.GetInt("TUTORIAL_SHOWN_PAGE_SPECIAL_" + intValue, 0);
				PlayerPrefs.SetInt("TUTORIAL_SHOWN_PAGE_SPECIAL_" + intValue, num + 1);
			}
			if (currentPage.showNextPageKey != null && currentPage.showNextPageKey != string.Empty && (!currentPage.showNextPageOnlyWithSpecial || (currentPage.showNextPageOnlyWithSpecial && GameData.getSpecialCartType() != Vehicle.CART_NORMAL)))
			{
				nextPage = currentPage.showNextPageKey;
				currentPage.gameObject.SetActiveRecursively(false);
			}
		}
		if (nextPage == null)
		{
			animator.reverse = true;
			animator.activate();
			bganimator.reverse = true;
			bganimator.activate();
			Time.timeScale = 1f;
			CancelInvoke("setTutorialDisable");
			Invoke("setTutorialDisable", 0.25f);
			if (GameManager.getInstance().getGameState() == 4)
			{
				GameManager.getInstance().stopGame();
			}
			else
			{
				GameManager.getInstance().setGameState(2);
			}
			nextPage = null;
			GameManager.handleBackKey = true;
		}
	}

	public virtual void onAnimationComplete()
	{
		if (animator.reverse)
		{
			gameObject.SetActiveRecursively(false);
			continueButton.deactivate();
			background.active = false;
		}
		animating = false;
	}

	private void setTutorialDisable()
	{
		isTutorialEnable = false;
		PauseMenu.isPauseMenuEnable = false;
	}

	public virtual void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (fromIGM)
			{
				animator.reverse = true;
				animator.activate();
				bganimator.reverse = true;
				bganimator.activate();
				Time.timeScale = 1f;
				setTutorialDisable();
				nextPage = null;
				isTutorialFinished = true;
				currentPage = null;
				GameManager.getInstance().pauseGame();
				fromIGM = false;
				return;
			}
			animator.reverse = true;
			animator.activate();
			setTutorialDisable();
			bganimator.reverse = true;
			bganimator.activate();
			Time.timeScale = 1f;
			if (GameManager.getInstance().getGameState() == 4)
			{
				GameManager.getInstance().stopGame();
			}
			else
			{
				GameManager.getInstance().setGameState(2);
			}
			nextPage = null;
		}
		if (!isTutorialFinished)
		{
			if (Input.anyKey && !animating)
			{
				onContinue();
			}
			else if (nextPage != null)
			{
				showPage(nextPage, true);
				nextPage = null;
			}
		}
	}

	public virtual bool isVisible()
	{
		return gameObject.active;
	}
}
