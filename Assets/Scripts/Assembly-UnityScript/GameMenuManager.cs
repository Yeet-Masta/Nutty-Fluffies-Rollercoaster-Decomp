using UnityEngine;

public class GameMenuManager : BaseMenuManager
{
	public BaseMenu itemStore;

	public AlphaAnimator fadeIn;

	private bool displayedMenu;

	private bool hasCreatedLevel;

	private bool wasInPauseMenu;

	public override void Awake()
	{
		base.Awake();
		loadingMenu.gameObject.SetActiveRecursively(false);
		BaseMenuManager.MENU_SCENE = false;
		itemStore.deactivate();
		missionMenu.deactivate();
	}

	public new static GameMenuManager getInstance()
	{
		return BaseMenuManager.instance as GameMenuManager;
	}

	public virtual void Start()
	{
		wasInPauseMenu = false;
		bool canBuyCarts = GameData.isFeatureUnlocked(0x1E | GameData.FEATURE_BUY_CART);
		BaseMenu.anim = AnimationManager.getInstance();
		BaseMenu.mmgr = this;
		GameData.createTrainForLevel();
		if (!hasCreatedLevel)
		{
			GameManager.getInstance().loadLevel();
			hasCreatedLevel = true;
		}
		if (canBuyCarts)
		{
			menuSwitcher.activateMenu(itemStore);
			fadeIn.invokeRef = gameObject;
			displayedMenu = true;
		}
		else
		{
			displayedMenu = false;
			startGame();
		}
	}

	public virtual void onItemStore()
	{
		menuSwitcher.activateMenu(itemStore);
		fadeIn.invokeRef = gameObject;
		CommunityFeatures.getInstance().offerIfNeeded();
	}

	public virtual void startGame()
	{
		if (displayedMenu)
		{
			fadeIn.reverse = true;
			fadeIn.activate();
		}
		else
		{
			GameManager.getInstance().startGame();
			menuSwitcher.activeMenu = null;
			setStoreButtonVisible(false);
			setTokenCounterVisible(false);
			setMenuTitle(null);
		}
		loadingMenu.deactivate();
	}

	public virtual void displayMissionsIngame()
	{
		MissionMenu.WAS_FORCED_TO_WATCH = true;
		MissionMenu.ENTERED_FROM_PAUSE_MENU = true;
		menuSwitcher.activateMenu(missionMenu);
		fadeIn.invokeRef = gameObject;
		displayedMenu = true;
		wasInPauseMenu = true;
	}

	public virtual void continueFromMissions()
	{
		fadeIn.reverse = true;
		fadeIn.activate();
		setTokenCounterVisible(false);
	}

	private void showMissions()
	{
	}

	public virtual void onEndGame()
	{
		if (GameData.LOG)
		{
			Debug.Log(" GAmeManuManager ");
		}
		if (RollerCoasterMissions.getInstance().checkMissions(GameData.MAX_ACTIVE_MISSIONS))
		{
			MissionMenu.WAS_FORCED_TO_WATCH = true;
			missionMenu.nextMenu = itemStore;
			menuSwitcher.activateMenu(missionMenu);
			fadeIn.invokeRef = gameObject;
			displayedMenu = true;
		}
		else
		{
			GameManager.getInstance().endGame();
			displayedMenu = false;
		}
		if (displayedMenu)
		{
			fadeIn.reverse = false;
			fadeIn.activate();
		}
	}

	public virtual void onRestartGame()
	{
		tokenAmount.setValueImmediately(GameData.getTokenAmount());
		GameData.createTrainForLevel();
		fadeIn.invokeRef = gameObject;
		if (RollerCoasterMissions.getInstance().checkMissions(GameData.MAX_ACTIVE_MISSIONS))
		{
			MissionMenu.WAS_FORCED_TO_WATCH = true;
			missionMenu.nextMenu = itemStore;
			menuSwitcher.activateMenu(missionMenu);
			displayedMenu = true;
		}
		else
		{
			Start();
			if (displayedMenu)
			{
				CommunityFeatures.getInstance().offerIfNeeded();
			}
		}
		if (displayedMenu)
		{
			fadeIn.reverse = false;
			fadeIn.activate();
		}
	}

	public virtual void onFadeInComplete()
	{
		loadingMenu.deactivate();
		if (fadeIn.reverse)
		{
			if (wasInPauseMenu)
			{
				missionMenu.deactivate();
				GameManager.getInstance().pauseGame();
				fadeIn.reverse = false;
				fadeIn.activate();
				wasInPauseMenu = false;
			}
			else if (displayedMenu)
			{
				itemStore.deactivate();
				missionMenu.deactivate();
				GameManager.getInstance().startGame();
				setMenuTitle(null);
				setStoreButtonVisible(false);
				setTokenCounterVisible(false);
				displayedMenu = false;
			}
		}
	}

	// Opened the coin store when in-app purchases were available, which they no
	// longer are. Kept empty: the base version would do something else.
	public override void onStore()
	{
	}

	public override void onNextMenu()
	{
		bool canBuyCarts = GameData.isFeatureUnlocked(0x1E | GameData.FEATURE_BUY_CART);
		if (menuSwitcher.activeMenu.nextMenu == itemStore && !canBuyCarts)
		{
			startGame();
		}
		else
		{
			base.onNextMenu();
		}
		if (menuSwitcher.activeMenu.nextMenu == itemStore)
		{
			CommunityFeatures.getInstance().offerIfNeeded();
		}
	}

	public override void onCommunityFeatureCompleted()
	{
		if (menuSwitcher.activeMenu == coinStoreMenu)
		{
			menuSwitcher.activeMenu.activate();
		}
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

	public override void onPreviousMenu()
	{
		if (ResultScreen.ACTIVE && menuSwitcher.activeMenu == coinStoreMenu)
		{
			GameManager.getInstance().gameUI.resultScreen.setVisible(true);
			coinStoreMenu.deactivate();
			setMenuTitle(null);
			BaseMenuManager.getInstance().setStoreButtonVisible(true);
		}
		else
		{
			base.onPreviousMenu();
		}
	}
}
