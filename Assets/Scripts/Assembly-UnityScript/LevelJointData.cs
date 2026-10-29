using System;
using UnityEngine;

[Serializable]
public class LevelJointData : LevelEntityData
{
	public const int JOINT_FIXED = 1;

	public const int JOINT_SPRING = 2;

	public const int JOINT_HINGE = 3;

	public int jointType;

	public Vector2 anchor;

	public int connectionRef;

	public LevelJointData(int referenceId, int type, int connectionRef)
		: base(referenceId, 3)
	{
		anchor = new Vector2(0f, 0f);
		this.connectionRef = -1;
		jointType = type;
		this.connectionRef = connectionRef;
	}
}
