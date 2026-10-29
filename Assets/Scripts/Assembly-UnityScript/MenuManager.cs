using UnityEngine;

public class MenuManager : BaseMenuManager
{
	private static bool WAS_PLAYING;

	private static bool WAS_IN_STORY;

	private static bool ASPECT_INITED;

	public MainMenu mainMenu;

	public LevelMenu levelMenu;

	public LevelPackMenu levelPackMenu;

	public BaseMenu itemShopMenu;

	public BaseMenu cartShopMenu;

	public BaseMenu infoMenu;

	public BaseMenu creditsMenu;

	public BaseMenu historyMenu;

	public ButtonPressAnimator buttonAnimator;

	public VGUIText versionString;

	public GameObject exitButton;

	public tk2dTextMesh quality;

	private bool goingToStory;

	private int frameCounter;

	private bool isStartingGame;

	private string[] ASPECTS = new string[4] { "4:3", "3:2", "16:9", "16:10" };

	private float[] ASPECT_FACTORS = new float[4] { 1.3333334f, 1.5f, 1.7777778f, 1.6f };

	private bool languagePopup;

	private bool aspectPopup;

	private static int currentAspect;

	public new static MenuManager getInstance()
	{
		return BaseMenuManager.instance as MenuManager;
	}

	public override void Awake()
	{
		base.Awake();
		CreditsMenu.isLoaded = false;
		GameData.reloadDynamicProperties();
		MissionMenu.GOING_TO_MENU = false;
		Resources.UnloadUnusedAssets();
		MenuButton.clickAnimator = buttonAnimator;
		mainMenu.deactivate();
		levelMenu.deactivate();
		levelPackMenu.deactivate();
		loadingMenu.activate();
		missionMenu.deactivate();
		infoMenu.deactivate();
		creditsMenu.deactivate();
		historyMenu.deactivate();
		coinStoreMenu.Awake();
		missionMenu.nextMenu = mainMenu;
		if (!GameData.TEST_BUILD)
		{
			versionString.setText(GameData.VERSION_STRING);
		}
		else if (GameData.LOCALIZATION_TEST)
		{
			versionString.setText(GameData.VERSION_STRING_VISIBLE + " (LOCALIZATION BUILD)");
		}
		else
		{
			versionString.setText(GameData.VERSION_STRING_VISIBLE + " (TEST BUILD)");
		}
		useGUILayout = false;
		exitButton.SetActiveRecursively(false);
		GameData.TRAIN_DATA.cartList = null;
		if (GameData.LOCALIZATION_TEST && !ASPECT_INITED)
		{
			ASPECT_INITED = true;
			setAspectRatio(PlayerPrefs.GetInt("ASPECT_RATION", 0));
		}
		Resources.UnloadUnusedAssets();
		playMenuMusic();
	}

	public virtual void Start()
	{
		RollerCoasterMissions.getInstance();
		RollerCoasterMissions.getInstance().updateMissionScores();
		BaseMenu.anim = AnimationManager.getInstance();
		if (WAS_PLAYING)
		{
			if (RollerCoasterMissions.getInstance().checkMissions(GameData.MAX_ACTIVE_MISSIONS))
			{
				menuSwitcher.activateMenu(missionMenu);
			}
			else
			{
				menuSwitcher.activateMenu(levelMenu);
				if (!BaseMenuManager.getInstance().descriptNewFeatures())
				{
					CommunityFeatures.getInstance().offerIfNeeded();
				}
			}
		}
		else if (WAS_IN_STORY)
		{
			menuSwitcher.activateMenu(infoMenu);
		}
		else
		{
			menuSwitcher.activateMenu(mainMenu);
		}
		Resources.UnloadUnusedAssets();
		NotificationManager.getInstance().runRevenueCheck();
		WAS_IN_STORY = false;
		WAS_PLAYING = false;
		VersionChecker.getInstance().getVersion(GameData.VERSION_CHECK_URL, gameObject);
		bool featureUnlocked = GameData.isFeatureUnlocked(2);
		bool infoShown = PlayerPrefs.GetInt("CHRISTMASUPDATE_INFO_SHOWN", 0) > 0;
		if (featureUnlocked && !infoShown)
		{
			showMessage(TextData.COMMUNITY_TITLE_OFFER2, TextData.CHRISTMAS_PACKAGE_UNLOCK_TEXT, featureImages[12]);
			PlayerPrefs.SetInt("CHRISTMASUPDATE_INFO_SHOWN", 1);
		}
	}

	public virtual void OnApplicationFocus(bool focus)
	{
		if (focus && frameCounter > 10)
		{
			frameCounter = 10;
		}
		if (!focus)
		{
			pauseCurrentMusic();
		}
		else
		{
			continueCurrentMusic();
		}
	}

	public virtual void OnApplicationQuit()
	{
		if (GameData.LOG)
		{
			print("Application quitting");
		}
		menuSwitcher.activeMenu.deactivate();
	}

	public virtual void playMenuMusic()
	{
		playMusic("Music/menumusic", true);
	}

	public virtual void onResetGame()
	{
		GameData.resetSavedValues();
		updateTokenAmount();
	}

