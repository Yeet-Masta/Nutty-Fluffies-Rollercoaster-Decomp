// NFPort: SDK removed, bodies stubbed by tools/stub_sdks.py
using System.Collections.Generic;
using UnityEngine;

public class TapjoyPluginAndroid : MonoBehaviour
{
	private static object TapjoyConnect
	{
		get
		{ return default; }
	}

	private static object TapjoyConnectInstance
	{
		get
		{ return default; }
	}

	public static void SetCallbackHandler(string handlerName)
	{ }

	public static void RequestTapjoyConnect(string appID, string secretKey)
	{ }

	public static void RequestTapjoyConnect(string appID, string secretKey, Dictionary<string, string> flags)
	{ }

	public static void EnableLogging(bool enable)
	{ }

	public static void ActionComplete(string actionID)
	{ }

	public static void SetUserID(string userID)
	{ }

	public static void ShowOffers()
	{ }

	public static void GetTapPoints()
	{ }

	public static void SpendTapPoints(int points)
	{ }

	public static void AwardTapPoints(int points)
	{ }

	public static int QueryTapPoints()
	{ return default; }

	public static void SetEarnedPointsNotifier()
	{ }

	public static void ShowDefaultEarnedCurrencyAlert()
	{ }

	public static void GetDisplayAd()
	{ }

	public static void ShowDisplayAd()
	{ }

	public static void HideDisplayAd()
	{ }

	public static void SetDisplayAdSize(TapjoyDisplayAdSize size)
	{ }

	public static void EnableDisplayAdAutoRefresh(bool enable)
	{ }

	public static void RefreshDisplayAd()
	{ }

	public static void MoveDisplayAd(int x, int y)
	{ }

	public static void SetTransitionEffect(int transition)
	{ }

	public static void GetFullScreenAd()
	{ }

	public static void ShowFullScreenAd()
	{ }

	public static void GetDailyRewardAd()
	{ }

	public static void ShowDailyRewardAd()
	{ }

	public static void SendShutDownEvent()
	{ }

	public static void SendIAPEvent(string name, float price, int quantity, string currencyCode)
	{ }

	public static void ShowOffersWithCurrencyID(string currencyID, bool selector)
	{ }

	public static void GetDisplayAdWithCurrencyID(string currencyID)
	{ }

	public static void GetFullScreenAdWithCurrencyID(string currencyID)
	{ }

	public static void SetCurrencyMultiplier(float multiplier)
	{ }

	public static void GetDailyRewardAdWithCurrencyID(string currencyID)
	{ }
}
