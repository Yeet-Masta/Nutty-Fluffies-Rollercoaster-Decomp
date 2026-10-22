using System;

[Serializable]
public class LevelJoint : LevelEntity
{
	public int connectReferenceId;

	public LevelJoint()
	{
		connectReferenceId = -2;
	}
}
