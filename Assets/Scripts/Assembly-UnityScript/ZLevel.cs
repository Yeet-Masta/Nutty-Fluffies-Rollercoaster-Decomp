using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class ZLevel
{
	public const float PARTITION_WIDTH = 14f;

	public const float SUBDIVISION = 2f;

	private static ZLevel instance;

	private List<Vector3> points;

	private List<Vector3>[] partition;

	private float[] maxY;

	private float minX;

	public static ZLevel getInstance()
	{
		if (instance == null)
		{
			instance = new ZLevel();
		}
		return instance;
	}

	public virtual void create(Level level)
	{
		points = new List<Vector3>();
		Component[] componentsInChildren = level.gameObject.GetComponentsInChildren(typeof(LevelSpline), true);
		if (componentsInChildren.Length > 0)
		{
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				addSpline(componentsInChildren[i] as LevelSpline);
			}
		}
		createPartitions();
	}

	private void addSpline(LevelSpline spline)
	{
		Vector3[] collisionMeshVertices = spline.getCollisionMeshVertices();
		Vector3 position = spline.transform.position;
		Vector3 prev = (collisionMeshVertices[0] + collisionMeshVertices[1]) * 0.5f;
		addPoint(prev + position);
		for (int i = 2; i < collisionMeshVertices.Length; i += 2)
		{
			Vector3 point = (collisionMeshVertices[i] + collisionMeshVertices[i + 1]) * 0.5f;
			float span = (point.x - prev.x) / 2f;
			for (float offset = 2f; offset < span; offset += 2f)
			{
				Vector3 interpolated = prev + (point - prev) * (offset / span);
				addPoint(interpolated + position);
			}
			addPoint(point + position);
			prev = point;
		}
	}

	private void addPoint(Vector3 pt)
	{
		points.Add(pt);
	}

	private void createPartitions()
	{
		float lowestX = 999999.9f;
		float highestX = -999999.9f;
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(points);
		while (enumerator.MoveNext())
		{
			Vector3 point = (Vector3)enumerator.Current;
			if (!(point.x <= highestX))
			{
				highestX = point.x;
				UnityRuntimeServices.Update(enumerator, point);
			}
			if (!(point.x >= lowestX))
			{
				lowestX = point.x;
				UnityRuntimeServices.Update(enumerator, point);
			}
		}
		int partitionCount = (int)(Mathf.Ceil(highestX - lowestX) / 14f);
		partition = new List<Vector3>[partitionCount];
		maxY = new float[partitionCount];
		for (int i = 0; i < partitionCount; i++)
		{
			partition[i] = new List<Vector3>();
			maxY[i] = -9999f;
		}
		IEnumerator enumerator2 = UnityRuntimeServices.GetEnumerator(points);
		while (enumerator2.MoveNext())
		{
			Vector3 point = (Vector3)enumerator2.Current;
			int partitionIndex = (int)((point.x - lowestX) / 14f);
			UnityRuntimeServices.Update(enumerator2, point);
			if (partitionIndex < 0)
			{
				partitionIndex = 0;
			}
			else if (partitionIndex >= partitionCount)
			{
				partitionIndex = partitionCount - 1;
			}
			partition[partitionIndex].Add(point);
			UnityRuntimeServices.Update(enumerator2, point);
			if (!(point.y <= maxY[partitionIndex]))
			{
				maxY[partitionIndex] = point.y;
				UnityRuntimeServices.Update(enumerator2, point);
			}
		}
		minX = lowestX;
		points = null;
	}

	public virtual float getYLimit(float x)
	{
		int partitionIndex = (int)Mathf.Floor((x - minX) / 14f);
		if (partitionIndex >= partition.Length)
		{
			partitionIndex = partition.Length - 1;
		}
		else if (partitionIndex < 0)
		{
			partitionIndex = 0;
		}
		return maxY[partitionIndex];
	}

	public virtual float getZ(Vector3 position)
	{
		int partitionIndex = (int)Mathf.Floor((position.x - minX) / 14f);
		float closestDistSq = 99999f;
		float result = 0f;
		Vector3 delta = default(Vector3);
		float distSq = 0f;
		if (partitionIndex >= partition.Length)
		{
			partitionIndex = partition.Length - 1;
		}
		else if (partitionIndex < 0)
		{
			partitionIndex = 0;
		}
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(partition[partitionIndex]);
		while (enumerator.MoveNext())
		{
			Vector3 point = (Vector3)enumerator.Current;
			delta = position - point;
			UnityRuntimeServices.Update(enumerator, point);
			distSq = delta.sqrMagnitude;
			if (!(closestDistSq <= distSq))
			{
				result = point.z;
				UnityRuntimeServices.Update(enumerator, point);
				closestDistSq = distSq;
			}
		}
		if (partitionIndex > 0)
		{
			IEnumerator enumerator2 = UnityRuntimeServices.GetEnumerator(partition[partitionIndex - 1]);
			while (enumerator2.MoveNext())
			{
				Vector3 point = (Vector3)enumerator2.Current;
				delta = position - point;
				UnityRuntimeServices.Update(enumerator2, point);
				distSq = delta.sqrMagnitude;
				if (!(closestDistSq <= distSq))
				{
					result = point.z;
					UnityRuntimeServices.Update(enumerator2, point);
					closestDistSq = distSq;
				}
			}
		}
		if (partitionIndex < partition.Length - 1)
		{
			IEnumerator enumerator3 = UnityRuntimeServices.GetEnumerator(partition[partitionIndex + 1]);
			while (enumerator3.MoveNext())
			{
				Vector3 point = (Vector3)enumerator3.Current;
				delta = position - point;
				UnityRuntimeServices.Update(enumerator3, point);
				distSq = delta.sqrMagnitude;
				if (!(closestDistSq <= distSq))
				{
					result = point.z;
					UnityRuntimeServices.Update(enumerator3, point);
					closestDistSq = distSq;
				}
			}
		}
		return result;
	}
}
