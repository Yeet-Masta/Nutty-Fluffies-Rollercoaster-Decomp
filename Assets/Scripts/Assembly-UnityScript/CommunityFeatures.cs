using UnityEngine;

public class CommunityFeatures : MonoBehaviour
{
	private const bool DEBUG_COMMUNITY_FEATURES = false;

	private static CommunityFeatures instance;

	private MenuManager mmgr;

	public MessageBox messageBox;

	public int offerInterval = 5;

	public static string COMMUNITY_TEXT_STORE_POST_FACEBOOK = "I love Nutty Fluffies! You should try it too!";

	public static string COMMUNITY_TEXT_STORE_POST_TWITTER = "I love Nutty Fluffies! You should try it too!";

	public static string COMMUNITY_TEXT_RESULT_SCREEN_FACEBOOK = "I just finished {0} and got {1} hearts!";

	public static string COMMUNITY_TEXT_RESULT_SCREEN_TWITTER = "I just finished {0} and got {1} hearts!";

	public static string COMMUNITY_TEXT_EMAIL_POST = "I just completed the last track! w00t!";

	public static string COMMUNITY_TEXT_EMAIL_TITLE = "Email Title";

	public static readonly string[] FEATURE_KEY = new string[6] { "TWITTER_POST", "FACEBOOK_POST", "FACEBOOK_LIKE", "SEND_EMAIL", "DOUBLE_COINS", "TAPJOY_OFFERWALL" };

	public static int[] FEATURE_VALUES = new int[6] { -1, -1, -1, -1, 0, 0 };

	public int[] featureUseLimits = new int[6] { 1, 1, 1, 1, 0, 0 };

	public int[] featureShowLimits = new int[6] { 2, 2, 3, 2, 0, 0 };

	public GameObject[] featureGraphics;

	public const int MODE_TWITTER_POST = 0;

	public const int MODE_FACEBOOK_POST = 1;

	public const int MODE_FACEBOOK_LIKE = 2;

	public const int MODE_SEND_EMAIL = 3;

	public const int MODE_DOUBLE_COINS = 4;

	public const int MODE_TAPJOY_OFFERWALL = 5;

	public const int MODE_COUNT = 6;

	public static readonly int[] FEATURE_OFFER_ORDER = new int[5] { 2, 0, 4, 3, 5 };

	private int offerMode;

	private int internalOfferInterval;

	public static bool showingOffer;

	public static CommunityFeatures getInstance()
	{
		return instance;
	}

	public virtual void Awake()
	{
		useGUILayout = false;
		offerInterval = PlayerPrefs.GetInt("DP_COMMUNITY_OFFER_INTERVAL", offerInterval);
		featureShowLimits[0] = PlayerPrefs.GetInt("DP_FEATURE_SHOW_LIMIT_TWITTER", featureShowLimits[0]);
		featureShowLimits[1] = PlayerPrefs.GetInt("DP_FEATURE_SHOW_LIMIT_FACEBOOK_POST", featureShowLimits[1]);
		featureShowLimits[2] = PlayerPrefs.GetInt("DP_FEATURE_SHOW_LIMIT_FACEBOOK_LIKE", featureShowLimits[2]);
		featureShowLimits[3] = PlayerPrefs.GetInt("DP_FEATURE_SHOW_LIMIT_EMAIL", featureShowLimits[3]);
		featureShowLimits[4] = PlayerPrefs.GetInt("DP_FEATURE_SHOW_LIMIT_DOUBLE_COINS", featureShowLimits[4]);
		featureShowLimits[5] = PlayerPrefs.GetInt("DP_COMPLETE_TAPJOY_OFFERWALL", featureShowLimits[5]);
		instance = this;
		messageBox.deactivate();
		for (int i = 0; i < FEATURE_VALUES.Length; i++)
		{
			FEATURE_VALUES[i] = StoreItem.getDefaultPrice("DP_PURCHASE_" + FEATURE_KEY[i]);
		}
		updateInternalInterval();
	}

