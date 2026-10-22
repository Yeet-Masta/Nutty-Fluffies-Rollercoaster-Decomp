using System;
using UnityEngine;

[Serializable]
public class ObjectPartData
{
	[Serializable]
	public enum Flags
	{
		TEXTURE_FLIP_X = 1,
		TEXTURE_FLIP_Y = 2,
		TEXTURE_TILE = 4,
		TEXTURE_EDITOR_ONLY = 8
	}

	[Serializable]
	public enum PartType
	{
		PART_NONE = 0,
		PART_TEXTURE = 1,
		PART_BOX_COLLISION = 2,
		PART_SPHERE_COLLISION = 3,
		PART_CONVEX_COLLISION = 4,
		PART_BOX_TRIGGER = 5,
		PART_SPHERE_TRIGGER = 6,
		NUM_OBJECT_PART_TYPES = PART_SPHERE_TRIGGER
	}

	[NonSerialized]
	public static readonly string[] PART_NAMES = new string[7] { "<none>", "Texture", "Box Collision", "Sphere Collision", "Polygon Collision", "Box Trigger", "Sphere Trigger" };

	public int type;

	public Vector2 origin;

	public float rotation;

	public Flags flags;

	public ObjectPartData(PartType type, Vector2 origin, float rotation)
	{
		this.type = (int)type;
		this.origin = origin;
		this.rotation = rotation;
	}

	public virtual void setFlag(Flags flag, bool state)
	{
		flags &= ~flag;
		if (state)
		{
			flags |= flag;
		}
	}

	public virtual string getDesc()
	{
		return PART_NAMES[type];
	}

	public virtual bool checkFlag(Flags flag)
	{
		return (flags & flag) != 0;
	}
}
