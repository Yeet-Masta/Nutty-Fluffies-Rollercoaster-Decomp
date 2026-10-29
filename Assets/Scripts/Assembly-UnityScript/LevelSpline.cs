using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

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
			IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(splinePoints);
			while (enumerator.MoveNext())
			{
				Vector2 vector = (Vector2)enumerator.Current;
				list.Add(vector);
				UnityRuntimeServices.Update(enumerator, vector);
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
		int result;
		if (!base.equalsTo(other))
		{
			result = 0;
		}
		else if (spline.splineData.Count != (other as LevelSpline).spline.splineData.Count)
		{
			result = 0;
		}
		else
		{
			int num = 0;
			IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(spline.splineData);
			while (true)
			{
				if (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					if (!(obj is CSpline2.CSplinePoint))
					{
						obj = RuntimeServices.Coerce(obj, typeof(CSpline2.CSplinePoint));
					}
					CSpline2.CSplinePoint cSplinePoint = (CSpline2.CSplinePoint)obj;
					if (cSplinePoint.position != (other as LevelSpline).spline.splineData[num++].position)
					{
						result = 0;
						break;
					}
					continue;
				}
				result = 1;
				break;
			}
		}
		return (byte)result != 0;
	}

	public virtual void updatePoint(int index)
	{
		recreate();
	}

	public virtual void recreate()
	{
		if (!RuntimeServices.EqualityOperator(spline, null))
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
		int i = 0;
		Component[] componentsInChildren = GetComponentsInChildren(typeof(LevelSplineSection));
		for (int length = componentsInChildren.Length; i < length; i++)
		{
			MeshRenderer meshRenderer = ((MeshRenderer)componentsInChildren[i].gameObject.GetComponent(typeof(MeshRenderer))) as MeshRenderer;
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
		int num = 1;
		if (!CommonData.EDITOR_ACTIVE)
		{
			num = splitPts.Length + 1;
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
		graphicSections = new GameObject[num];
		graphicMeshes = new Mesh[num];
		graphicMeshQueue = new int[num];
		float num2 = 0f;
		int num3 = 0;
		for (int j = 0; j < num; j++)
		{
			GameObject gameObject = LevelObjectFactory.getInstance().createSplineSection(this.gameObject);
			int num4 = vertices.Count;
			if (!CommonData.EDITOR_ACTIVE)
			{
				num4 = (j >= num - 1 ? vertices.Count : splitPts[j]) - num3;
			}
			Mesh mesh = new Mesh();
			graphicMeshes[j] = mesh;
			MeshFilter component = gameObject.GetComponent<MeshFilter>();
			Vector3[] array2 = new Vector3[num4 * 2];
			Vector2[] array3 = new Vector2[num4 * 2];
			MeshRenderer component2 = gameObject.GetComponent<MeshRenderer>();
			component2.material = new Material(component2.material);
			component2.sharedMaterial.renderQueue = 3000 - j * 2;
			graphicMeshQueue[j] = component2.sharedMaterial.renderQueue;
			bounding = new Rect(10000f, 10000f, -10000f, -10000f);
			for (int k = num3; k < num3 + num4; k++)
			{
				int num5 = (k - num3) * 2;
				array2[num5].x = vertices[k].x;
				array2[num5].y = vertices[k].y;
				array2[num5].z = vertices[k].z + GRAPHIC_MESH_Z_OFFSET;
				array2[num5 + 1].x = vertices[k].x;
				array2[num5 + 1].y = vertices[k].y;
				array2[num5 + 1].z = vertices[k].z + GRAPHIC_MESH_Z_OFFSET;
				array3[num5].x = num2 / height;
				array3[num5].y = 1f;
				array3[num5 + 1].x = num2 / height;
				array3[num5 + 1].y = 0f;
				if (k < num3 + num4 - 1)
				{
					num2 += ((Vector2)(vertices[k + 1] - vertices[k])).magnitude;
				}
				if (!(bounding.xMin <= array2[num5].x))
				{
					bounding.xMin = array2[num5].x;
				}
				else if (!(bounding.xMax >= array2[num5].x))
				{
					bounding.xMax = array2[num5].x;
				}
				if (!(bounding.yMin <= array2[num5].y))
				{
					bounding.yMin = array2[num5].y;
				}
				else if (!(bounding.yMax >= array2[num5].y))
				{
					bounding.yMax = array2[num5].y;
				}
			}
			adjustVertices(vertices, num3, array2);
			mesh.vertices = array2;
			mesh.uv = array3;
			mesh.triangles = createTriangleIndices(num4, false);
			mesh.RecalculateBounds();
			component.mesh = mesh;
			component.sharedMesh.RecalculateBounds();
			num3 += num4 - 1;
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
			Vector3[] array = new Vector3[vertices.Count * 2];
			for (int i = 0; i < vertices.Count; i++)
			{
				int num = i * 2;
				array[num].x = vertices[i].x;
				array[num].y = vertices[i].y;
				array[num].z = 0f - COLLIDER_WIDTH + vertices[i].z;
				array[num + 1].x = vertices[i].x;
				array[num + 1].y = vertices[i].y;
				array[num + 1].z = COLLIDER_WIDTH + vertices[i].z;
			}
			collisionMesh.vertices = array;
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
		int[] array = new int[(vertexCount - 1) * 6];
		for (int i = 0; i < array.Length / 3; i += 2)
		{
			int num = i * 3;
			if (col)
			{
				array[num] = i;
				array[num + 1] = i + 1;
				array[num + 2] = i + 2;
				array[num + 3] = i + 2;
				array[num + 4] = i + 1;
				array[num + 5] = i + 3;
			}
			else
			{
				array[num] = i;
				array[num + 1] = i + 2;
				array[num + 2] = i + 1;
				array[num + 3] = i + 2;
				array[num + 4] = i + 3;
				array[num + 5] = i + 1;
			}
		}
		return array;
	}

	private void adjustVertices(List<Vector3> splinePts, int startInd, Vector3[] vertices)
	{
		for (int i = 0; i < vertices.Length / 2; i++)
		{
			if (i + startInd > 0 && i + startInd < splinePts.Count - 1)
			{
				int num = i * 2;
				Vector3 vector = splinePts[i + startInd] - splinePts[i - 1 + startInd];
				Vector3 vector2 = splinePts[i + 1 + startInd] - splinePts[i + startInd];
				float x = vector.x;
				vector.x = vector.y;
				vector.y = 0f - x;
				x = vector2.x;
				vector2.x = vector2.y;
				vector2.y = 0f - x;
				Vector3 vector3 = vector + vector2;
				vector3.Normalize();
				vertices[num + 1] = vertices[num] + vector3 * height;
			}
		}
		if (startInd == 0)
		{
			Vector3 vector4 = vertices[2] - vertices[0];
			float x2 = vector4.x;
			vector4.x = vector4.y;
			vector4.y = 0f - x2;
			vector4.Normalize();
			vertices[1] = vertices[0] + vector4 * height;
		}
		if (startInd + vertices.Length / 2 == splinePts.Count)
		{
			Vector3 vector4 = vertices[vertices.Length - 2] - vertices[vertices.Length - 4];
			float x2 = vector4.x;
			vector4.x = vector4.y;
			vector4.y = 0f - x2;
			vector4.Normalize();
			vertices[vertices.Length - 1] = vertices[vertices.Length - 2] + vector4 * height;
		}
	}

	private List<Vector3> addZDepth(List<Vector2> vertices)
	{
		List<Vector3> list = new List<Vector3>();
		float num = 0f;
		list.Add(new Vector3(vertices[0].x, vertices[0].y, num));
		for (int i = 1; i < vertices.Count - 1; i++)
		{
			Vector2 vector = vertices[i] - vertices[i - 1];
			Vector2 vector2 = vertices[i + 1] - vertices[i];
			vector.Normalize();
			vector2.Normalize();
			float num2 = (vector.x * vector2.y - vector.y * vector2.x) * DEPTH_FACTOR;
			if (!(num2 >= 0f))
			{
				num2 = 0f - num2;
			}
			num += num2;
			list.Add(new Vector3(vertices[i].x, vertices[i].y, num));
		}
		list.Add(new Vector3(vertices[vertices.Count - 1].x, vertices[vertices.Count - 1].y, num));
		return list;
	}

	private void tweakZ(List<Vector3> vertices)
	{
		Vector3 vector = vertices[vertices.Count - 1] - vertices[0];
		float x = vertices[0].x;
		for (int i = 1; i < vertices.Count; i++)
		{
			float num = vertices[i].x - x;
			float x2 = vector.x;
			float num2 = num / x2;
			Vector3 value = vertices[i];
			value.z -= vector.z * num2;
			vertices[i] = value;
		}
	}

	private bool isSelfIntersecting(List<Vector2> vertices)
	{
		float num = vertices[0].x;
		int num2 = 1;
		int result;
		while (true)
		{
			if (num2 < vertices.Count)
			{
				float x = vertices[num2].x;
				if (!(x >= num))
				{
					result = 1;
					break;
				}
				num = x;
				num2++;
				continue;
			}
			result = 0;
			break;
		}
		return (byte)result != 0;
	}

	private float getAngleDiff(float a2, float a1)
	{
		float num = a2 - a1;
		if (!(num <= 180f))
		{
			num -= 360f;
		}
		else if (!(num >= -180f))
		{
			num += 360f;
		}
		return num;
	}

	private bool angleInRange(float a, float aMin, float aMax)
	{
		bool flag;
		if (!(aMin <= aMax))
		{
			flag = !(a > aMax);
			if (!flag)
			{
				flag = !(a < aMin);
			}
		}
		else
		{
			flag = !(a < aMin);
			if (flag)
			{
				flag = !(a > aMax);
			}
		}
		return flag;
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
		float num = 0f;
		float num2 = 0f;
		float a = 0f;
		for (int i = 1; i < splinePoints.Count; i++)
		{
			Vector2 vector = splinePoints[i - 1];
			Vector2 vector2 = splinePoints[i];
			Vector2 vector3 = vector2 - vector;
			float num3 = Mathf.Atan2(vector3.y, vector3.x) * 57.29578f;
			if (!(num3 >= 0f))
			{
				num3 += 360f;
			}
			else if (!(num3 <= 360f))
			{
				num3 -= 360f;
			}
			if (i > 1)
			{
				float sectSize = getSectSize(num2, num);
				float angleDiff = getAngleDiff(num3, a);
				if (!angleInRange(num3, num2, num))
				{
					float num4 = Mathf.Abs(getAngleDiff(num3, num2));
					float num5 = Mathf.Abs(getAngleDiff(num3, num));
					if (num4 < num5 || (num4 == num5 && !(angleDiff >= 0f)))
					{
						num2 = num3;
					}
					else if (num4 > num5 || (num4 == num5 && !(angleDiff <= 0f)))
					{
						num = num3;
					}
					float sectSize2 = getSectSize(num2, num);
					if (!(sectSize2 < 180f) && !(sectSize >= 180f))
					{
						if (indices != null)
						{
							indices.Add(i);
						}
						if (pts != null)
						{
							pts.Add(vector2);
						}
						num2 = num3;
						num = num3;
					}
				}
			}
			else
			{
				num2 = num3;
				num = num3;
			}
			a = num3;
		}
	}
}
