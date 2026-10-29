using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CustomProfiler
{
	private static Dictionary<string, float> totalTimeDict;

	private static Dictionary<string, float> interTimeDict;

	public static void start()
	{
		totalTimeDict = new Dictionary<string, float>();
		interTimeDict = new Dictionary<string, float>();
	}

	public static void begin(string elem)
	{
		if (interTimeDict != null)
		{
			interTimeDict[elem] = Time.realtimeSinceStartup;
		}
	}

	public static void end(string elem)
	{
		if (interTimeDict != null)
		{
			float elapsed = Time.realtimeSinceStartup - interTimeDict[elem];
			if (totalTimeDict.ContainsKey(elem))
			{
				totalTimeDict[elem] += elapsed;
			}
			else
			{
				totalTimeDict[elem] = elapsed;
			}
		}
	}

	public static void report()
	{
		if (GameData.LOG)
		{
			Debug.Log("=== PROFILED TIME ===");
		}
		foreach (string key in totalTimeDict.Keys)
		{
			if (GameData.LOG)
			{
				Debug.Log(" > '" + key + "': " + totalTimeDict[key]);
			}
		}
		if (GameData.LOG)
		{
			Debug.Log("=== END ===");
		}
		totalTimeDict = null;
		interTimeDict = null;
	}
}
