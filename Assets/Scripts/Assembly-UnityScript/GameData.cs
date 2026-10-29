using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class GameData
{
	[Serializable]
	public class CartData
	{
		public int animalIndex;
	}

	[Serializable]
	public class VehicleData
	{
		public const int TRAIN_TYPE_WOOD = 0;

		public const int TRAIN_TYPE_METAL = 1;

		public const int TRAIN_TYPE_LOOP = 2;

		public const int TRAIN_TYPE_EXTREME = 3;

		public int trainType;

		public int specialCart;

		public CartData[] cartList;

		public static VehicleData createTestTrain(int specialCart)
		{
			VehicleData vehicleData = new VehicleData();
			int cartCount = 8;
			vehicleData.trainType = 0;
			vehicleData.cartList = new CartData[cartCount];
			for (int i = 0; i < cartCount; i++)
			{
				vehicleData.cartList[i] = new CartData();
				vehicleData.cartList[i].animalIndex = i % 5;
			}
			vehicleData.specialCart = specialCart;
			return vehicleData;
		}

		public static VehicleData createBasicTrain()
		{
			VehicleData vehicleData = new VehicleData();
			vehicleData.cartList = new CartData[MIN_CART_COUNT];
			for (int i = 0; i < MIN_CART_COUNT; i++)
			{
				vehicleData.cartList[i] = new CartData();
				vehicleData.cartList[i].animalIndex = 0;
			}
			vehicleData.specialCart = Vehicle.CART_NORMAL;
			return vehicleData;
		}
	}

	public static readonly int ANIMAL_NONE = -1;

	public const int ANIMAL_MOUSE = 0;

	public const int ANIMAL_KITTEN = 1;

	public const int ANIMAL_PUPPY = 2;

	public const int ANIMAL_PIG = 3;

	public const int ANIMAL_ELEPHANT = 4;

	public const int ANIMAL_COUNT = 5;

	public static int[] ANIMAL_HEART_VALUE = new int[5] { 1, 1, 1, 1, 1 };

	public static string[] ANIMAL_DESCRIPTIONS = new string[6] { "The Mouse gives one heart at each\ncheckpoint, and 2 hearts at the finish.", "The Cat gives extra hearts going\ndownhill, and 4 hearts at the finish.", "The Dog gives extra hearts from jumps,\nand 6 hearts at the finish.", "The Pig gives extra hearts from bumps,\nand 8 hearts at the finish.", "The Elephant gives extra hearts from\ndownhill and jumping, 10 hearts at finish.", "Get one extra cart immediately!" };

	public static string GAME_NAME = "Nutty Fluffies";

	public static string SERVER_ADDRESS = "http://10.10.254.216/~pekkasarkimo/";

	public static string FACEBOOK_LIKE_URL = "http://www.facebook.com/RedLynxGamer";

	public static string APP_STORE_ID = "com.UBI.A90.WW";

	public static string VERSION_CHECK_URL;

	public static string APPSTORE_PURCHASE_PAGE_GOOGLE = "https://play.google.com/store/apps/details?id=com.UBI.A90.WW";

	public static string MAC_APPSTORE_RATING_URL = "macappstore://itunes.apple.com/app/id550995178";

	public static string GOOGLE_PLAY_RATING_URL = "market://details?id=com.UBI.A90.WW";

	public static bool USE_TEST_APPSTORE_SERVER = true;

	public static string APPPERSE_KEY = "40155c55bbe24e3cac4d533570ce7dc1";

	public static string APPSTORE_REVIEW_PAGE = "itms-apps://ax.itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id=553783329";

	public static float APPSTORE_REVIEW_SCORE_PERCENT = 0.75f;

	public static int APPSTORE_REVIEW_LAUNCHES = 1;

	public static int APPSTORE_REVIEW_MINIMUM_LEVEL = 6;

	public static int APPSTORE_REVIEW_MINIMUM_LEVELPACK;

	public static int APPSTORE_ASK_AGAIN_HOURS = 1;

	public static bool TEST_BUILD;

	public static bool UNLOCK_LEVELS;

	public static string VERSION_STRING = "1.0.5";

	public static string VERSION_STRING_VISIBLE = "V" + VERSION_STRING;

	public static string BUILD_STRING = string.Empty;

	public static bool SHOW_FPS = TEST_BUILD;

	public static bool LOCALIZATION_TEST;

	public static bool LOG = true;

	public static bool EXTRA_LEVEL_PACKS = true;


	public static bool SCALEDOWN;

	public static int targetFPS = 60;

	private static List<LevelPack> LEVEL_PACKS;

	private static int ACTIVE_LEVEL_PACK;

	public static int SELECTED_LEVEL_INDEX;

	private static bool INITED;

	public static int LEVEL_FLAG_BUILT = 65536;

	public const int HEARTS_FOR_SPEED = 1;

	public const int HEARTS_FOR_G = 1;

	public const int HEARTS_FOR_HEIGHT = 1;

	public static int CART_LIMIT = 10;

	public static int MIN_CART_COUNT = 4;

	public static int MAX_ACTIVE_MISSIONS = 4;

	public static int CARTS_TO_UNLOCK_NEXT_LEVEL = 4;

	public const int BONUS_HEARTS = 0;

	public const int BONUS_MOUSE = 1;

	public const int BONUS_CAT = 2;

	public const int BONUS_DOG = 3;

	public const int BONUS_PIG = 4;

	public const int BONUS_ELEPHANT = 5;

	public const int BONUS_UNLOCK_NEXT = 6;

	public const int BONUS_FINISH = 7;

	public const int BONUS_TYPE_COUNT = 8;

	public static readonly int BONUS_MOST_VALUABLE = 5;

	public static float[] BONUS_VALUES = new float[8] { 0.045f, 2f, 8f, 8f, 10f, 12f, 0f, 0f };

	public static int HEARTS_PER_ANIMAL_DROP;

	public static int INITIAL_TOKEN_AMOUNT = 50;

	public static DefaultPrice[] DEFAULT_PRICES = new DefaultPrice[21]
	{
		new DefaultPrice("DP_CONSTRUCTION_PRICE", 50),
		new DefaultPrice("ITEM_ANIMAL_MOUSE", 0),
		new DefaultPrice("ITEM_ANIMAL_CAT", 2),
		new DefaultPrice("ITEM_ANIMAL_DOG", 3),
		new DefaultPrice("ITEM_ANIMAL_PIG", 4),
		new DefaultPrice("ITEM_ANIMAL_ELEPHANT", 5),
		new DefaultPrice("DP_WOOD_CART", 15),
		new DefaultPrice("DP_METAL_CART", 15),
		new DefaultPrice("DP_LOOP_CART", 15),
		new DefaultPrice("DP_EXTREME_CART", 15),
		new DefaultPrice("DP_PURCHASE_TOKENS1", 300),
		new DefaultPrice("DP_PURCHASE_TOKENS2", 800),
		new DefaultPrice("DP_PURCHASE_TOKENS3", 2500),
		new DefaultPrice("DP_PURCHASE_TOKENS4", 7000),
		new DefaultPrice("DP_PURCHASE_TOKENS5", 25000),
		new DefaultPrice("DP_PURCHASE_TOKENS6", 60000),
		new DefaultPrice("DP_PURCHASE_FACEBOOK_POST", 100),
		new DefaultPrice("DP_PURCHASE_SEND_EMAIL", 75),
		new DefaultPrice("DP_PURCHASE_TWITTER_POST", 100),
		new DefaultPrice("DP_PURCHASE_FACEBOOK_LIKE", 75),
		new DefaultPrice("DP_PURCHASE_TAPJOY_OFFERWALL", 0)
	};

	public static int TRACK_REVENUE = 20;

	public static int TRACK_REVENUE_MAX = 80;

	private static float[] LEVEL_PACK_PRICE_MULTIPLIERS = new float[5] { 1f, 1.5f, 2f, 2.5f, 1f };

	private static float[] LEVEL_INDEX_PRICE_MULTIPLIERS = new float[11]
	{
		1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f, 1f,
		1f
	};

	private static float[] LEVEL_PRICE_INCREMENT_MULTIPLIERS = new float[5] { 0f, 0f, 0f, 0f, 1f };

	public static List<StoreKitProduct> INAPP_PURCHASE_LIST;

	public static Dictionary<string, int> INAPP_PURCHASE_TOKENS = new Dictionary<string, int>();

	public static int SOUND_LEVEL = 2;

	public static int TOTAL_TOKENS;

	public static VehicleData TRAIN_DATA = new VehicleData();

	public static int[] NOTIFICATION_DAYS = new int[4] { 1, 2, 5, 10 };

	public const int FEATURE_NONE = 0;

	public const int FEATURE_CART_STORE = 1;

	public const int FEATURE_CAT = 2;

	public const int FEATURE_DOG = 4;

	public const int FEATURE_PIG = 8;

	public const int FEATURE_ELEPHANT = 16;

	public const int FEATURE_METAL_PACK = 1024;

	public const int FEATURE_LOOP_PACK = 2048;

	public const int FEATURE_EXTREME_PACK = 4096;

	public const int FEATURE_DOUBLE_COINS = 8192;

	public const int FEATURE_HISTORY_SCREEN = 16384;

	public const int FEATURE_ROCKET_CART = 32768;

	public const int FEATURE_MAGNET_CART = 65536;

	public static readonly int FEATURE_HOVER_CART = 131072;

	public static readonly int FEATURE_BUY_CART = 262144;

	private static int FEATURE_LIST = 0;

	private static int FEATURE_DESCRIPT_LIST;

	public static float ANIMAL_REMOVE_AMOUNT_MAX_PERCENT = 0.7f;

	public static int ANIMAL_REMOVE_AMOUNT_MAX = 8;

	public static int ANIMAL_REMOVE_AMOUNT_MIN = 2;

	public static Resolution actualDeviceResolution;

	private static bool isResolutionSet;

	public static void init()
	{
		if (!isResolutionSet)
		{
			actualDeviceResolution = Screen.currentResolution;
			isResolutionSet = true;
		}
		if (!INITED)
		{
			Application.runInBackground = true;
			reloadLevelPacks();
			INITED = true;
			load();
			RollerCoasterStatistics.getInstance();
			if (!TEST_BUILD)
			{
				UNLOCK_LEVELS = false;
			}
			else if (TOTAL_TOKENS < 10000)
			{
				TOTAL_TOKENS = 10000;
			}
			if (!CommonData.IN_EDITOR_MODE)
			{
				TRAIN_DATA = VehicleData.createBasicTrain();
			}
			if (LOCALIZATION_TEST)
			{
				CARTS_TO_UNLOCK_NEXT_LEVEL = 1;
				BONUS_VALUES[0] = 1f;
			}
		}
	}

	private static void loadDynamicFloatPropertyArray(float[] array, string key)
	{
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = PlayerPrefs.GetFloat(key + i, array[i]);
		}
	}

	private static void loadDynamicProperties()
	{
		CART_LIMIT = PlayerPrefs.GetInt("DP_CART_LIMIT", CART_LIMIT);
		MIN_CART_COUNT = PlayerPrefs.GetInt("DP_MIN_CART_COUNT", MIN_CART_COUNT);
		INITIAL_TOKEN_AMOUNT = PlayerPrefs.GetInt("DP_INITIAL_TOKEN_AMOUNT", INITIAL_TOKEN_AMOUNT);
		reloadDynamicProperties();
	}

	public static void reloadDynamicProperties()
	{
		APPSTORE_REVIEW_SCORE_PERCENT = PlayerPrefs.GetFloat("DP_APPSTORE_REVIEW_SCORE_PERCENT", APPSTORE_REVIEW_SCORE_PERCENT);
		APPSTORE_REVIEW_LAUNCHES = (int)PlayerPrefs.GetFloat("DP_APPSTORE_REVIEW_LAUNCHES", APPSTORE_REVIEW_LAUNCHES);
		APPSTORE_REVIEW_MINIMUM_LEVEL = (int)PlayerPrefs.GetFloat("DP_APPSTORE_REVIEW_MINIMUM_LEVEL", APPSTORE_REVIEW_MINIMUM_LEVEL);
		APPSTORE_REVIEW_MINIMUM_LEVELPACK = (int)PlayerPrefs.GetFloat("DP_APPSTORE_REVIEW_MINIMUM_LEVELPACK", APPSTORE_REVIEW_MINIMUM_LEVELPACK);
		APPSTORE_ASK_AGAIN_HOURS = (int)PlayerPrefs.GetFloat("DP_APPSTORE_ASK_AGAIN_HOURS", APPSTORE_ASK_AGAIN_HOURS);
		TRACK_REVENUE = PlayerPrefs.GetInt("DP_TRACK_REVENUE", TRACK_REVENUE);
		TRACK_REVENUE_MAX = PlayerPrefs.GetInt("DP_TRACK_REVENUE_MAX", TRACK_REVENUE_MAX);
		BONUS_VALUES[0] = PlayerPrefs.GetFloat("DP_COINS_PER_HEARTS", BONUS_VALUES[0]);
		for (int i = 0; i < LEVEL_PACK_PRICE_MULTIPLIERS.Length; i++)
		{
			LEVEL_PACK_PRICE_MULTIPLIERS[i] = PlayerPrefs.GetFloat("DP_LEVEL_PACK_PRICE_MULTIPLIER" + (i + 1), LEVEL_PACK_PRICE_MULTIPLIERS[i]);
		}
		for (int j = 0; j < LEVEL_PRICE_INCREMENT_MULTIPLIERS.Length; j++)
		{
			LEVEL_PRICE_INCREMENT_MULTIPLIERS[j] = PlayerPrefs.GetFloat("LEVEL_PRICE_INCREMENT_MULTIPLIERS" + (j + 1), LEVEL_PRICE_INCREMENT_MULTIPLIERS[j]);
		}
		ANIMAL_REMOVE_AMOUNT_MAX_PERCENT = PlayerPrefs.GetFloat("DP_ANIMAL_REMOVE_AMOUNT_MAX_PERCENT", ANIMAL_REMOVE_AMOUNT_MAX_PERCENT);
		ANIMAL_REMOVE_AMOUNT_MAX = PlayerPrefs.GetInt("DP_ANIMAL_REMOVE_AMOUNT_MAX", ANIMAL_REMOVE_AMOUNT_MAX);
		ANIMAL_REMOVE_AMOUNT_MIN = PlayerPrefs.GetInt("DP_ANIMAL_REMOVE_AMOUNT_MIN", ANIMAL_REMOVE_AMOUNT_MIN);
	}

	public static void load()
	{
		TOTAL_TOKENS = PlayerPrefs.GetInt("TOTAL_TOKENS", INITIAL_TOKEN_AMOUNT);
		SOUND_LEVEL = PlayerPrefs.GetInt("SOUND_LEVEL", 2);
		FEATURE_DESCRIPT_LIST = PlayerPrefs.GetInt("FEATURE_DESCRIPT_LIST", FEATURE_LIST);
		FEATURE_LIST = PlayerPrefs.GetInt("FEATURE_LIST", FEATURE_LIST);
		FEATURE_DESCRIPT_LIST &= FEATURE_LIST;
		loadDynamicProperties();
	}

	public static void save()
	{
		PlayerPrefs.SetInt("SOUND_LEVEL", SOUND_LEVEL);
	}

	public static void reloadLevelPacks()
	{
		LEVEL_PACKS = LevelPack.getLevelPackList();
		int count = 0;
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(LEVEL_PACKS);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is LevelPack))
			{
				obj = RuntimeServices.Coerce(obj, typeof(LevelPack));
			}
			LevelPack levelPack = (LevelPack)obj;
			loadLevelPackValues(levelPack);
			UnityRuntimeServices.Update(enumerator, levelPack);
			count++;
		}
		LEVEL_PACKS[0].flags = LEVEL_PACKS[0].flags | 1;
		LEVEL_PACKS[0].levelList[0].flags = LEVEL_PACKS[0].levelList[0].flags | 1;
	}

	public static LevelPack getActiveLevelPack()
	{
		return (ACTIVE_LEVEL_PACK < 0 || ACTIVE_LEVEL_PACK >= LEVEL_PACKS.Count) ? null : LEVEL_PACKS[ACTIVE_LEVEL_PACK];
	}

	public static int getActiveLevelPackIndex()
	{
		return ACTIVE_LEVEL_PACK;
	}

	public static LevelPack.LevelInfo getSelectedLevel()
	{
		LevelPack activeLevelPack = getActiveLevelPack();
		return activeLevelPack == null ? null : activeLevelPack.getLevelInfo(SELECTED_LEVEL_INDEX);
	}

	public static int getSelectedLevelIndex()
	{
		return SELECTED_LEVEL_INDEX;
	}

	public static LevelPack getLevelPack(int index)
	{
		return LEVEL_PACKS[index];
	}

	public static int getLevelPackCount()
	{
		return LEVEL_PACKS.Count;
	}

	public static void setActiveLevelPack(int index)
	{
		ACTIVE_LEVEL_PACK = index;
	}

	public static int getTokenAmount()
	{
		return TOTAL_TOKENS;
	}

	public static void addTokens(int amount)
	{
		TOTAL_TOKENS += amount;
		PlayerPrefs.SetInt("TOTAL_TOKENS", TOTAL_TOKENS);
		PlayerPrefs.Save();
	}

	public static bool canBuy(int price)
	{
		return TOTAL_TOKENS >= price;
	}

	public static void resetSavedValues()
	{
		PlayerPrefs.DeleteAll();
		FEATURE_LIST = 0;
		load();
	}

	public static int getNumberOfCarts(LevelPack pack, int levelIndex)
	{
		int i;
		for (i = 0; !RuntimeServices.EqualityOperator(pack, LEVEL_PACKS[i]) && i < LEVEL_PACKS.Count - 1; i++)
		{
		}
		int startingCartAmount = getStartingCartAmount(i, levelIndex);
		int carts = PlayerPrefs.GetInt("LEVEL_CART_NUM_" + pack.fileName + "_" + pack.levelList[levelIndex].fileName, startingCartAmount);
		if (carts > CART_LIMIT)
		{
			carts = CART_LIMIT;
		}
		return carts;
	}

	public static void setNumberOfCarts(LevelPack pack, int levelIndex, int num)
	{
		PlayerPrefs.SetInt("LEVEL_CART_NUM_" + pack.fileName + "_" + pack.levelList[levelIndex].fileName, num);
	}

	private static int getStartingCartAmount(int packIndex, int levelIndex)
	{
		return MIN_CART_COUNT;
	}

	public static int getIndexOfActiveTheme()
	{
		return getSelectedLevel().settings.getIntValue("wagontype", 0);
	}

	public static int getSpecialCartType()
	{
		return getSelectedLevel().settings.getIntValue("specialcart", Vehicle.CART_NORMAL);
	}

	private static void loadLevelPackValues(LevelPack pack)
	{
		foreach (LevelPack.LevelInfo levelInfo in pack.levelList)
		{
			int flags = PlayerPrefs.GetInt("LEVEL_FLAGS_" + pack.fileName + "_" + levelInfo.fileName, 0);
			if ((flags & 1) != 0)
			{
				levelInfo.flags |= 1;
			}
			if ((flags & LEVEL_FLAG_BUILT) != 0)
			{
				levelInfo.flags |= LEVEL_FLAG_BUILT;
			}
			if ((flags & 8) != 0)
			{
				levelInfo.flags |= 8;
			}
			if (UNLOCK_LEVELS)
			{
				levelInfo.flags |= 1;
			}
		}
		pack.flags = 4 & pack.flags;
		if ((PlayerPrefs.GetInt("LEVELPACK_FLAGS_" + pack.fileName, 0) & 1) != 0 || UNLOCK_LEVELS)
		{
			pack.flags |= 1;
		}
		pack.levelList[0].flags = pack.levelList[0].flags | 1;
	}

	public static bool unlockLevel(int index)
	{
		LevelPack activeLevelPack = getActiveLevelPack();
		if (index >= activeLevelPack.levelList.Count)
		{
			return false;
		}
		else if (activeLevelPack.getLevelInfo(index).isUnlocked())
		{
			return true;
		}
		else
		{
			activeLevelPack.unlockLevel(index);
			PlayerPrefs.SetInt("LEVEL_FLAGS_" + activeLevelPack.fileName + "_" + activeLevelPack.levelList[index].fileName, activeLevelPack.levelList[index].flags);
			PlayerPrefs.Save();
			RollerCoasterStatistics.getInstance().roundUnlockedNextTrack = true;
			return true;
		}
	}

	public static void unlockLevelPack(int index)
	{
		if (index < LEVEL_PACKS.Count)
		{
			LevelPack levelPack = LEVEL_PACKS[index];
			if (!levelPack.isUnlocked())
			{
				levelPack.flags |= 1;
				PlayerPrefs.SetInt("LEVELPACK_FLAGS_" + levelPack.fileName, levelPack.flags);
				PlayerPrefs.Save();
				RollerCoasterStatistics.getInstance().roundUnlockedNextTrack = true;
			}
		}
	}

	public static void unlockNextLevel()
	{
		if (!unlockLevel(SELECTED_LEVEL_INDEX + 1))
		{
			unlockLevelPack(ACTIVE_LEVEL_PACK + 1);
		}
	}

	public static bool isNextLevelOrLevelpackUnlocked()
	{
		LevelPack activeLevelPack = getActiveLevelPack();
		int nextLevel = SELECTED_LEVEL_INDEX + 1;
		if (nextLevel >= activeLevelPack.levelList.Count)
		{
			int nextPack = ACTIVE_LEVEL_PACK + 1;
			if (nextPack >= LEVEL_PACKS.Count)
			{
				return true;
			}
			else
			{
				activeLevelPack = LEVEL_PACKS[nextPack];
				return activeLevelPack.isUnlocked();
			}
		}
		else
		{
			return activeLevelPack.getLevelInfo(nextLevel).isUnlocked();
		}
	}

	public static void completeLevel(int index, int carts)
	{
		LevelPack activeLevelPack = getActiveLevelPack();
		activeLevelPack.completeLevel(index);
		PlayerPrefs.SetInt("LEVEL_FLAGS_" + activeLevelPack.fileName + "_" + activeLevelPack.levelList[index].fileName, activeLevelPack.levelList[index].flags);
		PlayerPrefs.SetInt("LEVEL_FINISH_CART_NUM_" + activeLevelPack.fileName + "_" + activeLevelPack.levelList[index].fileName, carts);
		if (carts < CART_LIMIT)
		{
			carts++;
		}
		setNumberOfCarts(activeLevelPack, SELECTED_LEVEL_INDEX, carts);
		PlayerPrefs.Save();
	}

	public static void buildLevel(int index)
	{
		LevelPack activeLevelPack = getActiveLevelPack();
		LevelPack.LevelInfo levelInfo = activeLevelPack.getLevelInfo(index);
		levelInfo.setFlag(LEVEL_FLAG_BUILT);
		PlayerPrefs.SetInt("LEVEL_FLAGS_" + activeLevelPack.fileName + "_" + activeLevelPack.levelList[index].fileName, levelInfo.flags);
		PlayerPrefs.Save();
	}

	public static StoreKitProduct getInAppPurchaseProduct(string id)
	{
		if (INAPP_PURCHASE_LIST != null)
		{
			foreach (StoreKitProduct storeKitProduct in INAPP_PURCHASE_LIST)
			{
				if (storeKitProduct.productIdentifier == id)
				{
					return storeKitProduct;
				}
			}
		}
		return null;
	}

	public static int getInAppPurchaseProductCount()
	{
		return INAPP_PURCHASE_LIST == null ? 0 : INAPP_PURCHASE_LIST.Count;
	}

	public static void setInAppPurchased(string id)
	{
		if (getInAppPurchaseProduct(id) != null)
		{
			addTokens(getTokenValue(id));
		}
	}

	public static int getTokenValue(string productId)
	{
		if (INAPP_PURCHASE_TOKENS.ContainsKey(productId))
		{
			int multiplier = 1;
			if (isFeatureUnlocked(8192))
			{
				multiplier = 2;
			}
			return INAPP_PURCHASE_TOKENS[productId] * multiplier;
		}
		else
		{
			return -1;
		}
	}

	public static int getLevelPackProgress(int index)
	{
		LevelPack levelPack = LEVEL_PACKS[index];
		if (levelPack.levelList.Count == 0)
		{
			return 0;
		}
		else
		{
			float earned = 0f;
			float possible = 0f;
			for (int i = 0; i < levelPack.levelList.Count; i++)
			{
				int startingCartAmount = getStartingCartAmount(index, i);
				possible += (float)(CART_LIMIT - startingCartAmount);
				earned += (float)(getNumberOfCarts(levelPack, i) - startingCartAmount);
			}
			return (int)Mathf.Ceil(earned * 100f / possible);
		}
	}

	public static int getLevelProgress(LevelPack levelPack, int index)
	{
		if (levelPack.levelList.Count == 0)
		{
			return 0;
		}
		else
		{
			float cartLimit = CART_LIMIT;
			int finishedCarts = PlayerPrefs.GetInt("LEVEL_FINISH_CART_NUM_" + levelPack.fileName + "_" + levelPack.levelList[index].fileName, 0);
			return (int)Mathf.Ceil((float)finishedCarts * 100f / cartLimit);
		}
	}

	public static string formatTokenValue(string value)
	{
		int pos = value.Length - 3;
		if (pos < 0)
		{
			return value;
		}
		else
		{
			string formatted = string.Empty;
			while (pos >= 0)
			{
				formatted = value.Substring(pos, 3) + " " + formatted;
				pos -= 3;
			}
			if (pos > -3)
			{
				formatted = value.Substring(0, 3 + pos) + " " + formatted;
			}
			return formatted;
		}
	}

	public static void unlockFeature(int feature)
	{
		if (feature != 0)
		{
			FEATURE_LIST |= feature;
			PlayerPrefs.SetInt("FEATURE_LIST", FEATURE_LIST);
			PlayerPrefs.Save();
		}
	}

	public static void lockFeature(int feature)
	{
		if (feature != 0)
		{
			FEATURE_LIST &= ~feature;
			PlayerPrefs.SetInt("FEATURE_LIST", FEATURE_LIST);
			FEATURE_DESCRIPT_LIST &= ~feature;
			PlayerPrefs.SetInt("FEATURE_DESCRIPT_LIST", FEATURE_DESCRIPT_LIST);
		}
	}

	public static bool isFeatureUnlocked(int feature)
	{
		return (FEATURE_LIST & feature) != 0;
	}

	public static void setFeatureDescribed(int feature)
	{
		FEATURE_DESCRIPT_LIST |= feature;
		PlayerPrefs.SetInt("FEATURE_DESCRIPT_LIST", FEATURE_DESCRIPT_LIST);
		PlayerPrefs.Save();
	}

	public static int getNewFeaturesList()
	{
		return FEATURE_LIST ^ FEATURE_DESCRIPT_LIST;
	}

	public static int getNextTrackPrice()
	{
		LevelPack levelPack = getActiveLevelPack();
		int levelIndex = SELECTED_LEVEL_INDEX + 1;
		int packIndex = ACTIVE_LEVEL_PACK;
		if (levelIndex >= levelPack.levelList.Count)
		{
			levelIndex = 0;
			packIndex++;
			if (packIndex >= LEVEL_PACKS.Count)
			{
				return -1;
			}
			levelPack = LEVEL_PACKS[packIndex];
		}
		LevelPack.LevelInfo levelInfo = levelPack.getLevelInfo(levelIndex);
		return levelInfo.testFlag(LEVEL_FLAG_BUILT) ? 0 : getTrackPrice(packIndex, levelIndex);
	}

	public static int getTrackPrice(int levelPack, int level)
	{
		float basePrice = DEFAULT_PRICES[0].price;
		float priceMultiplier = getPriceMultiplier(levelPack, level);
		float price = basePrice * priceMultiplier;
		if (levelPack < LEVEL_PRICE_INCREMENT_MULTIPLIERS.Length)
		{
			price += LEVEL_PRICE_INCREMENT_MULTIPLIERS[levelPack] * (float)level * basePrice;
		}
		return (int)price;
	}

	public static bool hasEnoughMoneyToUnlockNextTrack(int cash)
	{
		return cash >= getNextTrackPrice();
	}

	public static float getPriceMultiplier(int packIndex, int levelIndex)
	{
		if (packIndex >= LEVEL_PACK_PRICE_MULTIPLIERS.Length)
		{
			packIndex = LEVEL_PACK_PRICE_MULTIPLIERS.Length - 1;
		}
		if (levelIndex >= LEVEL_INDEX_PRICE_MULTIPLIERS.Length)
		{
			levelIndex = LEVEL_INDEX_PRICE_MULTIPLIERS.Length - 1;
		}
		return LEVEL_PACK_PRICE_MULTIPLIERS[packIndex] * LEVEL_INDEX_PRICE_MULTIPLIERS[levelIndex];
	}

	public static float getActivePriceMultiplier()
	{
		return getPriceMultiplier(ACTIVE_LEVEL_PACK, SELECTED_LEVEL_INDEX);
	}

	public static void createTrainForLevel()
	{
		TRAIN_DATA = loadTrain(ACTIVE_LEVEL_PACK, SELECTED_LEVEL_INDEX);
	}

	public static VehicleData loadTrain(int packIndex, int levelIndex)
	{
		LevelPack levelPack = getLevelPack(packIndex);
		int numberOfCarts = getNumberOfCarts(levelPack, levelIndex);
		string lhs = "LEVEL_ANIMAL_" + levelPack.fileName + "_" + levelPack.levelList[levelIndex].fileName;
		VehicleData vehicleData = new VehicleData();
		int cartIndex = 0;
		vehicleData.cartList = new CartData[numberOfCarts];
		for (cartIndex = 0; cartIndex < numberOfCarts; cartIndex++)
		{
			vehicleData.cartList[cartIndex] = new CartData();
			vehicleData.cartList[cartIndex].animalIndex = PlayerPrefs.GetInt(lhs + cartIndex, ANIMAL_NONE);
		}
		vehicleData.trainType = getIndexOfActiveTheme();
		vehicleData.specialCart = getSelectedLevel().settings.getIntValue("specialcart", Vehicle.CART_NORMAL);
		if (!isFeatureUnlocked(0x1E | FEATURE_BUY_CART))
		{
			for (cartIndex = 0; cartIndex < numberOfCarts; cartIndex++)
			{
				vehicleData.cartList[cartIndex].animalIndex = 0;
			}
			saveTrain();
		}
		return vehicleData;
	}

	public static void saveTrain()
	{
		if (TRAIN_DATA.cartList != null)
		{
			LevelPack activeLevelPack = getActiveLevelPack();
			string lhs = "LEVEL_ANIMAL_" + activeLevelPack.fileName + "_" + activeLevelPack.levelList[SELECTED_LEVEL_INDEX].fileName;
			for (int i = 0; i < TRAIN_DATA.cartList.Length; i++)
			{
				PlayerPrefs.SetInt(lhs + i, TRAIN_DATA.cartList[i].animalIndex);
			}
			PlayerPrefs.Save();
		}
	}

	public static int getNumberOfAnimalsInTrainData(int animalIndex)
	{
		int count = 0;
		for (int i = 0; i < TRAIN_DATA.cartList.Length; i++)
		{
			if (TRAIN_DATA.cartList[i].animalIndex == animalIndex)
			{
				count++;
			}
		}
		return count;
	}

	public static void saveHeartRecord(int hearts, int maximum)
	{
		LevelPack activeLevelPack = getActiveLevelPack();
		string rhs = string.Empty + activeLevelPack.fileName + "_" + activeLevelPack.levelList[SELECTED_LEVEL_INDEX].fileName;
		string key = "LEVEL_RECORD_HEARTS_" + rhs;
		string key2 = "LEVEL_MAXIMUM_HEARTS_" + rhs;
		if (hearts > maximum)
		{
			maximum = hearts;
		}
		if (PlayerPrefs.GetInt(key, 0) < hearts)
		{
			PlayerPrefs.SetInt(key, hearts);
			PlayerPrefs.SetInt(key2, maximum);
		}
	}

	public static int getHeartRecord(int levelIndex)
	{
		LevelPack activeLevelPack = getActiveLevelPack();
		string rhs = string.Empty + activeLevelPack.fileName + "_" + activeLevelPack.levelList[levelIndex].fileName;
		string key = "LEVEL_RECORD_HEARTS_" + rhs;
		return PlayerPrefs.GetInt(key, 0) != 0 ? PlayerPrefs.GetInt(key, 0) : -1;
	}

	public static int getHeartRecordPercentage(int levelIndex)
	{
		LevelPack activeLevelPack = getActiveLevelPack();
		string rhs = string.Empty + activeLevelPack.fileName + "_" + activeLevelPack.levelList[levelIndex].fileName;
		string key = "LEVEL_RECORD_HEARTS_" + rhs;
		string key2 = "LEVEL_MAXIMUM_HEARTS_" + rhs;
		if (PlayerPrefs.GetInt(key, 0) == 0)
		{
			return -1;
		}
		else
		{
			float f = PlayerPrefs.GetInt(key, 0) * 100 / PlayerPrefs.GetInt(key2, 0);
			return (int)Mathf.Min(100f, Mathf.Floor(f));
		}
	}

	public static string genLeaderboard(int pack, int index)
	{
		string id = string.Empty;
		switch (pack)
		{
		case 0:
			id = "woodtrack_" + (index + 1);
			break;
		case 1:
			id = "metaltrack_" + (index + 1);
			break;
		case 2:
			id = "looptrack_" + (index + 1);
			break;
		case 3:
			id = "extremetrack_" + (index + 1);
			break;
		}
		if (LOG)
		{
			Debug.Log("LB: " + id);
		}
		return id;
	}

	public static bool hasInternet()
	{
		return Application.internetReachability != NetworkReachability.NotReachable;
	}

	// In-app purchases needed the Google Play billing SDK, which is gone (see
	// StoreManager), so the coin store's purchase options never become available.
	public static bool canMakeInAppPurchases()
	{
		return false;
	}
}
