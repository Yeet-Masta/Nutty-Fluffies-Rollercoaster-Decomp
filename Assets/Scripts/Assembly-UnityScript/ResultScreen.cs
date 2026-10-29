using System.Collections.Generic;
using UnityEngine;

public class ResultScreen : VGUIElement
{
	public const float BONUS_ENTRY_INTERVAL = 0.2f;

	public const float EPIC_SCORE_TRESHOLD = 0.9f;

	public const float GOOD_SCORE_TRESHOLD = 0.5f;

	public const float SPECIAL_HEARTS_MAX_MULTIPLY = 0.15f;

	public static bool ACTIVE;

	public ParticleEmitter totalTokenEmitter;

	public PowerMeter powerMeter;

	public int currentBonusIndex;

	public ResultList resultList;

	public List<ResultList.BonusEntry> bonusList;

	public float tokenSum;

	public float heartSum;

	public AudioClip bonusSound;

	public Transform content;

	public AlphaAnimator bgAnimator;

	public ParticleEmitter[] paperEmitters;

	public MenuButton twitterButton;

	public MenuButton facebookButton;

	public GameObject cartImage;

	public GameObject coinsImage;

	public MenuButton continueButton;

	public VGUIText continueButtonPriceText;

	public GameObject continueButtonLock;

	public MenuButton menuButton;

	public MenuButton restartButton;

	public GameObject restartButtonCartAdditionLabel;

	private int maxScore;

	private int maxScoreWithCurrentSetup;

	private bool activated;

	private float scorePercent;

	private int continuePresses;

	private bool enoughtMoneyForNext;

	private bool hasTwittered;

	private bool hasFacebooked;

	private bool errorNotEnoughMoney;

	private bool backToPauseMenu;

	public virtual void calculateMaxScores()
	{
		maxScoreWithCurrentSetup = getMaxScoreWithCurrentSetup();
		maxScore = getMaxScoreWithBestSetup();
		powerMeter.prepare(maxScore);
		powerMeter.prepareFriendScores();
	}

	public override void Awake()
	{
	}

	public virtual void activate(float appearDelay)
	{
		setVisible(true);
		Invoke("activateInvoke", appearDelay);
		ACTIVE = true;
		errorNotEnoughMoney = false;
		backToPauseMenu = true;
	}

	private void activateInvoke()
	{
		activated = true;
		bonusList = createBonusList();
		currentBonusIndex = 0;
		tokenSum = 0f;
		heartSum = 0f;
		continuePresses = 0;
		enoughtMoneyForNext = true;
		hasFacebooked = false;
		hasTwittered = false;
		resultList.activate();
		float activePriceMultiplier = GameData.getActivePriceMultiplier();
		int scoreAmount = GameManager.getInstance().getScoreAmount();
		GameData.addTokens((int)Mathf.Round((float)scoreAmount * activePriceMultiplier * GameData.BONUS_VALUES[0]));
		Invoke("addBonusType", 0.2f);
		base.activate();
		Time.timeScale = 1f;
		AnimationManager.getInstance().alpha(content, 0f, 1f);
		bgAnimator.activate();
		float num2 = getEstimatedScoreWithMaxCarts(scoreAmount);
		float num3 = maxScore;
		scorePercent = num2 / num3;
		RollerCoasterStatistics.getInstance().finish(scorePercent, scorePercent > 0.9f);
		RollerCoasterMissions.getInstance().checkMissions(GameData.MAX_ACTIVE_MISSIONS);
		bool roundDidFinish = RollerCoasterStatistics.getInstance().roundDidFinish;
		RollerCoasterSounds instance = RollerCoasterSounds.getInstance();
		bool flag = scorePercent > 0.9f && roundDidFinish;
		instance.playResultMusic(flag);
		if (roundDidFinish)
		{
			OnlineServiceManager.getInstance().submitLeaderboardScore(GameData.genLeaderboard(GameData.getActiveLevelPackIndex(), GameData.getSelectedLevelIndex()), scoreAmount);
		}
		int i = 0;
		ParticleEmitter[] array = paperEmitters;
		for (int length = array.Length; i < length; i++)
		{
			array[i].emit = false;
			array[i].ClearParticles();
		}
		totalTokenEmitter.ClearParticles();
		powerMeter.activate();
		Invoke("showFriendsInvoke", 1f);
		continueButton.gameObject.SetActiveRecursively(false);
		menuButton.gameObject.SetActiveRecursively(false);
		restartButton.gameObject.SetActiveRecursively(false);
		Invoke("showReplayButton", 0.5f);
		GameData.saveHeartRecord(scoreAmount, maxScore);
		if (BaseMenuManager.getInstance() != null)
		{
			BaseMenuManager.getInstance().setTokenCounterVisible(true);
			BaseMenuManager.getInstance().setStoreButtonVisible(false);
		}
		if (!CommonData.IN_EDITOR_MODE)
		{
			bool flag2 = GameData.hasInternet() && CommunityFeatures.getInstance().isAvailable(0) && CommunityFeatures.getInstance().isAvailable(1);
			bool activeRecursively = flag2;
			twitterButton.gameObject.SetActiveRecursively(activeRecursively);
			facebookButton.gameObject.SetActiveRecursively(activeRecursively);
			facebookButton.setDimmed(false);
			twitterButton.setDimmed(false);
		}
	}

