using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class MeshTools
{
	public static Vector3[] pushVertices(Vector3[] to, Vector3[] from)
	{
		Vector3[] array = new Vector3[Extensions.get_length((System.Array)to) + Extensions.get_length((System.Array)from)];
		int num = 0;
		int num2 = Extensions.get_length((System.Array)to);
		for (num = 0; num < Extensions.get_length((System.Array)to); num++)
		{
			array[num] = to[num];
		}
		int num3 = Extensions.get_length((System.Array)from);
		for (num = 0; num < num3; num++)
		{
			array[num + num2] = from[num];
		}
		return array;
	}

	public static Vector2[] pushUVs(Vector2[] to, Vector2[] from)
	{
		Vector2[] array = new Vector2[Extensions.get_length((System.Array)to) + Extensions.get_length((System.Array)from)];
		int num = 0;
		int num2 = Extensions.get_length((System.Array)to);
		for (num = 0; num < Extensions.get_length((System.Array)to); num++)
		{
			array[num] = to[num];
		}
		int num3 = Extensions.get_length((System.Array)from);
		for (num = 0; num < num3; num++)
		{
			array[num + num2] = from[num];
		}
		return array;
	}

	public static int[] pushTriangles(int[] to, int[] from)
	{
		int[] array = new int[Extensions.get_length((System.Array)to) + Extensions.get_length((System.Array)from)];
		int num = 0;
		int num2 = Extensions.get_length((System.Array)to);
		for (num = 0; num < Extensions.get_length((System.Array)to); num++)
		{
			array[num] = to[num];
		}
		int num3 = Extensions.get_length((System.Array)from);
		for (num = 0; num < num3; num++)
		{
			array[num + num2] = from[num];
		}
		return array;
	}
}
