using System;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
	[Serializable]
	private class LayerComparer : IComparer<LevelLayer>
	{
		public virtual int Compare(LevelLayer a, LevelLayer b)
		{
			return a.layer > b.layer ? -1 : (a.layer < b.layer ? 1 : 0);
		}
	}

	public Vector2 startPoint = new Vector2(0f, 0f);

	public Vector2 topLeftLimit = new Vector2(-10f, -10f);

	public Vector2 bottomRightLimit = new Vector2(10f, 10f);

	public int maxId = 1;

	public ThemeSettings themeSettings;

	public CustomSettings customSettings = new CustomSettings();

	public string fileName = "Untitled";

	public string oldLeaderboardDoNotUse = string.Empty;

	private LevelLayer[] layers;

	private Transform[] layerTransforms;

	private Camera2D myCamera;

	private Transform cameraTransform;

	public Dictionary<Mesh, Vector3[]> splineMeshVerticesCache = new Dictionary<Mesh, Vector3[]>();

	public bool hasKilledChildren;

	public virtual void Awake()
	{
		useGUILayout = false;
		cacheLayers();
	}

	public virtual void cacheLayers()
	{
		Component[] componentsInChildren = gameObject.GetComponentsInChildren(typeof(LevelLayer), true);
		layerTransforms = new Transform[componentsInChildren.Length];
		layers = new LevelLayer[componentsInChildren.Length];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			layers[i] = componentsInChildren[i] as LevelLayer;
			layerTransforms[i] = layers[i].transform;
		}
		myCamera = Camera.main.gameObject.GetComponent<Camera2D>() as Camera2D;
		cameraTransform = Camera.main.transform;
	}

	public virtual void updateLayers()
	{
		Vector3 position = cameraTransform.position;
		float zoomFactor = myCamera.getZoomFactor();
		int num = 0;
		int i = 0;
		LevelLayer[] array = layers;
		for (int length = array.Length; i < length; i++)
		{
			float x = array[i].parallaxFactor.x;
			if (array[i].layer != 0)
			{
				float num2 = 1f;
				num2 = zoomFactor / (1f + x * (zoomFactor - 1f));
				float num3 = position.x * (0f - (x - 1f));
				float num4 = position.y * (0f - (x - 1f));
				num3 += (position.x - num3) * (1f - num2);
				num4 += (position.y - num4) * (1f - num2);
				layerTransforms[num].localPosition = new Vector3(num3, num4, -array[i].layer * 100);
				layerTransforms[num].localScale = new Vector3(num2, num2, 1f);
			}
			num++;
		}
	}

	public virtual void Update()
	{
		updateLayers();
	}

	public virtual LevelLayer[] getLayers()
	{
		Component[] componentsInChildren = gameObject.GetComponentsInChildren(typeof(LevelLayer), true);
		List<LevelLayer> list = new List<LevelLayer>(componentsInChildren.Length);
		foreach (Component component in componentsInChildren)
		{
			list.Add(component as LevelLayer);
		}
		list.Sort(new LayerComparer());
		return list.ToArray();
	}

	public virtual LevelLayer getLayerForNumber(int num)
	{
		int num2 = 0;
		LevelLayer[] array = getLayers();
		int length = array.Length;
		object result;
		while (true)
		{
			if (num2 < length)
			{
				if (array[num2].layer == num)
				{
					result = array[num2];
					break;
				}
				num2++;
				continue;
			}
			result = null;
			break;
		}
		return (LevelLayer)result;
	}

	public virtual LevelEntity findEntity(int uniqueID, GameObject customRoot)
	{
		Component[] componentsInChildren = (customRoot ? customRoot : gameObject).GetComponentsInChildren(typeof(LevelEntity), true);
		int num = 0;
		Component[] array = componentsInChildren;
		int length = array.Length;
		object result;
		while (true)
		{
			if (num < length)
			{
				LevelEntity levelEntity = array[num] as LevelEntity;
				if (levelEntity.referenceId == uniqueID)
				{
					result = levelEntity;
					break;
				}
				num++;
				continue;
			}
			Debug.Log("Did not find entity for ID " + uniqueID);
			result = null;
			break;
		}
		return (LevelEntity)result;
	}

	public virtual LevelEntity findEntity(int uniqueID)
	{
		return findEntity(uniqueID, null);
	}

	public virtual void initMaxRef()
	{
		getNextReferenceId();
	}

	public virtual int getNextReferenceId()
	{
		Component[] componentsInChildren = gameObject.GetComponentsInChildren(typeof(LevelEntity), true);
		foreach (Component component in componentsInChildren)
		{
			LevelEntity levelEntity = component as LevelEntity;
			int num = levelEntity.referenceId & ~LevelEntityData.REFERENCE_ID_BIT_THEME;
			if (num > maxId)
			{
				maxId = num;
			}
		}
		return maxId + 1;
	}

	public virtual int reserveNewReferenceId()
	{
		int nextReferenceId = getNextReferenceId();
		maxId++;
		Debug.Log("RESERVED ID: " + nextReferenceId);
		return nextReferenceId;
	}
}
