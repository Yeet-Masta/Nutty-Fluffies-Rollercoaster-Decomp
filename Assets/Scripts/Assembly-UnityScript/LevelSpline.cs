using System.Collections.Generic;
using UnityEngine;

public class LevelSpline : LevelEntity
{
	public static float DEPTH_FACTOR = 0.5f;

	public static float COLLIDER_WIDTH = 0.8f;

	public static float GRAPHIC_MESH_Z_OFFSET = -0.5f;

	public static float MAX_Z_OFFSET_IN_STEP = 0.2f;

	public static readonly int SPLINE_LAYER = 1024;

	public CSpline2 spline;

	public float height;

	public Rect bounding;

	public string texture;

	private GameObject[] graphicSections;

	private Mesh[] graphicMeshes;

	private int[] graphicMeshQueue;

	private Mesh collisionMesh;

	private int[] splitPoints;

	public virtual List<Vector2> getSplinePoints(CSpline2 spline)
	{
		return spline.convertToVertices(0.002f, 0.01f, 4f);
	}

	public virtual void create(CSpline2 spline, float height)
	{
		this.height = height;
		List<Vector2> splinePoints = getSplinePoints(spline);
		List<Vector3> list = null;
		if (!spline.testSelfIntersection())
		{
			list = new List<Vector3>();
			foreach (Vector2 vector in splinePoints)
			{
				list.Add(vector);
			}
		}
		else
		{
			list = addZDepth(splinePoints);
			tweakZ(list);
		}
		List<int> list2 = new List<int>();
		calculateSplitPoints(spline, list2, null);
		splitPoints = list2.ToArray();
		createGraphicMeshes(list, splitPoints, height);
		createCollisioMesh(list);
	}

	public virtual int getRenderQueueLevel(int pointIndex)
	{
		int i;
		for (i = 0; i < splitPoints.Length && pointIndex > splitPoints[i]; i++)
		{
		}
		return graphicMeshQueue[i];
	}

	public override bool equalsTo(LevelEntity other)
	{
		if (!base.equalsTo(other))
		{
			return false;
		}
		if (spline.splineData.Count != (other as LevelSpline).spline.splineData.Count)
		{
			return false;
		}
		int index = 0;
		foreach (CSpline2.CSplinePoint cSplinePoint in spline.splineData)
		{
			if (cSplinePoint.position != (other as LevelSpline).spline.splineData[index++].position)
			{
				return false;
			}
		}
		return true;
	}

	public virtual void updatePoint(int index)
	{
		recreate();
	}

	public virtual void recreate()
	{
		if (spline != null)
		{
			create(spline, height);
		}
	}

	public virtual void setTexture(string tex)
	{
		texture = tex;
		TextureManager.UV uvs = new TextureManager.UV();
		Texture2D texture2D = TextureManager.getInstance().createTexture(FileManager.getInstance().locateTexture(tex, TextureManager.TextureType.SPLINE), uvs);
		texture2D.wrapMode = TextureWrapMode.Repeat;
		foreach (Component component in GetComponentsInChildren(typeof(LevelSplineSection)))
		{
			MeshRenderer meshRenderer = ((MeshRenderer)component.gameObject.GetComponent(typeof(MeshRenderer))) as MeshRenderer;
			meshRenderer.material.mainTexture = texture2D;
			meshRenderer.material.SetColor("_MainColor", new Color(0.5f, 0.5f, 0.5f));
		}
	}

	public virtual string getTexture()
	{
		return texture;
	}

	public virtual Vector3[] getGraphicMeshVertices()
	{
		if (!CommonData.EDITOR_ACTIVE)
		{
			Debug.LogError("getGraphicMeshVertices should only be called in editor!!!");
		}
		return graphicMeshes[0].vertices;
	}

	public virtual Vector3[] getCollisionMeshVertices()
	{
		return collisionMesh.vertices;
	}

