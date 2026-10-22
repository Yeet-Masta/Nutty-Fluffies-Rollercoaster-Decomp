using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class ObjectListData
{
	private int maxObjectId;

	[NonSerialized]
	public static readonly int PACK_ALL = -1;

	[NonSerialized]
	public const int TYPE_OBJECTS = 1;

	[NonSerialized]
	public const int TYPE_SCENERY = 2;

	[NonSerialized]
	public const int TYPE_BOTH = 3;

	private List<ObjectPackData> packList;

	private int maxCatId;

	private Dictionary<int, int> objIDtoPackCache;

	private ObjectData errorObject;

	public ObjectListData()
	{
		packList = new List<ObjectPackData>();
		objIDtoPackCache = new Dictionary<int, int>();
		maxObjectId = 0;
		errorObject = new ObjectData(0, "ERROR", 1f);
		errorObject.addPart(new ObjectPartTextureData("error.png", new Vector2(0f, 0f), 0f));
		errorObject.addPart(new ObjectPartBoxCollisionData(new Rect(0f, 0f, 1f, 1f), 0f));
	}

	public virtual int getMaxObjectId()
	{
		return maxObjectId;
	}

	public virtual int getMaxCatId()
	{
		return maxCatId;
	}

	public virtual void recacheObjectIDs()
	{
		int num = 0;
		objIDtoPackCache.Clear();
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(packList);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is ObjectPackData))
			{
				obj = RuntimeServices.Coerce(obj, typeof(ObjectPackData));
			}
			ObjectPackData objectPackData = (ObjectPackData)obj;
			ObjectData[] objects = objectPackData.getObjects();
			UnityRuntimeServices.Update(enumerator, objectPackData);
			int i = 0;
			ObjectData[] array = objects;
			for (int length = array.Length; i < length; i++)
			{
				objIDtoPackCache.Add(array[i].id, num);
			}
			num++;
		}
	}

	public virtual int getPackIndexForPackName(string packName)
	{
		int num = 0;
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(packList);
		int result;
		while (true)
		{
			if (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				if (!(obj is ObjectPackData))
				{
					obj = RuntimeServices.Coerce(obj, typeof(ObjectPackData));
				}
				ObjectPackData objectPackData = (ObjectPackData)obj;
				if (objectPackData.name == packName)
				{
					result = num;
					break;
				}
				num++;
				continue;
			}
			result = -1;
			break;
		}
		return result;
	}

	public virtual int getPackIndexForObjectID(int id)
	{
		return objIDtoPackCache.ContainsKey(id) ? objIDtoPackCache[id] : (-1);
	}

	public virtual ObjectPackData getPackForObjectId(int id)
	{
		return objIDtoPackCache.ContainsKey(id) ? packList[objIDtoPackCache[id]] : null;
	}

	public virtual ObjectPackData getPackForSplineTexture(string texture)
	{
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(packList);
		object result;
		while (true)
		{
			if (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				if (!(obj is ObjectPackData))
				{
					obj = RuntimeServices.Coerce(obj, typeof(ObjectPackData));
				}
				ObjectPackData objectPackData = (ObjectPackData)obj;
				if (objectPackData.getSplineTextures().Contains(texture))
				{
					result = objectPackData;
					break;
				}
				continue;
			}
			result = null;
			break;
		}
		return (ObjectPackData)result;
	}

	public virtual int getNumObjects(int catIndex, int sceneryMode)
	{
		int result;
		if (catIndex != -1)
		{
			result = ((catIndex >= 0 && catIndex < packList.Count) ? packList[catIndex].getNumObjects(sceneryMode) : 0);
		}
		else
		{
			int num = 0;
			IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(packList);
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				if (!(obj is ObjectPackData))
				{
					obj = RuntimeServices.Coerce(obj, typeof(ObjectPackData));
				}
				ObjectPackData objectPackData = (ObjectPackData)obj;
				num += objectPackData.getNumObjects(sceneryMode);
				UnityRuntimeServices.Update(enumerator, objectPackData);
			}
			result = num;
		}
		return result;
	}

	public virtual int getNumPacks()
	{
		return packList.Count;
	}

	public virtual ObjectData[] getObjects()
	{
		List<ObjectData> list = new List<ObjectData>();
		for (int i = 0; i < packList.Count; i++)
		{
			ObjectData[] objectsInPack = getObjectsInPack(i);
			list.AddRange(objectsInPack);
		}
		return list.ToArray();
	}

	public virtual ObjectData[] getObjectsInPack(int catIndex)
	{
		return (catIndex >= 0 && catIndex < packList.Count) ? packList[catIndex].getObjects() : null;
	}

	public virtual ObjectPackData[] getPacks()
	{
		return packList.ToArray();
	}

	public virtual string[] getPackNames()
	{
		string[] array = new string[packList.Count];
		int num = 0;
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(packList);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is ObjectPackData))
			{
				obj = RuntimeServices.Coerce(obj, typeof(ObjectPackData));
			}
			ObjectPackData objectPackData = (ObjectPackData)obj;
			array[num++] = objectPackData.name;
			UnityRuntimeServices.Update(enumerator, objectPackData);
		}
		return array;
	}

	public virtual void addObject(string packName, ObjectData obj)
	{
		addObject(getPackIndexForPackName(packName), obj);
	}

	public virtual ObjectPackData getPackForName(string packName)
	{
		int packIndexForPackName = getPackIndexForPackName(packName);
		return (packIndexForPackName != -1) ? packList[packIndexForPackName] : null;
	}

	public virtual void addObject(int catIndex, ObjectData obj)
	{
		if (catIndex >= 0 && catIndex < packList.Count)
		{
			packList[catIndex].addObject(obj);
			if (obj.id > maxObjectId)
			{
				maxObjectId = obj.id;
			}
			recacheObjectIDs();
		}
	}

	public virtual void addPack(ObjectPackData cat)
	{
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(packList);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is ObjectPackData))
			{
				obj = RuntimeServices.Coerce(obj, typeof(ObjectPackData));
			}
			ObjectPackData objectPackData = (ObjectPackData)obj;
			if (objectPackData.name == cat.name)
			{
				return;
			}
		}
		packList.Add(cat);
	}

	public virtual ObjectPackData getPack(int catIndex)
	{
		return (catIndex >= 0 && catIndex < packList.Count) ? packList[catIndex] : null;
	}

	public virtual List<string> getSplineTextures()
	{
		List<string> list = new List<string>();
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(packList);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is ObjectPackData))
			{
				obj = RuntimeServices.Coerce(obj, typeof(ObjectPackData));
			}
			ObjectPackData objectPackData = (ObjectPackData)obj;
			list.AddRange(objectPackData.getSplineTextures());
			UnityRuntimeServices.Update(enumerator, objectPackData);
		}
		return list;
	}

	public virtual void removeObjectByID(int id)
	{
		int index = objIDtoPackCache[id];
		packList[index].removeObjectByID(id);
		recacheObjectIDs();
	}

	public virtual int getObjectIndexByID(int id)
	{
		int index = objIDtoPackCache[id];
		return packList[index].getObjectIndexByID(id);
	}

	public virtual void removePackAt(int index)
	{
		packList.RemoveAt(index);
	}

	public virtual ObjectData getIndObjectInPack(int packIndex, int objIndexInPack, int sceneryMode)
	{
		ObjectPackData objectPackData = packList[packIndex];
		int num = 0;
		ObjectData[] objects = objectPackData.getObjects();
		int num2 = 0;
		ObjectData[] array = objects;
		int length = array.Length;
		object result;
		while (true)
		{
			if (num2 < length)
			{
				if (array[num2].matchesSceneryType(sceneryMode))
				{
					if (num == objIndexInPack)
					{
						result = array[num2];
						break;
					}
					num++;
				}
				num2++;
				continue;
			}
			result = null;
			break;
		}
		return (ObjectData)result;
	}

	public virtual ObjectData getIndObject(int globalIndex)
	{
		int num = 0;
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(packList);
		object result;
		while (true)
		{
			if (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				if (!(obj is ObjectPackData))
				{
					obj = RuntimeServices.Coerce(obj, typeof(ObjectPackData));
				}
				ObjectPackData objectPackData = (ObjectPackData)obj;
				int numObjects = objectPackData.getNumObjects(3);
				UnityRuntimeServices.Update(enumerator, objectPackData);
				if (globalIndex >= num && globalIndex < num + numObjects)
				{
					result = objectPackData.getObjectByIndex(globalIndex - num);
					break;
				}
				num += numObjects;
				continue;
			}
			result = null;
			break;
		}
		return (ObjectData)result;
	}

	public virtual ObjectData getObject(int id)
	{
		ObjectData objectByID;
		if (!objIDtoPackCache.ContainsKey(id))
		{
			objectByID = errorObject;
		}
		else
		{
			int index = objIDtoPackCache[id];
			objectByID = packList[index].getObjectByID(id);
		}
		return objectByID;
	}

	public virtual void clearObjects()
	{
		maxObjectId = 0;
		maxCatId = 0;
		objIDtoPackCache.Clear();
		packList.Clear();
	}
}
