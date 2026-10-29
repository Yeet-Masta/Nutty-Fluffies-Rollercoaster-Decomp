using System;

[Serializable]
public class Mission
{
	public const int FLAG_COMPLETED = 1;

	public const int FLAG_DISABLED = 2;

	public static readonly int FLAG_REQUIRE_FINISH = 32768;

	public string description;

	public int activeRounds;

	public string id;

	public int flags;

	public float time;

	public int score;

	public int type;

	public float value;

	public int featureUnlock;

	public string achievementKey;

	public int originalScore;

	public int attempts;

	public virtual bool testFlag(int flag)
	{
		return (flags & flag) != 0;
	}

	public virtual int intValue()
	{
		return (int)value;
	}
}
