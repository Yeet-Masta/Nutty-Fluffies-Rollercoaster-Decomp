using System;
using System.Collections.Generic;

[Serializable]
public class ObjectData
{
	public static int FLAG_ALLOW_DYNAMIC = 256;

	public static int FLAG_ALLOW_OBJECT_COLLISION = 512;

	public static int FLAG_ALLOW_BREAKING = 1024;

	public static int FLAG_ALLOW_TRAIN_COLLISION = 2048;

	public static int FLAG_IS_OPTIMIZED = 4096;

	public static int FLAG_IS_SCENERY = 8192;

	public static int FLAG_X_REPEAT_MASK = 15;

	public static int FLAG_Y_REPEAT_MASK = 240;

	public int id;

	public string name;

	public float mass;

	public int flags;

	public int deviceList;

	public int hitpoints;

	private List<ObjectPartData> partList;

	public LevelObject cloneRef;

	public ObjectData(int id, string name, float mass)
	{
		partList = new List<ObjectPartData>();
		this.id = id;
		this.name = name;
		this.mass = mass;
	}

	public virtual bool checkDevice(int dev)
	{
		return (deviceList & dev) != 0;
	}

	public virtual void setDevice(int dev, bool state)
	{
		deviceList &= ~dev;
		if (state)
		{
			deviceList |= dev;
		}
	}

	public virtual string getDevicesDesc()
	{
		string text = string.Empty;
		for (int i = 0; i < Enum.GetValues(typeof(CommonData.DeviceGeneration)).Length; i++)
		{
			text = checkDevice(1 << i) ? (text + "- ") : (text + (CommonData.deviceChars[i] + " "));
		}
		return text;
	}

	public virtual bool matchesSceneryType(int sceneryType)
	{
		bool flag = checkFlag(FLAG_IS_SCENERY);
		return sceneryType == 3 || (sceneryType == 2 && flag) || (sceneryType == 1 && !flag);
	}

	public virtual void setFlag(int flag, bool state)
	{
		flags &= ~flag;
		if (state)
		{
			flags |= flag;
		}
	}

	public virtual bool checkFlag(int flag)
	{
		return (flags & flag) != 0;
	}

	public virtual int getNumParts()
	{
		return partList.Count;
	}

	public virtual List<ObjectPartData> getParts()
	{
		return partList;
	}

	public virtual void addPart(ObjectPartData obj)
	{
		partList.Add(obj);
	}

	public virtual void removePartAt(int index)
	{
		partList.RemoveAt(index);
	}

	public virtual void swapParts(int from, int to)
	{
		ObjectPartData value = partList[from];
		partList[from] = partList[to];
		partList[to] = value;
	}

	public virtual ObjectPartData getIndPart(int i)
	{
		return i < partList.Count ? partList[i] : null;
	}
}
