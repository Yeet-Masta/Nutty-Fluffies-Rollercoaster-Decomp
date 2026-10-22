using System;
using System.Collections.Generic;

[Serializable]
public class LevelObjectData : LevelEntityData
{
	[NonSerialized]
	public const int FLAG_OBJECT_COLLIDER = 65536;

	[NonSerialized]
	public const int FLAG_DYNAMIC = 131072;

	[NonSerialized]
	public const int FLAG_SLEEP = 262144;

	[NonSerialized]
	public const int FLAG_FLIP_X = 524288;

	[NonSerialized]
	public const int FLAG_FLIP_Y = 1048576;

	[NonSerialized]
	public const int FLAG_ALLOW_BREAKING = 2097152;

	[NonSerialized]
	public const int FLAG_DO_NOT_BATCH = 4194304;

	[NonSerialized]
	public const int FLAG_TRAIN_COLLIDER = 8388608;

	public int objectId;

	public List<LevelEntityData> childList;

	public int zDepth;

	public LevelObjectData(int referenceId, int id)
		: base(referenceId, 2)
	{
		childList = new List<LevelEntityData>();
		objectId = id;
	}

	public virtual void addChild(LevelEntityData child)
	{
		childList.Add(child);
	}

	public virtual int getRepeat(int repeatMask)
	{
		int maskShift = Bits.GetMaskShift(repeatMask);
		return (flags & repeatMask) >> maskShift;
	}

	public virtual void setRepeat(int repeatMask, int repeat)
	{
		int maskShift = Bits.GetMaskShift(repeatMask);
		flags &= ~repeatMask;
		flags |= repeat << maskShift;
	}
}
