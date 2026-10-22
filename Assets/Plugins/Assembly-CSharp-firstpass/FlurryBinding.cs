// NFPort: SDK removed, bodies stubbed by tools/stub_sdks.py
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class FlurryBinding
{
	private static string dictionaryToString(Dictionary<string, string> dict)
	{ return default; }
	private static void _flurryStartSession(string apiKey) { }

	public static void startSession(string apiKey)
	{ }
	private static void _flurryLogEvent(string eventName, bool isTimed) { }

	public static void logEvent(string eventName, bool isTimed)
	{ }
	private static void _flurryLogEventWithParameters(string eventName, string parameters, bool isTimed) { }

	public static void logEventWithParameters(string eventName, Dictionary<string, string> parameters, bool isTimed)
	{ }
	private static void _flurryEndTimedEvent(string eventName, string parameters) { }

	public static void endTimedEvent(string eventName)
	{ }

	public static void endTimedEvent(string eventName, Dictionary<string, string> parameters)
	{ }
	private static void _flurrySetAge(int age) { }

	public static void setAge(int age)
	{ }
	private static void _flurrySetGender(string gender) { }

	public static void setGender(string gender)
	{ }
	private static void _flurrySetUserId(string userId) { }

	public static void setUserid(string userId)
	{ }
	private static void _flurrySetSessionReportsOnCloseEnabled(bool sendSessionReportsOnClose) { }

	public static void setSessionReportsOnCloseEnabled(bool sendSessionReportsOnClose)
	{ }
	private static void _flurrySetSessionReportsOnPauseEnabled(bool setSessionReportsOnPauseEnabled) { }

	public static void setSessionReportsOnPauseEnabled(bool setSessionReportsOnPauseEnabled)
	{ }
	private static void _flurrySetAppCircleEnabled(bool shouldEnable) { }

	public static void setAppCircleEnabled(bool shouldEnable)
	{ }
	private static void _flurryGetHook(string hook, int xLoc, int yLoc) { }

	public static void getHook(string hook, int xLoc, int yLoc)
	{ }
	private static void _flurryUpdateHook() { }

	public static void updateHook()
	{ }
	private static void _flurryRemoveHook() { }

	public static void removeHook()
	{ }
	private static void _flurryOpenCatalog(string hook, string orientation, bool animated) { }

	public static void openCatalog(string hook, string orientation, bool animated)
	{ }
	private static void _flurryOpenTakeover(string hook, string orientation) { }

	public static void openTakeover(string hook, string orientation)
	{ }
	private static string _flurryGetOffer(string hook) { return default; }

	public static Hashtable getOffer(string hook)
	{ return default; }
	private static int _flurryGetOfferCount(string hook) { return default; }

	public static int getOfferCount(string hook)
	{ return default; }
	private static bool _flurryAppAdIsAvailable(string hook) { return default; }

	public static bool appAdIsAvailable(string hook)
	{ return default; }
	private static void _flurrySetClipsEnabled(bool shouldEnable) { }

	public static void setClipsEnabled(bool shouldEnable)
	{ }
	private static bool _flurryIsVideoAdEnabled(string hook) { return default; }

	public static bool isVideoAdEnabled(string hook)
	{ return default; }
	private static void _flurryOpenVideoTakeover(string hook, string orientation, string rewardMessage, string cookies) { }

	public static void openVideoTakeover(string hook, string orientation, string rewardMessage, Dictionary<string, string> cookies)
	{ }
}