	private void createGraphicMeshes(List<Vector3> vertices, int[] splitPts, float height)
	{
		int sectionCount = 1;
		if (!CommonData.EDITOR_ACTIVE)
		{
			sectionCount = splitPts.Length + 1;
		}
		else
		{
			Component[] componentsInChildren = transform.GetComponentsInChildren(typeof(LevelSplineSection));
			int i = 0;
			Component[] array = componentsInChildren;
			for (int length = array.Length; i < length; i++)
			{
				if (array[i].gameObject != this)
				{
					UnityEngine.Object.Destroy(array[i].gameObject);
				}
			}
		}
		graphicSections = new GameObject[sectionCount];
		graphicMeshes = new Mesh[sectionCount];
		graphicMeshQueue = new int[sectionCount];
		float distance = 0f;
		int start = 0;
		for (int j = 0; j < sectionCount; j++)
		{
			GameObject gameObject = LevelObjectFactory.getInstance().createSplineSection(this.gameObject);
			int count = vertices.Count;
			if (!CommonData.EDITOR_ACTIVE)
			{
				count = (j >= sectionCount - 1 ? vertices.Count : splitPts[j]) - start;
			}
			Mesh mesh = new Mesh();
			graphicMeshes[j] = mesh;
			MeshFilter component = gameObject.GetComponent<MeshFilter>();
			Vector3[] meshVertices = new Vector3[count * 2];
			Vector2[] meshUvs = new Vector2[count * 2];
			MeshRenderer component2 = gameObject.GetComponent<MeshRenderer>();
			component2.material = new Material(component2.material);
			component2.sharedMaterial.renderQueue = 3000 - j * 2;
			graphicMeshQueue[j] = component2.sharedMaterial.renderQueue;
			bounding = new Rect(10000f, 10000f, -10000f, -10000f);
			for (int k = start; k < start + count; k++)
			{
				int vertexIndex = (k - start) * 2;
				meshVertices[vertexIndex].x = vertices[k].x;
				meshVertices[vertexIndex].y = vertices[k].y;
				meshVertices[vertexIndex].z = vertices[k].z + GRAPHIC_MESH_Z_OFFSET;
				meshVertices[vertexIndex + 1].x = vertices[k].x;
				meshVertices[vertexIndex + 1].y = vertices[k].y;
				meshVertices[vertexIndex + 1].z = vertices[k].z + GRAPHIC_MESH_Z_OFFSET;
				meshUvs[vertexIndex].x = distance / height;
				meshUvs[vertexIndex].y = 1f;
				meshUvs[vertexIndex + 1].x = distance / height;
				meshUvs[vertexIndex + 1].y = 0f;
				if (k < start + count - 1)
				{
					distance += ((Vector2)(vertices[k + 1] - vertices[k])).magnitude;
				}
				if (!(bounding.xMin <= meshVertices[vertexIndex].x))
				{
					bounding.xMin = meshVertices[vertexIndex].x;
				}
				else if (!(bounding.xMax >= meshVertices[vertexIndex].x))
				{
					bounding.xMax = meshVertices[vertexIndex].x;
				}
				if (!(bounding.yMin <= meshVertices[vertexIndex].y))
				{
					bounding.yMin = meshVertices[vertexIndex].y;
				}
				else if (!(bounding.yMax >= meshVertices[vertexIndex].y))
				{
					bounding.yMax = meshVertices[vertexIndex].y;
				}
			}
			adjustVertices(vertices, start, meshVertices);
			mesh.vertices = meshVertices;
			mesh.uv = meshUvs;
			mesh.triangles = createTriangleIndices(count, false);
			mesh.RecalculateBounds();
			component.mesh = mesh;
			component.sharedMesh.RecalculateBounds();
			start += count - 1;
		}
		if (!string.IsNullOrEmpty(texture) && texture.Length > 0)
		{
			setTexture(texture);
		}
	}

