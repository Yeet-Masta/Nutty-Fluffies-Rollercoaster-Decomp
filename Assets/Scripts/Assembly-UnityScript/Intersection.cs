using System;
using Boo.Lang;
using UnityEngine;

[Serializable]
public class Intersection
{
	public static bool LineIntersect(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4, ICallable callback)
	{
		float num = default(float);
		float num2 = default(float);
		float num3 = default(float);
		float num4 = default(float);
		float num5 = default(float);
		float num6 = float.Epsilon;
		num3 = (y4 - y3) * (x2 - x1) - (x4 - x3) * (y2 - y1);
		num4 = (x4 - x3) * (y1 - y3) - (y4 - y3) * (x1 - x3);
		num5 = (x2 - x1) * (y1 - y3) - (y2 - y1) * (x1 - x3);
		int result;
		if (!(Mathf.Abs(num4) >= num6) && !(Mathf.Abs(num5) >= num6) && !(Mathf.Abs(num3) >= num6))
		{
			if (callback != null)
			{
				callback.Call(new object[2]
				{
					(x1 + x2) * 0.5f,
					(y1 + y2) * 0.5f
				});
			}
			result = 1;
		}
		else if (!(Mathf.Abs(num3) >= num6))
		{
			result = 0;
		}
		else
		{
			num = num4 / num3;
			num2 = num5 / num3;
			if (num < 0f || num > 1f || num2 < 0f || !(num2 <= 1f))
			{
				result = 0;
			}
			else
			{
				if (callback != null)
				{
					callback.Call(new object[2]
					{
						x1 + num * (x2 - x1),
						y1 + num * (y2 - y1)
					});
				}
				result = 1;
			}
		}
		return (byte)result != 0;
	}
}
