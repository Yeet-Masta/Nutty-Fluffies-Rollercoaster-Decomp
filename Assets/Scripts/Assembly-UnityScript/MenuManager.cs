using System;
using UnityEngine;

[Serializable]
public class MenuManager : BaseMenuManager
{
	[Serializable]
	internal class _0024OnGUI_0024locals_00241479
	{
		internal int _0024languageId;

		internal int _0024aspectSelection;
	}

	[Serializable]
	internal class _0024OnGUI_0024closure_0024286
	{
		internal _0024OnGUI_0024locals_00241479 _0024_0024locals_00241481;

		public _0024OnGUI_0024closure_0024286(_0024OnGUI_0024locals_00241479 _0024_0024locals_00241481)
		{
			this._0024_0024locals_00241481 = _0024_0024locals_00241481;
		}

		public void Invoke()
		{
			if (GameData.LOG)
			{
				MonoBehaviour.print("SETTING LANG TO: " + LocalizationManager.LANGUAGES[_0024_0024locals_00241481._0024languageId]);
			}
			LocalizationManager.getInstance().setLanguage(LocalizationManager.LANGUAGES[_0024_0024locals_00241481._0024languageId]);
			Application.LoadLevel("MenuScene");
		}
	}

	[Serializable]
	internal class _0024OnGUI_0024closure_0024287
	{
		internal MenuManager _0024this_00241482;

		internal _0024OnGUI_0024locals_00241479 _0024_0024locals_00241483;

		public _0024OnGUI_0024closure_0024287(MenuManager _0024this_00241482, _0024OnGUI_0024locals_00241479 _0024_0024locals_00241483)
		{
			this._0024this_00241482 = _0024this_00241482;
			this._0024_0024locals_00241483 = _0024_0024locals_00241483;
		}

		public void Invoke()
		{
			_0024this_00241482.setAspectRatio(_0024_0024locals_00241483._0024aspectSelection);
		}
	}

	[NonSerialized]
	private static bool WAS_PLAYING;

	[NonSerialized]
	private static bool WAS_PLAYING_AT_LEAST_ONCE;

	[NonSerialized]
	private static bool WAS_IN_STORY;

	[NonSerialized]
	private static bool CURSOR_INITED;

	[NonSerialized]
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

	private bool hasLaunchNotification;

	private bool goingToStory;

	private int frameCounter;

	private bool isStartingGame;

	private string[] ASPECTS;

	private float[] ASPECT_FACTORS;

	private bool languagePopup;

	private bool aspectPopup;

	[NonSerialized]
	private static int currentAspect;

	public MenuManager()
	{
		ASPECTS = new string[4] { "4:3", "3:2", "16:9", "16:10" };
		ASPECT_FACTORS = new float[4] { 1.3333334f, 1.5f, 1.7777778f, 1.6f };
	}

	public new static MenuManager getInstance()
	{
		return BaseMenuManager.instance as MenuManager;
	}

	public static bool CanDisplayUrgentNews()
	{
		return WAS_PLAYING_AT_LEAST_ONCE ? true : false;
	}

	public static void ClearDisplayUrgentNews()
	{
		if (GameData.LOG)
		{
			Debug.Log("RKN ClearDisplayUrgentNews ClearDisplayUrgentNews");
		}
		WAS_PLAYING_AT_LEAST_ONCE = false;
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
		bool flag = GameData.isFeatureUnlocked(2);
		bool flag2 = PlayerPrefs.GetInt("CHRISTMASUPDATE_INFO_SHOWN", 0) > 0;
		if (flag && !flag2)
		{
			showMessage(TextData.COMMUNITY_TITLE_OFFER2, TextData.CHRISTMAS_PACKAGE_UNLOCK_TEXT, featureImages[12]);
			PlayerPrefs.SetInt("CHRISTMASUPDATE_INFO_SHOWN", 1);
		}
	}

