using UnityEngine;

public class VGUITextNative : VGUIText
{
	private Mesh mesh;

	private MeshFilter meshFilter;

	private MeshRenderer meshRenderer;

	private int textureWidth;

	private int textureHeight;

	public int textWidth;

	public int textHeight;

	public int fontPointSize;

	public string fontFamilyName;

	protected override void findTextMesh()
	{
		template = text;
		meshRenderer = (MeshRenderer)GetComponent(typeof(MeshRenderer));
		meshFilter = (MeshFilter)GetComponent(typeof(MeshFilter));
		mesh = new Mesh();
		meshFilter.mesh = mesh;
		textureWidth = 2;
		for (textureHeight = 2; textureWidth < textWidth; textureWidth <<= 1)
		{
		}
		while (textureHeight < textHeight)
		{
			textureHeight <<= 1;
		}
		mesh.Clear();
		Vector3[] vertices = new Vector3[4]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(1f, 0f, 0f),
			new Vector3(1f, -1f, 0f),
			new Vector3(0f, -1f, 0f)
		};
		Vector2[] uv = new Vector2[4]
		{
			new Vector2(0f, 1f),
			new Vector2(1f, 1f),
			new Vector2(1f, 0f),
			new Vector2(0f, 0f)
		};
		int[] triangles = new int[6] { 3, 1, 0, 2, 1, 3 };
		int i = 0;
		Vector3[] array2 = vertices;
		for (int length = array2.Length; i < length; i++)
		{
			array2[i].x = array2[i].x * ((float)textureWidth / 1024f);
			array2[i].y = array2[i].y * ((float)textureHeight / 1024f);
		}
		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
		mesh.RecalculateBounds();
	}

	public override void setColor(Color to)
	{
	}

	public override void setText(string to)
	{
		text = to;
	}
}
