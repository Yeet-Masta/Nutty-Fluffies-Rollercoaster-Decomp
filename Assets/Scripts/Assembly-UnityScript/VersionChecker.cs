using UnityEngine;

public class VersionChecker : MonoBehaviour
{
	private static VersionChecker instance;

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
		int counter = PlayerPrefs.GetInt("UPDATE_CHECK_COUNTER", 0);
		counter++;
		if (counter > checkInterval)
		{
			if (GameData.LOG)
			{
				Debug.Log("Getting version: " + infoUrl);
			}
			counter = 0;
		}
		PlayerPrefs.SetInt("UPDATE_CHECK_COUNTER", counter);
		PlayerPrefs.Save();
	}
}
