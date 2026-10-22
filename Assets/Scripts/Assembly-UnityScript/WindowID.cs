using System;

[Serializable]
public class WindowID
{
	[NonSerialized]
	public const int OBJECTS = 1;

	[NonSerialized]
	public const int SAVE = 2;

	[NonSerialized]
	public const int LOAD = 3;

	[NonSerialized]
	public const int INSPECTOR = 4;

	[NonSerialized]
	public const int LAYERS = 5;

	[NonSerialized]
	public const int TOOLTIP = 6;

	[NonSerialized]
	public const int CONFIG_SETTINGS = 7;

	[NonSerialized]
	public const int NEW = 8;

	[NonSerialized]
	public const int PACK_EDIT = 9;

	[NonSerialized]
	public const int TEXTURE_CONVERTER = 10;

	[NonSerialized]
	public const int GLOBAL_SETTINGS = 11;

	[NonSerialized]
	public const int VERIFY = 12;

	[NonSerialized]
	public const int APPLY_PHYSICS = 13;
}
