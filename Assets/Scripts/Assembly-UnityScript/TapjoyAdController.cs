using System;
using UnityEngine;

[Serializable]
[DefaultExecutionOrder(-100)]
public class TapjoyAdController : MonoBehaviour
{
	[NonSerialized]
	protected static TapjoyAdController instance;

	private Resolution screenResolution;

	private bool hideAfterSomeTime;

	private bool displayAd;

	private bool displayFullScreenAd;

	private bool adLoaded;

	private bool fullScreenAdLoaded;

	private bool adDisplayed;

	[NonSerialized]
	private static bool isNetworkAvailable;

	private bool loadingTapjoyPoints;

	private string bannerAdKey;

	private string featureAdKey;

	public static TapjoyAdController getInstance()
	{
		return instance;
	}

	public virtual void Awake()
	{
		if (instance == null)
		{
			instance = this;
			adLoaded = false;
			TapjoyManager.TapjoyConnectSuccessEvent += TapjoyConnectSuccessEvent;
			TapjoyManager.TapjoyConnectFailsEvent += TapjoyConnectFailsEvent;
			TapjoyManager.TapPointsLoadedEvent += TapPointsLoadedEvent;
			TapjoyManager.TapPointsLoadedErrorEvent += TapPointsLoadedErrorEvent;
			TapjoyManager.TapPointsSpentEvent += TapPointsSpentEvent;
			TapjoyManager.TapPointsSpendErrorEvent += TapPointsSpendErrorEvent;
			TapjoyManager.TapPointsAwardedEvent += TapPointsAwardedEvent;
			TapjoyManager.TapPointsAwardErrorEvent += TapPointsAwardErrorEvent;
			TapjoyManager.CurrencyEarnedEvent += CurrencyEarnedEvent;
			TapjoyManager.FullScreenAdLoadedEvent += FullScreenAdLoadedEvent;
			TapjoyManager.FullScreenAdErrorEvent += FullScreenAdErrorEvent;
			TapjoyManager.DailyRewardAdLoadedEvent += DailyRewardAdLoadedEvent;
			TapjoyManager.DailyRewardAdErrorEvent += DailyRewardAdErrorEvent;
			TapjoyManager.DisplayAdLoadedEvent += DisplayAdLoadedEvent;
			TapjoyManager.DisplayAdErrorEvent += DisplayAdErrorEvent;
			TapjoyManager.VideoAdStartEvent += VideoAdStartEvent;
			TapjoyManager.VideoAdErrorEvent += VideoAdErrorEvent;
			TapjoyManager.VideoAdCompleteEvent += VideoAdCompleteEvent;
			// NFPort: AndroidJNI thread attach removed with the ad SDK
			checkNetworkStatus();
			UnityEngine.Object.DontDestroyOnLoad(this);
		}
	}

	public virtual void ConnectToTapjoy()
	{
		TapjoyPlugin.EnableLogging(true);
		TapjoyPlugin.SetCallbackHandler("TapjoyManager");
		TapjoyPlugin.RequestTapjoyConnect(GameData.TAPJOY_APP_ID, GameData.TAPJOY_KEY);
		TapjoyPlugin.EnableLogging(false);
	}

	public virtual void checkNetworkStatus()
	{
		bool flag = isNetworkAvailable;
		isNetworkAvailable = GameData.hasInternet();
		if (GameData.LOG)
		{
			Debug.Log("Anji check Nework status in tapjoy " + isNetworkAvailable);
		}
		if (isNetworkAvailable && !flag)
		{
			ConnectToTapjoy();
			if (!fullScreenAdLoaded)
			{
				LoadFullScreenAd();
			}
		}
	}

	public virtual void DisplayAd(bool autoHide, string key)
	{
		bannerAdKey = "ads_" + key + "_active";
		if (GameData.LOG)
		{
			Debug.Log("Anji DisplayAd is called..." + autoHide);
		}
		hideAfterSomeTime = autoHide;
		displayAd = true;
		if (!adLoaded)
		{
			TapjoyPlugin.GetDisplayAd();
		}
		else
		{
			ShowAd();
		}
	}

	public virtual void LoadAd()
	{
		displayAd = false;
		hideAfterSomeTime = false;
		if (!adLoaded)
		{
			TapjoyPlugin.GetDisplayAd();
		}
	}

	public virtual void ShowAd()
	{
		HideAd();
		Debug.Log("Anji bannerAdKey is : " + bannerAdKey);
		if (PlayerPrefs.GetInt(bannerAdKey, 1) == 0)
		{
			return;
		}
		checkNetworkStatus();
		if (isNetworkAvailable)
		{
			adDisplayed = true;
			SetAdPosition();
			if (hideAfterSomeTime)
			{
				hideAfterSomeTime = false;
				Invoke("HideAd", 7f);
			}
		}
	}

	public virtual void LoadFullScreenAd()
	{
		TapjoyPlugin.GetFullScreenAd();
	}

	public virtual void DisplayFullScreenAd(string key)
	{
		featureAdKey = "ads_" + key + "_active";
		if (PlayerPrefs.GetInt(featureAdKey, 1) == 0)
		{
			return;
		}
		checkNetworkStatus();
		if (isNetworkAvailable)
		{
			if (fullScreenAdLoaded)
			{
				HideAd();
				fullScreenAdLoaded = false;
				TapjoyPlugin.ShowFullScreenAd();
				Invoke("LoadFullScreenAd", 2f);
			}
			else
			{
				displayFullScreenAd = true;
			}
		}
	}

