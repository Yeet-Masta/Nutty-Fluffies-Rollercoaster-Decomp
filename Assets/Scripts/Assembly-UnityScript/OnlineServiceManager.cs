using System;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;

[Serializable]
public class OnlineServiceManager
{
	[Serializable]
	public class LeaderboardScore
	{
		public int score;

		public string name;

		public LeaderboardScore(int _score, string _name)
		{
			score = _score;
			name = _name;
		}
	}

	private static OnlineServiceManager instance;

	private List<LeaderboardScore> leaderboardScores;

	private string currentLeaderboard;

	public bool DEBUG_MODE = true;

	private OnlineServiceManager()
	{
	}

	public static OnlineServiceManager getInstance()
	{
		if (RuntimeServices.EqualityOperator(instance, null))
		{
			instance = new OnlineServiceManager();
		}
		return instance;
	}

	public virtual bool login()
	{
		return true;
	}

	public virtual void loadLeaderboardScores(string leaderboardID)
	{
		if (GameData.LOG)
		{
			Debug.Log("Request scores for leaderboard: " + leaderboardID);
		}
		leaderboardScores = null;
		currentLeaderboard = leaderboardID;
		leaderboardScores = new List<LeaderboardScore>();
	}

	public virtual string getMyName()
	{
		return "Pekka";
	}

	public virtual void awardAchievement(string achievementId)
	{
		if (GameData.LOG)
		{
			Debug.Log("awardAchievement: " + achievementId);
		}
	}

	public virtual void submitLeaderboardScore(string leaderboardID, int score)
	{
		if (GameData.LOG)
		{
			Debug.Log("submit score of " + score + " on leaderboard: " + leaderboardID);
		}
		PlayerPrefs.SetInt("SCORE_" + leaderboardID, score);
	}

	public virtual bool hasLoadedScores()
	{
		return leaderboardScores.Count > 0;
	}

	public virtual List<LeaderboardScore> getLoadedLeaderboardScores()
	{
		return leaderboardScores;
	}
}
