using System;
using Boo.Lang.Runtime;
using UnityEngine;

[Serializable]
public class Statistics
{
	[NonSerialized]
	protected static Statistics instance;

	public int totalPlayCount;

	public int failCount;

	public int finishCount;

	public int roundsWithoutFail;

	public float roundTime;

	public bool roundDidFinish;

	public static Statistics getInstance()
	{
		if (RuntimeServices.EqualityOperator(instance, null))
		{
			instance = new Statistics();
			instance.loadStatistics();
		}
		return instance;
	}

	public virtual void loadStatistics()
	{
		totalPlayCount = PlayerPrefs.GetInt("STAT_TOTAL_PLAY_COUNT", 0);
		failCount = PlayerPrefs.GetInt("STAT_FAIL_COUNT", 0);
		finishCount = PlayerPrefs.GetInt("STAT_FINISH_COUNT", 0);
		roundsWithoutFail = PlayerPrefs.GetInt("STAT_ROUNDS_WIHOUT_FAIL", 0);
	}

	public virtual void startLevel()
	{
		totalPlayCount++;
		roundTime = Time.time;
		roundDidFinish = false;
		PlayerPrefs.SetInt("STAT_TOTAL_PLAY_COUNT", totalPlayCount);
	}

	public virtual void finishLevel()
	{
		finishCount++;
		PlayerPrefs.SetInt("STAT_FINISH_COUNT", finishCount);
		roundsWithoutFail++;
		PlayerPrefs.SetInt("STAT_ROUNDS_WIHOUT_FAIL", roundsWithoutFail);
		if (GameData.LOG)
		{
			Debug.Log("NO FAILS: " + roundsWithoutFail);
		}
		roundTime = Time.time - roundTime;
		roundDidFinish = true;
		printStats();
	}

	public virtual void failLevel()
	{
		failCount++;
		PlayerPrefs.SetInt("STAT_FAIL_COUNT", failCount);
		roundsWithoutFail = 0;
		PlayerPrefs.SetInt("STAT_ROUNDS_WIHOUT_FAIL", roundsWithoutFail);
	}

	public virtual void printStats()
	{
		if (GameData.LOG)
		{
			if (GameData.LOG)
			{
				Debug.Log("TOTAL PLAY COUNT: " + totalPlayCount);
			}
			if (GameData.LOG)
			{
				Debug.Log("FAIL COUNT: " + failCount);
			}
			if (GameData.LOG)
			{
				Debug.Log("FINISH COUNT: " + finishCount);
			}
		}
	}
}
