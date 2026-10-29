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
		myCamera = Camera.main.gameObject.GetComponent<Camera2D>();
		cameraTransform = Camera.main.transform;
	}

	public virtual void updateLayers()
	{
		Vector3 position = cameraTransform.position;
		float zoomFactor = myCamera.getZoomFactor();
		int layerIndex = 0;
		int i = 0;
		LevelLayer[] array = layers;
		for (int length = array.Length; i < length; i++)
		{
			float x = array[i].parallaxFactor.x;
			if (array[i].layer != 0)
			{
				float scale = 1f;
				scale = zoomFactor / (1f + x * (zoomFactor - 1f));
				float offsetX = position.x * (0f - (x - 1f));
				float offsetY = position.y * (0f - (x - 1f));
				offsetX += (position.x - offsetX) * (1f - scale);
				offsetY += (position.y - offsetY) * (1f - scale);
				layerTransforms[layerIndex].localPosition = new Vector3(offsetX, offsetY, -array[i].layer * 100);
				layerTransforms[layerIndex].localScale = new Vector3(scale, scale, 1f);
			}
			layerIndex++;
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
		int index = 0;
		LevelLayer[] allLayers = getLayers();
		int length = allLayers.Length;
		while (index < length)
		{
			if (allLayers[index].layer == num)
			{
				return allLayers[index];
			}
			index++;
		}
		return null;
	}

	public virtual LevelEntity findEntity(int uniqueID, GameObject customRoot)
	{
		Component[] componentsInChildren = (customRoot ? customRoot : gameObject).GetComponentsInChildren(typeof(LevelEntity), true);
		int index = 0;
		Component[] array = componentsInChildren;
		int length = array.Length;
		while (index < length)
		{
			LevelEntity levelEntity = array[index] as LevelEntity;
			if (levelEntity.referenceId == uniqueID)
			{
				return levelEntity;
			}
			index++;
		}
		Debug.Log("Did not find entity for ID " + uniqueID);
		return null;
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
			int id = levelEntity.referenceId & ~LevelEntityData.REFERENCE_ID_BIT_THEME;
			if (id > maxId)
			{
				maxId = id;
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
