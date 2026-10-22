// NFPort: SDK removed, bodies stubbed by tools/stub_sdks.py
using System.Collections.Generic;
using UnityEngine;

public class TwitterAndroid
{
	private static object _plugin;

	static TwitterAndroid()
	{ }

	public static void init(string consumerKey, string consumerSecret)
	{ }

	public static bool isLoggedIn()
	{ return default; }

	public static void showLoginDialog()
	{ }

	public static void logout()
	{ }

	public static void postUpdate(string update)
	{ }

	public static void postUpdateWithImage(string update, byte[] image)
	{ }

	public static void getHomeTimeline()
	{ }

	public static void getFollowers()
	{ }

	public static void performRequest(string methodType, string path, Dictionary<string, string> parameters)
	{ }
}
