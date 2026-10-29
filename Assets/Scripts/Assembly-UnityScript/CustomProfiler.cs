using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

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
		if (!RuntimeServices.EqualityOperator(interTimeDict, null))
		{
			interTimeDict[elem] = Time.realtimeSinceStartup;
		}
	}

	public static void end(string elem)
	{
		if (!RuntimeServices.EqualityOperator(interTimeDict, null))
		{
			float num = Time.realtimeSinceStartup - interTimeDict[elem];
			if (totalTimeDict.ContainsKey(elem))
			{
				totalTimeDict[elem] += num;
			}
			else
			{
				totalTimeDict[elem] = num;
			}
		}
	}

	public static void report()
	{
		if (GameData.LOG)
		{
			Debug.Log("=== PROFILED TIME ===");
		}
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(totalTimeDict.Keys);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is string))
			{
				obj = RuntimeServices.Coerce(obj, typeof(string));
			}
			string text = (string)obj;
			if (GameData.LOG)
			{
				Debug.Log(" > '" + text + "': " + totalTimeDict[text]);
				UnityRuntimeServices.Update(enumerator, text);
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
