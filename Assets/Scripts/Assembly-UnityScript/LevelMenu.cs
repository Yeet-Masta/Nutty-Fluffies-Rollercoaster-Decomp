using System.Collections.Generic;
using UnityEngine;

public class LevelMenu : StoreBaseMenu
{
	public VGUIButton levelButton;

	public VGUIGrid levelGrid;

	public VGUIScrollingContainer packContainer;

	public GameObject cartImage;

	public VGUIText titleText;

	public GameObject coinItemImage;

	public BlinkAnimator storeButtonBlink;

	public GameObject cancelButton;

	private bool wasActive;

	private bool complainingAboutCash;

	private bool informingNewFeature;

	private int lastSelectedLevel;

	private bool goingBack;

	public ParticleEmitter[] paperEmitters;

	private List<GameObject> createdButtons = new List<GameObject>();

	public override void activate()
	{
		base.activate();
		goingBack = false;
		complainingAboutCash = false;
		informingNewFeature = false;
		initEverything();
		cancelButton.SetActiveRecursively(true);
	}

	public virtual void initEverything()
	{
		int activeLevelPackIndex = GameData.getActiveLevelPackIndex();
		int selectedLevelIndex = GameData.getSelectedLevelIndex();
		LevelPack levelPack = GameData.getActiveLevelPack();
		if (activeLevelPackIndex < GameData.getLevelPackCount() - 1 && selectedLevelIndex == levelPack.getLevelCount() - 1 && !wasActive)
		{
			int nextPack = activeLevelPackIndex + 1;
			if (GameData.getLevelPack(nextPack).isUnlocked() && !GameData.getLevelPack(nextPack).isExternal())
			{
				float price = GameData.getTrackPrice(nextPack, 0);
				LevelPack levelPack2 = GameData.getLevelPack(nextPack);
				if (!levelPack2.getLevelInfo(0).testFlag(GameData.LEVEL_FLAG_BUILT) && GameData.canBuy((int)price))
				{
					levelPack = levelPack2;
					GameData.setActiveLevelPack(nextPack);
				}
			}
		}
		GameData.SELECTED_LEVEL_INDEX = 0;
		int levelCount = levelPack.getLevelCount();
		GetComponentsInChildren(typeof(VGUIButton), true);
		createLevelPages(levelCount);
		createLevelButtons(levelCount);
		int page = PlayerPrefs.GetInt("LEVEL_MENU_SELECTION" + GameData.getActiveLevelPackIndex(), 0);
		int buttonsPerPage = levelGrid.maxRows * levelGrid.maxColumns;
		for (int i = 0; i < levelCount; i++)
		{
			if (!levelPack.getLevelInfo(i).testFlag(1))
			{
				page = (i - 1) / buttonsPerPage;
				break;
			}
		}
		if (page < 0 || page >= packContainer.pages.Length)
		{
			page = 0;
		}
		packContainer.firstActiveStep = page;
		packContainer.setCurrentStep(page, true);
		packContainer.immediatePosition();
		wasActive = true;
		setPurchaseBlink();
		disablePaperEmitters();
	}

	public override void reactivate()
	{
		initEverything();
	}

	private void setPurchaseBlink()
	{
		LevelPack activeLevelPack = GameData.getActiveLevelPack();
		int levelCount = activeLevelPack.getLevelCount();
		bool blink = false;
		for (int i = 0; i < levelCount; i++)
		{
			LevelPack.LevelInfo levelInfo = activeLevelPack.getLevelInfo(i);
			if (levelInfo.isUnlocked() && !levelInfo.testFlag(GameData.LEVEL_FLAG_BUILT))
			{
				float price = (float)items[0].price * GameData.getPriceMultiplier(GameData.getActiveLevelPackIndex(), i);
				if (!GameData.canBuy((int)price))
				{
					blink = true;
					break;
				}
			}
		}
		if (blink)
		{
			storeButtonBlink.activate();
		}
		else
		{
			storeButtonBlink.deactivate();
		}
	}

