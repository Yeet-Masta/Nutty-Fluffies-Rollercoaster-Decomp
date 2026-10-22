// NFPort: SDK removed, bodies stubbed by tools/stub_sdks.py
using System.Runtime.InteropServices;
using UnityEngine;

public class CrossPromotionBinding
{
	private static string webNewsURL;
	private static void _crossPromoSetUrgentNewsURLs(string iphoneURL, string ipadURL) { }

	public static void setUrgentNewsURLs(string iphoneURL, string ipadURL)
	{ }
	private static void _crossPromoSetWebNewsURLs(string iphoneURL, string ipadURL) { }

	public static void setWebNewsURLs(string iphoneURL, string ipadURL)
	{ }
	private static void _crossPromoSetInfoBadgeURLs(string iphoneURL, string ipadURL) { }

	public static void setInfoBadgeURLs(string iphoneURL, string ipadURL)
	{ }
	private static void _crossPromoUrgentNewsUpdateInterval(int seconds) { }

	public static void setUrgentNewsUpdateInterval(int seconds)
	{ }
	private static void _crossPromoWebNewsUpdateInterval(int seconds) { }

	public static void setWebNewsUpdateInterval(int seconds)
	{ }
	private static void _crossPromoInfoBadgeUpdateInterval(int seconds) { }

	public static void setInfoBadgeUpdateInterval(int seconds)
	{ }
	private static void _crossPromoStartComponents(int componentMask) { }

	public static void startComponents(bool urgentNews, bool webNews, bool infoBadge)
	{ }
	private static void _crossPromoInfoBadgeType(int type, float animLength, float x, float y, int alignment) { }

	public static void setInfoBadgeType(InfoBadgeAnimationType type, float animLengthSeconds, Vector2 position, InfoBadgeAlignment alignment)
	{ }
	private static void _crossPromoShowWebNews() { }

	public static void showWebNews()
	{ }
	private static void _crossPromoShowUrgentNews() { }

	public static void showUrgentNews()
	{ }
	private static void _crossPromoReloadWebNews() { }

	public static void reloadWebNews()
	{ }
	private static bool _crossPromoHasWebNews() { return default; }

	public static bool hasWebNews()
	{ return default; }
	private static void _crossPromoReloadUrgentNews() { }

	public static void reloadUrgentNews()
	{ }
	private static void _crossPromoSetInfoBadgeVisibility(int visible) { }

	public static void setInfoBadgeVisibility(bool visible)
	{ }
	private static void _crossPromoReloadInfoBadge() { }

	public static void reloadInfoBadge()
	{ }
}
