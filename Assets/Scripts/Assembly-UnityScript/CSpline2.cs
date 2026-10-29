using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class CSpline2
{
	[Serializable]
	public class CSplinePoint
	{
		public Vector2 position;

		public Vector2 tangentIn;

		public Vector2 tangentOut;

		public CSplinePoint(Vector2 position, Vector2 tangentIn, Vector2 tangentOut)
		{
			this.position = position;
			this.tangentIn = tangentIn;
			this.tangentOut = tangentOut;
		}

		public CSplinePoint(CSplinePoint other)
		{
			position = other.position;
			tangentIn = other.tangentIn;
			tangentOut = other.tangentOut;
		}
	}

	public List<CSplinePoint> splineData = new List<CSplinePoint>();

	public virtual CSpline2 getCopy()
	{
		CSpline2 cSpline = new CSpline2();
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(splineData);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is CSplinePoint))
			{
				obj = RuntimeServices.Coerce(obj, typeof(CSplinePoint));
			}
			CSplinePoint cSplinePoint = (CSplinePoint)obj;
			cSpline.addPoint(new CSplinePoint(cSplinePoint));
			UnityRuntimeServices.Update(enumerator, cSplinePoint);
		}
		return cSpline;
	}

	public virtual Vector2 interpolate(float t)
	{
		float num = Mathf.Floor(t);
		if (!(num >= 0f) && !(num < (float)splineData.Count))
		{
			Debug.Log("invalid segment");
			num = 0f;
			t = 0f;
		}
		int num2 = (int)(num + 1f);
		if (num2 >= splineData.Count)
		{
			num2 = (int)num;
		}
		CSplinePoint cSplinePoint = splineData[(int)num];
		CSplinePoint cSplinePoint2 = splineData[num2];
		return CubicInterpolation.interpolate(t - num, cSplinePoint.position, cSplinePoint.tangentOut, cSplinePoint2.position, cSplinePoint2.tangentIn);
	}

	public virtual void setPointPosition(int index, Vector2 pos)
	{
		CSplinePoint cSplinePoint = splineData[index];
		cSplinePoint.position = pos;
		splineData[index] = cSplinePoint;
	}

	public virtual void setPointTangentIn(int index, Vector2 tangent)
	{
		CSplinePoint cSplinePoint = splineData[index];
		cSplinePoint.tangentIn = tangent;
		splineData[index] = cSplinePoint;
	}

	public virtual void setPointTangentOut(int index, Vector2 tangent)
	{
		CSplinePoint cSplinePoint = splineData[index];
		cSplinePoint.tangentOut = tangent;
		splineData[index] = cSplinePoint;
	}

	public virtual CSplinePoint getPoint(int index)
	{
		return (index < 0 || index >= splineData.Count) ? null : splineData[index];
	}

	public virtual void addPoint(CSplinePoint pt)
	{
		splineData.Add(pt);
	}

	public virtual void addPoint(Vector2 position, Vector2 tangentIn, Vector2 tangentOut)
	{
		CSplinePoint cSplinePoint = new CSplinePoint(Vector2.zero, Vector2.zero, Vector2.zero);
		cSplinePoint.position = position;
		cSplinePoint.tangentIn = tangentIn;
		cSplinePoint.tangentOut = tangentOut;
		splineData.Add(cSplinePoint);
	}

	public virtual void addPoint(CSplinePoint pt, int index)
	{
		splineData.Insert(index, pt);
	}

	public virtual void removePoint(int index)
	{
		splineData.RemoveAt(index);
	}

	public virtual int getPointAmount()
	{
		return splineData.Count;
	}

	public virtual List<Vector2> convertToVertices(float vertexTreshold, float step, float maxSegmentLength)
	{
		List<Vector2> list = new List<Vector2>();
		float num = maxSegmentLength * maxSegmentLength;
		for (int i = 0; i < splineData.Count - 1; i++)
		{
			float num2 = step;
			Vector2 rhs = splineData[i].tangentOut;
			rhs.Normalize();
			Vector2 vector = splineData[i].position;
			Vector2 vector2 = vector;
			list.Add(vector);
			for (; num2 < 1f; num2 += step)
			{
				Vector2 vector3 = interpolate((float)i + num2);
				Vector2 vector4 = vector3 - vector;
				Vector2 vector5 = vector3 - vector2;
				vector4.Normalize();
				if (vector5.sqrMagnitude > num || !(Vector2.Dot(vector4, rhs) >= 1f - vertexTreshold))
				{
					list.Add(vector3);
					rhs = vector4;
					vector2 = vector3;
				}
				vector = vector3;
			}
		}
		CSplinePoint cSplinePoint = splineData[splineData.Count - 1];
		list.Add(cSplinePoint.position);
		return list;
	}

	private static bool lineLineIntersection(Vector2 aStart, Vector2 aEnd, Vector2 bStart, Vector2 bEnd)
	{
		float num = (aEnd.y - aStart.y) * (bEnd.x - bStart.x) - (aEnd.x - aStart.x) * (bEnd.y - bStart.y);
		int num2;
		if (!(num <= -1E-05f) && !(num >= 1E-05f))
		{
			num2 = 0;
		}
		else
		{
			float num3 = ((aEnd.x - aStart.x) * (bStart.y - aStart.y) - (aEnd.y - aStart.y) * (bStart.x - aStart.x)) / num;
			if (num3 < 0f || !(num3 <= 1f))
			{
				num2 = 0;
			}
			else
			{
				float num4 = ((bEnd.x - bStart.x) * (bStart.y - aStart.y) - (bEnd.y - bStart.y) * (bStart.x - aStart.x)) / num;
				num2 = num4 < 0f ? 0 : 1;
				if (num2 != 0)
				{
					num2 = num4 > 1f ? 0 : 1;
				}
			}
		}
		return (byte)num2 != 0;
	}

	public virtual bool testSelfIntersection()
	{
		int num = 0;
		int result;
		while (true)
		{
			if (num < splineData.Count - 2)
			{
				int num2 = num + 1;
				while (num2 < splineData.Count - 2)
				{
					if (!lineLineIntersection(splineData[num].position, splineData[num + 1].position, splineData[num2].position, splineData[num2 + 1].position))
					{
						num2++;
						continue;
					}
					goto IL_0063;
				}
				num++;
				continue;
			}
			result = 0;
			break;
			IL_0063:
			result = 1;
			break;
		}
		return (byte)result != 0;
	}
}
