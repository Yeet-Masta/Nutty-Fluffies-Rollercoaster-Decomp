using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class ObjectListLoader
{
	public static ObjectListLoader instance;

	public static ObjectListLoader getInstance()
	{
		if (instance == null)
		{
			instance = new ObjectListLoader();
		}
		return instance;
	}

	public virtual ObjectListData createTestObjects(string name)
	{
		ObjectListData objectListData = new ObjectListData();
		ObjectData objectData = new ObjectData(1, "box", 1f);
		objectListData.addObject(string.Empty, objectData);
		objectData.addPart(new ObjectPartTextureData("Crate_256px.png", new Vector2(0f, 0f), 0f));
		objectData.addPart(new ObjectPartBoxCollisionData(new Rect(0f, 0f, 1.25f, 1.25f), 0f));
		objectData = new ObjectData(2, "sphere", 1f);
		objectListData.addObject(string.Empty, objectData);
		objectData.addPart(new ObjectPartTextureData("Roller_256px.png", new Vector2(0f, 0f), 0f));
		objectData.addPart(new ObjectPartSphereCollisionData(Vector2.zero, 1f));
		return objectListData;
	}

	public virtual void loadObjectList(ObjectListData toData, string objectPackName)
	{
		string packPath = FileManager.getInstance().locateObjectPack(objectPackName);
		ObjectPackData objectPackData = new ObjectPackData(objectPackName);
		objectPackData.fullPath = packPath;
		toData.addPack(objectPackData);
		string json = FileManager.getInstance().loadFileText(packPath + "/objects.json");
		if (json == null)
		{
			return;
		}
		JSONObject root = new JSONObject(json);
		JSONObject field = root.GetField("splinetextures");
		if ((bool)field)
		{
			foreach (JSONObject json2 in field.list)
			{
				string str = json2.GetField("name").str;
				objectPackData.toggleSplineTexture(str);
			}
		}
		JSONObject field2 = root.GetField("objects");
		if (field2 != null)
		{
			foreach (JSONObject json3 in field2.list)
			{
				if (json3 == null)
				{
					continue;
				}
				ObjectData objectData = new ObjectData((int)json3.GetField("id").n, json3.GetField("name").str, (float)json3.GetFieldSafe("mass", new JSONObject(1f)).n);
				objectData.flags = UnityBuiltins.parseInt(json3.GetFieldSafe("flags", new JSONObject(0f)).n);
				objectData.deviceList = UnityBuiltins.parseInt(json3.GetFieldSafe("dev", new JSONObject(0f)).n);
				if (toData.getPackIndexForObjectID(objectData.id) != -1)
				{
					continue;
				}
				toData.addObject(objectPackData.name, objectData);
				JSONObject field3 = json3.GetField("parts");
				foreach (JSONObject json4 in field3.list)
				{
					ObjectPartData objectPartData = null;
					float posX = (float)json4.GetField("x").n;
					float posY = (float)json4.GetField("y").n;
					int flags = (int)json4.GetFieldSafe("flags", new JSONObject(0f)).n;
					float rotation = (float)json4.GetField("rotation").n;
					double n = json4.GetField("type").n;
					if (n == 1.0)
					{
						ObjectPartTextureData objectPartTextureData = new ObjectPartTextureData(json4.GetField("texture").str, new Vector2(posX, posY), rotation);
						if (json4.HasField("r"))
						{
							objectPartTextureData.color.r = (float)json4.GetField("r").n;
						}
						if (json4.HasField("g"))
						{
							objectPartTextureData.color.g = (float)json4.GetField("g").n;
						}
						if (json4.HasField("b"))
						{
							objectPartTextureData.color.b = (float)json4.GetField("b").n;
						}
						if (json4.HasField("flipX"))
						{
							objectPartTextureData.setFlag(ObjectPartData.Flags.TEXTURE_FLIP_X, true);
						}
						if (json4.HasField("flipY"))
						{
							objectPartTextureData.setFlag(ObjectPartData.Flags.TEXTURE_FLIP_Y, true);
						}
						if (json4.HasField("editOnly"))
						{
							objectPartTextureData.setFlag(ObjectPartData.Flags.TEXTURE_EDITOR_ONLY, true);
						}
						objectPartTextureData.textureSourceSize = new Vector2((float)json4.GetFieldSafe("width", new JSONObject(1f)).n, (float)json4.GetFieldSafe("height", new JSONObject(1f)).n);
						if (json4.HasField("stretch"))
						{
							objectPartTextureData.allowStretch = true;
							objectPartTextureData.stretchedScale = json4.DecodeVector2("stretch");
						}
						objectPartData = objectPartTextureData;
					}
					else if (n == 2.0)
					{
						ObjectPartBoxCollisionData objectPartBoxCollisionData = new ObjectPartBoxCollisionData(new Rect(posX, posY, (float)json4.GetField("width").n, (float)json4.GetField("height").n), rotation);
						objectPartData = objectPartBoxCollisionData;
					}
					else if (n == 3.0)
					{
						ObjectPartSphereCollisionData objectPartSphereCollisionData = new ObjectPartSphereCollisionData(new Vector2(posX, posY), (float)json4.GetField("radius").n);
						objectPartData = objectPartSphereCollisionData;
					}
					else if (n == 5.0)
					{
						ObjectPartBoxTriggerData objectPartBoxTriggerData = new ObjectPartBoxTriggerData(new Rect(posX, posY, (float)json4.GetFieldSafe("width", new JSONObject(1f)).n, (float)json4.GetFieldSafe("height", new JSONObject(1f)).n), rotation);
						objectPartBoxTriggerData.script = json4.GetFieldSafe("script", new JSONObject(string.Empty)).str;
						objectPartBoxTriggerData.@params = json4.GetFieldSafe("params", new JSONObject(string.Empty)).str;
						objectPartData = objectPartBoxTriggerData;
					}
					else if (n == 6.0)
					{
						ObjectPartSphereTriggerData objectPartSphereTriggerData = new ObjectPartSphereTriggerData(new Vector2(posX, posY), (float)json4.GetFieldSafe("radius", new JSONObject(1f)).n);
						objectPartSphereTriggerData.script = json4.GetFieldSafe("script", new JSONObject(string.Empty)).str;
						objectPartSphereTriggerData.@params = json4.GetFieldSafe("params", new JSONObject(string.Empty)).str;
						objectPartData = objectPartSphereTriggerData;
					}
					objectPartData.flags = (ObjectPartData.Flags)flags;
					objectData.addPart(objectPartData);
				}
			}
		}
		toData.recacheObjectIDs();
	}
}
