using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class ZLevel
{
	[NonSerialized]
	public const float PARTITION_WIDTH = 14f;

	[NonSerialized]
	public const float SUBDIVISION = 2f;

	[NonSerialized]
	private static ZLevel instance;

	private List<Vector3> points;

	private List<Vector3>[] partition;

	private float[] maxY;

	private float minX;

	public static ZLevel getInstance()
	{
		if (RuntimeServices.EqualityOperator(instance, null))
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
		Vector3 vector = (collisionMeshVertices[0] + collisionMeshVertices[1]) * 0.5f;
		addPoint(vector + position);
		for (int i = 2; i < collisionMeshVertices.Length; i += 2)
		{
			Vector3 vector2 = (collisionMeshVertices[i] + collisionMeshVertices[i + 1]) * 0.5f;
			float num = (vector2.x - vector.x) / 2f;
			for (float num2 = 2f; num2 < num; num2 += 2f)
			{
				Vector3 vector3 = vector + (vector2 - vector) * (num2 / num);
				addPoint(vector3 + position);
			}
			addPoint(vector2 + position);
			vector = vector2;
		}
	}

	private void addPoint(Vector3 pt)
	{
		points.Add(pt);
	}

	private void createPartitions()
	{
		float num = 999999.9f;
		float num2 = -999999.9f;
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(points);
		while (enumerator.MoveNext())
		{
			Vector3 vector = (Vector3)enumerator.Current;
			if (!(vector.x <= num2))
			{
				num2 = vector.x;
				UnityRuntimeServices.Update(enumerator, vector);
			}
			if (!(vector.x >= num))
			{
				num = vector.x;
				UnityRuntimeServices.Update(enumerator, vector);
			}
		}
		int num3 = (int)(Mathf.Ceil(num2 - num) / 14f);
		partition = new List<Vector3>[num3];
		maxY = new float[num3];
		for (int i = 0; i < num3; i++)
		{
			partition[i] = new List<Vector3>();
			maxY[i] = -9999f;
		}
		IEnumerator enumerator2 = UnityRuntimeServices.GetEnumerator(points);
		while (enumerator2.MoveNext())
		{
			Vector3 vector2 = (Vector3)enumerator2.Current;
			int num4 = (int)((vector2.x - num) / 14f);
			UnityRuntimeServices.Update(enumerator2, vector2);
			if (num4 < 0)
			{
				num4 = 0;
			}
			else if (num4 >= num3)
			{
				num4 = num3 - 1;
			}
			partition[num4].Add(vector2);
			UnityRuntimeServices.Update(enumerator2, vector2);
			if (!(vector2.y <= maxY[num4]))
			{
				maxY[num4] = vector2.y;
				UnityRuntimeServices.Update(enumerator2, vector2);
			}
		}
		minX = num;
		points = null;
	}

	public virtual float getYLimit(float x)
	{
		int num = (int)Mathf.Floor((x - minX) / 14f);
		if (num >= partition.Length)
		{
			num = partition.Length - 1;
		}
		else if (num < 0)
		{
			num = 0;
		}
		return maxY[num];
	}

	public virtual float getZ(Vector3 position)
	{
		int num = (int)Mathf.Floor((position.x - minX) / 14f);
		float num2 = 99999f;
		float result = 0f;
		Vector3 vector = default(Vector3);
		float num3 = default(float);
		if (num >= partition.Length)
		{
			num = partition.Length - 1;
		}
		else if (num < 0)
		{
			num = 0;
		}
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(partition[num]);
		while (enumerator.MoveNext())
		{
			Vector3 vector2 = (Vector3)enumerator.Current;
			vector = position - vector2;
			UnityRuntimeServices.Update(enumerator, vector2);
			num3 = vector.sqrMagnitude;
			if (!(num2 <= num3))
			{
				result = vector2.z;
				UnityRuntimeServices.Update(enumerator, vector2);
				num2 = num3;
			}
		}
		if (num > 0)
		{
			IEnumerator enumerator2 = UnityRuntimeServices.GetEnumerator(partition[num - 1]);
			while (enumerator2.MoveNext())
			{
				Vector3 vector3 = (Vector3)enumerator2.Current;
				vector = position - vector3;
				UnityRuntimeServices.Update(enumerator2, vector3);
				num3 = vector.sqrMagnitude;
				if (!(num2 <= num3))
				{
					result = vector3.z;
					UnityRuntimeServices.Update(enumerator2, vector3);
					num2 = num3;
				}
			}
		}
		if (num < partition.Length - 1)
		{
			IEnumerator enumerator3 = UnityRuntimeServices.GetEnumerator(partition[num + 1]);
			while (enumerator3.MoveNext())
			{
				Vector3 vector4 = (Vector3)enumerator3.Current;
				vector = position - vector4;
				UnityRuntimeServices.Update(enumerator3, vector4);
				num3 = vector.sqrMagnitude;
				if (!(num2 <= num3))
				{
					result = vector4.z;
					UnityRuntimeServices.Update(enumerator3, vector4);
					num2 = num3;
				}
			}
		}
		return result;
	}
}
