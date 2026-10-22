using System;
using UnityEngine;

[Serializable]
public class VGUIUtilities
{
	public static Rect getScaledRectFromBounds(Rect r)
	{
		Camera main = Camera.main;
		float aspect = main.aspect;
		float left = 1f * r.x / (float)Screen.width * aspect * main.orthographicSize * 2f;
		float top = 1f * r.y / (float)Screen.height * main.orthographicSize * 2f;
		float width = 1f * r.width / (float)Screen.width * aspect * main.orthographicSize * 2f;
		float height = 1f * r.height / (float)Screen.height * main.orthographicSize * 2f;
		return new Rect(left, top, width, height);
	}

	public static void setMaterialAndResizeImage(VGUIImage imgElement, Material mat)
	{
		imgElement.setMaterial(mat);
		Texture mainTexture = mat.mainTexture;
		Rect scaledRectFromBounds = getScaledRectFromBounds(new Rect(0f, 0f, mainTexture.width, mainTexture.height));
		float num = scaledRectFromBounds.width * 0.5f;
		float num2 = scaledRectFromBounds.height * 0.5f;
		Mesh mesh = new Mesh();
		MeshFilter meshFilter = ((MeshFilter)imgElement.GetComponentInChildren(typeof(MeshFilter))) as MeshFilter;
		meshFilter.mesh = mesh;
		Vector3[] vertices = new Vector3[4]
		{
			new Vector3(0f - num, 0f - num2),
			new Vector3(num, 0f - num2),
			new Vector3(0f - num, num2),
			new Vector3(num, num2)
		};
		Vector2[] uv = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		int[] triangles = new int[6] { 0, 1, 2, 2, 1, 3 };
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
	}
}
