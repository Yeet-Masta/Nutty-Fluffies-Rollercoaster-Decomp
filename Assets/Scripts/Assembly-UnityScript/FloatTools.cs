using System;
using UnityEngine;

[Serializable]
public class FloatTools
{
	public static float clampDecimals(float from, int numDecimals)
	{
		int num = (int)Mathf.Pow(10f, numDecimals);
		return Mathf.Floor(from * (float)num) / (float)num;
	}

	public static string getAsString(float from, int numDecimals)
	{
		float num = clampDecimals(from, numDecimals);
		return string.Empty + num;
	}
}
