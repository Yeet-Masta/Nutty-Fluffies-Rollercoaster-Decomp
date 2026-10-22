using System;
using UnityEngine;

[Serializable]
public class PlayerPrefsTool
{
	public static void SetBool(string name, bool value)
	{
		PlayerPrefs.SetInt(name, value ? 1 : 0);
	}

	public static bool GetBool(string name)
	{
		return PlayerPrefs.GetInt(name) == 1;
	}

	public static void SetStringArray()
	{
	}

	public static void SetFloatArray()
	{
	}
}