	public override void deactivate()
	{
		base.deactivate();
		storeButtonBlink.deactivate();
		if (wasActive)
		{
			PlayerPrefs.SetInt("LEVEL_MENU_SELECTION" + GameData.getActiveLevelPackIndex(), packContainer.intValue);
			PlayerPrefs.Save();
			wasActive = false;
		}
		foreach (ParticleEmitter paperEmitter in paperEmitters)
		{
			paperEmitter.emit = false;
			paperEmitter.ClearParticles();
		}
		CancelInvoke("reSelectLastLevel");
		createdButtons.Clear();
		wasActive = false;
	}

	private void createLevelButtons(int levelCount)
	{
		createdButtons.Clear();
		int buttonsPerPage = levelGrid.maxRows * levelGrid.maxColumns;
		int activeLevelPackIndex = GameData.getActiveLevelPackIndex();
		for (int i = 0; i < levelCount; i++)
		{
			VGUIGrid component = packContainer.pages[i / buttonsPerPage].GetComponent<VGUIGrid>();
			LevelButton component2 = component.createEntry().GetComponent<LevelButton>();
			component2.gameObject.name = "LevelButton";
			component2.intValue = i;
			component2.invokeObject = gameObject;
			LevelPack.LevelInfo levelInfo = GameData.getActiveLevelPack().getLevelInfo(i);
			float price = GameData.getTrackPrice(activeLevelPackIndex, i);
			component2.activate(levelInfo, i, (int)price);
			createdButtons.Add(component2.gameObject);
		}
	}

