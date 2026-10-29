using UnityEngine;

public class BaseMenuManager : MonoBehaviour
{
	protected static BaseMenuManager instance;

	public static bool MENU_SCENE = true;

	public VGUIRunningValueTK2D tokenAmount;

	public VGUIButton storeButton;

	public GameObject tokenCounter;

	public MessageBox messageBox;

	public AudioClip notificationSound;

	public MenuSwitcher menuSwitcher;

	public BaseMenu loadingMenu;

	public BaseMenu missionMenu;

	public MenuHeader menuHeader;

	public GameObject[] featureImages;

	public AudioClip featureDialogSound;

	public GameObject cashImage;

	public GameObject coinsAndHeartsImage;

	public AudioClip cashSound;

	public AudioSource musicSource;

	public AudioClip fireworksSound;

	public BaseMenu coinStoreMenu;

	public BaseMenu freeCoinsMenu;

	protected bool coinStoreButtonVisible;

	private string currentMusic;

	public static BaseMenuManager getInstance()
	{
		return instance;
	}

	public virtual void Awake()
	{
		MENU_SCENE = true;
		instance = this;
		BaseMenu.mmgr = instance;
		Application.targetFrameRate = GameData.targetFPS;
		GameData.init();
		CommonData.init();
		tokenAmount.setValueImmediately(GameData.getTokenAmount());
		messageBox.deactivate();
		menuHeader.setText(null);
		coinStoreMenu.deactivate();
		freeCoinsMenu.deactivate();
	}

	public virtual void showMessage(string title, string message, GameObject imagePrefab)
	{
		GeneralSoundEffects.playSound(notificationSound);
		messageBox.activate(title, message, imagePrefab, false, false);
	}

	public virtual void showMessage(string title, string message, GameObject imagePrefab, bool closeButton)
	{
		GeneralSoundEffects.playSound(notificationSound);
		messageBox.activate(title, message, imagePrefab, closeButton, false);
	}

	public virtual void showErrorMessage(string title, string message, GameObject imagePrefab)
	{
		GeneralSoundEffects.playSound(notificationSound);
		messageBox.activate(title, message, imagePrefab, false, true);
	}

	public virtual float updateTokenAmount()
	{
		return tokenAmount.setValue(GameData.getTokenAmount());
	}

	public virtual void setStoreButtonVisible(bool visible)
	{
		bool allowed = visible && GameData.canMakeInAppPurchases();
		bool showStore = allowed;
		storeButton.gameObject.SetActiveRecursively(showStore);
		if (GameData.LOG)
		{
			Debug.Log("STORE BUTTON VISIBLITY TO: " + showStore);
		}
		if (!coinStoreButtonVisible && showStore)
		{
			AnimationManager.getInstance().alpha(storeButton.transform, 0f, 1f);
		}
		coinStoreButtonVisible = visible;
	}

	public virtual void setTokenCounterVisible(bool visible)
	{
		tokenCounter.SetActiveRecursively(visible);
	}

	public virtual void showLoadingScreen(bool show)
	{
		if (show)
		{
			loadingMenu.activate();
		}
		else
		{
			loadingMenu.deactivate();
		}
	}

	public virtual void onNextMenu()
	{
		BaseMenu nextMenu = menuSwitcher.activeMenu.nextMenu;
		menuSwitcher.switchMenu(nextMenu);
		if (nextMenu != freeCoinsMenu)
		{
			GameData.lockFeature(8192);
		}
		if (nextMenu != missionMenu && nextMenu != coinStoreMenu && nextMenu != freeCoinsMenu)
		{
			descriptNewFeatures();
		}
	}

	public virtual void onPreviousMenu()
	{
		BaseMenu previousMenu = menuSwitcher.activeMenu.previousMenu;
		menuSwitcher.switchMenu(previousMenu);
	}

