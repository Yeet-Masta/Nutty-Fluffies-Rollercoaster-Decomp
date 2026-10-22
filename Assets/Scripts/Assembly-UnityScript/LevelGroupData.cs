using System;
using System.Collections.Generic;

[Serializable]
public class LevelGroupData : LevelEntityData
{
	public List<LevelEntityData> childList;

	public LevelGroupData(int referenceId)
		: base(referenceId, 5)
	{
		childList = new List<LevelEntityData>();
	}

	public virtual void addChild(LevelEntityData child)
	{
		childList.Add(child);
	}
}
