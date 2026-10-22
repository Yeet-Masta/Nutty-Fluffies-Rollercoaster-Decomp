using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[AddComponentMenu("2D Toolkit/tk2dStaticSpriteBatcher")]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class tk2dStaticSpriteBatcher : MonoBehaviour
{
	public tk2dBatchedSprite[] batchedSprites;

	public tk2dSpriteCollectionData spriteCollection;

	private Mesh mesh;

	private Mesh colliderMesh;

	private void Awake()
	{
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
		Build();
	}

	public void Build()
	{
		mesh.Clear();
		if ((bool)colliderMesh)
		{
			Object.Destroy(colliderMesh);
			colliderMesh = null;
		}
		if (!spriteCollection || batchedSprites == null || batchedSprites.Length == 0)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		tk2dBatchedSprite[] array = batchedSprites;
		foreach (tk2dBatchedSprite tk2dBatchedSprite2 in array)
		{
			tk2dSpriteDefinition tk2dSpriteDefinition2 = spriteCollection.spriteDefinitions[tk2dBatchedSprite2.spriteId];
			num += tk2dSpriteDefinition2.positions.Length;
			num2 += tk2dSpriteDefinition2.indices.Length;
		}
		Vector3[] array2 = new Vector3[num];
		Color[] array3 = new Color[num];
		Vector2[] array4 = new Vector2[num];
		int[] array5 = new int[num2];
		int num3 = 0;
		int num4 = 0;
		tk2dBatchedSprite[] array6 = batchedSprites;
		foreach (tk2dBatchedSprite tk2dBatchedSprite3 in array6)
		{
			tk2dSpriteDefinition tk2dSpriteDefinition3 = spriteCollection.spriteDefinitions[tk2dBatchedSprite3.spriteId];
			for (int k = 0; k < tk2dSpriteDefinition3.indices.Length; k++)
			{
				array5[num4 + k] = num3 + tk2dSpriteDefinition3.indices[k];
			}
			for (int l = 0; l < tk2dSpriteDefinition3.positions.Length; l++)
			{
				Vector3 vector = new Vector3(tk2dSpriteDefinition3.positions[l].x * tk2dBatchedSprite3.localScale.x, tk2dSpriteDefinition3.positions[l].y * tk2dBatchedSprite3.localScale.y, tk2dSpriteDefinition3.positions[l].z * tk2dBatchedSprite3.localScale.z);
				vector = tk2dBatchedSprite3.rotation * vector;
				vector += tk2dBatchedSprite3.position;
				array2[num3 + l] = vector;
				array4[num3 + l] = tk2dSpriteDefinition3.uvs[l];
				array3[num3 + l] = tk2dBatchedSprite3.color;
			}
			num4 += tk2dSpriteDefinition3.indices.Length;
			num3 += tk2dSpriteDefinition3.positions.Length;
		}
		mesh.vertices = array2;
		mesh.uv = array4;
		mesh.colors = array3;
		mesh.triangles = array5;
		mesh.RecalculateBounds();
		if (GetComponent<Renderer>().sharedMaterial != spriteCollection.materials[0])
		{
			GetComponent<Renderer>().material = spriteCollection.materials[0];
		}
		BuildPhysicsMesh();
	}

	private void BuildPhysicsMesh()
	{
		MeshCollider meshCollider = GetComponent<MeshCollider>();
		if (meshCollider != null && GetComponent<Collider>() != meshCollider)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		tk2dBatchedSprite[] array = batchedSprites;
		foreach (tk2dBatchedSprite tk2dBatchedSprite2 in array)
		{
			tk2dSpriteDefinition tk2dSpriteDefinition2 = spriteCollection.spriteDefinitions[tk2dBatchedSprite2.spriteId];
			if (tk2dSpriteDefinition2.colliderType == tk2dSpriteDefinition.ColliderType.Box)
			{
				num += 24;
				num2 += 8;
			}
			else if (tk2dSpriteDefinition2.colliderType == tk2dSpriteDefinition.ColliderType.Mesh)
			{
				num += tk2dSpriteDefinition2.colliderIndicesFwd.Length;
				num2 += tk2dSpriteDefinition2.colliderVertices.Length;
			}
		}
		if (num == 0)
		{
			if ((bool)colliderMesh)
			{
				Object.Destroy(colliderMesh);
			}
			return;
		}
		if (meshCollider == null)
		{
			meshCollider = gameObject.AddComponent<MeshCollider>();
		}
		if (colliderMesh == null)
		{
			colliderMesh = new Mesh();
		}
		colliderMesh.Clear();
		int num3 = 0;
		Vector3[] array2 = new Vector3[num2];
		int num4 = 0;
		int[] array3 = new int[num];
		tk2dBatchedSprite[] array4 = batchedSprites;
		foreach (tk2dBatchedSprite tk2dBatchedSprite3 in array4)
		{
			tk2dSpriteDefinition tk2dSpriteDefinition3 = spriteCollection.spriteDefinitions[tk2dBatchedSprite3.spriteId];
			if (tk2dSpriteDefinition3.colliderType == tk2dSpriteDefinition.ColliderType.Box)
			{
				Vector3 vector = new Vector3(tk2dSpriteDefinition3.colliderVertices[0].x * tk2dBatchedSprite3.localScale.x, tk2dSpriteDefinition3.colliderVertices[0].y * tk2dBatchedSprite3.localScale.y, tk2dSpriteDefinition3.colliderVertices[0].z * tk2dBatchedSprite3.localScale.z);
				Vector3 vector2 = new Vector3(tk2dSpriteDefinition3.colliderVertices[1].x * tk2dBatchedSprite3.localScale.x, tk2dSpriteDefinition3.colliderVertices[1].y * tk2dBatchedSprite3.localScale.y, tk2dSpriteDefinition3.colliderVertices[1].z * tk2dBatchedSprite3.localScale.z);
				Vector3 vector3 = vector - vector2;
				Vector3 vector4 = vector + vector2;
				array2[num3] = tk2dBatchedSprite3.rotation * new Vector3(vector3.x, vector3.y, vector3.z) + tk2dBatchedSprite3.position;
				array2[num3 + 1] = tk2dBatchedSprite3.rotation * new Vector3(vector3.x, vector3.y, vector4.z) + tk2dBatchedSprite3.position;
				array2[num3 + 2] = tk2dBatchedSprite3.rotation * new Vector3(vector4.x, vector3.y, vector3.z) + tk2dBatchedSprite3.position;
				array2[num3 + 3] = tk2dBatchedSprite3.rotation * new Vector3(vector4.x, vector3.y, vector4.z) + tk2dBatchedSprite3.position;
				array2[num3 + 4] = tk2dBatchedSprite3.rotation * new Vector3(vector3.x, vector4.y, vector3.z) + tk2dBatchedSprite3.position;
				array2[num3 + 5] = tk2dBatchedSprite3.rotation * new Vector3(vector3.x, vector4.y, vector4.z) + tk2dBatchedSprite3.position;
				array2[num3 + 6] = tk2dBatchedSprite3.rotation * new Vector3(vector4.x, vector4.y, vector3.z) + tk2dBatchedSprite3.position;
				array2[num3 + 7] = tk2dBatchedSprite3.rotation * new Vector3(vector4.x, vector4.y, vector4.z) + tk2dBatchedSprite3.position;
				int[] array5 = new int[24]
				{
					0, 1, 2, 2, 1, 3, 6, 5, 4, 7,
					5, 6, 3, 7, 6, 2, 3, 6, 4, 5,
					1, 4, 1, 0
				};
				int[] array6 = new int[24]
				{
					2, 1, 0, 3, 1, 2, 4, 5, 6, 6,
					5, 7, 6, 7, 3, 6, 3, 2, 1, 5,
					4, 0, 1, 4
				};
				float num5 = tk2dBatchedSprite3.localScale.x * tk2dBatchedSprite3.localScale.y * tk2dBatchedSprite3.localScale.z;
				int[] array7 = ((!(num5 >= 0f)) ? array5 : array6);
				for (int k = 0; k < array7.Length; k++)
				{
					array3[num4 + k] = num3 + array7[k];
				}
				num4 += 24;
				num3 += 8;
			}
			else if (tk2dSpriteDefinition3.colliderType == tk2dSpriteDefinition.ColliderType.Mesh)
			{
				for (int l = 0; l < tk2dSpriteDefinition3.colliderVertices.Length; l++)
				{
					Vector3 vector5 = new Vector3(tk2dSpriteDefinition3.colliderVertices[l].x * tk2dBatchedSprite3.localScale.x, tk2dSpriteDefinition3.colliderVertices[l].y * tk2dBatchedSprite3.localScale.y, tk2dSpriteDefinition3.colliderVertices[l].z * tk2dBatchedSprite3.localScale.z);
					vector5 = tk2dBatchedSprite3.rotation * vector5;
					vector5 += tk2dBatchedSprite3.position;
					array2[num3 + l] = vector5;
				}
				float num6 = tk2dBatchedSprite3.localScale.x * tk2dBatchedSprite3.localScale.y * tk2dBatchedSprite3.localScale.z;
				int[] array8 = ((!(num6 >= 0f)) ? tk2dSpriteDefinition3.colliderIndicesBack : tk2dSpriteDefinition3.colliderIndicesFwd);
				for (int m = 0; m < array8.Length; m++)
				{
					array3[num4 + m] = num3 + array8[m];
				}
				num4 += tk2dSpriteDefinition3.colliderIndicesFwd.Length;
				num3 += tk2dSpriteDefinition3.colliderVertices.Length;
			}
		}
		colliderMesh.vertices = array2;
		colliderMesh.triangles = array3;
		meshCollider.sharedMesh = colliderMesh;
	}
}