	private void showReplayButton()
	{
		restartButton.gameObject.SetActiveRecursively(true);
		(((ScaleAnimator)restartButton.GetComponent(typeof(ScaleAnimator))) as ScaleAnimator).activate();
		bool roundEarnedNewCart = RollerCoasterStatistics.getInstance().roundEarnedNewCart;
		restartButtonCartAdditionLabel.SetActiveRecursively(roundEarnedNewCart);
		if (roundEarnedNewCart)
		{
			Component[] componentsInChildren = restartButtonCartAdditionLabel.GetComponentsInChildren(typeof(CartImage));
			foreach (Component component in componentsInChildren)
			{
				int specialCartType = GameManager.getInstance().getSpecialCartType();
				((CartImage)component).activate(GameManager.getInstance().getTrainType(), specialCartType);
			}
		}
	}

	public override void deactivate()
	{
		disablePaperEmitters();
		powerMeter.deactivate();
		activated = false;
		CancelInvoke();
		resultList.deactivate();
		base.deactivate();
		CancelInvoke("restoreSharingButtons");
		ACTIVE = false;
	}

	public virtual void onFacebookShare()
	{
		if (!facebookButton.isDimmed())
		{
			SharingManager.getInstance().addListener(gameObject);
			shareResult(SharingManager.Service.Facebook, CommunityFeatures.COMMUNITY_TEXT_RESULT_SCREEN_FACEBOOK);
		}
	}

	public virtual void onTwitterShare()
	{
		if (!twitterButton.isDimmed())
		{
			SharingManager.getInstance().addListener(gameObject);
			shareResult(SharingManager.Service.Twitter, CommunityFeatures.COMMUNITY_TEXT_RESULT_SCREEN_TWITTER);
		}
	}

	public virtual void shareResult(SharingManager.Service service, string text)
	{
		SharingManager.getInstance().shareMessage(service, string.Format(text, heartSum));
	}

	public virtual void onContinueButton()
	{
		if (BaseMenuManager.getInstance().messageBox.isActive() || !ACTIVE)
		{
			return;
		}
		if (!CommonData.IN_EDITOR_MODE)
		{
			continuePresses++;
			bool flag = GameData.isNextLevelOrLevelpackUnlocked();
			int cash = MissionManager.getInstance().getActiveCompletedMissionScore() + GameData.getTokenAmount();
			enoughtMoneyForNext = GameData.hasEnoughMoneyToUnlockNextTrack(cash);
			if (!flag)
			{
				GameMenuManager.getInstance().showErrorMessage(TextData.RESULT_TITLE_CARTS_NEEDED, TextData.RESULT_TEXT_CARTS_NEEDED, cartImage);
				enoughtMoneyForNext = true;
				return;
			}
			if (!enoughtMoneyForNext)
			{
				GameMenuManager.getInstance().showErrorMessage(TextData.RESULT_TITLE_COINS_NEEDED, TextData.RESULT_TEXT_COINS_NEEDED, coinsImage);
				GameMenuManager.getInstance().messageBox.setCallBack(gameObject, "onNotEnoughMoneyDialogClosed");
				errorNotEnoughMoney = true;
				return;
			}
			RollerCoasterSounds.getInstance().stopMusic();
			GameManager.getInstance().gotoNextLevel();
		}
		else
		{
			onEndGame();
		}
	}