	public virtual void OnApplicationFocus(bool focus)
	{
		if (GameData.LOG)
		{
			MonoBehaviour.print("Focus: " + focus);
		}
		if (GameData.LOG)
		{
			Debug.Log("RKN OnApplicationFocus OnApplicationFocus OnApplicationFocus");
		}
		if (focus && frameCounter > 10)
		{
			frameCounter = 10;
		}
		if (!focus)
		{
			if (GameData.LOG)
			{
				Debug.Log("Sound Pause");
			}
			pauseCurrentMusic();
		}
		else
		{
			continueCurrentMusic();
			if (GameData.LOG)
			{
				Debug.Log("Sound Resume");
			}
		}
	}

	public virtual void OnApplicationQuit()
	{
		if (GameData.LOG)
		{
			MonoBehaviour.print("Application quitting");
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

	public virtual void onCommunity()
	{
		if (GameData.LOG)
		{
			MonoBehaviour.print("community");
		}
		if (BaseMenuManager.getInstance() != null)
		{
			BaseMenuManager.getInstance().logEvent("Urgent News");
		}
	}

	public virtual void onToggleQuality()
	{
		int num = PlayerPrefs.GetInt("ANDROID_LOW_DEF");
		quality = (tk2dTextMesh)GameObject.Find("Text").GetComponent(typeof(tk2dTextMesh));
		if (num == 0)
		{
			num = 1;
			quality.setText("HD");
		}
		else
		{
			num = 0;
			quality.setText("SD");
		}
		PlayerPrefs.SetInt("ANDROID_LOW_DEF", num);
		CommonData.reinit();
	}

	public virtual void onNews()
	{
		if (GameData.LOG)
		{
			Debug.Log("RKN onNews onNews onNews");
		}
		if (GameData.LOG)
		{
			MonoBehaviour.print("show news");
		}
		CrossPromotionAndroid.showWebNews();
		if (BaseMenuManager.getInstance() != null)
		{
			BaseMenuManager.getInstance().logEvent("Read Crosspromo News");
		}
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
			if (GameData.LOG)
			{
				MonoBehaviour.print("going to start game ");
			}
			WAS_PLAYING = true;
			WAS_PLAYING_AT_LEAST_ONCE = true;
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
			MonoBehaviour.print("PRODUDTS RECEIVED");
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
		if (frameCounter != 5)
		{
		}
	}

	public virtual void OnGUI()
	{
		_0024OnGUI_0024locals_00241479 obj = new _0024OnGUI_0024locals_00241479();
		if (GameData.LOCALIZATION_TEST && !(menuSwitcher.activeMenu != mainMenu))
		{
			obj._0024languageId = 0;
			if (Popup.List(new Rect(Screen.width - 68, 84f, 64f, 20f), ref languagePopup, ref obj._0024languageId, "Lang: " + LocalizationManager.getInstance().getLanguage(), LocalizationManager.LANGUAGES, GUI.skin.button, new _0024OnGUI_0024closure_0024286(obj).Invoke, CustomLayoutOption.MinWidth(64f)))
			{
			}
			obj._0024aspectSelection = 0;
			if (!Popup.List(new Rect(Screen.width - 140, 84f, 64f, 20f), ref aspectPopup, ref obj._0024aspectSelection, string.Empty + ASPECTS[currentAspect], ASPECTS, GUI.skin.button, new _0024OnGUI_0024closure_0024287(this, obj).Invoke, CustomLayoutOption.MinWidth(64f)))
			{
			}
		}
	}

	private void setAspectRatio(int index)
	{
		int num = 768;
		int width = (int)(ASPECT_FACTORS[index] * (float)num);
		currentAspect = index;
		Screen.SetResolution(width, num, Screen.fullScreenMode);
		PlayerPrefs.SetInt("ASPECT_RATION", currentAspect);
		Application.LoadLevel("MenuScene");
	}

	public override void onLoadingMenuDeactivation()
	{
		if (GameData.LOG)
		{
			Debug.Log("RKN onLoadingMenuDeactivation :");
		}
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