	private void createCollisioMesh(List<Vector3> vertices)
	{
		MeshCollider component = gameObject.GetComponent<MeshCollider>();
		if (!CommonData.EDITOR_ACTIVE)
		{
			if (collisionMesh == null)
			{
				collisionMesh = new Mesh();
			}
			else
			{
				collisionMesh.Clear();
			}
			Vector3[] colliderVertices = new Vector3[vertices.Count * 2];
			for (int i = 0; i < vertices.Count; i++)
			{
				int index = i * 2;
				colliderVertices[index].x = vertices[i].x;
				colliderVertices[index].y = vertices[i].y;
				colliderVertices[index].z = 0f - COLLIDER_WIDTH + vertices[i].z;
				colliderVertices[index + 1].x = vertices[i].x;
				colliderVertices[index + 1].y = vertices[i].y;
				colliderVertices[index + 1].z = COLLIDER_WIDTH + vertices[i].z;
			}
			collisionMesh.vertices = colliderVertices;
			collisionMesh.triangles = createTriangleIndices(vertices.Count, true);
		}
		else
		{
			collisionMesh = graphicMeshes[0];
		}
		component.sharedMesh = null;
		component.sharedMesh = collisionMesh;
	}

	private int[] createTriangleIndices(int vertexCount, bool col)
	{
		int[] indices = new int[(vertexCount - 1) * 6];
		for (int i = 0; i < indices.Length / 3; i += 2)
		{
			int index = i * 3;
			if (col)
			{
				indices[index] = i;
				indices[index + 1] = i + 1;
				indices[index + 2] = i + 2;
				indices[index + 3] = i + 2;
				indices[index + 4] = i + 1;
				indices[index + 5] = i + 3;
			}
			else
			{
				indices[index] = i;
				indices[index + 1] = i + 2;
				indices[index + 2] = i + 1;
				indices[index + 3] = i + 2;
				indices[index + 4] = i + 3;
				indices[index + 5] = i + 1;
			}
		}
		return indices;
	}

	private void adjustVertices(List<Vector3> splinePts, int startInd, Vector3[] vertices)
	{
		for (int i = 0; i < vertices.Length / 2; i++)
		{
			if (i + startInd > 0 && i + startInd < splinePts.Count - 1)
			{
				int index = i * 2;
				Vector3 normalIn = splinePts[i + startInd] - splinePts[i - 1 + startInd];
				Vector3 normalOut = splinePts[i + 1 + startInd] - splinePts[i + startInd];
				float x = normalIn.x;
				normalIn.x = normalIn.y;
				normalIn.y = 0f - x;
				x = normalOut.x;
				normalOut.x = normalOut.y;
				normalOut.y = 0f - x;
				Vector3 normal = normalIn + normalOut;
				normal.Normalize();
				vertices[index + 1] = vertices[index] + normal * height;
			}
		}
		if (startInd == 0)
		{
			Vector3 endNormal = vertices[2] - vertices[0];
			float x2 = endNormal.x;
			endNormal.x = endNormal.y;
			endNormal.y = 0f - x2;
			endNormal.Normalize();
			vertices[1] = vertices[0] + endNormal * height;
		}
		if (startInd + vertices.Length / 2 == splinePts.Count)
		{
			Vector3 endNormal = vertices[vertices.Length - 2] - vertices[vertices.Length - 4];
			float x2 = endNormal.x;
			endNormal.x = endNormal.y;
			endNormal.y = 0f - x2;
			endNormal.Normalize();
			vertices[vertices.Length - 1] = vertices[vertices.Length - 2] + endNormal * height;
		}
	}

	private List<Vector3> addZDepth(List<Vector2> vertices)
	{
		List<Vector3> list = new List<Vector3>();
		float z = 0f;
		list.Add(new Vector3(vertices[0].x, vertices[0].y, z));
		for (int i = 1; i < vertices.Count - 1; i++)
		{
			Vector2 dirIn = vertices[i] - vertices[i - 1];
			Vector2 dirOut = vertices[i + 1] - vertices[i];
			dirIn.Normalize();
			dirOut.Normalize();
			float zStep = (dirIn.x * dirOut.y - dirIn.y * dirOut.x) * DEPTH_FACTOR;
			if (!(zStep >= 0f))
			{
				zStep = 0f - zStep;
			}
			z += zStep;
			list.Add(new Vector3(vertices[i].x, vertices[i].y, z));
		}
		list.Add(new Vector3(vertices[vertices.Count - 1].x, vertices[vertices.Count - 1].y, z));
		return list;
	}