	public virtual void Start()
	{
		mmgr = MenuManager.getInstance();
		COMMUNITY_TEXT_STORE_POST_FACEBOOK = LocalizationManager.localize("COMMUNITY_TEXT_STORE_POST_FACEBOOK");
		COMMUNITY_TEXT_STORE_POST_TWITTER = LocalizationManager.localize("COMMUNITY_TEXT_STORE_POST_TWITTER");
		COMMUNITY_TEXT_RESULT_SCREEN_FACEBOOK = LocalizationManager.localize("COMMUNITY_TEXT_RESULT_SCREEN_FACEBOOK");
		COMMUNITY_TEXT_RESULT_SCREEN_TWITTER = LocalizationManager.localize("COMMUNITY_TEXT_RESULT_SCREEN_TWITTER");
		COMMUNITY_TEXT_EMAIL_POST = LocalizationManager.localize("COMMUNITY_TEXT_EMAIL_POST");
		COMMUNITY_TEXT_EMAIL_TITLE = SharingManager.getInstance().shareLinkTitle;
	}

	private void updateInternalInterval()
	{
		int enabledCount = 0;
		for (int i = 0; i < FEATURE_OFFER_ORDER.Length; i++)
		{
			if (isEnabled(FEATURE_OFFER_ORDER[i]))
			{
				enabledCount++;
			}
		}
		if (enabledCount > 0)
		{
			internalOfferInterval = Mathf.Min(15, offerInterval * (FEATURE_OFFER_ORDER.Length + 1) - enabledCount * offerInterval);
		}
		else
		{
			internalOfferInterval = 99999999;
		}
	}

	public virtual bool isEnabled(int feature)
	{
		int useCount = PlayerPrefs.GetInt(FEATURE_KEY[feature] + "_COUNTER", 0);
		int allowed;
		if (!isAvailable(feature))
		{
			allowed = 0;
		}
		else if (feature == 4 && !GameData.canMakeInAppPurchases())
		{
			allowed = 0;
		}
		else
		{
			allowed = useCount < featureUseLimits[feature] ? 1 : 0;
			if (allowed == 0)
			{
				allowed = featureUseLimits[feature] == 0 ? 1 : 0;
			}
		}
		return (byte)allowed != 0;
	}

	private bool isEnabledForOffer(int feature)
	{
		int allowed;
		if (!isEnabled(feature))
		{
			allowed = 0;
		}
		else
		{
			int showCount = PlayerPrefs.GetInt(FEATURE_KEY[feature] + "_SHOW", 0);
			allowed = showCount < featureShowLimits[feature] ? 1 : 0;
			if (allowed == 0)
			{
				allowed = featureShowLimits[feature] == 0 ? 1 : 0;
			}
		}
		return (byte)allowed != 0;
	}

	public virtual bool isAvailable(int feature)
	{
		// 5 is the Tapjoy offer wall, which is gone.
		return feature != 5;
	}

	public virtual bool hasSupport()
	{
		return isAvailable(0) || isAvailable(1) || isAvailable(2) || isAvailable(3) || isAvailable(5);
	}

	public virtual bool offerIfNeeded()
	{
		if (GameData.hasInternet())
		{
			incrementCounter("COMMUNITY_OFFER");
			int offerCounter = PlayerPrefs.GetInt("COMMUNITY_OFFER", 0);
			int lotsOfMoneyLimit = PlayerPrefs.GetInt("DP_COMMUNITY_LOTS_OF_MONEY_LIMIT", 250);
			int cash = MissionManager.getInstance().getActiveCompletedMissionScore() + GameData.getTokenAmount();
			bool canUnlockNextTrack = GameData.hasEnoughMoneyToUnlockNextTrack(cash);
			bool fewCarts = GameData.getNumberOfCarts(GameData.getActiveLevelPack(), GameData.getSelectedLevelIndex()) < 6;
			bool lotsOfMoney = GameData.getTokenAmount() > lotsOfMoneyLimit;
			if (!canUnlockNextTrack || (fewCarts && !lotsOfMoney))
			{
				if (!canUnlockNextTrack && offerCounter >= offerInterval && GameManager.getInstance().restarts > 3)
				{
					incrementCounter("COMMUNITY_OFFER");
				}
				if (offerCounter >= internalOfferInterval)
				{
					PlayerPrefs.SetInt("COMMUNITY_OFFER", 0);
					showOffer(pickOfferMode());
					PlayerPrefs.Save();
					return true;
				}
			}
		}
		return false;
	}

