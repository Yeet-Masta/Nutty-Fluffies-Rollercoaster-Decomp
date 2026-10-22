using System;
using UnityEngine;

[Serializable]
public class RectTools
{
	public static Rect scaleRectFromCenter(Rect r, double scale)
	{
		r.x = (float)((double)r.x - ((double)r.width * scale - (double)r.width) * 0.5);
		r.y = (float)((double)r.y - ((double)r.height * scale - (double)r.height) * 0.5);
		r.width = (float)((double)r.width * scale);
		r.height = (float)((double)r.height * scale);
		return r;
	}

	public static Rect setRectCenter(Rect r, Vector2 pos)
	{
		r.x = pos.x - r.width * 0.5f;
		r.y = pos.y - r.height * 0.5f;
		return r;
	}

	public static Rect interpolateRectCenter(Rect r, Vector2 posTo, double time)
	{
		r.x = (float)((double)posTo.x * time + (double)(r.x + r.width * 0.5f) * (1.0 - time) - (double)(r.width * 0.5f));
		r.y = (float)((double)posTo.y * time + (double)(r.y + r.height * 0.5f) * (1.0 - time) - (double)(r.height * 0.5f));
		return r;
	}

	public static Rect scaleRectCenter(Rect r, double scale)
	{
		r.x = (float)((double)(r.x - (float)Screen.width * 0.5f) * scale + (double)((float)Screen.width * 0.5f));
		r.y = (float)((double)(r.y - (float)Screen.height * 0.5f) * scale + (double)((float)Screen.height * 0.5f));
		return r;
	}
}
