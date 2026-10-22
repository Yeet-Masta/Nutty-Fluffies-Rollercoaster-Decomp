using System;

[Serializable]
public class Cubic : Ease
{
	public static double EaseIn(double t, double b, double c, double d)
	{
		t /= d;
		return c * t * t * t + b;
	}

	public static double EaseOut(double t, double b, double c, double d)
	{
		t /= d;
		return c * ((t - 1.0) * t * t + 1.0) + b;
	}

	public static double EaseInOut(double t, double b, double c, double d)
	{
		t /= d / 2.0;
		double result;
		if (!(t >= 1.0))
		{
			result = c / 2.0 * t * t * t + b;
		}
		else
		{
			t -= 2.0;
			result = c / 2.0 * (t * t * t + 2.0) + b;
		}
		return result;
	}
}
