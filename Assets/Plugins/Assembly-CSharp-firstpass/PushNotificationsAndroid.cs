// NFPort: SDK removed, bodies stubbed by tools/stub_sdks.py
using UnityEngine;

public class PushNotificationsAndroid : MonoBehaviour
{
	private static object pushwoosh;

	public static void InitPushwoosh(string manager)
	{ }

	public void setIntTag(string tagName, int tagValue)
	{ }

	public void unregisterDevice()
	{ }

	public void setStringTag(string tagName, string tagValue)
	{ }

	public void clearLocalNotifications()
	{ }

	public void scheduleLocalNotification(string message, int seconds)
	{ }

	public void scheduleLocalNotification(string message, int seconds, string userdata)
	{ }

	public static string getPushToken()
	{ return default; }

	private void onRegisteredForPushNotifications(string token)
	{ }

	private void onFailedToRegisteredForPushNotifications(string error)
	{ }

	private void onPushNotificationsReceived(string payload)
	{ }
}