	public virtual void showOffer(int mode)
	{
		showingOffer = true;
		if (mode <= -1)
		{
			return;
		}
		offerMode = mode;
		// 5 is the Tapjoy offer wall (gone), 4 has no offer.
		if (5 == offerMode || 4 == offerMode)
		{
			return;
		}
		int reward = FEATURE_VALUES[offerMode];
		string message = null;
		string title = null;
		switch (offerMode)
		{
		case 0:
			message = TextData.COMMUNITY_TEXT_OFFER_TWITTER.Replace("%d", string.Empty + reward);
			title = TextData.COMMUNITY_TITLE_OFFER1;
			break;
		case 1:
			message = TextData.COMMUNITY_TEXT_OFFER_POST_FACEBOOK.Replace("%d", string.Empty + reward);
			title = TextData.COMMUNITY_TITLE_OFFER2;
			break;
		case 2:
			message = TextData.COMMUNITY_TEXT_OFFER_LIKE_FACEBOOK.Replace("%d", string.Empty + reward);
			title = TextData.COMMUNITY_TITLE_OFFER1;
			break;
		case 3:
			message = TextData.COMMUNITY_TEXT_OFFER_EMAIL.Replace("%d", string.Empty + reward);
			title = TextData.COMMUNITY_TITLE_OFFER2;
			break;
		case 4:
			message = TextData.COMMUNITY_TEXT_OFFER_DOUBLE_COINS;
			title = TextData.COMMUNITY_TITLE_OFFER1;
			break;
		case 5:
			message = TextData.COMMUNITY_TEXT_OFFER_TAPJOY_OFFERWALL;
			title = TextData.COMMUNITY_TITLE_OFFER2;
			break;
		}
		updateInternalInterval();
		if (PlayerPrefs.GetInt(FEATURE_KEY[offerMode] + "_COUNTER", 0) < featureUseLimits[offerMode] || featureUseLimits[offerMode] == 0)
		{
			incrementCounter(FEATURE_KEY[offerMode] + "_SHOW");
			messageBox.activate(title, message, featureGraphics[offerMode], true, false);
			return;
		}
		switch (offerMode)
		{
		case 0:
			postTwitter();
			break;
		case 1:
			faceBookAdvert();
			break;
		case 2:
			faceBookLike();
			break;
		case 3:
			sendEmail();
			break;
		}
	}

	public virtual void postTwitter()
	{
		BaseMenuManager.getInstance().showLoadingScreen(true);
		Invoke("onSharingFailed", 30f);
		SharingManager.getInstance().addListener(gameObject);
		SharingManager.getInstance().shareMessage(SharingManager.Service.Twitter, COMMUNITY_TEXT_STORE_POST_TWITTER);
	}

	public virtual void faceBookLike()
	{
		BaseMenuManager.getInstance().showLoadingScreen(true);
		Invoke("onSharingFailed", 30f);
		Application.OpenURL(GameData.FACEBOOK_LIKE_URL);
		onFaceBookLikeCompleted();
	}

	public virtual void faceBookAdvert()
	{
		BaseMenuManager.getInstance().showLoadingScreen(true);
		Invoke("onSharingFailed", 30f);
		SharingManager.getInstance().addListener(gameObject);
		SharingManager.getInstance().shareMessage(SharingManager.Service.Facebook, COMMUNITY_TEXT_STORE_POST_FACEBOOK);
	}

	public virtual void sendEmail()
	{
		BaseMenuManager.getInstance().showLoadingScreen(true);
		Invoke("onSharingFailed", 30f);
		SharingManager.getInstance().addListener(gameObject);
		string communityTextEmailPost = COMMUNITY_TEXT_EMAIL_POST;
		SharingManager.getInstance().shareMessage(SharingManager.Service.Email, COMMUNITY_TEXT_EMAIL_TITLE, string.Format(communityTextEmailPost, SharingManager.getInstance().shareLinkURL));
	}

