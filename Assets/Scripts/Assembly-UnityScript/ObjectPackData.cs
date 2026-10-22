using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityScript.Lang;

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
		int result;
		if (sceneryMode == 3)
		{
			result = objectList.Count;
		}
		else
		{
			int num = 0;
			IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(objectList);
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				if (!(obj is ObjectData))
				{
					obj = RuntimeServices.Coerce(obj, typeof(ObjectData));
				}
				ObjectData objectData = (ObjectData)obj;
				if (objectData.matchesSceneryType(sceneryMode))
				{
					num++;
				}
			}
			result = num;
		}
		return result;
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
		return (objectIndexByID != -1) ? objectList[objectIndexByID] : null;
	}

	public virtual int getObjectIndexByID(int id)
	{
		int num = 0;
		int result;
		while (true)
		{
			if (num < objectList.Count)
			{
				if (objectList[num].id == id)
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