	public virtual bool isMenuActive()
	{
		bool result = false;
		if (menuSwitcher.activeMenu != null)
		{
			if (menuSwitcher.activeMenu == freeCoinsMenu && (freeCoinsMenu as FreeCoinStoreMenu).isActive())
			{
				result = true;
			}
			else if (menuSwitcher.activeMenu == coinStoreMenu && (coinStoreMenu as CoinStoreMenu).isActive())
			{
				result = true;
			}
		}
		return result;
	}

	public virtual void onGoBack()
	{
		menuSwitcher.switchBack();
	}

	public virtual void onBrowsePacks()
	{
	}

	public virtual bool descriptNewFeatures()
	{
		int newFeaturesList = GameData.getNewFeaturesList();
		if (newFeaturesList != 0)
		{
			messageBox.enablePaperEmitters(true);
			if ((newFeaturesList & 2) != 0)
			{
				showMessage(TextData.FEATURE_TITLE_CAT, TextData.FEATURE_TEXT_CAT + "\n" + TextData.ANIMAL_DESCRIPTIONS[1], featureImages[1]);
			}
			if ((newFeaturesList & 4) != 0)
			{
				showMessage(TextData.FEATURE_TITLE_DOG, TextData.FEATURE_TEXT_DOG + "\n" + TextData.ANIMAL_DESCRIPTIONS[2], featureImages[2]);
			}
			if ((newFeaturesList & 8) != 0)
			{
				showMessage(TextData.FEATURE_TITLE_PIG, TextData.FEATURE_TEXT_PIG + "\n" + TextData.ANIMAL_DESCRIPTIONS[3], featureImages[3]);
			}
			if ((newFeaturesList & 0x10) != 0)
			{
				showMessage(TextData.FEATURE_TITLE_ELEPHANT, TextData.FEATURE_TEXT_ELEPHANT + "\n" + TextData.ANIMAL_DESCRIPTIONS[4], featureImages[4]);
			}
			if ((newFeaturesList & 0x400) != 0)
			{
				showMessage(TextData.FEATURE_TITLE_METAL, TextData.FEATURE_TEXT_METAL, featureImages[5]);
			}
			if ((newFeaturesList & 0x800) != 0)
			{
				showMessage(TextData.FEATURE_TITLE_LOOP, TextData.FEATURE_TEXT_LOOP, featureImages[6]);
			}
			if ((newFeaturesList & 0x1000) != 0)
			{
				showMessage(TextData.FEATURE_TITLE_EXTREME, TextData.FEATURE_TEXT_EXTREME, featureImages[7]);
			}
			if ((newFeaturesList & 0x4000) != 0)
			{
				showMessage(TextData.FEATURE_TITLE_HISTORY, TextData.FEATURE_TEXT_HISTORY, featureImages[8]);
			}
			if ((newFeaturesList & 0x8000) != 0)
			{
				showMessage(TextData.FEATURE_TITLE_ROCKET_CART, TextData.FEATURE_TEXT_ROCKET_CART, featureImages[9]);
			}
			if ((newFeaturesList & 0x10000) != 0)
			{
				showMessage(TextData.FEATURE_TITLE_MAGNET_CART, TextData.FEATURE_TEXT_MAGNET_CART, featureImages[10]);
			}
			if ((newFeaturesList & GameData.FEATURE_HOVER_CART) != 0)
			{
				showMessage(TextData.FEATURE_TITLE_HOVER_CART, TextData.FEATURE_TEXT_HOVER_CART, featureImages[11]);
			}
			if ((newFeaturesList & GameData.FEATURE_BUY_CART) != 0)
			{
				showMessage(TextData.FEATURE_TITLE_CART, TextData.FEATURE_TEXT_CART, featureImages[0]);
			}
			GameData.setFeatureDescribed(newFeaturesList);
			if (RollerCoasterSounds.getInstance() != null)
			{
				RollerCoasterSounds.getInstance().stopMusic();
				RollerCoasterSounds.getInstance().playMusic(featureDialogSound);
			}
			else
			{
				playMusicClip(featureDialogSound, false);
			}
			musicSource.volume = 1f;
			musicSource.loop = false;
			return true;
		}
		return false;
	}