	private void tweakZ(List<Vector3> vertices)
	{
		Vector3 span = vertices[vertices.Count - 1] - vertices[0];
		float x = vertices[0].x;
		for (int i = 1; i < vertices.Count; i++)
		{
			float dx = vertices[i].x - x;
			float x2 = span.x;
			float fraction = dx / x2;
			Vector3 value = vertices[i];
			value.z -= span.z * fraction;
			vertices[i] = value;
		}
	}

	private bool isSelfIntersecting(List<Vector2> vertices)
	{
		float minX = vertices[0].x;
		int index = 1;
		while (index < vertices.Count)
		{
			float x = vertices[index].x;
			if (!(x >= minX))
			{
				return true;
			}
			minX = x;
			index++;
		}
		return false;
	}

	private float getAngleDiff(float a2, float a1)
	{
		float diff = a2 - a1;
		if (!(diff <= 180f))
		{
			diff -= 360f;
		}
		else if (!(diff >= -180f))
		{
			diff += 360f;
		}
		return diff;
	}

	private bool angleInRange(float a, float aMin, float aMax)
	{
		bool inRange;
		if (!(aMin <= aMax))
		{
			inRange = !(a > aMax) || !(a < aMin);
		}
		else
		{
			inRange = !(a < aMin) && !(a > aMax);
		}
		return inRange;
	}

	private float getSectSize(float minAngle, float maxAngle)
	{
		return minAngle <= maxAngle ? (maxAngle - minAngle) : (maxAngle + (360f - minAngle));
	}

	public virtual void calculateSplitPoints(CSpline2 spl, List<int> indices, List<Vector2> pts)
	{
		if (indices != null)
		{
			indices.Clear();
		}
		if (pts != null)
		{
			pts.Clear();
		}
		List<Vector2> splinePoints = getSplinePoints(spl);
		float maxAngle = 0f;
		float minAngle = 0f;
		float a = 0f;
		for (int i = 1; i < splinePoints.Count; i++)
		{
			Vector2 prev = splinePoints[i - 1];
			Vector2 point = splinePoints[i];
			Vector2 delta = point - prev;
			float angle = Mathf.Atan2(delta.y, delta.x) * 57.29578f;
			if (!(angle >= 0f))
			{
				angle += 360f;
			}
			else if (!(angle <= 360f))
			{
				angle -= 360f;
			}
			if (i > 1)
			{
				float sectSize = getSectSize(minAngle, maxAngle);
				float angleDiff = getAngleDiff(angle, a);
				if (!angleInRange(angle, minAngle, maxAngle))
				{
					float diffToMin = Mathf.Abs(getAngleDiff(angle, minAngle));
					float diffToMax = Mathf.Abs(getAngleDiff(angle, maxAngle));
					if (diffToMin < diffToMax || (diffToMin == diffToMax && !(angleDiff >= 0f)))
					{
						minAngle = angle;
					}
					else if (diffToMin > diffToMax || (diffToMin == diffToMax && !(angleDiff <= 0f)))
					{
						maxAngle = angle;
					}
					if (!(getSectSize(minAngle, maxAngle) < 180f) && !(sectSize >= 180f))
					{
						if (indices != null)
						{
							indices.Add(i);
						}
						if (pts != null)
						{
							pts.Add(point);
						}
						minAngle = angle;
						maxAngle = angle;
					}
				}
			}
			else
			{
				minAngle = angle;
				maxAngle = angle;
			}
			a = angle;
		}
	}
}