	private void disableLoading()
	{
		BaseMenuManager.getInstance().showLoadingScreen(false);
	}

	public virtual void onFacebookPostCompleted()
	{
		showCoinDialog(FEATURE_VALUES[1], TextData.COMMUNITY_TITLE_FACEBOOK_POST);
	}

	public virtual void onEmailPostCompleted()
	{
		showCoinDialog(FEATURE_VALUES[3], TextData.COMMUNITY_TITLE_EMAIL);
	}

	public virtual void onFaceBookLikeCompleted()
	{
		showCoinDialog(FEATURE_VALUES[2], TextData.COMMUNITY_TITLE_FACEBOOK_LIKE);
	}

	public virtual void onTwitterPostCompleted()
	{
		showCoinDialog(FEATURE_VALUES[0], TextData.COMMUNITY_TITLE_TWITTER);
	}

	public virtual void onSharingFailed()
	{
		CancelInvoke("onSharingFailed");
		BaseMenuManager.getInstance().showLoadingScreen(false);
	}

	private void showCoinDialog(int value, string title)
	{
		CancelInvoke("onSharingFailed");
		BaseMenuManager.getInstance().showLoadingScreen(false);
		incrementCounter(FEATURE_KEY[offerMode] + "_COUNTER");
		if (PlayerPrefs.GetInt(FEATURE_KEY[offerMode] + "_COUNTER", 0) <= featureUseLimits[offerMode])
		{
			BaseMenuManager baseMenuManager = BaseMenuManager.getInstance();
			GameData.addTokens(value);
			baseMenuManager.updateTokenAmount();
			GeneralSoundEffects.playSound(baseMenuManager.cashSound);
			string message = TextData.COMMUNITY_TEXT_RECEIVE_COINS.Replace("%d", string.Empty + value);
			baseMenuManager.showMessage(title, message, baseMenuManager.cashImage);
			baseMenuManager.onCommunityFeatureCompleted();
		}
	}

	public virtual void onDialogContinue()
	{
		showingOffer = false;
		messageBox.onContinue();
		switch (offerMode)
		{
		case 0:
			postTwitter();
			break;
		case 1:
			faceBookAdvert();
			break;
		case 2:
			faceBookLike();
			break;
		case 3:
			sendEmail();
			break;
		}
	}

	public virtual void onDialogClose()
	{
		showingOffer = false;
		messageBox.onContinue();
		GameData.lockFeature(8192);
	}

	public virtual void OnApplicationPause()
	{
		Application.runInBackground = true;
	}

	public virtual void OnApplicationFocus(bool focus)
	{
		if (focus)
		{
			Application.runInBackground = false;
		}
	}

	public virtual void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && showingOffer)
		{
			Invoke("onBackButtonCloseDialog", 0.2f);
		}
	}

	public virtual bool onBackButtonCloseDialog()
	{
		if (messageBox.isActive())
		{
			messageBox.onBackButton();
			showingOffer = false;
		}
		return messageBox.isActive();
	}

	public virtual bool isDialogBoxClosed()
	{
		return !messageBox.isActive();
	}

	private void incrementCounter(string key)
	{
		int count = PlayerPrefs.GetInt(key, 0);
		count++;
		PlayerPrefs.SetInt(key, count);
		PlayerPrefs.Save();
	}

	private int pickOfferMode()
	{
		int index = PlayerPrefs.GetInt("COMMUNITY_OFFER_INDEX", 0);
		int tries = 0;
		while (!isEnabledForOffer(FEATURE_OFFER_ORDER[index]))
		{
			index++;
			if (index >= FEATURE_OFFER_ORDER.Length)
			{
				index = 0;
			}
			tries++;
			if (tries >= FEATURE_OFFER_ORDER.Length)
			{
				return -1;
			}
		}
		int nextIndex = index + 1;
		if (nextIndex >= FEATURE_OFFER_ORDER.Length)
		{
			nextIndex = 0;
		}
		PlayerPrefs.SetInt("COMMUNITY_OFFER_INDEX", nextIndex);
		return FEATURE_OFFER_ORDER[index];
	}
}
