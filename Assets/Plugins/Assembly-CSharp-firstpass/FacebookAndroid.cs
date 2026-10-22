// NFPort: SDK removed, bodies stubbed by tools/stub_sdks.py
using System;
using System.Collections.Generic;
using UnityEngine;

public class FacebookAndroid
{
	private static object _facebookBinding;

	private static object _facebookBindingClass;

	private static object _currentActivity;

	private static string _appID;

	private static byte _requestID;

	static FacebookAndroid()
	{ }

	public static void init(string appId)
	{ }

	public static void login()
	{ }

	public static void loginWithRequestedPermissions(string[] permissions, string urlSchemeSuffix)
	{ }

	public static void loginWithRequestedPermissions(string[] permissions)
	{ }

	public static void informFacebookInstall()
	{ }

	public static void logout()
	{ }

	public static void showPostMessageDialog()
	{ }

	public static void showPostMessageDialogWithOptions(string link, string linkName, string linkToImage, string caption)
	{ }

	public static void showDialog(string dialogType, Dictionary<string, string> parameters)
	{ }

	public static string getAccessToken()
	{ return default; }

	public static bool isSessionValid()
	{ return default; }

	public static void restRequest(string restMethod, string httpMethod, Dictionary<string, string> parameters)
	{ }
}
