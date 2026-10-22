// NFPort: SDK removed, bodies stubbed by tools/stub_sdks.py
using System;
using System.Collections.Generic;
using UnityEngine;

public class FlurryAndroid
{
	public static object flurryAgent;

	private static object currentActivity;

	static FlurryAndroid()
	{ }

	private static string dictionaryToString(Dictionary<string, string> _dict)
	{ return default; }

	private static object dictionaryToAJO(Dictionary<string, string> _dict)
	{ return default; }

	public static void startSession(string _key)
	{ }

	public static void endSession()
	{ }

	public static void logEvent(string _eventId, bool _timed)
	{ }

	public static void logEvent(string _eventId, Dictionary<string, string> _parameters)
	{ }

	public static void logEventWithParameters(string _eventId, Dictionary<string, string> _parameters, bool _timed)
	{ }
}
