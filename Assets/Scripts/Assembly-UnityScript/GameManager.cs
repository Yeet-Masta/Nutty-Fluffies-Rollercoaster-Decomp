using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public const int STATE_NONE = 0;

	public const int STATE_START = 1;

	public const int STATE_DRIVE = 2;

	public const int STATE_FINISH = 3;

	public const int STATE_CRASH = 4;

	public const int STATE_ENDING = 5;

	public const int STATE_RESTARTING = 6;

	public const int CRASH_DELAY = 2;

	private static GameManager instance;

	private GameObject gameRoot;

	private Level level;

	private Vehicle vehicle;

	private int gameState;

	private int collectedScore;

	private int specialCartType;

	private int trainType;

	private bool vehicleFinished;

	private Vector2 startPosition;

	private bool fading;

	private bool inGameMode;

	private bool hasCheckedMissions;

	public GameCamera cameraRef;

	public GameUI gameUI;

	[HideInInspector]
	public RollerCoasterStatistics stat;

	[HideInInspector]
	public RollerCoasterSounds sm;

	public Controller controller;

	public GameObject fpsCounter;

	public static bool handleBackKey = true;

	public static bool isInPauseMenu;

	[HideInInspector]
	public int restarts;

	public LocalizationManager localizationManager;

	private int frameCounter;

	private Vector2 startPoint;

	private int backCount;

	private bool m_back;

	public static GameManager getInstance()
	{
		return instance;
	}

	public virtual void Awake()
	{
		Application.targetFrameRate = GameData.targetFPS;
		inGameMode = false;
		Resources.UnloadUnusedAssets();
		localizationManager.Awake();
		CommonData.init();
		GameData.init();
		GameData.reloadDynamicProperties();
		useGUILayout = false;
		instance = this;
		gameRoot = gameObject;
		setGameState(0);
		stat = RollerCoasterStatistics.getInstance();
		sm = RollerCoasterSounds.getInstance();
		controller = NFPortInput.UseTouch ? (NFPortInput.UseTouch ? (NFPortInput.UseTouch ? controller.GetComponent<TouchScreenController>() as Controller : controller.GetComponent<KeyboardController>() as Controller) : controller.GetComponent<KeyboardController>() as Controller) : controller.GetComponent<KeyboardController>() as Controller;
		restarts = 0;
		gameUI.hideAll();
		gameUI.activatePauseButton(false);
	}

	public virtual void Start()
	{
		if (GameData.SCALEDOWN && Screen.width > 854)
		{
			Screen.SetResolution((int)((float)Screen.width * 0.67f), (int)((float)Screen.height * 0.67f), Screen.fullScreenMode);
		}
		handleBackKey = true;
		RollerCoasterMissions.getInstance();
		EffectManager.getInstance().reset();
		sm = RollerCoasterSounds.getInstance();
	}

	public virtual void OnApplicationQuit()
	{
		if (GameData.LOG)
		{
			print("Application quitting");
		}
	}

	public virtual void startGame()
	{
		gameUI.fadeIn.invokeRef = gameObject;
		isInPauseMenu = false;
		if (CommonData.IN_EDITOR_MODE)
		{
			loadLevel();
		}
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (GameData.LOG)
		{
			print("ZLEVEL COMPLETE AT " + Time.realtimeSinceStartup + " = (" + (Time.realtimeSinceStartup - realtimeSinceStartup) + ")");
		}
		TutorialManager.getInstance().Awake();
		if (GameData.LOG)
		{
			print("TUTORIAL COMPLETE AT " + Time.realtimeSinceStartup + " = (" + (Time.realtimeSinceStartup - realtimeSinceStartup) + ")");
		}
		GameData.VehicleData vehicleData = null;
		vehicleData = CommonData.IN_EDITOR_MODE ? GameData.VehicleData.createTestTrain(specialCartType) : GameData.TRAIN_DATA;
		vehicle = VehicleFactory.getInstance().create(vehicleData, gameRoot, new Vector3(startPoint.x, startPoint.y, 0f));
		vehicle.controller = controller;
		specialCartType = vehicleData.specialCart;
		trainType = vehicleData.trainType;
		if (GameData.LOG)
		{
			print("VEHICLE AT " + Time.realtimeSinceStartup + " = (" + (Time.realtimeSinceStartup - realtimeSinceStartup) + ")");
		}
		cameraRef.cameraTarget = vehicle.getFirstCart().transform;
		cameraRef.adjustForTrainLength(vehicle.getCartCount());
		cameraRef.lastCart = vehicle.getLastCart().transform;
		sm.prepare();
		sm.playGameMusic(level.themeSettings.musicNames);
		if (GameData.LOG)
		{
			print("AFTER MUSIC AT " + Time.realtimeSinceStartup + " = (" + (Time.realtimeSinceStartup - realtimeSinceStartup) + ")");
		}
		setGameState(1);
		collectedScore = 0;
		if (GameData.LOG)
		{
			print("AFTER STATES AT " + Time.realtimeSinceStartup + " = (" + (Time.realtimeSinceStartup - realtimeSinceStartup) + ")");
		}
		gameUI.showSpecialActive(specialCartType, 1);
		Time.timeScale = 1f;
		stat.startLevel();
		if (GameData.LOG)
		{
			print("START LEVEL COMPLETE AT " + Time.realtimeSinceStartup + " = (" + (Time.realtimeSinceStartup - realtimeSinceStartup) + ")");
		}
		controller.reset();
		if (!GameData.SHOW_FPS)
		{
			UnityEngine.Object.Destroy(fpsCounter);
		}
		if (GameData.LOG)
		{
			print("ALL COMPLETE AT " + Time.realtimeSinceStartup + " = (" + (Time.realtimeSinceStartup - realtimeSinceStartup) + ")");
		}
		frameCounter = 0;
		fading = true;
		vehicleFinished = false;
		LevelFactory.getInstance().resetLevel(level);
		cameraRef.setPosition(startPosition, 1f);
		cameraRef.resetZoom();
		inGameMode = true;
		hasCheckedMissions = false;
		gameUI.resultScreen.calculateMaxScores();
	}

	public virtual void loadLevel()
	{
		ObjectListData objects = new ObjectListData();
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (GameData.LOG)
		{
			print("STARTING TO LOAD LEVEL AT " + Time.realtimeSinceStartup);
		}
		LevelData levelData = null;
		if (CommonData.IN_EDITOR_MODE)
		{
			levelData = LevelLoader.getInstance().load(CommonData.LEVEL_PACK_NAME, CommonData.LEVEL_NAME, objects);
		}
		else
		{
			levelData = LevelLoaderBinary.getInstance().load(CommonData.LEVEL_PACK_NAME, CommonData.LEVEL_NAME, objects);
			if (levelData == null)
			{
				levelData = LevelLoader.getInstance().load(CommonData.LEVEL_PACK_NAME, CommonData.LEVEL_NAME, objects);
			}
		}
		specialCartType = levelData.customSettings.getIntValue("specialcart", -1);
		level = LevelFactory.getInstance().create(levelData, gameRoot, objects);
		LevelFactory.getInstance().createAutoResetTrigger(level, levelData);
		if (GameData.LOG)
		{
			print("LOAD COMPLETE AT " + Time.realtimeSinceStartup + " = (" + (Time.realtimeSinceStartup - realtimeSinceStartup) + ")");
		}
		ZLevel.getInstance().create(level);
		startPoint = levelData.startPoint;
		cameraRef.setLimits(levelData.topLeftLimit, levelData.bottomRightLimit);
		cameraRef.setPosition(levelData.startPoint, 1f);
		startPosition = levelData.startPoint;
		sm = RollerCoasterSounds.getInstance();
		sm.playGameMusic(level.themeSettings.musicNames);
	}

	public virtual Level getLevel()
	{
		return level;
	}

	public virtual void pauseGame()
	{
		isInPauseMenu = true;
		handleBackKey = false;
		if (gameState != 3 && gameState != 1)
		{
			gameUI.fadeIn.invokeRef = gameObject;
			CancelInvoke();
			gameUI.pauseGame();
			Time.timeScale = 0f;
			if (vehicle != null)
			{
				vehicle.stopSounds();
			}
			sm.stopGameSounds();
			PauseMenu.handlebackKey = true;
		}
	}

	public virtual void resumeGame()
	{
		PauseMenu.handlebackKey = false;
		isInPauseMenu = false;
		if (gameState == 4)
		{
			stopGame();
		}
		else
		{
			gameUI.resumeGame();
			vehicle.startSounds();
			sm.resumeGameSounds();
			gameUI.fadeIn.gameObject.SetActiveRecursively(false);
		}
		Time.timeScale = 1f;
		handleBackKey = true;
	}

	public virtual void gotoNextLevel()
	{
		MissionMenu.GOING_TO_MENU = true;
		gameUI.endGame();
		vehicle.stopSounds();
		Time.timeScale = 1f;
		GameMenuManager.getInstance().onEndGame();
		inGameMode = false;
	}

	public virtual void Update()
	{
		if (CustomInput.getKeyDown(27) && handleBackKey && gameState == 2)
		{
			backCount = 0;
			m_back = true;
		}
		if (m_back)
		{
			handleBackKey = false;
			backCount++;
		}
		if (backCount == 10)
		{
			pauseGame();
			m_back = false;
			backCount = 0;
		}
		frameCounter++;
		if (frameCounter == 3)
		{
			gameUI.fadeIn.activate();
			fading = true;
		}
	}

	public virtual void restart()
	{
		if (gameState != 0)
		{
			handleBackKey = true;
			isInPauseMenu = false;
			setGameState(6);
			gameUI.endGame();
			vehicle.stopSounds();
			sm.playGameMusic(level.themeSettings.musicNames);
			Time.timeScale = 1f;
		}
	}

	public virtual void crash()
	{
		if (gameState == 2)
		{
			stat.failLevel();
			setGameState(4);
			vehicle.setCrashed();
			updateTrainData(true);
			Invoke("stopGame", 2f);
		}
	}

	public virtual void endGame()
	{
		if (gameState < 5)
		{
			gameUI.fadeIn.invokeRef = gameObject;
			Time.timeScale = 1f;
			gameUI.endGame();
			if (CommonData.IN_EDITOR_MODE)
			{
				setGameState(0);
			}
			else
			{
				setGameState(5);
			}
		}
	}

	private IEnumerator resetResolution()
	{
		Screen.SetResolution(GameData.actualDeviceResolution.width, GameData.actualDeviceResolution.height, Screen.fullScreenMode);
		yield return null;
	}

	private void returnToMenuScene()
	{
		if (GameData.SCALEDOWN)
		{
			StartCoroutine(resetResolution());
		}
		if (!CommonData.IN_EDITOR_MODE)
		{
			TextureManager.getInstance().purge();
			MaterialManager.getInstance().purge();
			Resources.UnloadUnusedAssets();
		}
		Application.LoadLevel("MenuScene");
	}

	public virtual void finishGame()
	{
		handleBackKey = false;
		if (!isPlaying())
		{
			return;
		}
		vehicleFinished = true;
		if (!CommonData.IN_EDITOR_MODE)
		{
			updateTrainData(false);
		}
		CancelInvoke();
		stat.finishLevel();
		sm.playFinish();
		float num = 0.5f;
		int maxAnimalIndex = vehicle.getMaxAnimalIndex();
		num += 0.15f * (float)vehicle.getCartCount();
		num += GameData.BONUS_VALUES[maxAnimalIndex + 1] * 0.15f;
		Invoke("stopGame", Mathf.Max(1f, num));
		if (!CommonData.IN_EDITOR_MODE)
		{
			if (vehicle.getPassengerCartCount() < GameData.CART_LIMIT)
			{
				stat.roundEarnedNewCart = true;
			}
			if (!hasCheckedMissions)
			{
				RollerCoasterMissions.getInstance().checkMissions(GameData.MAX_ACTIVE_MISSIONS);
				hasCheckedMissions = true;
			}
			GameData.completeLevel(GameData.SELECTED_LEVEL_INDEX, vehicle.getPassengerCartCount());
			if (vehicle.getPassengerCartCount() >= GameData.CARTS_TO_UNLOCK_NEXT_LEVEL)
			{
				GameData.unlockNextLevel();
			}
		}
		vehicle.onFinish();
		gameUI.showFinishBadge();
		RollerCoasterSounds.getInstance().playFinishArriveSound();
	}

	public virtual void stopGame()
	{
		vehicle.onStopGame();
		setGameState(3);
	}

	public virtual void useSpecial()
	{
		if (isPlaying() && vehicle.activateSpecial())
		{
			gameUI.showSpecialActive(specialCartType, 0);
			sm.playSpecialActivation(specialCartType);
			sm.playSpecialLoop(specialCartType);
		}
	}

	public virtual void onCheckPoint(int type)
	{
		sm.playCheckPoint();
	}

	public virtual void onOutOfLevel()
	{
		stat.fellOutOfTrack();
		crash();
	}

	public virtual void onFadeInComplete()
	{
		fading = false;
		if (gameState == 5)
		{
			returnToMenuScene();
		}
		else if (gameState == 6)
		{
			setGameState(0);
			CancelInvoke();
			if (CommonData.IN_EDITOR_MODE)
			{
				UnityEngine.Object.Destroy(level.gameObject);
			}
			UnityEngine.Object.Destroy(vehicle.gameObject);
			restarts++;
			if (CommonData.IN_EDITOR_MODE)
			{
				startGame();
				return;
			}
			gameUI.fadeIn.invokeRef = GameMenuManager.getInstance().gameObject;
			GameMenuManager.getInstance().onRestartGame();
			inGameMode = false;
		}
		else if (gameState == 1)
		{
			gameUI.fadeIn.gameObject.SetActiveRecursively(false);
			if (CommonData.IN_EDITOR_MODE || restarts > 0 || !TutorialManager.getInstance().showPage("controls"))
			{
				setGameState(2);
			}
		}
	}

	public virtual bool isFading()
	{
		return fading;
	}

	public virtual void onSpecialReady()
	{
		if (isControllingEnabled())
		{
			gameUI.showSpecialActive(specialCartType, 1);
			sm.playSpecialReady();
		}
	}

	public virtual void onCartInCheckPoint(Cart cart, int type)
	{
		if (isControllingEnabled())
		{
			int cartScoreInCheckPoint = vehicle.getCartScoreInCheckPoint(cart, type);
			if (cartScoreInCheckPoint > 0)
			{
				addAnimalScore(cartScoreInCheckPoint, cart.animalIndex, cart);
			}
		}
	}

	public virtual void onSpecialUsed()
	{
		sm.playSpecialDeactivation();
	}

	public virtual void onAnimalDrop(Cart cart)
	{
		if (isControllingEnabled())
		{
			stat.itemDropped();
		}
	}

	public virtual void addScores(int amount)
	{
		if (!vehicle.hasCrashed)
		{
			collectedScore += amount;
		}
	}

	public virtual void addAnimalScore(int amount, int animalIndex)
	{
		if (!vehicle.hasCrashed && animalIndex != GameData.ANIMAL_NONE)
		{
			RollerCoasterStatistics.getInstance().animalHearts(animalIndex, amount);
			collectedScore += amount;
		}
	}

	public virtual void addAnimalScore(int amount, int animalIndex, Cart cart)
	{
		if (!vehicle.hasCrashed && animalIndex != GameData.ANIMAL_NONE)
		{
			addAnimalScore(amount, animalIndex);
			cart.emitHearts(amount);
		}
	}

	public virtual int getScoreAmount()
	{
		return collectedScore;
	}

	public virtual GameObject getGameRoot()
	{
		return gameRoot;
	}

	public virtual Vehicle getVehicle()
	{
		return vehicle;
	}

	public virtual void setGameState(int newState)
	{
		if (gameState == newState)
		{
			return;
		}
		gameState = newState;
		switch (newState)
		{
		case 1:
			gameUI.startGame();
			RollerCoasterSounds.getInstance().playStart();
			break;
		case 2:
			gameUI.activatePauseButton(true);
			break;
		case 3:
			if (!hasCheckedMissions)
			{
				RollerCoasterMissions.getInstance().checkMissions(GameData.MAX_ACTIVE_MISSIONS);
				hasCheckedMissions = true;
			}
			gameUI.showResults();
			cameraRef.cameraTarget = vehicle.getMiddleCart().transform;
			break;
		}
	}

	public virtual bool isControllingEnabled()
	{
		bool flag = gameState == 2;
		if (flag)
		{
			flag = !vehicleFinished;
		}
		return flag;
	}

	public virtual bool isPlaying()
	{
		bool flag = gameState == 2;
		if (flag)
		{
			flag = !vehicleFinished;
		}
		if (flag)
		{
			flag = Time.timeScale > 0f;
		}
		return flag;
	}

	public virtual bool isInGameMode()
	{
		return inGameMode;
	}

	public virtual int getSpecialCartType()
	{
		return specialCartType;
	}

	public virtual int getTrainType()
	{
		return trainType;
	}

	public virtual int getGameState()
	{
		return gameState;
	}

	public virtual void updateTrainData(bool didCrash)
	{
		float aNIMAL_REMOVE_AMOUNT_MAX_PERCENT = GameData.ANIMAL_REMOVE_AMOUNT_MAX_PERCENT;
		int aNIMAL_REMOVE_AMOUNT_MAX = GameData.ANIMAL_REMOVE_AMOUNT_MAX;
		int aNIMAL_REMOVE_AMOUNT_MIN = GameData.ANIMAL_REMOVE_AMOUNT_MIN;
		int length = GameData.TRAIN_DATA.cartList.Length;
		if (!didCrash)
		{
			int num = (int)Mathf.Min(aNIMAL_REMOVE_AMOUNT_MAX, Mathf.Max(aNIMAL_REMOVE_AMOUNT_MIN, (float)length * aNIMAL_REMOVE_AMOUNT_MAX_PERCENT));
			int num2 = 0;
			for (int i = 1; i < vehicle.carts.Length; i++)
			{
				if (!vehicle.carts[i].hasAnimal())
				{
					if (num2 < num)
					{
						GameData.TRAIN_DATA.cartList[i - 1].animalIndex = GameData.ANIMAL_NONE;
						num2++;
					}
				}
				else
				{
					GameData.TRAIN_DATA.cartList[i - 1].animalIndex = vehicle.carts[i].animalIndex;
				}
			}
		}
		else
		{
			int num3 = (int)Mathf.Min(aNIMAL_REMOVE_AMOUNT_MAX, Mathf.Max(aNIMAL_REMOVE_AMOUNT_MIN, (float)length * aNIMAL_REMOVE_AMOUNT_MAX_PERCENT));
			int num4 = 0;
			int num5 = 0;
			while (num3 > 0 && num5 < 30)
			{
				num5++;
				if (!(UnityEngine.Random.value <= 0.5f) && GameData.TRAIN_DATA.cartList[num4].animalIndex != GameData.ANIMAL_NONE)
				{
					GameData.TRAIN_DATA.cartList[num4].animalIndex = GameData.ANIMAL_NONE;
					num3--;
				}
				num4 = (num4 + 1) % length;
			}
		}
		GameData.saveTrain();
	}

	public virtual void OnApplicationFocus(bool focus)
	{
		if (!focus)
		{
			sm.pauseMusic();
			if (gameState == 2 && !vehicleFinished && !isInPauseMenu)
			{
				pauseGame();
			}
		}
		else
		{
			sm.continueMusic();
		}
	}
}