	public virtual void onMenuButton()
	{
		if (!ACTIVE)
		{
			return;
		}
		if (!CommonData.IN_EDITOR_MODE)
		{
			RollerCoasterSounds.getInstance().stopMusic();
			GameManager.getInstance().gotoNextLevel();
		}
		else
		{
			onEndGame();
		}
	}

	public virtual void onRestart()
	{
		if (ACTIVE)
		{
			(MenuButton.clickAnimator as ButtonPressAnimator).restore();
			RollerCoasterSounds.getInstance().stopMusic();
			GameManager.getInstance().restart();
		}
	}

	public virtual void onEndGame()
	{
		if (ACTIVE)
		{
			RollerCoasterSounds.getInstance().stopMusic();
			GameManager.getInstance().endGame();
		}
	}

	private List<ResultList.BonusEntry> createBonusList()
	{
		List<ResultList.BonusEntry> list = new List<ResultList.BonusEntry>();
		ResultList.BonusEntry bonusEntry = null;
		GameManager.getInstance().getVehicle();
		float activePriceMultiplier = GameData.getActivePriceMultiplier();
		for (int i = 0; i < 8; i++)
		{
			bonusEntry = new ResultList.BonusEntry();
			bonusEntry.type = i;
			bonusEntry.value = 0;
			bonusEntry.multiplier = 0;
			switch (i)
			{
			case 0:
				bonusEntry.value = (int)((float)GameManager.getInstance().getScoreAmount() * activePriceMultiplier);
				bonusEntry.multiplier = 0;
				break;
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
				if (GameData.getNumberOfAnimalsInTrainData(i - 1) > 0)
				{
					bonusEntry.multiplier = 1;
				}
				else
				{
					bonusEntry.multiplier = 0;
				}
				bonusEntry.value = RollerCoasterStatistics.getInstance().roundAnimalHearts[i - 1];
				break;
			case 6:
				if (RollerCoasterStatistics.getInstance().roundUnlockedNextTrack)
				{
					bonusEntry.value = (int)(GameData.BONUS_VALUES[i] * activePriceMultiplier);
					bonusEntry.multiplier = -1;
				}
				break;
			}
			if (bonusEntry.multiplier != 0)
			{
				list.Add(bonusEntry);
			}
		}
		return list;
	}

