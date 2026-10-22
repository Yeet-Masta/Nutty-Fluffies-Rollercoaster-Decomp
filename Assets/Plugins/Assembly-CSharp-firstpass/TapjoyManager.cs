using System;
using UnityEngine;

public class TapjoyManager : MonoBehaviour
{
	private static TapjoyManager instance;

	public static event Action<string> TapjoyConnectSuccessEvent;

	public static event Action<string> TapjoyConnectFailsEvent;

	public static event Action<string> TapPointsLoadedEvent;

	public static event Action<string> TapPointsLoadedErrorEvent;

	public static event Action<string> TapPointsSpentEvent;

	public static event Action<string> TapPointsSpendErrorEvent;

	public static event Action<string> TapPointsAwardedEvent;

	public static event Action<string> TapPointsAwardErrorEvent;

	public static event Action<string> CurrencyEarnedEvent;

	public static event Action<string> FullScreenAdLoadedEvent;

	public static event Action<string> FullScreenAdErrorEvent;

	public static event Action<string> DailyRewardAdLoadedEvent;

	public static event Action<string> DailyRewardAdErrorEvent;

	public static event Action<string> DisplayAdLoadedEvent;

	public static event Action<string> DisplayAdErrorEvent;

	public static event Action<string> VideoAdStartEvent;

	public static event Action<string> VideoAdErrorEvent;

	public static event Action<string> VideoAdCompleteEvent;

	public void Awake()
	{
		if (instance == null)
		{
			gameObject.name = GetType().ToString();
			UnityEngine.Object.DontDestroyOnLoad(this);
			instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	public void TapjoyConnectSuccess(string message)
	{
		if (TapjoyConnectSuccessEvent != null)
		{
			TapjoyConnectSuccessEvent(message);
		}
	}

	public void TapjoyConnectFail(string message)
	{
		if (TapjoyConnectFailsEvent != null)
		{
			TapjoyConnectFailsEvent(message);
		}
	}

	public void TapPointsLoaded(string message)
	{
		if (TapPointsLoadedEvent != null)
		{
			TapPointsLoadedEvent("Total TapPoints: " + TapjoyPlugin.QueryTapPoints());
		}
	}

	public void TapPointsLoadedError(string message)
	{
		if (TapPointsLoadedErrorEvent != null)
		{
			TapPointsLoadedErrorEvent("TapPointsLoadedError: " + message);
		}
	}

	public void TapPointsSpent(string message)
	{
		if (TapPointsSpentEvent != null)
		{
			TapPointsSpentEvent("Total TapPoints: " + TapjoyPlugin.QueryTapPoints());
		}
	}

	public void TapPointsSpendError(string message)
	{
		if (TapPointsSpendErrorEvent != null)
		{
			TapPointsSpendErrorEvent("TapPointsSpendError: " + message);
		}
	}

	public void TapPointsAwarded(string message)
	{
		if (TapPointsAwardedEvent != null)
		{
			TapPointsAwardedEvent("Total TapPoints: " + TapjoyPlugin.QueryTapPoints());
		}
	}

	public void TapPointsAwardError(string message)
	{
		TapPointsAwardErrorEvent("TapPointsAwardError: " + message);
	}

	public void CurrencyEarned(string message)
	{
		if (CurrencyEarnedEvent != null)
		{
			CurrencyEarnedEvent("Currency Earned: " + message);
		}
	}

	public void FullScreenAdLoaded(string message)
	{
		if (FullScreenAdLoadedEvent != null)
		{
			FullScreenAdLoadedEvent("FullScreenAdLoaded: " + message);
		}
	}

	public void FullScreenAdError(string message)
	{
		if (FullScreenAdErrorEvent != null)
		{
			FullScreenAdErrorEvent("FullScreenAdError: " + message);
		}
	}

	public void DailyRewardAdLoaded(string message)
	{
		if (DailyRewardAdLoadedEvent != null)
		{
			DailyRewardAdLoadedEvent("DailyRewardAd: " + message);
		}
		TapjoyPlugin.ShowDailyRewardAd();
	}

	public void DailyRewardAdError(string message)
	{
		if (DailyRewardAdErrorEvent != null)
		{
			DailyRewardAdErrorEvent("DailyRewardAd: " + message);
		}
	}

	public void DisplayAdLoaded(string message)
	{
		if (DisplayAdLoadedEvent != null)
		{
			DisplayAdLoadedEvent("DisplayAdLoaded: " + message);
		}
	}

	public void DisplayAdError(string message)
	{
		if (DisplayAdErrorEvent != null)
		{
			DisplayAdErrorEvent("DisplayAdError: " + message);
		}
	}

	public void VideoAdStart(string message)
	{
		if (VideoAdStartEvent != null)
		{
			VideoAdStartEvent("VideoAdStart: " + message);
		}
	}

	public void VideoAdError(string message)
	{
		if (VideoAdErrorEvent != null)
		{
			VideoAdErrorEvent("VideoAdError: " + message);
		}
	}

	public void VideoAdComplete(string message)
	{
		if (VideoAdCompleteEvent != null)
		{
			VideoAdCompleteEvent("VideoAdComplete: " + message);
		}
	}
}
