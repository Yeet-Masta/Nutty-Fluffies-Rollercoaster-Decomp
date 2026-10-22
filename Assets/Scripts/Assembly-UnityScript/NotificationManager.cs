using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class NotificationManager : MonoBehaviour
{
	[NonSerialized]
	private static readonly int SECONDS_IN_DAY = 86400;

	[NonSerialized]
	private const long TICKS_IN_SECOND = 10000000L;

	[NonSerialized]
	private static NotificationManager instance;

	private bool activated;

	private bool inited;

	private bool didStart;

	public static NotificationManager getInstance()
	{
		return instance;
	}

	public virtual void Awake()
	{
		if (instance != null && instance != this)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
		else
		{
			instance = this;
		}
	}

	public virtual void Start()
	{
		didStart = true;
	}

	public virtual void runRevenueCheck()
	{
		Invoke("revenueCheck", 0.5f);
	}

	public virtual void revenueCheck()
	{
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		if (!BaseMenuManager.MENU_SCENE)
		{
			return;
		}
		long num = DateTime.Now.Ticks / 10000000L;
		long num2 = long.Parse(PlayerPrefs.GetString("QUIT_TIME", "0"));
		if ((int)num2 != 0 && !activated)
		{
			long num3 = num - num2;
			int days = (int)(num3 / SECONDS_IN_DAY);
			int num4 = (int)getRevenue(days);
			if (num4 > 0 && MenuManager.getInstance() != null)
			{
				GeneralSoundEffects.playSound(MenuManager.getInstance().cashSound);
				string message = LocalizationManager.localize("REVENUE_TEXT").Replace("%d", string.Empty + GameData.formatTokenValue(string.Empty + num4));
				MenuManager.getInstance().showMessage(LocalizationManager.localize("REVENUE_TITLE"), message, MenuManager.getInstance().cashImage);
				GameData.addTokens(num4);
				MenuManager.getInstance().updateTokenAmount();
			}
			activated = true;
		}
	}

	public virtual void remoteNotificationReceivedWithURL(string url)
	{
		if (GameData.LOG)
		{
			Debug.Log("remoteNotificationReceived with URL: " + url);
		}
	}

	public virtual void OnApplicationPause(bool pause)
	{
		if (GameData.LOG)
		{
			Debug.Log("OnApplicationPause OnApplicationPause OnApplicationPause ");
		}
		if (pause)
		{
			createNotifications();
			activated = false;
		}
		else if (!activated)
		{
			Start();
			runRevenueCheck();
		}
	}

	public virtual void OnApplicationQuit()
	{
		if (GameData.LOG)
		{
			Debug.Log(" Notification Manager quit");
		}
		createNotifications();
	}

	private void createNotifications()
	{
		if (didStart)
		{
			long num = DateTime.Now.Ticks / 10000000L;
			PlayerPrefs.SetString("QUIT_TIME", string.Empty + num);
		}
	}

	private void createRevenueNotification(int number, int days)
	{
		int num = (int)getRevenue(days);
		if (num > 0)
		{
			string text = LocalizationManager.localize("REVENUE_TEXT_MESSAGE_" + number).Replace("%d", string.Empty + GameData.formatTokenValue(string.Empty + num));
		}
	}

	private float getRevenue(int days)
	{
		int num = Extensions.get_length((System.Array)GameData.NOTIFICATION_DAYS) - 1;
		float result;
		while (true)
		{
			if (num >= 0)
			{
				if (days >= GameData.NOTIFICATION_DAYS[num])
				{
					result = Mathf.Min((num + 1) * GameData.TRACK_REVENUE, GameData.TRACK_REVENUE_MAX);
					break;
				}
				num--;
				continue;
			}
			result = 0f;
			break;
		}
		return result;
	}
}
