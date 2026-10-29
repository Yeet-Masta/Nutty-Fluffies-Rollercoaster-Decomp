using System;
using UnityEngine;

[Serializable]
public class LevelEntityData
{
	public const int ENTITY_NONE = 0;

	public const int ENTITY_TRIGGER = 1;

	public const int ENTITY_OBJECT = 2;

	public const int ENTITY_JOINT = 3;

	public const int ENTITY_GROUP = 5;

	public const int ENTITY_SPLINE = 6;

	public static readonly int REFERENCE_ID_BIT_THEME = 1073741824;

	public Vector2 position;

	public float rotation;

	public int type;

	public int referenceId;

	public int flags;

	public int devices;

	public string scriptAttachment;

	public string scriptParams;

	public LevelEntityData(int referenceId, int type)
	{
		position = default(Vector2);
		this.type = type;
		this.referenceId = referenceId;
	}

	public virtual bool checkFlag(int flag)
	{
		return (flags & flag) != 0;
	}

	public virtual bool checkDevice(int dev)
	{
		return (devices & dev) != 0;
	}

	public virtual void setDevice(int dev, bool state)
	{
		devices &= ~dev;
		if (state)
		{
			devices |= dev;
		}
	}

	public virtual string getDevicesDesc()
	{
		string desc = string.Empty;
		for (int i = 0; i < Enum.GetValues(typeof(CommonData.DeviceGeneration)).Length; i++)
		{
			desc = checkDevice(1 << i) ? (desc + "- ") : (desc + (CommonData.deviceChars[i] + " "));
		}
		return desc;
	}
}
