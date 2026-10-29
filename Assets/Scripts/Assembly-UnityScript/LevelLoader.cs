using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class LevelLoader
{
	public static LevelLoader instance;

	public static LevelLoader getInstance()
	{
		if (instance == null)
		{
			instance = new LevelLoader();
		}
		return instance;
	}

	public virtual string[] getAllowedSplineTextures(ObjectListData objects)
	{
		ObjectPackData[] packs = objects.getPacks();
		List<string> list = new List<string>();
		foreach (ObjectPackData pack in packs)
		{
			list.AddRange(pack.getSplineTextures());
		}
		return list.ToArray();
	}

	public virtual LevelData loadFromString(string str, ObjectListData objects)
	{
		LevelData levelData = new LevelData();
		JSONObject json = new JSONObject(str);
		levelData.theme = ThemeLoader.getInstance().loadTheme(json.GetFieldSafe("theme", new JSONObject("\"" + "woodTrackTheme" + "\"")).str);
		if (levelData.theme == null)
		{
			return null;
		}
		levelData.startPoint = new Vector2((float)json.GetFieldSafe("startx", new JSONObject(0f)).n, (float)json.GetFieldSafe("starty", new JSONObject(0f)).n);
		levelData.topLeftLimit = new Vector2((float)json.GetFieldSafe("left", new JSONObject(-10f)).n, (float)json.GetFieldSafe("top", new JSONObject(-10f)).n);
		levelData.bottomRightLimit = new Vector2((float)json.GetFieldSafe("right", new JSONObject(10f)).n, (float)json.GetFieldSafe("bottom", new JSONObject(10f)).n);
		levelData.oldLeaderboardDoNotUse = json.GetFieldSafe("leaderboard", new JSONObject("\"\"")).str;
		JSONObject field = json.GetField("layers");
		levelData.customSettings.load(json);
		string str2 = json.GetFieldSafe("packs", new JSONObject(string.Empty)).str;
		ObjectPackData objectPackData = null;
		if (str2 != null && str2.Length > 0)
		{
			string[] packNames = str2.Split(","[0]);
			int i = 0;
			string[] array2 = packNames;
			for (int length = array2.Length; i < length; i++)
			{
				ObjectListLoader.getInstance().loadObjectList(objects, array2[i]);
				objectPackData = objects.getPackForName(array2[i]);
				levelData.usedObjectPacks.Add(objectPackData);
			}
		}
		foreach (string packName in levelData.theme.settings.allowedObjectPacks)
		{
			if (objects.getPackForName(packName) == null)
			{
				ObjectListLoader.getInstance().loadObjectList(objects, packName);
			}
			objectPackData = objects.getPackForName(packName);
			if (!levelData.usedObjectPacks.Contains(objectPackData))
			{
				levelData.usedObjectPacks.Add(objectPackData);
			}
		}
		List<LevelLayerData> layerData = levelData.layerData;
		loadLayers(field, layerData, objects);
		return levelData;
	}

	public virtual LevelData load(string packName, string trackName, ObjectListData objects)
	{
		string json = null;
		CustomProfiler.start();
		CustomProfiler.begin("LevelLoader::load");
		string path = FileManager.getInstance().locateLevel(packName, trackName);
		json = FileManager.getInstance().loadFileText(path);
		if (string.IsNullOrEmpty(json))
		{
			Debug.LogWarning("Could not open track '" + trackName + "'");
			return null;
		}
		LevelData levelData = loadFromString(json, objects);
		levelData.fileName = trackName;
		CustomProfiler.end("LevelLoader::load");
		CustomProfiler.report();
		return levelData;
	}

	public virtual void loadLayers(JSONObject layers, List<LevelLayerData> layerData, ObjectListData objectList)
	{
		foreach (JSONObject json in layers.list)
		{
			int layerId = (int)json.GetField("layer").n;
			LevelLayerData levelLayerData = new LevelLayerData(layerId);
			layerData.Add(levelLayerData);
			levelLayerData.themeInfo.layerName = json.GetFieldSafe("name", new JSONObject("\"<none>\"")).str;
			levelLayerData.themeInfo.parallaxSpeed = json.DecodeVector2("parallax");
			levelLayerData.themeInfo.colorMultiplier = json.DecodeColor3("color");
			levelLayerData.themeInfo.flags = (int)json.GetFieldSafe("flags", new JSONObject(0f)).n;
			levelLayerData.deviceList = (int)json.GetFieldSafe("dev", new JSONObject(0f)).n;
			JSONObject field = json.GetField("objects");
			if (field == null)
			{
				continue;
			}
			foreach (JSONObject json2 in field.list)
			{
				if (json2 == null)
				{
					continue;
				}
				float x = (float)json2.GetField("x").n;
				float y = (float)json2.GetField("y").n;
				double n = json2.GetFieldSafe("dev", new JSONObject(0f)).n;
				float typeValue = (float)json2.GetField("type").n;
				float id = (float)json2.GetField("id").n;
				int flags = (int)json2.GetFieldSafe("flags", new JSONObject(0f)).n;
				float rotation = (float)json2.GetFieldSafe("rotation", new JSONObject(0f)).n;
				string str = json2.GetFieldSafe("osa", new JSONObject(string.Empty)).str;
				string str2 = json2.GetFieldSafe("osaparams", new JSONObject(string.Empty)).str;
				LevelEntityData levelEntityData = null;
				float type = typeValue;
				if (type == 2f)
				{
					float objectId = (float)json2.GetField("object").n;
					float z = (float)json2.GetFieldSafe("z", new JSONObject(0f)).n;
					LevelObjectData levelObjectData = new LevelObjectData((int)id, (int)objectId);
					levelObjectData.zDepth = (int)z;
					levelEntityData = levelObjectData;
				}
				else if (type == 1f)
				{
					LevelTriggerData levelTriggerData = new LevelTriggerData((int)id, (int)json2.GetField("triggertype").n);
					levelEntityData = levelTriggerData;
					levelTriggerData.size = new Vector2((float)json2.GetFieldSafe("width", new JSONObject(1f)).n, (float)json2.GetFieldSafe("height", new JSONObject(1f)).n);
					levelTriggerData.triggerScript = json2.GetFieldSafe("script", new JSONObject(string.Empty)).str;
					levelTriggerData.triggerParam = json2.GetFieldSafe("param", new JSONObject(string.Empty)).str;
				}
				else if (type == 6f)
				{
					Debug.Log("loading spline data");
					LevelSplineData levelSplineData = new LevelSplineData((int)id);
					levelSplineData.height = (float)json2.GetFieldSafe("height", new JSONObject(0.5f)).n;
					levelSplineData.texture = json2.GetFieldSafe("texture", new JSONObject("\"Junglecoaster_track.png\"")).str;
					bool flag = false;
					foreach (string allowedSplineTexture in getAllowedSplineTextures(objectList))
					{
						if (allowedSplineTexture == levelSplineData.texture)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						levelSplineData.texture = getAllowedSplineTextures(objectList)[0];
					}
					levelEntityData = levelSplineData;
					JSONObject field2 = json2.GetField("spline");
					if (field2 != null)
					{
						foreach (JSONObject json3 in field2.list)
						{
							Vector2 position = new Vector2((float)json3.GetField("xPos").n, (float)json3.GetField("yPos").n);
							Vector2 tangentIn = new Vector2((float)json3.GetField("xIn").n, (float)json3.GetField("yIn").n);
							Vector2 tangentOut = new Vector2((float)json3.GetField("xOut").n, (float)json3.GetField("yOut").n);
							levelSplineData.spline.addPoint(position, tangentIn, tangentOut);
						}
					}
				}
				if (levelEntityData != null)
				{
					levelEntityData.position.x = x;
					levelEntityData.position.y = y;
					levelEntityData.flags = flags;
					levelEntityData.rotation = rotation;
					levelEntityData.scriptAttachment = str;
					levelEntityData.scriptParams = str2;
					levelEntityData.devices = (int)n;
					if (NFPortLegacyDevice.generation == NFPortLegacyDeviceGeneration.iPhone3GS && (str == "OSARotate" || str == "OSAPendulum"))
					{
						levelEntityData = null;
					}
					if (levelEntityData != null)
					{
						levelLayerData.objectData.Add(levelEntityData);
					}
				}
			}
		}
	}

	private void fixDuplicates(List<LevelLayerData> layerData)
	{
		int maxId = 0;
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(layerData);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is LevelLayerData))
			{
				obj = RuntimeServices.Coerce(obj, typeof(LevelLayerData));
			}
			LevelLayerData levelLayerData = (LevelLayerData)obj;
			UnityRuntimeServices.Update(enumerator, levelLayerData);
			foreach (LevelEntityData levelEntityData in levelLayerData.objectData)
			{
				if (levelEntityData.referenceId > maxId)
				{
					maxId = levelEntityData.referenceId;
				}
			}
		}
		List<int> list = new List<int>();
		IEnumerator enumerator3 = UnityRuntimeServices.GetEnumerator(layerData);
		while (enumerator3.MoveNext())
		{
			object obj3 = enumerator3.Current;
			if (!(obj3 is LevelLayerData))
			{
				obj3 = RuntimeServices.Coerce(obj3, typeof(LevelLayerData));
			}
			LevelLayerData levelLayerData2 = (LevelLayerData)obj3;
			UnityRuntimeServices.Update(enumerator3, levelLayerData2);
			foreach (LevelEntityData levelEntityData2 in levelLayerData2.objectData)
			{
				if (list.Contains(levelEntityData2.referenceId))
				{
					maxId = (levelEntityData2.referenceId = maxId + 1);
				}
				list.Add(levelEntityData2.referenceId);
			}
		}
	}

	public virtual List<LevelEntityData> loadLevelObjectData()
	{
		return new List<LevelEntityData>();
	}

	public virtual void createTestLayerAndContents(List<LevelLayerData> layerData)
	{
		int nextId = 1;
		LevelLayerData levelLayerData = null;
		levelLayerData = new LevelLayerData(-1);
		levelLayerData.themeInfo.parallaxSpeed = new Vector2(0.9f, 0.9f);
		layerData.Add(levelLayerData);
		levelLayerData = new LevelLayerData(0);
		levelLayerData.themeInfo.parallaxSpeed = new Vector2(1f, 1f);
		layerData.Add(levelLayerData);
		levelLayerData = new LevelLayerData(1);
		levelLayerData.themeInfo.parallaxSpeed = new Vector2(1.1f, 1.1f);
		layerData.Add(levelLayerData);
		levelLayerData = new LevelLayerData(2);
		levelLayerData.themeInfo.parallaxSpeed = new Vector2(1.2f, 1.2f);
		layerData.Add(levelLayerData);
		LevelObjectData levelObjectData = new LevelObjectData(nextId++, 1);
		layerData[2].objectData.Add(levelObjectData);
		levelObjectData.position = new Vector3(10f, 2f, 0f);
		levelObjectData.flags = 65536;
		levelObjectData = new LevelObjectData(nextId++, 1);
		layerData[0].objectData.Add(levelObjectData);
		levelObjectData.position = new Vector3(-9.4f, 2f, 0f);
		levelObjectData.rotation = 30f;
		levelObjectData.flags = 196608;
		levelObjectData = new LevelObjectData(nextId++, 1);
		layerData[1].objectData.Add(levelObjectData);
		levelObjectData.position = new Vector3(-10f, -2f, 0f);
		levelObjectData.flags = 65536;
		levelObjectData = new LevelObjectData(nextId++, 2);
		layerData[0].objectData.Add(levelObjectData);
		levelObjectData.position = new Vector3(5f, 2f, 0f);
		levelObjectData.flags = 196608;
		levelObjectData = new LevelObjectData(nextId++, 1);
		layerData[1].objectData.Add(levelObjectData);
		levelObjectData.position = new Vector3(0f, 2f, 0f);
		levelObjectData.flags = 65536;
	}

	public virtual LevelData createEmpty()
	{
		LevelData levelData = new LevelData();
		levelData.layerData = new List<LevelLayerData>();
		LevelLayerData levelLayerData = new LevelLayerData(0);
		levelLayerData.themeInfo.parallaxSpeed = new Vector2(1f, 1f);
		levelData.layerData.Add(levelLayerData);
		levelData.startPoint.x = 0f;
		levelData.startPoint.y = 0f;
		return levelData;
	}

	public virtual void createTestLevel(LevelData data)
	{
		data.startPoint.x = -3f;
		data.startPoint.y = -6f;
		data.theme = ThemeLoader.getInstance().loadTheme("woodTrackTheme");
		createTestLayerAndContents(data.layerData);
		LevelTriggerData levelTriggerData = new LevelTriggerData(100, 1);
		levelTriggerData.position = new Vector3(14f, -7f, 0f);
		levelTriggerData.size = new Vector2(4f, 4f);
		levelTriggerData.triggerScript = "TriggerFinish";
		data.layerData[1].objectData.Add(levelTriggerData);
	}
}
