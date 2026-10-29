using System;
using UnityEngine;

public class BackgroundShade : MonoBehaviour
{
	private float process;

	private Mesh graphicMesh;

	public MeshFilter meshFilter;

	public MeshRenderer meshRendere;

	public int columns = 18;

	public float sectorWidth = 0.2f;

	public virtual void Awake()
	{
		graphicMesh = new Mesh();
		graphicMesh.vertices = new Vector3[columns * 4];
		graphicMesh.uv = new Vector2[columns * 4];
		int[] array = new int[columns * 6];
		int num = 0;
		for (int i = 0; i < columns; i++)
		{
			int num2 = i * 4;
			array[num++] = num2;
			array[num++] = num2 + 1;
			array[num++] = num2 + 3;
			array[num++] = num2;
			array[num++] = num2 + 3;
			array[num++] = num2 + 2;
			if (num2 + 3 >= columns * 4)
			{
				MonoBehaviour.print("ILLEGAL INDEX");
			}
		}
		graphicMesh.triangles = array;
		meshFilter.mesh = graphicMesh;
	}

	public virtual void Update()
	{
		float num = (float)Math.PI / (float)columns;
		process += 0.0005f;
		if (!(process <= num))
		{
			process -= num;
		}
		float num2 = process;
		int num3 = 0;
		Vector3[] vertices = graphicMesh.vertices;
		float num4 = 0f;
		float num5 = 2f;
		for (int i = 0; i < columns; i++)
		{
			float num6 = sectorWidth * Mathf.Sin(num2);
			float num7 = Mathf.Sin(num2 - num6);
			float num8 = Mathf.Cos(num2 - num6);
			vertices[num3].x = num4 * num8;
			vertices[num3++].y = (0f - num4) * num7;
			vertices[num3].x = num5 * num8;
			vertices[num3++].y = (0f - num5) * num7;
			num7 = Mathf.Sin(num2 + num6);
			num8 = Mathf.Cos(num2 + num6);
			vertices[num3].x = num4 * num8;
			vertices[num3++].y = (0f - num4) * num7;
			vertices[num3].x = num5 * num8;
			vertices[num3++].y = (0f - num5) * num7;
			num2 += num;
		}
		graphicMesh.vertices = vertices;
	}
}