	// Only did analytics logging; kept because buttons call it by name.
	public virtual void onCommunity()
	{
	}

	public virtual void onToggleQuality()
	{
		int lowDef = PlayerPrefs.GetInt("ANDROID_LOW_DEF");
		quality = (tk2dTextMesh)GameObject.Find("Text").GetComponent(typeof(tk2dTextMesh));
		if (lowDef == 0)
		{
			lowDef = 1;
			quality.setText("HD");
		}
		else
		{
			lowDef = 0;
			quality.setText("SD");
		}
		PlayerPrefs.SetInt("ANDROID_LOW_DEF", lowDef);
		CommonData.reinit();
	}

	// Opened the Ubisoft cross-promotion news page, which is gone; kept
	// because the news button calls it by name.
	public virtual void onNews()
	{
	}

	public override void onStore()
	{
		menuSwitcher.switchMenu(coinStoreMenu);
	}

	public override void onMessageBoxClosed(bool dismissed)
	{
		base.onMessageBoxClosed(dismissed);
		playMenuMusic();
	}

	public virtual void onMainMenuToLevelPacks()
	{
		if (RollerCoasterMissions.getInstance().checkMissions(GameData.MAX_ACTIVE_MISSIONS))
		{
			MissionMenu.WAS_FORCED_TO_WATCH = true;
			menuSwitcher.switchMenu(missionMenu);
			missionMenu.nextMenu = levelMenu;
		}
		else
		{
			onNextMenu();
		}
	}

	public virtual void startGame()
	{
		if (!isStartingGame)
		{
			isStartingGame = true;
			stopCurrentMusic();
			setStoreButtonVisible(false);
			setTokenCounterVisible(false);
			WAS_PLAYING = true;
			CustomInput.disableInput(true);
			loadingMenu.activate();
			loadingMenu.gameObject.SetActiveRecursively(true);
			LevelPack activeLevelPack = GameData.getActiveLevelPack();
			LevelPack.LevelInfo selectedLevel = GameData.getSelectedLevel();
			CommonData.LEVEL_PACK_NAME = activeLevelPack.fileName;
			CommonData.LEVEL_NAME = selectedLevel.fileName;
			Invoke("startInvoke", 0.2f);
			CustomInput.disableInput(false);
		}
	}

	public virtual void startInvoke()
	{
		Application.LoadLevel("GameScene");
	}

	public virtual void onInfoMenu()
	{
		menuSwitcher.switchMenu(infoMenu);
	}

	public virtual void onCreditsMenu()
	{
		menuSwitcher.switchMenu(creditsMenu);
	}

	public virtual void onHistoryMenu()
	{
		menuSwitcher.switchMenu(historyMenu);
	}

	public virtual void onStoryMenu()
	{
		loadingMenu.deactivate();
		WAS_IN_STORY = true;
		goingToStory = true;
		loadingMenu.activate();
	}

	public override void onProductListReceived()
	{
		base.onProductListReceived();
		if (GameData.LOG)
		{
			print("PRODUDTS RECEIVED");
		}
		(coinStoreMenu as CoinStoreMenu).onPurchasePricesReceived();
	}

	public override void Update()
	{
		base.Update();
		frameCounter++;
		if (frameCounter == 1)
		{
			loadingMenu.gameObject.SetActiveRecursively(false);
			loadingMenu.deactivate();
		}
	}

	public virtual void OnGUI()
	{
		if (GameData.LOCALIZATION_TEST && menuSwitcher.activeMenu == mainMenu)
		{
			int languageId = 0;
			Popup.List(new Rect(Screen.width - 68, 84f, 64f, 20f), ref languagePopup, ref languageId, "Lang: " + LocalizationManager.getInstance().getLanguage(), LocalizationManager.LANGUAGES, GUI.skin.button, delegate
			{
				if (GameData.LOG)
				{
					print("SETTING LANG TO: " + LocalizationManager.LANGUAGES[languageId]);
				}
				LocalizationManager.getInstance().setLanguage(LocalizationManager.LANGUAGES[languageId]);
				Application.LoadLevel("MenuScene");
			}, CustomLayoutOption.MinWidth(64f));
			int aspectSelection = 0;
			Popup.List(new Rect(Screen.width - 140, 84f, 64f, 20f), ref aspectPopup, ref aspectSelection, string.Empty + ASPECTS[currentAspect], ASPECTS, GUI.skin.button, delegate
			{
				setAspectRatio(aspectSelection);
			}, CustomLayoutOption.MinWidth(64f));
		}
	}

	private void setAspectRatio(int index)
	{
		int height = 768;
		int width = (int)(ASPECT_FACTORS[index] * (float)height);
		currentAspect = index;
		Screen.SetResolution(width, height, Screen.fullScreenMode);
		PlayerPrefs.SetInt("ASPECT_RATION", currentAspect);
		Application.LoadLevel("MenuScene");
	}

	public override void onLoadingMenuDeactivation()
	{
	}

	public override void onLoadingMenuActivation()
	{
		if (goingToStory)
		{
			goingToStory = false;
			Application.LoadLevel("StoryScene");
		}
	}
}
