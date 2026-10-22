using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[AddComponentMenu("2D Toolkit/tk2d9SliceSprite")]
[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class tk2dSlicedSprite : tk2dBaseSprite
{
	private Mesh mesh;

	private Vector2[] meshUvs;

	private Vector3[] meshVertices;

	private Color[] meshColors;

	private int[] meshIndices;

	public float borderTop = 0.2f;

	public float borderBottom = 0.2f;

	public float borderLeft = 0.2f;

	public float borderRight = 0.2f;

	private void Awake()
	{
		if ((bool)collection)
		{
			if (_spriteId < 0 || _spriteId >= collection.Count)
			{
				_spriteId = 0;
			}
			Build();
		}
	}

	protected void OnDestroy()
	{
		if ((bool)mesh)
		{
			NFPortCompat.DestroySafe(mesh);
		}
	}

	protected new void SetColors(Color[] dest)
	{
		Color color = _color;
		if (collection.premultipliedAlpha)
		{
			color.r *= color.a;
			color.g *= color.a;
			color.b *= color.a;
		}
		for (int i = 0; i < dest.Length; i++)
		{
			dest[i] = color;
		}
	}

	protected void SetGeometry(Vector3[] vertices, Vector2[] uvs)
	{
		tk2dSpriteDefinition tk2dSpriteDefinition2 = collection.spriteDefinitions[spriteId];
		if (tk2dSpriteDefinition2.positions.Length == 4)
		{
			float x = _scale.x;
			float y = _scale.y;
			Vector3[] positions = tk2dSpriteDefinition2.positions;
			Vector3 vector = positions[1] - positions[0];
			Vector3 vector2 = positions[2] - positions[0];
			Vector2[] uvs2 = tk2dSpriteDefinition2.uvs;
			Vector2 vector3 = tk2dSpriteDefinition2.uvs[1] - tk2dSpriteDefinition2.uvs[0];
			Vector2 vector4 = tk2dSpriteDefinition2.uvs[2] - tk2dSpriteDefinition2.uvs[0];
			Vector3 vector5 = new Vector3(positions[0].x * x, positions[0].y * y, positions[0].z * _scale.z);
			Vector3[] array = new Vector3[4]
			{
				vector5,
				vector5 + vector2 * borderBottom,
				vector5 + vector2 * (y - borderTop),
				vector5 + vector2 * y
			};
			Vector2[] array2 = new Vector2[4]
			{
				uvs2[0],
				uvs2[0] + vector4 * borderBottom,
				uvs2[0] + vector4 * (1f - borderTop),
				uvs2[0] + vector4
			};
			for (int i = 0; i < 4; i++)
			{
				meshVertices[i * 4] = array[i];
				meshVertices[i * 4 + 1] = array[i] + vector * borderLeft;
				meshVertices[i * 4 + 2] = array[i] + vector * (x - borderRight);
				meshVertices[i * 4 + 3] = array[i] + vector * x;
				meshUvs[i * 4] = array2[i];
				meshUvs[i * 4 + 1] = array2[i] + vector3 * borderLeft;
				meshUvs[i * 4 + 2] = array2[i] + vector3 * (1f - borderRight);
				meshUvs[i * 4 + 3] = array2[i] + vector3;
			}
		}
		else
		{
			for (int j = 0; j < vertices.Length; j++)
			{
				vertices[j] = Vector3.zero;
			}
		}
	}

	private void SetIndices()
	{
		meshIndices = new int[54]
		{
			0, 4, 1, 1, 4, 5, 1, 5, 2, 2,
			5, 6, 2, 6, 3, 3, 6, 7, 4, 8,
			5, 5, 8, 9, 5, 9, 6, 6, 9, 10,
			6, 10, 7, 7, 10, 11, 8, 12, 9, 9,
			12, 13, 9, 13, 10, 10, 13, 14, 10, 14,
			11, 11, 14, 15
		};
	}

	public override void Build()
	{
		meshUvs = new Vector2[16];
		meshVertices = new Vector3[16];
		meshColors = new Color[16];
		SetIndices();
		SetGeometry(meshVertices, meshUvs);
		SetColors(meshColors);
		Mesh mesh = new Mesh();
		mesh.vertices = meshVertices;
		mesh.colors = meshColors;
		mesh.uv = meshUvs;
		mesh.triangles = meshIndices;
		GetComponent<MeshFilter>().mesh = mesh;
		this.mesh = GetComponent<MeshFilter>().sharedMesh;
		UpdateMaterial();
	}

	protected override void UpdateGeometry()
	{
		UpdateGeometryImpl();
	}

	protected override void UpdateColors()
	{
		UpdateColorsImpl();
	}

	protected override void UpdateVertices()
	{
		UpdateGeometryImpl();
	}

	protected void UpdateColorsImpl()
	{
		SetColors(meshColors);
		mesh.colors = meshColors;
	}

	protected void UpdateGeometryImpl()
	{
		SetGeometry(meshVertices, meshUvs);
		mesh.vertices = meshVertices;
		mesh.uv = meshUvs;
		mesh.RecalculateBounds();
	}

	protected override void UpdateMaterial()
	{
		if (GetComponent<Renderer>().sharedMaterial != collection.spriteDefinitions[spriteId].material)
		{
			GetComponent<Renderer>().material = collection.spriteDefinitions[spriteId].material;
		}
	}

	protected override int GetCurrentVertexCount()
	{
		return 16;
	}
}
