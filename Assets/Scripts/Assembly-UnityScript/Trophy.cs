using System;
using Boo.Lang.Runtime;
using UnityEngine;

[Serializable]
public class Trophy
{
	[NonSerialized]
	public static string PREFS_FIXED_VERSION = "001";

	[NonSerialized]
	public static string PREFS_FIXED_PREFIX = "TROPHY_";

	[NonSerialized]
	public static string PREFS_OVERRIDE_VERSION;

	public string name;

	public float value;

	public double time;

	public Trophy(string name, float value)
	{
		this.name = name;
		this.value = value;
		time = DateTime.Now.ToFileTime();
	}

	private Trophy()
	{
	}

	private string getPrefPrefix(string valueName)
	{
		string rhs = PREFS_FIXED_VERSION;
		if (PREFS_OVERRIDE_VERSION != null)
		{
			rhs = PREFS_OVERRIDE_VERSION;
		}
		return "V" + rhs + "_" + PREFS_FIXED_PREFIX + "_" + name + "_" + valueName;
	}

	public virtual void saveToPrefs()
	{
		PlayerPrefs.SetFloat(getPrefPrefix("value"), value);
		PlayerPrefs.SetFloat(getPrefPrefix("time"), (float)time);
	}

	public static Trophy loadFromPrefs(string levelName, string name)
	{
		return loadFromPrefs(levelName + "_" + name);
	}

	public virtual bool isNonZero()
	{
		return value != 0f;
	}

	public static Trophy loadFromPrefs(string name)
	{
		Trophy trophy = new Trophy();
		trophy.name = name;
		trophy.value = PlayerPrefs.GetFloat(trophy.getPrefPrefix("value"), -1f);
		trophy.time = PlayerPrefs.GetFloat(trophy.getPrefPrefix("time"), -1f);
		return (trophy.time != -1.0) ? trophy : null;
	}

	public static bool loadFromPrefsIsNonZero(string name)
	{
		Trophy trophy = loadFromPrefs(name);
		return !RuntimeServices.EqualityOperator(trophy, null) && trophy.isNonZero();
	}

	public static Trophy saveNewToPrefs(string levelName, string name, float value)
	{
		return saveNewToPrefs(levelName + "_" + name, value);
	}

	public static Trophy saveNewToPrefs(string name, float value)
	{
		Trophy trophy = new Trophy(name, value);
		trophy.saveToPrefs();
		return trophy;
	}
}