	private void addBonusType()
	{
		if (!activated)
		{
			return;
		}
		if (currentBonusIndex < bonusList.Count)
		{
			resultList.instatiateBonus(bonusList[currentBonusIndex], false);
			float activePriceMultiplier = GameData.getActivePriceMultiplier();
			heartSum += bonusList[currentBonusIndex].value;
			tokenSum += (float)bonusList[currentBonusIndex].value * GameData.BONUS_VALUES[0] * activePriceMultiplier;
			currentBonusIndex++;
		}
		else if (!(scorePercent <= 0.9f) && RollerCoasterStatistics.getInstance().roundDidFinish)
		{
			powerMeter.playBell();
			powerMeter.animateLights();
			int i = 0;
			ParticleEmitter[] array = paperEmitters;
			for (int length = array.Length; i < length; i++)
			{
				array[i].emit = true;
			}
			Invoke("disablePaperEmitters", 25f);
			RollerCoasterSounds.getInstance().playFireworks();
			Invoke("showTotalCoinsEarned", 1.25f);
		}
		else if (!(scorePercent <= 0.5f))
		{
			powerMeter.animateLights();
			Invoke("showTotalCoinsEarned", 0.75f);
		}
		else
		{
			Invoke("showTotalCoinsEarned", 0.25f);
		}
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

	public virtual void showFriendsInvoke()
	{
		powerMeter.startResultEntryAnimations();
	}

	public virtual void onSlideInComplete()
	{
		if (activated)
		{
			GeneralSoundEffects.playSound(bonusSound);
			resultList.heartSum.setValue((int)heartSum);
			powerMeter.setValue(getEstimatedScoreWithMaxCarts((int)heartSum));
			Invoke("addBonusType", 0.2f);
		}
	}

	private void showContinueAndMenuButtons()
	{
		continueButton.gameObject.SetActiveRecursively(true);
		(((ScaleAnimator)continueButton.GetComponent(typeof(ScaleAnimator))) as ScaleAnimator).activate();
		menuButton.gameObject.SetActiveRecursively(true);
		(((ScaleAnimator)menuButton.GetComponent(typeof(ScaleAnimator))) as ScaleAnimator).activate();
	}

	public virtual void showTotalCoinsEarned()
	{
		float num = resultList.tokenSum.setValue((int)Mathf.Round(tokenSum));
		Invoke("addCoinsToTotalCoins", num + 0.25f);
		showContinueAndMenuButtons();
		int cash = MissionManager.getInstance().getActiveCompletedMissionScore() + GameData.getTokenAmount();
		bool flag = GameData.isNextLevelOrLevelpackUnlocked();
		bool flag2 = GameData.hasEnoughMoneyToUnlockNextTrack(cash);
		if (!flag)
		{
			continueButtonPriceText.gameObject.transform.parent.gameObject.SetActiveRecursively(false);
		}
		else
		{
			continueButtonLock.SetActiveRecursively(false);
			if (flag2)
			{
				continueButtonPriceText.gameObject.transform.parent.gameObject.SetActiveRecursively(false);
			}
			else
			{
				continueButtonPriceText.setText(string.Empty + GameData.getNextTrackPrice());
			}
		}
		continueButton.setDimmed(!flag);
	}

	public virtual void addCoinsToTotalCoins()
	{
		if (!CommonData.IN_EDITOR_MODE)
		{
			float num = BaseMenuManager.getInstance().updateTokenAmount();
			if (GameData.LOG)
			{
				Debug.Log("TIME " + num);
			}
			Invoke("playMoneyDropSound", num + 0.25f);
			BaseMenuManager.getInstance().setStoreButtonVisible(true);
		}
	}

	public override void Update()
	{
		if (BaseMenuManager.getInstance().isMenuActive() || !CustomInput.getKeyDown(27))
		{
			return;
		}
		if (errorNotEnoughMoney)
		{
			GameMenuManager.getInstance().messageBox.onDismiss();
			errorNotEnoughMoney = false;
			backToPauseMenu = false;
			return;
		}
		if (backToPauseMenu)
		{
			onMenuButton();
		}
		backToPauseMenu = true;
	}

	public virtual void playMoneyDropSound()
	{
		RollerCoasterSounds.getInstance().playMoneyDropSound((int)Mathf.Min(2f, Mathf.Floor(scorePercent / 33f)));
		totalTokenEmitter.Emit(150);
	}

	public virtual int getEstimatedScoreWithMaxCarts(int currentScore)
	{
		float num = currentScore;
		num /= (float)maxScoreWithCurrentSetup;
		num *= (float)maxScore;
		if (GameData.LOG)
		{
			if (GameData.LOG)
			{
				Debug.Log("CURRENT SCORE \t\t\t" + currentScore);
			}
			if (GameData.LOG)
			{
				Debug.Log("MAX WITH CURRENT SETUP \t" + maxScoreWithCurrentSetup);
			}
			if (GameData.LOG)
			{
				Debug.Log("MAX WITH ALL \t\t\t" + maxScore);
			}
		}
		return (int)Mathf.Round(num);
	}

	public virtual int getMaxScoreWithCurrentSetup()
	{
		int num = calculateCheckPointHearts();
		int num2 = calculateLevelHearts();
		int passengerCartCount = GameManager.getInstance().getVehicle().getPassengerCartCount();
		Vehicle vehicle = GameManager.getInstance().getVehicle();
		int num3 = 0;
		for (int i = 1; i < passengerCartCount + 1; i++)
		{
			Cart cart = vehicle.carts[i];
			int num4 = cart.heartValue;
			if (num4 == 0)
			{
				num4 = GameData.ANIMAL_HEART_VALUE[0];
			}
			num3 += num4 * num;
			if (cart.hasAnimal())
			{
				num3 = (int)((float)num3 + GameData.BONUS_VALUES[1 + cart.animalIndex]);
				int num5 = 0;
				if (cart.animal.givesExtraHeartsFromBumps)
				{
					num5 = Animal.MAXIMUM_EXTRA_HEARTS_FROM_BUMPS;
				}
				if (cart.animal.givesExtraHeartsWhenInAir)
				{
					num5 = Animal.MAXIMUM_EXTRA_HEARTS_FROM_AIR;
				}
				if (cart.animal.givesExtraHeartsWhenDropped)
				{
					num5 = Animal.MAXIMUM_EXTRA_HEARTS_FROM_DROPPING;
				}
				num3 = (int)((float)num3 + (float)num5 * 0.15f);
			}
			else
			{
				num3 = (int)((float)num3 + GameData.BONUS_VALUES[1]);
				num3 = (int)((float)num3 + 0.15f * (float)Mathf.Min(Mathf.Min(Animal.MAXIMUM_EXTRA_HEARTS_FROM_BUMPS, Animal.MAXIMUM_EXTRA_HEARTS_FROM_AIR), Animal.MAXIMUM_EXTRA_HEARTS_FROM_DROPPING));
			}
		}
		return num3 + num2;
	}

	public virtual int getMaxScoreWithBestSetup()
	{
		int num = calculateCheckPointHearts();
		int num2 = calculateLevelHearts();
		int cART_LIMIT = GameData.CART_LIMIT;
		GameManager.getInstance().getVehicle();
		int num3 = 0;
		for (int i = 0; i < cART_LIMIT; i++)
		{
			int num4 = GameData.ANIMAL_HEART_VALUE[4];
			num3 += num4 * num;
			num3 = (int)((float)num3 + GameData.BONUS_VALUES[5]);
			num3 = (int)((float)num3 + 0.15f * (float)Mathf.Max(Mathf.Max(Animal.MAXIMUM_EXTRA_HEARTS_FROM_BUMPS, Animal.MAXIMUM_EXTRA_HEARTS_FROM_AIR), Animal.MAXIMUM_EXTRA_HEARTS_FROM_DROPPING));
		}
		return num3 + num2;
	}

	private int calculateCheckPointHearts()
	{
		Component[] componentsInChildren = GameManager.getInstance().getGameRoot().GetComponentsInChildren(typeof(TriggerCheckPoint), true);
		int num = 0;
		foreach (Component component in componentsInChildren)
		{
			TriggerCheckPoint triggerCheckPoint = component as TriggerCheckPoint;
			if (triggerCheckPoint != null)
			{
				if (triggerCheckPoint.kind == 0)
				{
					num++;
				}
				else if (triggerCheckPoint.kind == 1)
				{
					num++;
				}
			}
		}
		return num;
	}

	private int calculateLevelHearts()
	{
		Component[] componentsInChildren = GameManager.getInstance().getGameRoot().GetComponentsInChildren(typeof(TriggerCoin), true);
		int num = 0;
		foreach (Component component in componentsInChildren)
		{
			TriggerCoin triggerCoin = component as TriggerCoin;
			if (triggerCoin != null)
			{
				num += triggerCoin.amount;
			}
		}
		return num;
	}

	public virtual void setVisible(bool visible)
	{
		if (!visible)
		{
			Vector3 localPosition = transform.localPosition;
			localPosition.x = -5;
			transform.localPosition = localPosition;
			return;
		}
		Vector3 localPosition2 = transform.localPosition;
		localPosition2.x = 0;
		transform.localPosition = localPosition2;
		if ((bool)BaseMenuManager.getInstance())
		{
			BaseMenuManager.getInstance().freeCoinsMenu.deactivate();
			BaseMenuManager.getInstance().coinStoreMenu.deactivate();
		}
	}

	public virtual void onNotEnoughMoneyDialogClosed()
	{
		backToPauseMenu = false;
		if (!enoughtMoneyForNext && GameData.canMakeInAppPurchases())
		{
			GameMenuManager.getInstance().onStore();
		}
	}

	public virtual void onSharingFailed()
	{
		Invoke("restoreSharingButtons", 3f);
	}

	public virtual void restoreSharingButtons()
	{
		twitterButton.setDimmed(hasTwittered);
		facebookButton.setDimmed(hasFacebooked);
	}

	public virtual void onFacebookPostCompleted()
	{
		hasFacebooked = true;
		facebookButton.setDimmed(hasFacebooked);
	}

	public virtual void onTwitterPostCompleted()
	{
		hasTwittered = true;
		twitterButton.setDimmed(hasTwittered);
	}
}
