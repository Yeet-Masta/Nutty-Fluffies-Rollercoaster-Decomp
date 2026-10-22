using System;
using System.Collections;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class ObjectListLoader
{
	[NonSerialized]
	public static ObjectListLoader instance;

	public static ObjectListLoader getInstance()
	{
		if (RuntimeServices.EqualityOperator(instance, null))
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
		string text = FileManager.getInstance().locateObjectPack(objectPackName);
		ObjectPackData objectPackData = new ObjectPackData(objectPackName);
		objectPackData.fullPath = text;
		toData.addPack(objectPackData);
		string text2 = FileManager.getInstance().loadFileText(text + "/objects.json");
		if (text2 == null)
		{
			return;
		}
		JSONObject jSONObject = new JSONObject(text2);
		JSONObject field = jSONObject.GetField("splinetextures");
		if ((bool)field)
		{
			IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(field.list);
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				if (!(obj is JSONObject))
				{
					obj = RuntimeServices.Coerce(obj, typeof(JSONObject));
				}
				JSONObject jSONObject2 = (JSONObject)obj;
				string str = jSONObject2.GetField("name").str;
				UnityRuntimeServices.Update(enumerator, jSONObject2);
				objectPackData.toggleSplineTexture(str);
			}
		}
		JSONObject field2 = jSONObject.GetField("objects");
		if (!RuntimeServices.EqualityOperator(field2, null))
		{
			IEnumerator enumerator2 = UnityRuntimeServices.GetEnumerator(field2.list);
			while (enumerator2.MoveNext())
			{
				object obj2 = enumerator2.Current;
				if (!(obj2 is JSONObject))
				{
					obj2 = RuntimeServices.Coerce(obj2, typeof(JSONObject));
				}
				JSONObject jSONObject3 = (JSONObject)obj2;
				if (RuntimeServices.EqualityOperator(jSONObject3, null))
				{
					continue;
				}
				ObjectData objectData = new ObjectData((int)jSONObject3.GetField("id").n, jSONObject3.GetField("name").str, (float)jSONObject3.GetFieldSafe("mass", new JSONObject(1f)).n);
				UnityRuntimeServices.Update(enumerator2, jSONObject3);
				objectData.flags = UnityBuiltins.parseInt(jSONObject3.GetFieldSafe("flags", new JSONObject(0f)).n);
				UnityRuntimeServices.Update(enumerator2, jSONObject3);
				objectData.deviceList = UnityBuiltins.parseInt(jSONObject3.GetFieldSafe("dev", new JSONObject(0f)).n);
				UnityRuntimeServices.Update(enumerator2, jSONObject3);
				if (toData.getPackIndexForObjectID(objectData.id) != -1)
				{
					continue;
				}
				toData.addObject(objectPackData.name, objectData);
				JSONObject field3 = jSONObject3.GetField("parts");
				UnityRuntimeServices.Update(enumerator2, jSONObject3);
				IEnumerator enumerator3 = UnityRuntimeServices.GetEnumerator(field3.list);
				while (enumerator3.MoveNext())
				{
					object obj3 = enumerator3.Current;
					if (!(obj3 is JSONObject))
					{
						obj3 = RuntimeServices.Coerce(obj3, typeof(JSONObject));
					}
					JSONObject jSONObject4 = (JSONObject)obj3;
					ObjectPartData objectPartData = null;
					float num = (float)jSONObject4.GetField("x").n;
					UnityRuntimeServices.Update(enumerator3, jSONObject4);
					float num2 = (float)jSONObject4.GetField("y").n;
					UnityRuntimeServices.Update(enumerator3, jSONObject4);
					int flags = (int)jSONObject4.GetFieldSafe("flags", new JSONObject(0f)).n;
					UnityRuntimeServices.Update(enumerator3, jSONObject4);
					float rotation = (float)jSONObject4.GetField("rotation").n;
					UnityRuntimeServices.Update(enumerator3, jSONObject4);
					double n = jSONObject4.GetField("type").n;
					UnityRuntimeServices.Update(enumerator3, jSONObject4);
					if (n == 1.0)
					{
						ObjectPartTextureData objectPartTextureData = new ObjectPartTextureData(jSONObject4.GetField("texture").str, new Vector2(num, num2), rotation);
						UnityRuntimeServices.Update(enumerator3, jSONObject4);
						if (jSONObject4.HasField("r"))
						{
							objectPartTextureData.color.r = (float)jSONObject4.GetField("r").n;
							UnityRuntimeServices.Update(enumerator3, jSONObject4);
						}
						if (jSONObject4.HasField("g"))
						{
							objectPartTextureData.color.g = (float)jSONObject4.GetField("g").n;
							UnityRuntimeServices.Update(enumerator3, jSONObject4);
						}
						if (jSONObject4.HasField("b"))
						{
							objectPartTextureData.color.b = (float)jSONObject4.GetField("b").n;
							UnityRuntimeServices.Update(enumerator3, jSONObject4);
						}
						if (jSONObject4.HasField("flipX"))
						{
							objectPartTextureData.setFlag(ObjectPartData.Flags.TEXTURE_FLIP_X, true);
						}
						if (jSONObject4.HasField("flipY"))
						{
							objectPartTextureData.setFlag(ObjectPartData.Flags.TEXTURE_FLIP_Y, true);
						}
						if (jSONObject4.HasField("editOnly"))
						{
							objectPartTextureData.setFlag(ObjectPartData.Flags.TEXTURE_EDITOR_ONLY, true);
						}
						objectPartTextureData.textureSourceSize = new Vector2((float)jSONObject4.GetFieldSafe("width", new JSONObject(1f)).n, (float)jSONObject4.GetFieldSafe("height", new JSONObject(1f)).n);
						UnityRuntimeServices.Update(enumerator3, jSONObject4);
						if (jSONObject4.HasField("stretch"))
						{
							objectPartTextureData.allowStretch = true;
							objectPartTextureData.stretchedScale = jSONObject4.DecodeVector2("stretch");
							UnityRuntimeServices.Update(enumerator3, jSONObject4);
						}
						objectPartData = objectPartTextureData;
					}
					else if (n == 2.0)
					{
						ObjectPartBoxCollisionData objectPartBoxCollisionData = new ObjectPartBoxCollisionData(new Rect(num, num2, (float)jSONObject4.GetField("width").n, (float)jSONObject4.GetField("height").n), rotation);
						UnityRuntimeServices.Update(enumerator3, jSONObject4);
						objectPartData = objectPartBoxCollisionData;
					}
					else if (n == 3.0)
					{
						ObjectPartSphereCollisionData objectPartSphereCollisionData = new ObjectPartSphereCollisionData(new Vector2(num, num2), (float)jSONObject4.GetField("radius").n);
						UnityRuntimeServices.Update(enumerator3, jSONObject4);
						objectPartData = objectPartSphereCollisionData;
					}
					else if (n == 5.0)
					{
						ObjectPartBoxTriggerData objectPartBoxTriggerData = new ObjectPartBoxTriggerData(new Rect(num, num2, (float)jSONObject4.GetFieldSafe("width", new JSONObject(1f)).n, (float)jSONObject4.GetFieldSafe("height", new JSONObject(1f)).n), rotation);
						UnityRuntimeServices.Update(enumerator3, jSONObject4);
						objectPartBoxTriggerData.script = jSONObject4.GetFieldSafe("script", new JSONObject(string.Empty)).str;
						UnityRuntimeServices.Update(enumerator3, jSONObject4);
						objectPartBoxTriggerData.@params = jSONObject4.GetFieldSafe("params", new JSONObject(string.Empty)).str;
						UnityRuntimeServices.Update(enumerator3, jSONObject4);
						objectPartData = objectPartBoxTriggerData;
					}
					else if (n == 6.0)
					{
						ObjectPartSphereTriggerData objectPartSphereTriggerData = new ObjectPartSphereTriggerData(new Vector2(num, num2), (float)jSONObject4.GetFieldSafe("radius", new JSONObject(1f)).n);
						UnityRuntimeServices.Update(enumerator3, jSONObject4);
						objectPartSphereTriggerData.script = jSONObject4.GetFieldSafe("script", new JSONObject(string.Empty)).str;
						UnityRuntimeServices.Update(enumerator3, jSONObject4);
						objectPartSphereTriggerData.@params = jSONObject4.GetFieldSafe("params", new JSONObject(string.Empty)).str;
						UnityRuntimeServices.Update(enumerator3, jSONObject4);
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
