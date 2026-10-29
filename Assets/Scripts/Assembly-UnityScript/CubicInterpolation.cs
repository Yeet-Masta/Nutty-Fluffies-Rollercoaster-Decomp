using System;
using UnityEngine;

[Serializable]
public class CubicInterpolation
{
	public static float interpolate(float t, float val1, float tangent1, float val2, float tangent2)
	{
		float t3 = t * t * t;
		float t2 = t * t;
		return (2f * t3 - 3f * t2 + 1f) * val1 + (t3 - 2f * t2 + t) * tangent1 + (-2f * t3 + 3f * t2) * val2 + (t3 - t2) * tangent2;
	}

	public static Vector2 interpolate(float t, Vector2 val1, Vector2 tangent1, Vector2 val2, Vector2 tangent2)
	{
		float t3 = t * t * t;
		float t2 = t * t;
		return (2f * t3 - 3f * t2 + 1f) * val1 + (t3 - 2f * t2 + t) * tangent1 + (-2f * t3 + 3f * t2) * val2 + (t3 - t2) * tangent2;
	}

	public static Vector3 interpolate(float t, Vector3 val1, Vector3 tangent1, Vector3 val2, Vector3 tangent2)
	{
		float t3 = t * t * t;
		float t2 = t * t;
		return (2f * t3 - 3f * t2 + 1f) * val1 + (t3 - 2f * t2 + t) * tangent1 + (-2f * t3 + 3f * t2) * val2 + (t3 - t2) * tangent2;
	}
}
