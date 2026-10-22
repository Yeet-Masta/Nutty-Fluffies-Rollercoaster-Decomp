using System;
using UnityEngine;

[Serializable]
public class MathFast
{
	public static Vector3 LerpVector(Vector3 one, Vector3 two, float t)
	{
		if (!(t <= 1f))
		{
			t = 1f;
		}
		else if (!(t >= 0f))
		{
			t = 0f;
		}
		return new Vector3(one.x + (two.x - one.x) * t, one.y + (two.y - one.y) * t, one.z + (two.z - one.z) * t);
	}

	public static float Smoothstep(float a, float b, float t)
	{
		if (!(t <= 1f))
		{
			t = 1f;
		}
		else if (!(t >= 0f))
		{
			t = 0f;
		}
		return a + t * t * (3f - 2f * t) * (b - a);
	}
}
