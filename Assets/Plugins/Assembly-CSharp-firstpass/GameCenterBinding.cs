// NFPort: SDK removed, bodies stubbed by tools/stub_sdks.py
using System.Runtime.InteropServices;
using UnityEngine;

public class GameCenterBinding
{
	private static bool _gameCenterIsGameCenterAvailable() { return default; }

	public static bool isGameCenterAvailable()
	{ return default; }
	private static void _gameCenterAuthenticateLocalPlayer() { }

	public static void authenticateLocalPlayer()
	{ }
	private static bool _gameCenterIsPlayerAuthenticated() { return default; }

	public static bool isPlayerAuthenticated()
	{ return default; }
	private static string _gameCenterPlayerAlias() { return default; }

	public static string playerAlias()
	{ return default; }
	private static string _gameCenterPlayerIdentifier() { return default; }

	public static string playerIdentifier()
	{ return default; }
	private static bool _gameCenterIsUnderage() { return default; }

	public static bool isUnderage()
	{ return default; }
	private static void _gameCenterRetrieveFriends(bool loadProfileImages) { }

	public static void retrieveFriends(bool loadProfileImages)
	{ }
	private static void _gameCenterLoadPlayerData(string playerIds, bool loadProfileImages) { }

	public static void loadPlayerData(string[] playerIdArray, bool loadProfileImages)
	{ }
	private static void _gameCenterLoadProfilePhotoForLocalPlayer() { }

	public static void loadProfilePhotoForLocalPlayer()
	{ }
	private static void _gameCenterLoadLeaderboardLeaderboardTitles() { }

	public static void loadLeaderboardTitles()
	{ }
	private static void _gameCenterReportScore(long score, long context, string leaderboardId) { }

	public static void reportScore(long score, string leaderboardId)
	{ }

	public static void reportScore(long score, long context, string leaderboardId)
	{ }
	private static void _gameCenterIssueScoreChallenge(long score, long context, string leaderboardId, string playerIds, string message) { }

	public static void issueScoreChallenge(long score, long context, string leaderboardId, string[] playerIds, string message)
	{ }
	private static void _gameCenterShowLeaderboardWithTimeScope(int timeScope) { }

	public static void showLeaderboardWithTimeScope(GameCenterLeaderboardTimeScope timeScope)
	{ }
	private static void _gameCenterShowLeaderboardWithTimeScopeAndLeaderboardId(int timeScope, string leaderboardId) { }

	public static void showLeaderboardWithTimeScopeAndLeaderboard(GameCenterLeaderboardTimeScope timeScope, string leaderboardId)
	{ }
	private static void _gameCenterRetrieveScores(bool friendsOnly, int timeScope, int start, int end) { }

	public static void retrieveScores(bool friendsOnly, GameCenterLeaderboardTimeScope timeScope, int start, int end)
	{ }
	private static void _gameCenterRetrieveScoresForLeaderboard(bool friendsOnly, int timeScope, int start, int end, string leaderboardId) { }

	public static void retrieveScores(bool friendsOnly, GameCenterLeaderboardTimeScope timeScope, int start, int end, string leaderboardId)
	{ }
	private static void _gameCenterRetrieveScoresForPlayerId(string playerId) { }

	public static void retrieveScoresForPlayerId(string playerId)
	{ }
	private static void _gameCenterRetrieveScoresForPlayerIdAndLeaderboard(string playerId, string leaderboardId) { }

	public static void retrieveScoresForPlayerId(string playerId, string leaderboardId)
	{ }
	private static void _gameCenterReportAchievement(string identifier, float percent) { }

	public static void reportAchievement(string identifier, float percent)
	{ }
	private static void _gameCenterIssueAchievementChallenge(string identifier, string playerIds, string message) { }

	public static void issueAchievementChallenge(string identifier, string[] playerIds, string message)
	{ }
	private static void _gameCenterGetAchievements() { }

	public static void getAchievements()
	{ }
	private static void _gameCenterResetAchievements() { }

	public static void resetAchievements()
	{ }
	private static void _gameCenterShowAchievements() { }

	public static void showAchievements()
	{ }
	private static void _gameCenterRetrieveAchievementMetadata() { }

	public static void retrieveAchievementMetadata()
	{ }
	private static void _gameCenterShowCompletionBannerForAchievements() { }

	public static void showCompletionBannerForAchievements()
	{ }
}
