using System;
using System.Collections.Generic;
using UnityEngine;

public class TapjoyPlugin : MonoBehaviour
{
	public static void SetCallbackHandler(string handlerName)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.SetCallbackHandler(handlerName);
		}
	}

	public static void RequestTapjoyConnect(string appID, string secretKey)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.RequestTapjoyConnect(appID, secretKey);
		}
	}

	public static void RequestTapjoyConnect(string appID, string secretKey, Dictionary<string, string> flags)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.RequestTapjoyConnect(appID, secretKey, flags);
		}
	}

	public static void EnableLogging(bool enable)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.EnableLogging(enable);
		}
	}

	public static void ActionComplete(string actionID)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.ActionComplete(actionID);
		}
	}

	public static void SetUserID(string userID)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.SetUserID(userID);
		}
	}

	public static void ShowOffers()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.ShowOffers();
		}
	}

	public static void GetTapPoints()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetTapPoints();
		}
	}

	public static void SpendTapPoints(int points)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.SpendTapPoints(points);
		}
	}

	public static void AwardTapPoints(int points)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.AwardTapPoints(points);
		}
	}

	public static int QueryTapPoints()
	{
		if (Application.platform == RuntimePlatform.OSXEditor)
		{
			return 0;
		}
		return TapjoyPluginAndroid.QueryTapPoints();
	}

	public static void ShowDefaultEarnedCurrencyAlert()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.ShowDefaultEarnedCurrencyAlert();
		}
	}

	public static void GetDisplayAd()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetDisplayAd();
		}
	}

	public static void ShowDisplayAd()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.ShowDisplayAd();
		}
	}

	public static void HideDisplayAd()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.HideDisplayAd();
		}
	}

	[Obsolete("SetDisplayAdContentSize is deprecated. Please use SetDisplayAdSize.")]
	public static void SetDisplayAdContentSize(int size)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.SetDisplayAdSize((TapjoyDisplayAdSize)size);
		}
	}

	public static void SetDisplayAdSize(TapjoyDisplayAdSize size)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.SetDisplayAdSize(size);
		}
	}

	[Obsolete("RefreshDisplayAd(bool enable) is deprecated. Please use EnableDisplayAdAutoRefresh.")]
	public static void RefreshDisplayAd(bool enable)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.EnableDisplayAdAutoRefresh(enable);
		}
	}

	public static void EnableDisplayAdAutoRefresh(bool enable)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.EnableDisplayAdAutoRefresh(enable);
		}
	}

	[Obsolete("RefreshDisplayAd() is deprecated. Please use GetDisplayAd.")]
	public static void RefreshDisplayAd()
	{
		TapjoyPluginAndroid.GetDisplayAd();
	}

	public static void MoveDisplayAd(int x, int y)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.MoveDisplayAd(x, y);
		}
	}

	[Obsolete("SetUserDefinedColorWithIntValue is deprecated. There is no navigation bar for iOS in v9.x.x+.")]
	public static void SetUserDefinedColorWithIntValue(int color)
	{
	}

	public static void SetTransitionEffect(int transition)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.SetTransitionEffect(transition);
		}
	}

	[Obsolete("GetFeaturedApp is deprecated, please use GetFullScreenAd instead.")]
	public static void GetFeaturedApp()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetFullScreenAd();
		}
	}

	[Obsolete("SetFeaturedAppDisplayCount is deprecated.")]
	public static void SetFeaturedAppDisplayCount(int displayCount)
	{
	}

	[Obsolete("ShowFeaturedAppFullScreenAd is deprecated, please use ShowFullScreenAd instead.")]
	public static void ShowFeaturedAppFullScreenAd()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.ShowFullScreenAd();
		}
	}

	public static void GetFullScreenAd()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetFullScreenAd();
		}
	}

	public static void ShowFullScreenAd()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.ShowFullScreenAd();
		}
	}

	[Obsolete("GetReEngagementAd is deprecated, please use GetDailyRewardAd instead.")]
	public static void GetReEngagementAd()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetDailyRewardAd();
		}
	}

	[Obsolete("ShowReEngagementAd is deprecated, please use ShowDailyRewardAd instead.")]
	public static void ShowReEngagementAd()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.ShowDailyRewardAd();
		}
	}

	public static void GetDailyRewardAd()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetDailyRewardAd();
		}
	}

	public static void ShowDailyRewardAd()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.ShowDailyRewardAd();
		}
	}

	[Obsolete("InitVideoAd is deprecated, video is now controlled via your Tapjoy Dashboard.")]
	public static void InitVideoAd()
	{
	}

	[Obsolete("SetVideoCacheCount is deprecated, video is now controlled via your Tapjoy Dashboard.")]
	public static void SetVideoCacheCount(int cacheCount)
	{
	}

	[Obsolete("EnableVideoCache is deprecated, video is now controlled via your Tapjoy Dashboard.")]
	public static void EnableVideoCache(bool enable)
	{
	}

	public static void SendShutDownEvent()
	{
		TapjoyPluginAndroid.SendShutDownEvent();
	}

	public static void SendIAPEvent(string name, float price, int quantity, string currencyCode)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.SendIAPEvent(name, price, quantity, currencyCode);
		}
	}

	public static void ShowOffersWithCurrencyID(string currencyID, bool selector)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.ShowOffersWithCurrencyID(currencyID, selector);
		}
	}

	public static void GetDisplayAdWithCurrencyID(string currencyID)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetDisplayAdWithCurrencyID(currencyID);
		}
	}

	[Obsolete("RefreshDisplayAdWithCurrencyID is deprecated, please use GetDisplayAdWithCurrencyID instead.")]
	public static void RefreshDisplayAdWithCurrencyID(string currencyID)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetDisplayAdWithCurrencyID(currencyID);
		}
	}

	[Obsolete("GetFeaturedAppWithCurrencyID is deprecated, please use GetFullScreenAdWithCurrencyID instead.")]
	public static void GetFeaturedAppWithCurrencyID(string currencyID)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetFullScreenAdWithCurrencyID(currencyID);
		}
	}

	public static void GetFullScreenAdWithCurrencyID(string currencyID)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetFullScreenAdWithCurrencyID(currencyID);
		}
	}

	public static void SetCurrencyMultiplier(float multiplier)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.SetCurrencyMultiplier(multiplier);
		}
	}

	[Obsolete("GetReEngagementAdWithCurrencyID is deprecated, please use GetDailyRewardAdWithCurrencyID instead.")]
	public static void GetReEngagementAdWithCurrencyID(string currencyID)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetDailyRewardAdWithCurrencyID(currencyID);
		}
	}

	public static void GetDailyRewardAdWithCurrencyID(string currencyID)
	{
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			TapjoyPluginAndroid.GetDailyRewardAdWithCurrencyID(currencyID);
		}
	}
}
