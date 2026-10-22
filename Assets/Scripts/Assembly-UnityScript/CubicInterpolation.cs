using System;
using UnityEngine;

[Serializable]
public class CubicInterpolation
{
	public static float interpolate(float t, float val1, float tangent1, float val2, float tangent2)
	{
		float num = t * t * t;
		float num2 = t * t;
		return (2f * num - 3f * num2 + 1f) * val1 + (num - 2f * num2 + t) * tangent1 + (-2f * num + 3f * num2) * val2 + (num - num2) * tangent2;
	}

	public static Vector2 interpolate(float t, Vector2 val1, Vector2 tangent1, Vector2 val2, Vector2 tangent2)
	{
		float num = t * t * t;
		float num2 = t * t;
		return (2f * num - 3f * num2 + 1f) * val1 + (num - 2f * num2 + t) * tangent1 + (-2f * num + 3f * num2) * val2 + (num - num2) * tangent2;
	}

	public static Vector3 interpolate(float t, Vector3 val1, Vector3 tangent1, Vector3 val2, Vector3 tangent2)
	{
		float num = t * t * t;
		float num2 = t * t;
		return (2f * num - 3f * num2 + 1f) * val1 + (num - 2f * num2 + t) * tangent1 + (-2f * num + 3f * num2) * val2 + (num - num2) * tangent2;
	}
}
