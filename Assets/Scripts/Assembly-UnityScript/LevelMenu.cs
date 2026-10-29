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
			int num = activeLevelPackIndex + 1;
			if (GameData.getLevelPack(num).isUnlocked() && !GameData.getLevelPack(num).isExternal())
			{
				float num2 = GameData.getTrackPrice(num, 0);
				LevelPack levelPack2 = GameData.getLevelPack(num);
				if (!levelPack2.getLevelInfo(0).testFlag(GameData.LEVEL_FLAG_BUILT) && GameData.canBuy((int)num2))
				{
					levelPack = levelPack2;
					GameData.setActiveLevelPack(num);
				}
			}
		}
		GameData.SELECTED_LEVEL_INDEX = 0;
		int levelCount = levelPack.getLevelCount();
		GetComponentsInChildren(typeof(VGUIButton), true);
		createLevelPages(levelCount);
		createLevelButtons(levelCount);
		int num3 = PlayerPrefs.GetInt("LEVEL_MENU_SELECTION" + GameData.getActiveLevelPackIndex(), 0);
		int num4 = levelGrid.maxRows * levelGrid.maxColumns;
		for (int i = 0; i < levelCount; i++)
		{
			if (!levelPack.getLevelInfo(i).testFlag(1))
			{
				num3 = (i - 1) / num4;
				break;
			}
		}
		if (num3 < 0 || num3 >= packContainer.pages.Length)
		{
			num3 = 0;
		}
		packContainer.firstActiveStep = num3;
		packContainer.setCurrentStep(num3, true);
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
		bool flag = false;
		for (int i = 0; i < levelCount; i++)
		{
			LevelPack.LevelInfo levelInfo = activeLevelPack.getLevelInfo(i);
			if (levelInfo.isUnlocked() && !levelInfo.testFlag(GameData.LEVEL_FLAG_BUILT))
			{
				float num = (float)items[0].price * GameData.getPriceMultiplier(GameData.getActiveLevelPackIndex(), i);
				if (!GameData.canBuy((int)num))
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
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
		int i = 0;
		ParticleEmitter[] array = paperEmitters;
		for (int length = array.Length; i < length; i++)
		{
			array[i].emit = false;
			array[i].ClearParticles();
		}
		CancelInvoke("reSelectLastLevel");
		createdButtons.Clear();
		wasActive = false;
	}

	private void createLevelButtons(int levelCount)
	{
		createdButtons.Clear();
		int num = levelGrid.maxRows * levelGrid.maxColumns;
		int activeLevelPackIndex = GameData.getActiveLevelPackIndex();
		for (int i = 0; i < levelCount; i++)
		{
			VGUIGrid component = packContainer.pages[i / num].GetComponent<VGUIGrid>();
			LevelButton component2 = component.createEntry().GetComponent<LevelButton>();
			component2.gameObject.name = "LevelButton";
			component2.intValue = i;
			component2.invokeObject = gameObject;
			LevelPack.LevelInfo levelInfo = GameData.getActiveLevelPack().getLevelInfo(i);
			float num2 = GameData.getTrackPrice(activeLevelPackIndex, i);
			component2.activate(levelInfo, i, (int)num2);
			createdButtons.Add(component2.gameObject);
		}
	}

	private void createLevelPages(int levels)
	{
		packContainer.removePages();
		int num = levelGrid.maxRows * levelGrid.maxColumns;
		int num2 = levels / num;
		if (num2 * num < levels)
		{
			num2++;
		}
		packContainer.pages = new GameObject[num2];
		packContainer.maxSteps = num2 - 1;
		packContainer.setCurrentStep(0, true);
		Vector3 zero = Vector3.zero;
		for (int i = 0; i < num2; i++)
		{
			packContainer.pages[i] = (GameObject)UnityEngine.Object.Instantiate(levelGrid.gameObject, zero, Quaternion.identity);
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
			float num = GameData.getTrackPrice(GameData.getActiveLevelPackIndex(), levelIndex);
			if (GameData.canBuy((int)num))
			{
				int intValue = packContainer.intValue;
				PlayerPrefs.SetInt("LEVEL_MENU_SELECTION" + GameData.getActiveLevelPackIndex(), packContainer.intValue);
				PlayerPrefs.Save();
				GameData.addTokens((int)(0f - num));
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
					int i = 0;
					ParticleEmitter[] array = paperEmitters;
					for (int length = array.Length; i < length; i++)
					{
						array[i].emit = true;
						array[i].transform.position = gameObject.transform.position;
						float z = array[i].transform.position.z - 0.5f;
						Vector3 position = array[i].transform.position;
						position.z = z;
						array[i].transform.position = position;
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
		int i = 0;
		ParticleEmitter[] array = paperEmitters;
		for (int length = array.Length; i < length; i++)
		{
			array[i].emit = false;
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
