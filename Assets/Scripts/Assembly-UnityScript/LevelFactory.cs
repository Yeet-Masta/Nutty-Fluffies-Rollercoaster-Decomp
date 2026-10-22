using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class LevelFactory
{
	[NonSerialized]
	private static LevelFactory instance;

	[NonSerialized]
	public const int LAYER_Z_OFFSET = 100;

	[NonSerialized]
	public const float OBJECT_Z_ORDER_STEP = 1f;

	[NonSerialized]
	public const int OBJECT_Z_STEP_LIMIT = 10;

	public static LevelFactory getInstance()
	{
		if (RuntimeServices.EqualityOperator(instance, null))
		{
			instance = new LevelFactory();
		}
		return instance;
	}

	public virtual Level create(LevelData data, GameObject parent, ObjectListData objects)
	{
		CustomProfiler.start();
		CustomProfiler.begin("LevelFactory::create");
		Level level = createLevel(parent);
		level.startPoint = data.startPoint;
		level.topLeftLimit = data.topLeftLimit;
		level.bottomRightLimit = data.bottomRightLimit;
		level.themeSettings = data.theme.settings;
		level.fileName = data.fileName;
		level.oldLeaderboardDoNotUse = data.oldLeaderboardDoNotUse;
		level.customSettings = data.customSettings;
		Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
		if (!RuntimeServices.EqualityOperator(data.theme, null))
		{
			bool flag = true;
			IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(data.theme.layerList);
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				if (!(obj is LevelLayerData))
				{
					obj = RuntimeServices.Coerce(obj, typeof(LevelLayerData));
				}
				LevelLayerData levelLayerData = (LevelLayerData)obj;
				LevelObjectFactory.USE_ALPHA_SHADER = true;
				flag = false;
				LevelLayer levelLayer = createLayer(level.gameObject, levelLayerData);
				UnityRuntimeServices.Update(enumerator, levelLayerData);
				createLayerObjects(levelLayer, objects, levelLayerData);
				UnityRuntimeServices.Update(enumerator, levelLayerData);
				if ((bool)levelLayer)
				{
					dictionary[levelLayerData.layer] = true;
					UnityRuntimeServices.Update(enumerator, levelLayerData);
				}
			}
			LevelObjectFactory.USE_ALPHA_SHADER = true;
		}
		IEnumerator enumerator2 = UnityRuntimeServices.GetEnumerator(data.layerData);
		while (enumerator2.MoveNext())
		{
			object obj2 = enumerator2.Current;
			if (!(obj2 is LevelLayerData))
			{
				obj2 = RuntimeServices.Coerce(obj2, typeof(LevelLayerData));
			}
			LevelLayerData levelLayerData2 = (LevelLayerData)obj2;
			if (dictionary.ContainsKey(levelLayerData2.layer))
			{
				levelLayerData2.themeInfo = null;
				UnityRuntimeServices.Update(enumerator2, levelLayerData2);
				LevelLayer layer = createLayer(level.gameObject, levelLayerData2);
				UnityRuntimeServices.Update(enumerator2, levelLayerData2);
				createLayerObjects(layer, objects, levelLayerData2);
				UnityRuntimeServices.Update(enumerator2, levelLayerData2);
			}
		}
		applyLayerZs(level);
		connectObjects(level);
		level.cacheLayers();
		cacheSplineVertices(level);
		level.initMaxRef();
		CustomProfiler.end("LevelFactory::create");
		CustomProfiler.report();
		return level;
	}

	private void cacheSplineVertices(Level level)
	{
		if (!CommonData.EDITOR_ACTIVE)
		{
			int i = 0;
			Component[] componentsInChildren = level.gameObject.GetComponentsInChildren(typeof(LevelSpline), true);
			for (int length = componentsInChildren.Length; i < length; i++)
			{
				LevelSpline levelSpline = componentsInChildren[i] as LevelSpline;
				MeshCollider meshCollider = (MeshCollider)levelSpline.gameObject.GetComponent(typeof(MeshCollider));
				level.splineMeshVerticesCache[meshCollider.sharedMesh] = meshCollider.sharedMesh.vertices;
			}
		}
	}

	public virtual void applyLayerZs(Level level)
	{
		float num = 0f;
		float num2 = 0f;
		Component[] componentsInChildren = level.gameObject.transform.GetComponentsInChildren(typeof(LevelLayer), true);
		int i = 0;
		Component[] array = componentsInChildren;
		for (int length = array.Length; i < length; i++)
		{
			LevelLayer levelLayer = array[i] as LevelLayer;
			float num3 = -levelLayer.layer * 100;
			float z = num3;
			Vector3 localPosition = levelLayer.gameObject.transform.localPosition;
			float num4 = (localPosition.z = z);
			Vector3 vector = (levelLayer.gameObject.transform.localPosition = localPosition);
			if (!(num3 >= num))
			{
				num = num3;
			}
			if (!(num3 <= num2))
			{
				num2 = num3;
			}
		}
		Camera.main.nearClipPlane = num - 100f;
		Camera.main.farClipPlane = num2 + 100f;
	}

	public virtual void createLayerObjects(LevelLayer layer, ObjectListData objects, LevelLayerData layerData)
	{
		if (layer == null)
		{
			return;
		}
		LevelObjectFactory levelObjectFactory = LevelObjectFactory.getInstance();
		LevelObjectFactory.setRenderQueueForLayer(layer.layer);
		LevelObjectFactory.LAYER = layer.layer;
		if (!RuntimeServices.EqualityOperator(layerData.objectData, null))
		{
			for (int i = 0; i < layerData.objectData.Count; i++)
			{
				LevelEntity levelEntity = levelObjectFactory.create(layerData.objectData[i], layer.gameObject, objects);
			}
		}
		layer.createGatheredMesh();
	}

	public virtual LevelLayer createLayer(GameObject parent, LevelLayerData layerData)
	{
		LevelLayer levelLayer = null;
		Component[] componentsInChildren = parent.transform.GetComponentsInChildren(typeof(LevelLayer), true);
		object result;
		if (CommonData.EDITOR_ACTIVE || !layerData.checkDevice(1 << CommonData.getDeviceGenerationInt()))
		{
			if (!RuntimeServices.EqualityOperator(componentsInChildren, null))
			{
				int i = 0;
				Component[] array = componentsInChildren;
				for (int length = array.Length; i < length; i++)
				{
					if ((array[i] as LevelLayer).layer == layerData.layer)
					{
						levelLayer = array[i] as LevelLayer;
						break;
					}
				}
			}
			if (levelLayer == null)
			{
				levelLayer = (LevelLayer)UnityEngine.Object.Instantiate(LevelDataPrefabs.getInstance().layerPrefab, Vector3.zero, Quaternion.identity);
				levelLayer.transform.parent = parent.transform;
				levelLayer.layer = layerData.layer;
			}
			if (!RuntimeServices.EqualityOperator(layerData.themeInfo, null))
			{
				levelLayer.parallaxFactor = layerData.themeInfo.parallaxSpeed;
				levelLayer.layerName = layerData.themeInfo.layerName;
				levelLayer.colorMultiplier = layerData.themeInfo.colorMultiplier;
				levelLayer.layerFlags = layerData.themeInfo.flags;
				levelLayer.updateColor();
				levelLayer.deviceList = layerData.deviceList;
			}
			result = levelLayer;
		}
		else
		{
			result = null;
		}
		return (LevelLayer)result;
	}

	public virtual Level createLevel(GameObject parent)
	{
		Level level = (Level)UnityEngine.Object.Instantiate(LevelDataPrefabs.getInstance().levelPrefab, Vector3.zero, Quaternion.identity);
		level.transform.parent = parent.transform;
		return level;
	}

	public virtual void connectObjects(Level level)
	{
		Component[] componentsInChildren = level.gameObject.GetComponentsInChildren(typeof(LevelJoint), true);
		Component[] componentsInChildren2 = level.gameObject.GetComponentsInChildren(typeof(LevelObject), true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			LevelJoint levelJoint = componentsInChildren[i] as LevelJoint;
			for (int j = 0; j < componentsInChildren2.Length; j++)
			{
				LevelObject levelObject = componentsInChildren2[j] as LevelObject;
				if (levelJoint.connectReferenceId == levelObject.referenceId)
				{
					Joint component = levelJoint.GetComponent<Joint>();
					Rigidbody component2 = levelObject.GetComponent<Rigidbody>();
					if (component2 != null)
					{
						component.connectedBody = component2;
					}
					else
					{
						Debug.LogError("object connected to joint must be dynamic! (ref: " + levelObject.referenceId + ")");
					}
				}
			}
		}
		Component[] componentsInChildren3 = level.gameObject.GetComponentsInChildren(typeof(LevelTrigger), true);
		int k = 0;
		Component[] array = componentsInChildren3;
		for (int length = array.Length; k < length; k++)
		{
			(array[k] as LevelTrigger).prepare();
		}
		Component[] componentsInChildren4 = level.gameObject.GetComponentsInChildren(typeof(ObjectScript), true);
		int l = 0;
		Component[] array2 = componentsInChildren4;
		for (int length2 = array2.Length; l < length2; l++)
		{
			(array2[l] as ObjectScript).prepare();
		}
	}

	public virtual void createAutoResetTrigger(Level level, LevelData data)
	{
		Vector3 zero = Vector3.zero;
		zero.x = (data.topLeftLimit.x + data.bottomRightLimit.x) * 0.5f;
		zero.y = data.topLeftLimit.y - 20f;
		GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(LevelDataPrefabs.getInstance().triggerPrefab, zero, Quaternion.identity);
		BoxCollider boxCollider = gameObject.gameObject.AddComponent<BoxCollider>();
		float x = data.bottomRightLimit.x - data.topLeftLimit.x + 1000f;
		Vector3 size = boxCollider.size;
		float num = (size.x = x);
		Vector3 vector = (boxCollider.size = size);
		float y = 15f;
		Vector3 size2 = boxCollider.size;
		float num2 = (size2.y = y);
		Vector3 vector3 = (boxCollider.size = size2);
		float z = 200f;
		Vector3 size3 = boxCollider.size;
		float num3 = (size3.z = z);
		Vector3 vector5 = (boxCollider.size = size3);
		boxCollider.isTrigger = true;
		gameObject.AddComponentByName(LevelTriggerData.TRIGGER_SCRIPT_NAMES[4]);
		gameObject.transform.parent = level.transform;
	}

	public virtual void resetLevel(Level level)
	{
		Component[] componentsInChildren = level.gameObject.GetComponentsInChildren(typeof(LevelTrigger), true);
		int i = 0;
		Component[] array = componentsInChildren;
		for (int length = array.Length; i < length; i++)
		{
			(array[i] as LevelTrigger).prepare();
		}
	}
}
