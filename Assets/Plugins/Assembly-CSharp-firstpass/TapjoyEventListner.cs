using UnityEngine;

public class TapjoyEventListner : MonoBehaviour
{
	private void OnEnable()
	{
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
	}

	private void OnDisable()
	{
		TapjoyManager.TapjoyConnectSuccessEvent -= TapjoyConnectSuccessEvent;
		TapjoyManager.TapjoyConnectFailsEvent -= TapjoyConnectFailsEvent;
		TapjoyManager.TapPointsLoadedEvent -= TapPointsLoadedEvent;
		TapjoyManager.TapPointsLoadedErrorEvent -= TapPointsLoadedErrorEvent;
		TapjoyManager.TapPointsSpentEvent -= TapPointsSpentEvent;
		TapjoyManager.TapPointsSpendErrorEvent -= TapPointsSpendErrorEvent;
		TapjoyManager.TapPointsAwardedEvent -= TapPointsAwardedEvent;
		TapjoyManager.TapPointsAwardErrorEvent -= TapPointsAwardErrorEvent;
		TapjoyManager.CurrencyEarnedEvent -= CurrencyEarnedEvent;
		TapjoyManager.FullScreenAdLoadedEvent -= FullScreenAdLoadedEvent;
		TapjoyManager.FullScreenAdErrorEvent -= FullScreenAdErrorEvent;
		TapjoyManager.DailyRewardAdLoadedEvent -= DailyRewardAdLoadedEvent;
		TapjoyManager.DailyRewardAdErrorEvent -= DailyRewardAdErrorEvent;
		TapjoyManager.DisplayAdLoadedEvent -= DisplayAdLoadedEvent;
		TapjoyManager.DisplayAdErrorEvent -= DisplayAdErrorEvent;
		TapjoyManager.VideoAdStartEvent -= VideoAdStartEvent;
		TapjoyManager.VideoAdErrorEvent -= VideoAdErrorEvent;
		TapjoyManager.VideoAdCompleteEvent -= VideoAdCompleteEvent;
	}

	private void TapjoyConnectSuccessEvent(string message)
	{
		Debug.Log(message);
	}

	public void TapjoyConnectFailsEvent(string message)
	{
		Debug.Log(message);
	}

	public void TapPointsLoadedEvent(string message)
	{
		Debug.Log("TapPointsLoaded: " + message);
	}

	public void TapPointsLoadedErrorEvent(string message)
	{
		Debug.Log("TapPointsLoadedError: " + message);
	}

	public void TapPointsSpentEvent(string message)
	{
		Debug.Log("TapPointsSpent: " + message);
	}

	public void TapPointsSpendErrorEvent(string message)
	{
		Debug.Log("TapPointsSpendError: " + message);
	}

	public void TapPointsAwardedEvent(string message)
	{
		Debug.Log("TapPointsAwarded: " + message);
	}

	public void TapPointsAwardErrorEvent(string message)
	{
		Debug.Log("TapPointsAwardError: " + message);
	}

	public void CurrencyEarnedEvent(string message)
	{
		Debug.Log("CurrencyEarned: " + message);
	}

	public void FullScreenAdLoadedEvent(string message)
	{
		Debug.Log("FullScreenAdLoaded: " + message);
	}

	public void FullScreenAdErrorEvent(string message)
	{
		Debug.Log("FullScreenAdError: " + message);
	}

	public void DailyRewardAdLoadedEvent(string message)
	{
		Debug.Log("DailyRewardAd: " + message);
	}

	public void DailyRewardAdErrorEvent(string message)
	{
		Debug.Log("DailyRewardAd: " + message);
	}

	public void DisplayAdLoadedEvent(string message)
	{
		Debug.Log("DisplayAdLoaded: " + message);
	}

	public void DisplayAdErrorEvent(string message)
	{
		Debug.Log("DisplayAdError: " + message);
	}

	public void VideoAdStartEvent(string message)
	{
		Debug.Log("VideoAdStart: " + message);
	}

	public void VideoAdErrorEvent(string message)
	{
		Debug.Log("VideoAdError: " + message);
	}

	public void VideoAdCompleteEvent(string message)
	{
		Debug.Log("VideoAdComplete: " + message);
	}
}
