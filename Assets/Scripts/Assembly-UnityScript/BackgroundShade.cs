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
		int[] triangles = new int[columns * 6];
		int index = 0;
		for (int i = 0; i < columns; i++)
		{
			int vertex = i * 4;
			triangles[index++] = vertex;
			triangles[index++] = vertex + 1;
			triangles[index++] = vertex + 3;
			triangles[index++] = vertex;
			triangles[index++] = vertex + 3;
			triangles[index++] = vertex + 2;
			if (vertex + 3 >= columns * 4)
			{
				print("ILLEGAL INDEX");
			}
		}
		graphicMesh.triangles = triangles;
		meshFilter.mesh = graphicMesh;
	}

	public virtual void Update()
	{
		float sectorAngle = (float)Math.PI / (float)columns;
		process += 0.0005f;
		if (!(process <= sectorAngle))
		{
			process -= sectorAngle;
		}
		float angle = process;
		int vertexIndex = 0;
		Vector3[] vertices = graphicMesh.vertices;
		float innerRadius = 0f;
		float outerRadius = 2f;
		for (int i = 0; i < columns; i++)
		{
			float halfWidth = sectorWidth * Mathf.Sin(angle);
			float sin = Mathf.Sin(angle - halfWidth);
			float cos = Mathf.Cos(angle - halfWidth);
			vertices[vertexIndex].x = innerRadius * cos;
			vertices[vertexIndex++].y = (0f - innerRadius) * sin;
			vertices[vertexIndex].x = outerRadius * cos;
			vertices[vertexIndex++].y = (0f - outerRadius) * sin;
			sin = Mathf.Sin(angle + halfWidth);
			cos = Mathf.Cos(angle + halfWidth);
			vertices[vertexIndex].x = innerRadius * cos;
			vertices[vertexIndex++].y = (0f - innerRadius) * sin;
			vertices[vertexIndex].x = outerRadius * cos;
			vertices[vertexIndex++].y = (0f - outerRadius) * sin;
			angle += sectorAngle;
		}
		graphicMesh.vertices = vertices;
	}
}
