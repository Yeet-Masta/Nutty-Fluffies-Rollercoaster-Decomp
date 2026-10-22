using System;
using UnityEngine;

[Serializable]
public class TimeTools
{
	public static string getFullTimeAsString(float totalTime)
	{
		int num = (int)totalTime;
		int num2 = num / 60;
		int num3 = num % 60;
		int num4 = (int)(totalTime * 100f % 100f);
		string empty = string.Empty;
		empty += num2 + ":";
		if (num3 < 10)
		{
			empty += "0";
		}
		empty += num3;
		empty += ":";
		if (num4 < 10)
		{
			empty += "0";
		}
		return empty + num4;
	}

	public static string getMinSecTimeAsString(float totalTime)
	{
		int num = (int)totalTime;
		int num2 = num / 60;
		int num3 = num % 60;
		int num4 = (int)(totalTime * 100f % 100f);
		string empty = string.Empty;
		empty += num2 + ":";
		if (num3 < 10)
		{
			empty += "0";
		}
		return empty + num3;
	}

	public static string getSecDsTimeAsString(float totalTime)
	{
		int num = (int)totalTime;
		int num2 = (int)Mathf.Floor(totalTime);
		int num3 = (int)(totalTime * 10f % 10f);
		string lhs = string.Empty;
		if (num2 < 10)
		{
			lhs += "0";
		}
		lhs += num2;
		return lhs + (":" + num3 + "0");
	}

	public static string getSecCsTimeAsString(float totalTime)
	{
		int num = (int)totalTime;
		int num2 = (int)Mathf.Floor(totalTime);
		int num3 = (int)(totalTime * 100f % 100f);
		string lhs = string.Empty;
		if (num2 < 10)
		{
			lhs += "0";
		}
		lhs += num2 + ":";
		if (num3 < 10)
		{
			lhs += "0";
		}
		return lhs + num3;
	}
}
