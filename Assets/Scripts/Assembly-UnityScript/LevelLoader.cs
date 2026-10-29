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
		int i = 0;
		ObjectPackData[] array = packs;
		for (int length = array.Length; i < length; i++)
		{
			list.AddRange(array[i].getSplineTextures());
		}
		return list.ToArray();
	}

	public virtual LevelData loadFromString(string str, ObjectListData objects)
	{
		LevelData levelData = new LevelData();
		JSONObject jSONObject = new JSONObject(str);
		levelData.theme = ThemeLoader.getInstance().loadTheme(jSONObject.GetFieldSafe("theme", new JSONObject("\"" + "woodTrackTheme" + "\"")).str);
		object result;
		if (levelData.theme == null)
		{
			result = null;
		}
		else
		{
			levelData.startPoint = new Vector2((float)jSONObject.GetFieldSafe("startx", new JSONObject(0f)).n, (float)jSONObject.GetFieldSafe("starty", new JSONObject(0f)).n);
			levelData.topLeftLimit = new Vector2((float)jSONObject.GetFieldSafe("left", new JSONObject(-10f)).n, (float)jSONObject.GetFieldSafe("top", new JSONObject(-10f)).n);
			levelData.bottomRightLimit = new Vector2((float)jSONObject.GetFieldSafe("right", new JSONObject(10f)).n, (float)jSONObject.GetFieldSafe("bottom", new JSONObject(10f)).n);
			levelData.oldLeaderboardDoNotUse = jSONObject.GetFieldSafe("leaderboard", new JSONObject("\"\"")).str;
			JSONObject field = jSONObject.GetField("layers");
			levelData.customSettings.load(jSONObject);
			string str2 = jSONObject.GetFieldSafe("packs", new JSONObject(string.Empty)).str;
			ObjectPackData objectPackData = null;
			if (str2 != null && str2.Length > 0)
			{
				string[] array = str2.Split(","[0]);
				int i = 0;
				string[] array2 = array;
				for (int length = array2.Length; i < length; i++)
				{
					ObjectListLoader.getInstance().loadObjectList(objects, array2[i]);
					objectPackData = objects.getPackForName(array2[i]);
					levelData.usedObjectPacks.Add(objectPackData);
				}
			}
			foreach (string text in levelData.theme.settings.allowedObjectPacks)
			{
				if (objects.getPackForName(text) == null)
				{
					ObjectListLoader.getInstance().loadObjectList(objects, text);
				}
				objectPackData = objects.getPackForName(text);
				if (!levelData.usedObjectPacks.Contains(objectPackData))
				{
					levelData.usedObjectPacks.Add(objectPackData);
				}
			}
			List<LevelLayerData> layerData = levelData.layerData;
			loadLayers(field, layerData, objects);
			result = levelData;
		}
		return (LevelData)result;
	}

	public virtual LevelData load(string packName, string trackName, ObjectListData objects)
	{
		string text = null;
		CustomProfiler.start();
		CustomProfiler.begin("LevelLoader::load");
		string path = FileManager.getInstance().locateLevel(packName, trackName);
		text = FileManager.getInstance().loadFileText(path);
		object result;
		if (string.IsNullOrEmpty(text))
		{
			Debug.LogWarning("Could not open track '" + trackName + "'");
			result = null;
		}
		else
		{
			LevelData levelData = loadFromString(text, objects);
			levelData.fileName = trackName;
			CustomProfiler.end("LevelLoader::load");
			CustomProfiler.report();
			result = levelData;
		}
		return (LevelData)result;
	}

	public virtual void loadLayers(JSONObject layers, List<LevelLayerData> layerData, ObjectListData objectList)
	{
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(layers.list);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is JSONObject))
			{
				obj = RuntimeServices.Coerce(obj, typeof(JSONObject));
			}
			JSONObject jSONObject = (JSONObject)obj;
			int layerId = (int)jSONObject.GetField("layer").n;
			UnityRuntimeServices.Update(enumerator, jSONObject);
			LevelLayerData levelLayerData = new LevelLayerData(layerId);
			layerData.Add(levelLayerData);
			levelLayerData.themeInfo.layerName = jSONObject.GetFieldSafe("name", new JSONObject("\"<none>\"")).str;
			UnityRuntimeServices.Update(enumerator, jSONObject);
			levelLayerData.themeInfo.parallaxSpeed = jSONObject.DecodeVector2("parallax");
			UnityRuntimeServices.Update(enumerator, jSONObject);
			levelLayerData.themeInfo.colorMultiplier = jSONObject.DecodeColor3("color");
			UnityRuntimeServices.Update(enumerator, jSONObject);
			levelLayerData.themeInfo.flags = (int)jSONObject.GetFieldSafe("flags", new JSONObject(0f)).n;
			UnityRuntimeServices.Update(enumerator, jSONObject);
			levelLayerData.deviceList = (int)jSONObject.GetFieldSafe("dev", new JSONObject(0f)).n;
			UnityRuntimeServices.Update(enumerator, jSONObject);
			JSONObject field = jSONObject.GetField("objects");
			UnityRuntimeServices.Update(enumerator, jSONObject);
			if (field == null)
			{
				continue;
			}
			IEnumerator enumerator2 = UnityRuntimeServices.GetEnumerator(field.list);
			while (enumerator2.MoveNext())
			{
				object obj2 = enumerator2.Current;
				if (!(obj2 is JSONObject))
				{
					obj2 = RuntimeServices.Coerce(obj2, typeof(JSONObject));
				}
				JSONObject jSONObject2 = (JSONObject)obj2;
				if (jSONObject2 == null)
				{
					continue;
				}
				float x = (float)jSONObject2.GetField("x").n;
				UnityRuntimeServices.Update(enumerator2, jSONObject2);
				float y = (float)jSONObject2.GetField("y").n;
				UnityRuntimeServices.Update(enumerator2, jSONObject2);
				double n = jSONObject2.GetFieldSafe("dev", new JSONObject(0f)).n;
				UnityRuntimeServices.Update(enumerator2, jSONObject2);
				float num = (float)jSONObject2.GetField("type").n;
				UnityRuntimeServices.Update(enumerator2, jSONObject2);
				float num2 = (float)jSONObject2.GetField("id").n;
				UnityRuntimeServices.Update(enumerator2, jSONObject2);
				int flags = (int)jSONObject2.GetFieldSafe("flags", new JSONObject(0f)).n;
				UnityRuntimeServices.Update(enumerator2, jSONObject2);
				float rotation = (float)jSONObject2.GetFieldSafe("rotation", new JSONObject(0f)).n;
				UnityRuntimeServices.Update(enumerator2, jSONObject2);
				string str = jSONObject2.GetFieldSafe("osa", new JSONObject(string.Empty)).str;
				UnityRuntimeServices.Update(enumerator2, jSONObject2);
				string str2 = jSONObject2.GetFieldSafe("osaparams", new JSONObject(string.Empty)).str;
				UnityRuntimeServices.Update(enumerator2, jSONObject2);
				LevelEntityData levelEntityData = null;
				float num3 = num;
				if (num3 == 2f)
				{
					float num4 = (float)jSONObject2.GetField("object").n;
					UnityRuntimeServices.Update(enumerator2, jSONObject2);
					float num5 = (float)jSONObject2.GetFieldSafe("z", new JSONObject(0f)).n;
					UnityRuntimeServices.Update(enumerator2, jSONObject2);
					LevelObjectData levelObjectData = new LevelObjectData((int)num2, (int)num4);
					levelObjectData.zDepth = (int)num5;
					levelEntityData = levelObjectData;
				}
				else if (num3 == 1f)
				{
					LevelTriggerData levelTriggerData = new LevelTriggerData((int)num2, (int)jSONObject2.GetField("triggertype").n);
					UnityRuntimeServices.Update(enumerator2, jSONObject2);
					levelEntityData = levelTriggerData;
					levelTriggerData.size = new Vector2((float)jSONObject2.GetFieldSafe("width", new JSONObject(1f)).n, (float)jSONObject2.GetFieldSafe("height", new JSONObject(1f)).n);
					UnityRuntimeServices.Update(enumerator2, jSONObject2);
					levelTriggerData.triggerScript = jSONObject2.GetFieldSafe("script", new JSONObject(string.Empty)).str;
					UnityRuntimeServices.Update(enumerator2, jSONObject2);
					levelTriggerData.triggerParam = jSONObject2.GetFieldSafe("param", new JSONObject(string.Empty)).str;
					UnityRuntimeServices.Update(enumerator2, jSONObject2);
				}
				else if (num3 == 6f)
				{
					Debug.Log("loading spline data");
					LevelSplineData levelSplineData = new LevelSplineData((int)num2);
					levelSplineData.height = (float)jSONObject2.GetFieldSafe("height", new JSONObject(0.5f)).n;
					UnityRuntimeServices.Update(enumerator2, jSONObject2);
					levelSplineData.texture = jSONObject2.GetFieldSafe("texture", new JSONObject("\"Junglecoaster_track.png\"")).str;
					UnityRuntimeServices.Update(enumerator2, jSONObject2);
					bool flag = false;
					int i = 0;
					string[] allowedSplineTextures = getAllowedSplineTextures(objectList);
					for (int length = allowedSplineTextures.Length; i < length; i++)
					{
						if (allowedSplineTextures[i] == levelSplineData.texture)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						levelSplineData.texture = getAllowedSplineTextures(objectList)[0];
					}
					levelEntityData = levelSplineData;
					JSONObject field2 = jSONObject2.GetField("spline");
					UnityRuntimeServices.Update(enumerator2, jSONObject2);
					if (field2 != null)
					{
						IEnumerator enumerator3 = UnityRuntimeServices.GetEnumerator(field2.list);
						while (enumerator3.MoveNext())
						{
							object obj3 = enumerator3.Current;
							if (!(obj3 is JSONObject))
							{
								obj3 = RuntimeServices.Coerce(obj3, typeof(JSONObject));
							}
							JSONObject jSONObject3 = (JSONObject)obj3;
							Vector2 position = new Vector2((float)jSONObject3.GetField("xPos").n, (float)jSONObject3.GetField("yPos").n);
							UnityRuntimeServices.Update(enumerator3, jSONObject3);
							Vector2 tangentIn = new Vector2((float)jSONObject3.GetField("xIn").n, (float)jSONObject3.GetField("yIn").n);
							UnityRuntimeServices.Update(enumerator3, jSONObject3);
							Vector2 tangentOut = new Vector2((float)jSONObject3.GetField("xOut").n, (float)jSONObject3.GetField("yOut").n);
							UnityRuntimeServices.Update(enumerator3, jSONObject3);
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
		int num = 0;
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
				if (levelEntityData.referenceId > num)
				{
					num = levelEntityData.referenceId;
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
					num = (levelEntityData2.referenceId = num + 1);
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
		int num = 1;
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
		LevelObjectData levelObjectData = new LevelObjectData(num++, 1);
		layerData[2].objectData.Add(levelObjectData);
		levelObjectData.position = new Vector3(10f, 2f, 0f);
		levelObjectData.flags = 65536;
		levelObjectData = new LevelObjectData(num++, 1);
		layerData[0].objectData.Add(levelObjectData);
		levelObjectData.position = new Vector3(-9.4f, 2f, 0f);
		levelObjectData.rotation = 30f;
		levelObjectData.flags = 196608;
		levelObjectData = new LevelObjectData(num++, 1);
		layerData[1].objectData.Add(levelObjectData);
		levelObjectData.position = new Vector3(-10f, -2f, 0f);
		levelObjectData.flags = 65536;
		levelObjectData = new LevelObjectData(num++, 2);
		layerData[0].objectData.Add(levelObjectData);
		levelObjectData.position = new Vector3(5f, 2f, 0f);
		levelObjectData.flags = 196608;
		levelObjectData = new LevelObjectData(num++, 1);
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
