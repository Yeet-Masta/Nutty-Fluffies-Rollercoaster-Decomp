using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectListData
{
	private int maxObjectId;

	public static readonly int PACK_ALL = -1;

	public const int TYPE_OBJECTS = 1;

	public const int TYPE_SCENERY = 2;

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
		foreach (ObjectPackData objectPackData in packList)
		{
			ObjectData[] objects = objectPackData.getObjects();
			foreach (ObjectData objectData in objects)
			{
				objIDtoPackCache.Add(objectData.id, num);
			}
			num++;
		}
	}

	public virtual int getPackIndexForPackName(string packName)
	{
		int num = 0;
		foreach (ObjectPackData objectPackData in packList)
		{
			if (objectPackData.name == packName)
			{
				return num;
			}
			num++;
		}
		return -1;
	}

	public virtual int getPackIndexForObjectID(int id)
	{
		return objIDtoPackCache.ContainsKey(id) ? objIDtoPackCache[id] : -1;
	}

	public virtual ObjectPackData getPackForObjectId(int id)
	{
		return objIDtoPackCache.ContainsKey(id) ? packList[objIDtoPackCache[id]] : null;
	}

	public virtual ObjectPackData getPackForSplineTexture(string texture)
	{
		foreach (ObjectPackData objectPackData in packList)
		{
			if (objectPackData.getSplineTextures().Contains(texture))
			{
				return objectPackData;
			}
		}
		return null;
	}

	public virtual int getNumObjects(int catIndex, int sceneryMode)
	{
		if (catIndex != -1)
		{
			return (catIndex >= 0 && catIndex < packList.Count) ? packList[catIndex].getNumObjects(sceneryMode) : 0;
		}
		else
		{
			int num = 0;
			foreach (ObjectPackData objectPackData in packList)
			{
				num += objectPackData.getNumObjects(sceneryMode);
			}
			return num;
		}
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
		foreach (ObjectPackData objectPackData in packList)
		{
			array[num++] = objectPackData.name;
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
		return packIndexForPackName != -1 ? packList[packIndexForPackName] : null;
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
		foreach (ObjectPackData objectPackData in packList)
		{
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
		foreach (ObjectPackData objectPackData in packList)
		{
			list.AddRange(objectPackData.getSplineTextures());
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
		while (num2 < length)
		{
			if (array[num2].matchesSceneryType(sceneryMode))
			{
				if (num == objIndexInPack)
				{
					return array[num2];
				}
				num++;
			}
			num2++;
		}
		return null;
	}

	public virtual ObjectData getIndObject(int globalIndex)
	{
		int num = 0;
		foreach (ObjectPackData objectPackData in packList)
		{
			int numObjects = objectPackData.getNumObjects(3);
			if (globalIndex >= num && globalIndex < num + numObjects)
			{
				return objectPackData.getObjectByIndex(globalIndex - num);
			}
			num += numObjects;
		}
		return null;
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
