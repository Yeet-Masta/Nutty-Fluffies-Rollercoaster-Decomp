using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class LevelLoaderBinary
{
	public static LevelLoaderBinary instance;

	public static int CURRENT_LEVEL_VERSION = 2;

	public static LevelLoaderBinary getInstance()
	{
		if (instance == null)
		{
			instance = new LevelLoaderBinary();
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

	public virtual LevelData loadFromStream(BinaryReader sr, ObjectListData objects)
	{
		LevelData levelData = new LevelData();
		byte b = 0;
		ObjectPackData objectPackData = null;
		while (b != 10)
		{
			b = sr.ReadByte();
			byte b2 = b;
			if (b2 == 13)
			{
				int num = sr.ReadByte();
				if (num < CURRENT_LEVEL_VERSION)
				{
					Debug.LogWarning("Too old track format!");
					return null;
				}
			}
			else if (b2 == 0)
			{
				levelData.startPoint.x = sr.ReadSingle();
				levelData.startPoint.y = sr.ReadSingle();
			}
			else if (b2 == 1)
			{
				levelData.topLeftLimit.x = sr.ReadSingle();
				levelData.bottomRightLimit.x = sr.ReadSingle();
				levelData.topLeftLimit.y = sr.ReadSingle();
				levelData.bottomRightLimit.y = sr.ReadSingle();
			}
			else if (b2 == 2)
			{
				levelData.oldLeaderboardDoNotUse = sr.ReadString();
			}
			else if (b2 == 3)
			{
				levelData.theme = ThemeLoader.getInstance().loadTheme(sr.ReadString());
				if (levelData.theme == null)
				{
					return null;
				}
			}
			else if (b2 == 4)
			{
				int num2 = sr.ReadByte();
				for (int i = 0; i < num2; i++)
				{
					string text = sr.ReadString();
					ObjectListLoader.getInstance().loadObjectList(objects, text);
					objectPackData = objects.getPackForName(text);
					levelData.usedObjectPacks.Add(objectPackData);
				}
			}
			else if (b2 == 5)
			{
				levelData.customSettings.loadBinary(sr);
			}
			else if (b2 == 6)
			{
				loadLayers(sr, levelData.layerData, objects);
			}
		}
		foreach (string text2 in levelData.theme.settings.allowedObjectPacks)
		{
			if (objects.getPackForName(text2) == null)
			{
				ObjectListLoader.getInstance().loadObjectList(objects, text2);
			}
			objectPackData = objects.getPackForName(text2);
			if (!levelData.usedObjectPacks.Contains(objectPackData))
			{
				levelData.usedObjectPacks.Add(objectPackData);
			}
		}
		return levelData;
	}

	public virtual LevelData load(string packName, string trackName, ObjectListData objects)
	{
		CustomProfiler.start();
		if (GameData.LOG)
		{
			Debug.Log("loading level " + trackName + " from pack " + packName);
		}
		CustomProfiler.begin("LevelLoaderBinary::load");
		BinaryReader binaryReader = null;
		try
		{
			string path = FileManager.getInstance().locateLevel(packName, trackName).Replace("json", "bin");
			byte[] buffer = FileManager.getInstance().loadFileBytes(path);
			binaryReader = new BinaryReader(new MemoryStream(buffer));
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Could not open track '" + trackName + "': " + ex);
			return null;
		}
		LevelData levelData = loadFromStream(binaryReader, objects);
		levelData.fileName = trackName;
		CustomProfiler.end("LevelLoaderBinary::load");
		CustomProfiler.report();
		return levelData;
	}

	public virtual void loadLayers(BinaryReader sr, List<LevelLayerData> layerData, ObjectListData objectList)
	{
		int num = sr.ReadByte();
		LevelLayerData levelLayerData = null;
		float x = 0f;
		float y = 0f;
		string scriptAttachment = null;
		string scriptParams = null;
		int flags = 0;
		int devices = 0;
		float rotation = 0f;
		for (int i = 0; i < num; i++)
		{
			int num2 = 0;
			while (num2 != 10)
			{
				num2 = sr.ReadByte();
				switch (num2)
				{
				case 7:
				{
					int layerId = sr.ReadSByte();
					levelLayerData = new LevelLayerData(layerId);
					layerData.Add(levelLayerData);
					break;
				}
				case 8:
					levelLayerData.themeInfo.layerName = sr.ReadString();
					levelLayerData.themeInfo.parallaxSpeed.x = sr.ReadSingle();
					levelLayerData.themeInfo.parallaxSpeed.y = sr.ReadSingle();
					levelLayerData.themeInfo.colorMultiplier.x = sr.ReadSingle();
					levelLayerData.themeInfo.colorMultiplier.y = sr.ReadSingle();
					levelLayerData.themeInfo.colorMultiplier.z = sr.ReadSingle();
					levelLayerData.themeInfo.flags = sr.ReadInt32();
					break;
				case 11:
					levelLayerData.deviceList = sr.ReadInt32();
					break;
				case 9:
				{
					int num3 = sr.ReadInt32();
					for (int j = 0; j < num3; j++)
					{
						LevelEntityData levelEntityData = null;
						int num4 = 0;
						do
						{
							num4 = sr.ReadByte();
							if (num4 != 12)
							{
								continue;
							}
							x = sr.ReadSingle();
							y = sr.ReadSingle();
							scriptAttachment = sr.ReadString();
							scriptParams = sr.ReadString();
							float num6 = sr.ReadInt32();
							flags = sr.ReadInt32();
							devices = sr.ReadInt32();
							rotation = sr.ReadSingle();
							float num7 = (int)sr.ReadByte();
							float num8 = num7;
							if (num8 == 2f)
							{
								int id = sr.ReadInt32();
								int zDepth = sr.ReadSByte();
								LevelObjectData levelObjectData = new LevelObjectData((int)num6, id);
								levelObjectData.zDepth = zDepth;
								levelEntityData = levelObjectData;
							}
							else if (num8 == 1f)
							{
								LevelTriggerData levelTriggerData = new LevelTriggerData((int)num6, sr.ReadByte());
								levelEntityData = levelTriggerData;
								levelTriggerData.size = new Vector2(sr.ReadSingle(), sr.ReadSingle());
								levelTriggerData.triggerScript = sr.ReadString();
								levelTriggerData.triggerParam = sr.ReadString();
							}
							else
							{
								if (num8 != 6f)
								{
									continue;
								}
								LevelSplineData levelSplineData = new LevelSplineData((int)num6);
								levelSplineData.height = sr.ReadSingle();
								levelSplineData.texture = sr.ReadString();
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
								int num9 = sr.ReadInt32();
								for (int l = 0; l < num9; l++)
								{
									Vector2 position = new Vector2(sr.ReadSingle(), sr.ReadSingle());
									Vector2 tangentIn = new Vector2(sr.ReadSingle(), sr.ReadSingle());
									Vector2 tangentOut = new Vector2(sr.ReadSingle(), sr.ReadSingle());
									levelSplineData.spline.addPoint(position, tangentIn, tangentOut);
								}
							}
						}
						while (num4 != 14);
						if (levelEntityData != null)
						{
							levelEntityData.position.x = x;
							levelEntityData.position.y = y;
							levelEntityData.flags = flags;
							levelEntityData.devices = devices;
							levelEntityData.rotation = rotation;
							levelEntityData.scriptAttachment = scriptAttachment;
							levelEntityData.scriptParams = scriptParams;
							levelLayerData.objectData.Add(levelEntityData);
						}
					}
					break;
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
