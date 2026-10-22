using System;

[Serializable]
public class LevelSplineData : LevelEntityData
{
	[NonSerialized]
	public const int SPLINE_INTERSECT = 8388608;

	public CSpline2 spline;

	public string texture;

	public float height;

	public LevelSplineData(int referenceId)
		: base(referenceId, 6)
	{
		spline = new CSpline2();
		height = 0.5f;
	}
}
