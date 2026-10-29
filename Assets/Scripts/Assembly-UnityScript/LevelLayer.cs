using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

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
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(gatherTable.Keys);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is TextureIntKey))
			{
				obj = RuntimeServices.Coerce(obj, typeof(TextureIntKey));
			}
			TextureIntKey textureIntKey = (TextureIntKey)obj;
			Texture texture = textureIntKey.texture;
			UnityRuntimeServices.Update(enumerator, textureIntKey);
			int val = textureIntKey.val;
			UnityRuntimeServices.Update(enumerator, textureIntKey);
			MeshGatherEntry meshGatherEntry = gatherTable[textureIntKey];
			UnityRuntimeServices.Update(enumerator, textureIntKey);
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
			Vector3[] array = new Vector3[meshGatherEntry.pts.Count * 4];
			Vector2[] array2 = new Vector2[meshGatherEntry.uvs.Count * 4];
			int[] array3 = new int[meshGatherEntry.inds.Count * 6];
			int num = 0;
			IEnumerator enumerator2 = UnityRuntimeServices.GetEnumerator(meshGatherEntry.pts);
			while (enumerator2.MoveNext())
			{
				object obj2 = enumerator2.Current;
				if (!(obj2 is Vector3[]))
				{
					obj2 = RuntimeServices.Coerce(obj2, typeof(Vector3[]));
				}
				Vector3[] array4 = (Vector3[])obj2;
				int k = 0;
				Vector3[] array5 = array4;
				for (int length = array5.Length; k < length; k++)
				{
					array[num++] = array5[k];
				}
			}
			num = 0;
			IEnumerator enumerator3 = UnityRuntimeServices.GetEnumerator(meshGatherEntry.uvs);
			while (enumerator3.MoveNext())
			{
				object obj3 = enumerator3.Current;
				if (!(obj3 is Vector2[]))
				{
					obj3 = RuntimeServices.Coerce(obj3, typeof(Vector2[]));
				}
				Vector2[] array6 = (Vector2[])obj3;
				int l = 0;
				Vector2[] array7 = array6;
				for (int length2 = array7.Length; l < length2; l++)
				{
					array2[num++] = array7[l];
				}
			}
			num = 0;
			int num2 = 0;
			IEnumerator enumerator4 = UnityRuntimeServices.GetEnumerator(meshGatherEntry.inds);
			while (enumerator4.MoveNext())
			{
				object obj4 = enumerator4.Current;
				if (!(obj4 is int[]))
				{
					obj4 = RuntimeServices.Coerce(obj4, typeof(int[]));
				}
				int[] array8 = (int[])obj4;
				int m = 0;
				int[] array9 = array8;
				for (int length3 = array9.Length; m < length3; m++)
				{
					array3[num++] = array9[m] + num2;
				}
				num2 += 4;
			}
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(LevelObjectFactory.getInstance().getPrefabs().objectTexturePrefab, Vector3.zero, Quaternion.identity);
			gameObject.transform.parent = transform;
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.name = "GatheredMesh (layer " + layer + ", texture '" + texture.name + "')";
			gameObject.GetComponent<Renderer>().sharedMaterial = MaterialManager.getInstance().getMaterialForUniqueID(texture as Texture2D, layer, true);
			gameObject.GetComponent<Renderer>().sharedMaterial.renderQueue = meshGatherEntry.queue;
			Mesh mesh = new Mesh();
			mesh.vertices = array;
			mesh.uv = array2;
			mesh.triangles = array3;
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
		string text = string.Empty;
		for (int i = 0; i < Enum.GetValues(typeof(CommonData.DeviceGeneration)).Length; i++)
		{
			text = checkDevice(1 << i) ? (text + "- ") : (text + (CommonData.deviceChars[i] + " "));
		}
		return text;
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
		Component[] componentsInChildren = gameObject.transform.GetComponentsInChildren(typeof(Renderer), true);
		int i = 0;
		Component[] array = componentsInChildren;
		for (int length = array.Length; i < length; i++)
		{
		}
	}
}
