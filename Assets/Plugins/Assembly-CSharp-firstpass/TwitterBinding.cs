// NFPort: SDK removed, bodies stubbed by tools/stub_sdks.py
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class TwitterBinding
{
	private static void _twitterInit(string consumerKey, string consumerSecret) { }

	public static void init(string consumerKey, string consumerSecret)
	{ }
	private static bool _twitterIsLoggedIn() { return default; }

	public static bool isLoggedIn()
	{ return default; }
	private static string _twitterLoggedInUsername() { return default; }

	public static string loggedInUsername()
	{ return default; }
	private static void _twitterLogin(string username, string password) { }

	public static void login(string username, string password)
	{ }
	private static void _twitterShowOauthLoginDialog() { }

	public static void showOauthLoginDialog()
	{ }
	private static void _twitterLogout() { }

	public static void logout()
	{ }
	private static void _twitterPostStatusUpdate(string status) { }

	public static void postStatusUpdate(string status)
	{ }
	private static void _twitterPostStatusUpdateWithImage(string status, string imagePath) { }

	public static void postStatusUpdate(string status, string pathToImage)
	{ }
	private static void _twitterGetHomeTimeline() { }

	public static void getHomeTimeline()
	{ }
	private static void _twitterPerformRequest(string methodType, string path, string parameters) { }

	public static void performRequest(string methodType, string path, Dictionary<string, string> parameters)
	{ }
	private static bool _twitterIsTweetSheetSupported() { return default; }

	public static bool isTweetSheetSupported()
	{ return default; }
	private static bool _twitterCanUserTweet() { return default; }

	public static bool canUserTweet()
	{ return default; }
	private static void _twitterShowTweetComposer(string status, string imagePath, string url) { }

	public static void showTweetComposer(string status)
	{ }

	public static void showTweetComposer(string status, string pathToImage)
	{ }

	public static void showTweetComposer(string status, string pathToImage, string link)
	{ }
}
