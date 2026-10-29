using System;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
	private static readonly int SECONDS_IN_DAY = 86400;

	private const long TICKS_IN_SECOND = 10000000L;

	private static NotificationManager instance;

	private bool activated;

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
		long now = DateTime.Now.Ticks / 10000000L;
		long quitTime = long.Parse(PlayerPrefs.GetString("QUIT_TIME", "0"));
		if ((int)quitTime != 0 && !activated)
		{
			long elapsed = now - quitTime;
			int days = (int)(elapsed / SECONDS_IN_DAY);
			int revenue = (int)getRevenue(days);
			if (revenue > 0 && MenuManager.getInstance() != null)
			{
				GeneralSoundEffects.playSound(MenuManager.getInstance().cashSound);
				string message = LocalizationManager.localize("REVENUE_TEXT").Replace("%d", string.Empty + GameData.formatTokenValue(string.Empty + revenue));
				MenuManager.getInstance().showMessage(LocalizationManager.localize("REVENUE_TITLE"), message, MenuManager.getInstance().cashImage);
				GameData.addTokens(revenue);
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
			long now = DateTime.Now.Ticks / 10000000L;
			PlayerPrefs.SetString("QUIT_TIME", string.Empty + now);
		}
	}

	private void createRevenueNotification(int number, int days)
	{
		int revenue = (int)getRevenue(days);
		if (revenue > 0)
		{
			LocalizationManager.localize("REVENUE_TEXT_MESSAGE_" + number).Replace("%d", string.Empty + GameData.formatTokenValue(string.Empty + revenue));
		}
	}

	private float getRevenue(int days)
	{
		int index = GameData.NOTIFICATION_DAYS.Length - 1;
		while (index >= 0)
		{
			if (days >= GameData.NOTIFICATION_DAYS[index])
			{
				return Mathf.Min((index + 1) * GameData.TRACK_REVENUE, GameData.TRACK_REVENUE_MAX);
			}
			index--;
		}
		return 0f;
	}
}
