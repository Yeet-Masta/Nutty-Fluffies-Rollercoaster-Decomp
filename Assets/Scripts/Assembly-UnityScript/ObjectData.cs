using System;
using System.Collections.Generic;

[Serializable]
public class ObjectData
{
	[NonSerialized]
	public static int FLAG_ALLOW_DYNAMIC = 256;

	[NonSerialized]
	public static int FLAG_ALLOW_OBJECT_COLLISION = 512;

	[NonSerialized]
	public static int FLAG_ALLOW_BREAKING = 1024;

	[NonSerialized]
	public static int FLAG_ALLOW_TRAIN_COLLISION = 2048;

	[NonSerialized]
	public static int FLAG_IS_OPTIMIZED = 4096;

	[NonSerialized]
	public static int FLAG_IS_SCENERY = 8192;

	[NonSerialized]
	public static int FLAG_X_REPEAT_MASK = 15;

	[NonSerialized]
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
			text = (checkDevice(1 << i) ? (text + "- ") : (text + (CommonData.deviceChars[i] + " ")));
		}
		return text;
	}

	public virtual bool matchesSceneryType(int sceneryType)
	{
		bool flag = checkFlag(FLAG_IS_SCENERY);
		bool flag2 = sceneryType == 3;
		if (!flag2)
		{
			flag2 = sceneryType == 2;
			if (flag2)
			{
				flag2 = flag;
			}
		}
		if (!flag2)
		{
			flag2 = sceneryType == 1;
			if (flag2)
			{
				flag2 = !flag;
			}
		}
		return flag2;
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
		return (i < partList.Count) ? partList[i] : null;
	}
}
