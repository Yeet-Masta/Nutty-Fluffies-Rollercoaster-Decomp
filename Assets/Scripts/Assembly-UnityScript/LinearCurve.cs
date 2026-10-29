using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LinearCurve
{
	public List<Vector2> controlPoints = new List<Vector2>();

	public virtual float getValue(float t)
	{
		Vector2 from = default(Vector2);
		Vector2 to = default(Vector2);
		if (!(controlPoints[0].x <= t))
		{
			return controlPoints[0].y;
		}
		for (int index = 0; index < controlPoints.Count; index++)
		{
			if (!(controlPoints[index].x >= t))
			{
				from = controlPoints[index];
				if (index >= controlPoints.Count - 1)
				{
					return from.y;
				}
				to = controlPoints[index + 1];
			}
		}
		Vector2 delta = to - from;
		return from.y + delta.y * (t - from.x) / delta.x;
	}

	public virtual void addControlPoint(Vector2 pt)
	{
		controlPoints.Add(pt);
	}

	public virtual int getPointAmount()
	{
		return controlPoints.Count;
	}

	public virtual Vector2 getPoint(int index)
	{
		return controlPoints[index];
	}
}
