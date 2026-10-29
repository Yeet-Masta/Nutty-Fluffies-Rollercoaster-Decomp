using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelLayer : MonoBehaviour
{
	[Serializable]
	public class MeshGatherEntry
	{
		public List<Vector3[]> pts = new List<Vector3[]>();

		public List<Vector2[]> uvs = new List<Vector2[]>();

		public List<int[]> inds = new List<int[]>();

		public List<float> zs = new List<float>();

		public int queue;
	}

	public const int FLAG_DISABLE_SCALE = 1;

	public int referenceId = -1;

	public int layer;

	public Vector2 parallaxFactor = Vector2.one;

	public string layerName;

	public Vector3 colorMultiplier = Vector3.one;

	public int layerFlags;

	public int deviceList;

	private Dictionary<TextureIntKey, MeshGatherEntry> gatherTable = new Dictionary<TextureIntKey, MeshGatherEntry>();

	public virtual void addTextureMesh(Texture tex, int zDepth, float zAdd, int queue, Vector3[] pts, Vector2[] uvs, int[] inds)
	{
		TextureIntKey key = new TextureIntKey((Texture2D)tex, zDepth);
		MeshGatherEntry meshGatherEntry = null;
		if (gatherTable.ContainsKey(key))
		{
			meshGatherEntry = gatherTable[key];
		}
		else
		{
			meshGatherEntry = new MeshGatherEntry();
			gatherTable[key] = meshGatherEntry;
		}
		meshGatherEntry.pts.Add(pts);
		meshGatherEntry.uvs.Add(uvs);
		meshGatherEntry.inds.Add(inds);
		meshGatherEntry.zs.Add(zAdd);
		meshGatherEntry.queue = queue;
	}

	public virtual void createGatheredMesh()
	{
		foreach (TextureIntKey textureIntKey in gatherTable.Keys)
		{
			Texture texture = textureIntKey.texture;
			int val = textureIntKey.val;
			MeshGatherEntry meshGatherEntry = gatherTable[textureIntKey];
			for (int i = 0; i < meshGatherEntry.pts.Count; i++)
			{
				for (int j = 0; j < meshGatherEntry.pts.Count; j++)
				{
					if (!(meshGatherEntry.zs[i] <= meshGatherEntry.zs[j]))
					{
						Vector3[] value = meshGatherEntry.pts[i];
						Vector2[] value2 = meshGatherEntry.uvs[i];
						int[] value3 = meshGatherEntry.inds[i];
						float value4 = meshGatherEntry.zs[i];
						meshGatherEntry.pts[i] = meshGatherEntry.pts[j];
						meshGatherEntry.uvs[i] = meshGatherEntry.uvs[j];
						meshGatherEntry.inds[i] = meshGatherEntry.inds[j];
						meshGatherEntry.zs[i] = meshGatherEntry.zs[j];
						meshGatherEntry.pts[j] = value;
						meshGatherEntry.uvs[j] = value2;
						meshGatherEntry.inds[j] = value3;
						meshGatherEntry.zs[j] = value4;
					}
				}
			}
			Vector3[] vertices = new Vector3[meshGatherEntry.pts.Count * 4];
			Vector2[] uvs = new Vector2[meshGatherEntry.uvs.Count * 4];
			int[] triangles = new int[meshGatherEntry.inds.Count * 6];
			int index = 0;
			foreach (Vector3[] points in meshGatherEntry.pts)
			{
				foreach (Vector3 point in points)
				{
					vertices[index++] = point;
				}
			}
			index = 0;
			foreach (Vector2[] uvSet in meshGatherEntry.uvs)
			{
				foreach (Vector2 uv in uvSet)
				{
					uvs[index++] = uv;
				}
			}
			index = 0;
			int vertexOffset = 0;
			foreach (int[] indices in meshGatherEntry.inds)
			{
				foreach (int vertexIndex in indices)
				{
					triangles[index++] = vertexIndex + vertexOffset;
				}
				vertexOffset += 4;
			}
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(LevelObjectFactory.getInstance().getPrefabs().objectTexturePrefab, Vector3.zero, Quaternion.identity);
			gameObject.transform.parent = transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.name = "GatheredMesh (layer " + layer + ", texture '" + texture.name + "')";
			gameObject.GetComponent<Renderer>().sharedMaterial = MaterialManager.getInstance().getMaterialForUniqueID(texture as Texture2D, layer, true);
			gameObject.GetComponent<Renderer>().sharedMaterial.renderQueue = meshGatherEntry.queue;
			Mesh mesh = new Mesh();
			mesh.vertices = vertices;
			mesh.uv = uvs;
			mesh.triangles = triangles;
			(((MeshFilter)gameObject.GetComponent(typeof(MeshFilter))) as MeshFilter).mesh = mesh;
			float z = 1f * (float)val;
			Vector3 localPosition = gameObject.transform.localPosition;
			localPosition.z = z;
			gameObject.transform.localPosition = localPosition;
		}
		gatherTable.Clear();
	}

	public virtual bool checkDevice(int dev)
	{
		return (deviceList & dev) != 0;
	}

	public virtual void setDevice(int dev, bool state)
	{
		deviceList &= ~dev;
		if (state)
		{
			deviceList |= dev;
		}
	}

	public virtual string getDevicesDesc()
	{
		string desc = string.Empty;
		for (int i = 0; i < Enum.GetValues(typeof(CommonData.DeviceGeneration)).Length; i++)
		{
			desc = checkDevice(1 << i) ? (desc + "- ") : (desc + (CommonData.deviceChars[i] + " "));
		}
		return desc;
	}

	public virtual void setFlag(int flag, bool state)
	{
		layerFlags &= ~flag;
		if (state)
		{
			layerFlags |= flag;
		}
	}

	public virtual bool checkFlag(int flag)
	{
		return (layerFlags & flag) != 0;
	}

	public virtual void updateColor()
	{
		gameObject.transform.GetComponentsInChildren(typeof(Renderer), true);
	}
}
