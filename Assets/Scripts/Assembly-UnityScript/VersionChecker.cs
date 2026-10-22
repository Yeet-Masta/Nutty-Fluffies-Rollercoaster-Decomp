using System;
using UnityEngine;

[Serializable]
public class VersionChecker : MonoBehaviour
{
	private GameObject listenerRef;

	[NonSerialized]
	private static VersionChecker instance;

	[NonSerialized]
	private static bool hasChecked;

	public int checkInterval;

	public static VersionChecker getInstance()
	{
		return instance;
	}

	public virtual void Awake()
	{
		instance = this;
	}

	public virtual void getVersion(string infoUrl, GameObject listener)
	{
		if (hasChecked)
		{
			return;
		}
		hasChecked = true;
		int num = PlayerPrefs.GetInt("UPDATE_CHECK_COUNTER", 0);
		num++;
		if (num > checkInterval)
		{
			if (GameData.LOG)
			{
				Debug.Log("Getting version: " + infoUrl);
			}
			num = 0;
		}
		PlayerPrefs.SetInt("UPDATE_CHECK_COUNTER", num);
		PlayerPrefs.Save();
	}
}