	public virtual void HideAd()
	{
		if (adDisplayed)
		{
			TapjoyPlugin.HideDisplayAd();
			adDisplayed = false;
			Invoke("LoadAd", 1f);
		}
		displayAd = false;
	}

	public virtual bool isAdDisplayed()
	{
		return adDisplayed;
	}

	public virtual void TapjoyConnectSuccessEvent(string message)
	{
		if (GameData.LOG)
		{
			Debug.Log(message);
		}
		Debug.Log("Anji tapjoy connection success ...");
		if (GameData.actualDeviceResolution.width <= 854)
		{
			TapjoyPlugin.SetDisplayAdSize(TapjoyDisplayAdSize.SIZE_320X50);
		}
	}

	public virtual void SetAdPosition()
	{
		if (GameData.actualDeviceResolution.width > 854)
		{
			if (GameData.LOG)
			{
				Debug.Log("Anji tapjoy ad set position > " + (Screen.width - 640));
			}
			TapjoyPlugin.MoveDisplayAd(GameData.actualDeviceResolution.width - 640, GameData.actualDeviceResolution.height - 100);
		}
		else
		{
			if (GameData.LOG)
			{
				Debug.Log("Anji tapjoy ad set position < " + (GameData.actualDeviceResolution.width - 320));
			}
			TapjoyPlugin.MoveDisplayAd(GameData.actualDeviceResolution.width - 320, GameData.actualDeviceResolution.height - 50);
		}
	}

	public virtual void TapjoyConnectFailsEvent(string message)
	{
		if (GameData.LOG)
		{
			Debug.Log(message);
		}
		displayAd = false;
	}

	public virtual void TapPointsLoadedEvent(string message)
	{
		if (GameData.LOG)
		{
			Debug.Log(message);
		}
		int num = TapjoyPlugin.QueryTapPoints();
		if (GameData.LOG)
		{
			Debug.Log("Anji Points received are " + num);
		}
		if (num > 0)
		{
			GameData.addTokens(num);
			TapjoyPlugin.SpendTapPoints(num);
			BaseMenuManager.getInstance().updateTokenAmount();
		}
		loadingTapjoyPoints = false;
	}

	public virtual void TapPointsLoadedErrorEvent(string message)
	{
		if (GameData.LOG)
		{
			Debug.Log(message);
		}
		loadingTapjoyPoints = false;
	}

	public virtual void TapPointsSpentEvent(string message)
	{
		if (GameData.LOG)
		{
			Debug.Log(message);
		}
	}

	public virtual void TapPointsSpendErrorEvent(string message)
	{
		if (GameData.LOG)
		{
			Debug.Log(message);
		}
	}

	public virtual void TapPointsAwardedEvent(string message)
	{
		if (GameData.LOG)
		{
			Debug.Log(message);
		}
	}

	public virtual void TapPointsAwardErrorEvent(string message)
	{
		if (GameData.LOG)
		{
			Debug.Log(message);
		}
	}

	public virtual void CurrencyEarnedEvent(string message)
	{
		if (GameData.LOG)
		{
			Debug.Log("Anji in currency earned " + message);
		}
	}

	public virtual void FullScreenAdLoadedEvent(string message)
	{
		if (GameData.LOG)
		{
			Debug.Log(message);
		}
		fullScreenAdLoaded = true;
		if (displayFullScreenAd)
		{
			DisplayFullScreenAd(featureAdKey);
			displayFullScreenAd = false;
		}
		getInstance().GetTapPoints();
	}

	public virtual void GetTapPoints()
	{
		if (!loadingTapjoyPoints)
		{
			loadingTapjoyPoints = true;
			TapjoyPlugin.GetTapPoints();
		}
	}

	public virtual void FullScreenAdErrorEvent(string message)
	{
		if (GameData.LOG)
		{
			Debug.Log(message);
		}
	}

	public virtual void DailyRewardAdLoadedEvent(string message)
	{
		if (GameData.LOG)
		{
			Debug.Log(message);
		}
	}

	public virtual void DailyRewardAdErrorEvent(string message)
	{
		if (GameData.LOG)
		{
			Debug.Log(message);
		}
	}

	public virtual void DisplayAdLoadedEvent(string message)
	{
		Debug.Log(message);
		adLoaded = true;
		if (displayAd)
		{
			if (GameData.LOG)
			{
				Debug.Log("Anji show ad is called ....");
			}
			ShowAd();
		}
	}

	public virtual void DisplayAdErrorEvent(string message)
	{
		if (GameData.LOG)
		{
			Debug.Log(message);
		}
	}

	public virtual void VideoAdStartEvent(string message)
	{
		if (GameData.LOG)
		{
			Debug.Log(message);
		}
	}

	public virtual void VideoAdErrorEvent(string message)
	{
		if (GameData.LOG)
		{
			Debug.Log(message);
		}
	}

	public virtual void VideoAdCompleteEvent(string message)
	{
		if (GameData.LOG)
		{
			Debug.Log(message);
		}
	}
}
