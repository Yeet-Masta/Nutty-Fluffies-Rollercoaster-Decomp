using System;
using System.Collections.Generic;

[Serializable]
public class ObjectPackData
{
	public string name;

	public string fullPath;

	private List<ObjectData> objectList;

	private List<string> splineTextureList;

	public ObjectPackData(string name)
	{
		objectList = new List<ObjectData>();
		splineTextureList = new List<string>();
		this.name = name;
	}

	public virtual ObjectData[] getObjects()
	{
		return objectList.ToArray();
	}

	public virtual int getNumObjects(int sceneryMode)
	{
		if (sceneryMode == 3)
		{
			return objectList.Count;
		}
		else
		{
			int count = 0;
			foreach (ObjectData objectData in objectList)
			{
				if (objectData.matchesSceneryType(sceneryMode))
				{
					count++;
				}
			}
			return count;
		}
	}

	public virtual void removeObjectByID(int id)
	{
		int objectIndexByID = getObjectIndexByID(id);
		objectList.RemoveAt(objectIndexByID);
	}

	public virtual void addObject(ObjectData obj)
	{
		objectList.Add(obj);
	}

	public virtual ObjectData getObjectByIndex(int index)
	{
		return objectList[index];
	}

	public virtual ObjectData getObjectByID(int id)
	{
		int objectIndexByID = getObjectIndexByID(id);
		return objectIndexByID != -1 ? objectList[objectIndexByID] : null;
	}

	public virtual int getObjectIndexByID(int id)
	{
		int index = 0;
		while (index < objectList.Count)
		{
			if (objectList[index].id == id)
			{
				return index;
			}
			index++;
		}
		return -1;
	}

	public virtual List<string> getSplineTextures()
	{
		return splineTextureList;
	}

	public virtual void toggleSplineTexture(string name)
	{
		if (splineTextureList.Contains(name))
		{
			splineTextureList.Remove(name);
		}
		else
		{
			splineTextureList.Add(name);
		}
	}
}
