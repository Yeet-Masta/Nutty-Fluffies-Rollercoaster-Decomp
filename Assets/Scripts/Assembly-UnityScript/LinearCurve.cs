using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LinearCurve
{
	public List<Vector2> controlPoints;

	public LinearCurve()
	{
		controlPoints = new List<Vector2>();
	}

	public virtual float getValue(float t)
	{
		Vector2 vector = default(Vector2);
		Vector2 vector2 = default(Vector2);
		float result;
		if (!(controlPoints[0].x <= t))
		{
			result = controlPoints[0].y;
		}
		else
		{
			int num = 0;
			while (true)
			{
				if (num < controlPoints.Count)
				{
					if (!(controlPoints[num].x >= t))
					{
						vector = controlPoints[num];
						if (num >= controlPoints.Count - 1)
						{
							result = vector.y;
							break;
						}
						vector2 = controlPoints[num + 1];
					}
					num++;
					continue;
				}
				Vector2 vector3 = vector2 - vector;
				result = vector.y + vector3.y * (t - vector.x) / vector3.x;
				break;
			}
		}
		return result;
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
