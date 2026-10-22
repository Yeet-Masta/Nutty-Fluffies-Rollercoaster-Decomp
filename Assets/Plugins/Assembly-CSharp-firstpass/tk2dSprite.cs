using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
[ExecuteInEditMode]
[AddComponentMenu("2D Toolkit/tk2dSprite")]
public class tk2dSprite : tk2dBaseSprite
{
	private Mesh mesh;

	private Vector3[] meshVertices;

	private Color[] meshColors;

	private void Awake()
	{
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
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

	public override void Build()
	{
		tk2dSpriteDefinition tk2dSpriteDefinition2 = collection.spriteDefinitions[spriteId];
		meshVertices = new Vector3[tk2dSpriteDefinition2.positions.Length];
		meshColors = new Color[tk2dSpriteDefinition2.positions.Length];
		SetPositions(meshVertices);
		SetColors(meshColors);
		if (mesh != null)
		{
			mesh.Clear();
			mesh.vertices = meshVertices;
			mesh.colors = meshColors;
			mesh.uv = tk2dSpriteDefinition2.uvs;
			mesh.triangles = tk2dSpriteDefinition2.indices;
		}
		UpdateMaterial();
		CreateCollider();
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
		UpdateVerticesImpl();
	}

	protected void UpdateColorsImpl()
	{
		SetColors(meshColors);
		mesh.colors = meshColors;
	}

	protected void UpdateVerticesImpl()
	{
		tk2dSpriteDefinition tk2dSpriteDefinition2 = collection.spriteDefinitions[spriteId];
		SetPositions(meshVertices);
		mesh.vertices = meshVertices;
		mesh.uv = tk2dSpriteDefinition2.uvs;
		mesh.bounds = GetBounds();
	}

	protected void UpdateGeometryImpl()
	{
		tk2dSpriteDefinition tk2dSpriteDefinition2 = collection.spriteDefinitions[spriteId];
		if (mesh.vertexCount > tk2dSpriteDefinition2.positions.Length)
		{
			mesh.triangles = tk2dSpriteDefinition2.indices;
			meshVertices = new Vector3[tk2dSpriteDefinition2.positions.Length];
			meshColors = new Color[tk2dSpriteDefinition2.positions.Length];
			SetPositions(meshVertices);
			SetColors(meshColors);
			mesh.vertices = meshVertices;
			mesh.colors = meshColors;
			mesh.uv = tk2dSpriteDefinition2.uvs;
			mesh.bounds = GetBounds();
		}
		else
		{
			meshVertices = new Vector3[tk2dSpriteDefinition2.positions.Length];
			meshColors = new Color[tk2dSpriteDefinition2.positions.Length];
			SetPositions(meshVertices);
			SetColors(meshColors);
			mesh.vertices = meshVertices;
			mesh.colors = meshColors;
			mesh.uv = tk2dSpriteDefinition2.uvs;
			mesh.triangles = tk2dSpriteDefinition2.indices;
			mesh.bounds = GetBounds();
		}
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
		return meshVertices.Length;
	}
}
