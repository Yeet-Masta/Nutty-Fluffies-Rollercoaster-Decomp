using System;

[Serializable]
public class LevelMarker : LevelEntity
{
	[NonSerialized]
	public const int MARKER_TOPLEFT = 1;

	[NonSerialized]
	public const int MARKER_BOTTOMRIGHT = 2;

	[NonSerialized]
	public const int MARKER_STARTPOINT = 3;

	public int markerType;

	public LevelMarker()
	{
		markerType = 1;
	}

	public override void Awake()
	{
		referenceId = -markerType;
	}
}
