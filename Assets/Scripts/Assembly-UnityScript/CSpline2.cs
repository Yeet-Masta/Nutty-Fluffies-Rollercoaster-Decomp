using System;
using System.Collections.Generic;
using UnityEngine;

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
		foreach (CSplinePoint cSplinePoint in splineData)
		{
			cSpline.addPoint(new CSplinePoint(cSplinePoint));
		}
		return cSpline;
	}

	public virtual Vector2 interpolate(float t)
	{
		float segment = Mathf.Floor(t);
		if (!(segment >= 0f) && !(segment < (float)splineData.Count))
		{
			Debug.Log("invalid segment");
			segment = 0f;
			t = 0f;
		}
		int nextIndex = (int)(segment + 1f);
		if (nextIndex >= splineData.Count)
		{
			nextIndex = (int)segment;
		}
		CSplinePoint cSplinePoint = splineData[(int)segment];
		CSplinePoint cSplinePoint2 = splineData[nextIndex];
		return CubicInterpolation.interpolate(t - segment, cSplinePoint.position, cSplinePoint.tangentOut, cSplinePoint2.position, cSplinePoint2.tangentIn);
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
		float maxSegmentLengthSq = maxSegmentLength * maxSegmentLength;
		for (int i = 0; i < splineData.Count - 1; i++)
		{
			float t = step;
			Vector2 rhs = splineData[i].tangentOut;
			rhs.Normalize();
			Vector2 lastVertex = splineData[i].position;
			Vector2 lastSample = lastVertex;
			list.Add(lastVertex);
			for (; t < 1f; t += step)
			{
				Vector2 sample = interpolate((float)i + t);
				Vector2 direction = sample - lastVertex;
				Vector2 sampleDelta = sample - lastSample;
				direction.Normalize();
				if (sampleDelta.sqrMagnitude > maxSegmentLengthSq || !(Vector2.Dot(direction, rhs) >= 1f - vertexTreshold))
				{
					list.Add(sample);
					rhs = direction;
					lastSample = sample;
				}
				lastVertex = sample;
			}
		}
		CSplinePoint cSplinePoint = splineData[splineData.Count - 1];
		list.Add(cSplinePoint.position);
		return list;
	}

	private static bool lineLineIntersection(Vector2 aStart, Vector2 aEnd, Vector2 bStart, Vector2 bEnd)
	{
		float denominator = (aEnd.y - aStart.y) * (bEnd.x - bStart.x) - (aEnd.x - aStart.x) * (bEnd.y - bStart.y);
		if (!(denominator <= -1E-05f) && !(denominator >= 1E-05f))
		{
			return false;
		}
		float ua = ((aEnd.x - aStart.x) * (bStart.y - aStart.y) - (aEnd.y - aStart.y) * (bStart.x - aStart.x)) / denominator;
		if (ua < 0f || !(ua <= 1f))
		{
			return false;
		}
		float ub = ((bEnd.x - bStart.x) * (bStart.y - aStart.y) - (bEnd.y - bStart.y) * (bStart.x - aStart.x)) / denominator;
		return !(ub < 0f) && !(ub > 1f);
	}

	public virtual bool testSelfIntersection()
	{
		for (int i = 0; i < splineData.Count - 2; i++)
		{
			for (int j = i + 1; j < splineData.Count - 2; j++)
			{
				if (lineLineIntersection(splineData[i].position, splineData[i + 1].position, splineData[j].position, splineData[j + 1].position))
				{
					return true;
				}
			}
		}
		return false;
	}
}
