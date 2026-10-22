// NFPort: SDK removed, bodies stubbed by tools/stub_sdks.py
using UnityEngine;

public class CrossPromotionAndroid
{
	private static object _currentActivity;

	private static object _unityWrapper;

	private static string webNewsURL;

	private static string urgentNewsURL;

	private static string urgentNewsURL_HD;

	private static string infoBadgeURL;

	private static long updateInterval;

	static CrossPromotionAndroid()
	{ }

	public static void setUrgentNewsURLs(string URL, string URL_HD)
	{ }

	public static void setWebNewsURLs(string URL)
	{ }

	public static void setInfoBadgeURLs(string URL)
	{ }

	public static void startComponents(bool urgentNews, bool webNews, bool infoBadge)
	{ }

	public static void setInfoBadgeType(InfoBadgeAnimationType type, float animLengthSeconds, Vector2 position, InfoBadgeAlignment alignment)
	{ }

	public static void showWebNews()
	{ }

	public static void showUrgentNews()
	{ }

	public static bool canShowUrgentNews()
	{ return default; }

	public static void reloadWebNews()
	{ }

	public static bool hasWebNews()
	{ return default; }

	public static void reloadUrgentNews()
	{ }

	public static void setInfoBadgeVisibility(bool visible)
	{ }

	public static void reloadInfoBadge()
	{ }
}