	private void createLevelPages(int levels)
	{
		packContainer.removePages();
		int buttonsPerPage = levelGrid.maxRows * levelGrid.maxColumns;
		int pageCount = levels / buttonsPerPage;
		if (pageCount * buttonsPerPage < levels)
		{
			pageCount++;
		}
		packContainer.pages = new GameObject[pageCount];
		packContainer.maxSteps = pageCount - 1;
		packContainer.setCurrentStep(0, true);
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < pageCount; i++)
		{
			packContainer.pages[i] = UnityEngine.Object.Instantiate(levelGrid.gameObject, zero, Quaternion.identity);
			packContainer.pages[i].transform.parent = packContainer.transform;
			Vector3 localPosition = packContainer.pages[i].transform.localPosition;
			localPosition.y = 0;
			packContainer.pages[i].transform.localPosition = localPosition;
			Vector3 localPosition2 = packContainer.pages[i].transform.localPosition;
			localPosition2.z = 0f;
			packContainer.pages[i].transform.localPosition = localPosition2;
			float x = zero.x;
			Vector3 localPosition3 = packContainer.pages[i].transform.localPosition;
			localPosition3.x = x;
			packContainer.pages[i].transform.localPosition = localPosition3;
			packContainer.pages[i].GetComponentsInChildren(typeof(Renderer), true);
			zero.x += packContainer.oneStepLength;
		}
		packContainer.Start();
	}

	public virtual void onSelectLevel(int levelIndex)
	{
		if (BaseMenuManager.getInstance().menuSwitcher.isSwitching() || goingBack)
		{
			return;
		}
		LevelPack.LevelInfo levelInfo = GameData.getActiveLevelPack().getLevelInfo(levelIndex);
		CancelInvoke("reSelectLastLevel");
		if (levelInfo.isUnlocked())
		{
			lastSelectedLevel = levelIndex;
			GameData.SELECTED_LEVEL_INDEX = levelIndex;
			if (!levelInfo.testFlag(GameData.LEVEL_FLAG_BUILT))
			{
				VGUIElement.blockingButtonPressed = false;
				onBuildTrack(levelIndex);
				if (!informingNewFeature && !complainingAboutCash)
				{
					Invoke("reSelectLastLevel", 1f);
				}
				return;
			}
			StoreBaseMenu.COMMON_PRICE_FACTOR = GameData.getActivePriceMultiplier();
			if (wasActive)
			{
				PlayerPrefs.SetInt("LEVEL_MENU_SELECTION" + GameData.getActiveLevelPackIndex(), packContainer.intValue);
				PlayerPrefs.Save();
				wasActive = false;
			}
			MenuManager.getInstance().startGame();
		}
	}

	public virtual void reSelectLastLevel()
	{
		onSelectLevel(lastSelectedLevel);
	}

	public virtual void onBuildTrack(int levelIndex)
	{
		LevelPack.LevelInfo levelInfo = GameData.getActiveLevelPack().getLevelInfo(levelIndex);
		if (levelInfo.isUnlocked())
		{
			if (levelInfo.testFlag(GameData.LEVEL_FLAG_BUILT))
			{
				return;
			}
			float price = GameData.getTrackPrice(GameData.getActiveLevelPackIndex(), levelIndex);
			if (GameData.canBuy((int)price))
			{
				int intValue = packContainer.intValue;
				PlayerPrefs.SetInt("LEVEL_MENU_SELECTION" + GameData.getActiveLevelPackIndex(), packContainer.intValue);
				PlayerPrefs.Save();
				GameData.addTokens((int)(0f - price));
				GameData.buildLevel(levelIndex);
				BaseMenu.mmgr.updateTokenAmount();
				activate();
				int intValue2 = GameData.getActiveLevelPack().getLevelInfo(levelIndex).settings.getIntValue("specialcart", Vehicle.CART_NORMAL);
				int feature = 0;
				switch (intValue2)
				{
				case 1:
					feature = 65536;
					break;
				case 0:
					feature = 32768;
					break;
				case 2:
					feature = GameData.FEATURE_HOVER_CART;
					break;
				}
				GameData.unlockFeature(feature);
				informingNewFeature = MenuManager.getInstance().descriptNewFeatures();
				packContainer.setCurrentStep(intValue, true);
				packContainer.immediatePosition();
				if (createdButtons.Count <= levelIndex)
				{
					return;
				}
				GameObject gameObject = createdButtons[levelIndex];
				if (gameObject != null && !informingNewFeature)
				{
					cancelButton.SetActiveRecursively(false);
					foreach (ParticleEmitter paperEmitter in paperEmitters)
					{
						paperEmitter.emit = true;
						paperEmitter.transform.position = gameObject.transform.position;
						float z = paperEmitter.transform.position.z - 0.5f;
						Vector3 position = paperEmitter.transform.position;
						position.z = z;
						paperEmitter.transform.position = position;
					}
					CancelInvoke("disablePaperEmitters");
					Invoke("disablePaperEmitters", 1.5f);
					GeneralSoundEffects.playSound(BaseMenuManager.getInstance().fireworksSound, 1);
				}
			}
			else if (!complainingAboutCash)
			{
				BaseMenu.mmgr.showErrorMessage(TextData.LEVELS_TITLE_NEED_COINS, TextData.LEVELS_TEXT_NEED_COINS, coinItemImage);
				complainingAboutCash = true;
			}
		}
	}

	public override void onMessageBoxClosed(bool dismissed)
	{
		cancelButton.SetActiveRecursively(true);
		if (GameData.canMakeInAppPurchases() && complainingAboutCash && !dismissed)
		{
			MenuManager.getInstance().onStore();
		}
		if (informingNewFeature)
		{
			reSelectLastLevel();
		}
		complainingAboutCash = false;
		informingNewFeature = false;
	}

	public virtual void disablePaperEmitters()
	{
		foreach (ParticleEmitter paperEmitter in paperEmitters)
		{
			paperEmitter.emit = false;
		}
	}

	public override void onBackButton()
	{
		goBack();
	}

	public virtual void goBack()
	{
		goingBack = true;
		MenuManager.getInstance().onPreviousMenu();
	}
}