	public virtual void playMusicClip(AudioClip music, bool loop)
	{
		if (music != null)
		{
			if (GameData.LOG)
			{
				print("Playing music: " + music.name);
			}
			musicSource.clip = music;
			musicSource.loop = loop;
			musicSource.Play();
			currentMusic = music.name;
		}
		else
		{
			musicSource.Stop();
			currentMusic = null;
		}
	}

	public virtual void stopCurrentMusic()
	{
		playMusic(null, true);
	}

	public virtual void pauseCurrentMusic()
	{
		if (currentMusic != null)
		{
			musicSource.Pause();
		}
	}

	public virtual void continueCurrentMusic()
	{
		if (currentMusic != null)
		{
			musicSource.Play();
		}
	}

	public virtual void setMusicVolume(float to)
	{
		musicSource.volume = to;
	}

	public virtual float getMusicVolume()
	{
		return musicSource.volume;
	}

	public virtual void playMusic(string name, bool loop)
	{
		if (name != currentMusic)
		{
			AudioClip music = null;
			if (name != null)
			{
				music = Resources.Load(name) as AudioClip;
			}
			playMusicClip(music, loop);
			currentMusic = name;
		}
	}

	public virtual void setMenuTitle(string title)
	{
		menuHeader.setText(title);
	}

	public virtual void onPurchaseComplete(string productId)
	{
		if (productId == "com.redlynx.rollercoaster.maxfluffies")
		{
			return;
		}
		updateTokenAmount();
		GeneralSoundEffects.playSound(cashSound);
		string message = TextData.IAP_TEXT_PURCHASE_COMPLETE.Replace("%d", string.Empty + GameData.formatTokenValue(string.Empty + GameData.getTokenValue(productId)));
		showMessage(TextData.IAP_TITLE_PURCHASE_COMPLETE, message, cashImage);
	}

	public virtual void onPurchaseFailed()
	{
		if (GameData.LOG)
		{
			print("onPurchaseFailed callback");
		}
		showMessage(TextData.IAP_TITLE_PURCHASE_FAIL, TextData.IAP_TEXT_PURCHASE_FAIL, null);
	}

	public virtual void onPurchaseCancel()
	{
		if (GameData.LOG)
		{
			print("onPurchaseCanceled callback");
		}
	}

	public virtual void onProductListReceived()
	{
		setStoreButtonVisible(coinStoreButtonVisible);
	}

	public virtual void onMessageBoxClosed(bool dismissed)
	{
		if (menuSwitcher.activeMenu != null)
		{
			menuSwitcher.activeMenu.onMessageBoxClosed(dismissed);
		}
	}

	public virtual void onLoadingMenuDeactivation()
	{
	}

	public virtual void onLoadingMenuActivation()
	{
	}

	public virtual void onCommunityFeatureCompleted()
	{
		if (menuSwitcher.activeMenu != null)
		{
			menuSwitcher.activeMenu.onCommunityFeatureCompleted();
		}
	}

	public virtual void onStore()
	{
		if (menuSwitcher.activeMenu != null)
		{
			menuSwitcher.activeMenu.onCommunityFeatureCompleted();
		}
	}

	public virtual void Update()
	{
		if (LoadingMenu.isLoading || !CustomInput.getKeyDown(27) || menuSwitcher.activeMenu == null)
		{
			return;
		}
		if (messageBox.isActive())
		{
			messageBox.onBackButton();
		}
		else
		{
			menuSwitcher.activeMenu.onBackButton();
		}
	}

	public virtual void onVersionReceived(string versionString)
	{
		print("COMPARING VERSIONS " + versionString + "  AND " + GameData.VERSION_STRING);
		if (versionString.CompareTo(GameData.VERSION_STRING) > 0)
		{
			messageBox.activate(TextData.UPDATE_TITLE, TextData.UPDATE_TEXT, coinsAndHeartsImage, true, false, gameObject, "onGetNewVersion");
		}
	}

	public virtual void onGetNewVersion(bool dismissed)
	{
	}

	public virtual void OnApplicationPause(bool pause)
	{
	}
}
