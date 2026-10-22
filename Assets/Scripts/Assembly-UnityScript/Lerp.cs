using System;
using UnityEngine;

[Serializable]
public class Lerp
{
	public static Vector3 LerpSmooth(Vector3 from, Vector4 to, float time)
	{
		time = Mathf.SmoothStep(0f, 1f, time);
		return MathFast.LerpVector(from, to, time);
	}

	public static Quaternion SlerpSmooth(Quaternion from, Quaternion to, float time)
	{
		time = Mathf.SmoothStep(0f, 1f, time);
		return Quaternion.Slerp(from, to, time);
	}
}
