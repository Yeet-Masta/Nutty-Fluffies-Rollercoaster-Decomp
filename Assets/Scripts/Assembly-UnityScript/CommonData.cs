using System;
using UnityEngine;

[Serializable]
public class CommonData
{
	[Serializable]
	public enum DeviceType
	{
		DEVICE_IPHONE = 0,
		DEVICE_IPHONE2X = 1,
		DEVICE_IPAD = 2,
		DEVICE_MAC = 3,
		DEVICE_MAC_HD = 4,
		DEVICE_ANDROID = 5,
		DEVICE_FLASH = 6
	}

	[Serializable]
	public enum DeviceGeneration
	{
		GEN_IPHONE = 0,
		GEN_IPHONE_3G = 1,
		GEN_IPHONE_3GS = 2,
		GEN_IPHONE_4 = 3,
		GEN_IPHONE_4S = 4,
		GEN_IPHONE_IPOD1 = 5,
		GEN_IPHONE_IPOD2 = 6,
		GEN_IPHONE_IPOD3 = 7,
		GEN_IPHONE_IPOD4 = 8,
		GEN_IPAD1 = 9,
		GEN_IPAD2 = 10,
		GEN_MAC = 11,
		GEN_MAC_HD = 12,
		GEN_ANDROID_LOW = 13,
		GEN_ANDROID_HIGH = 14,
		GEN_FLASH = 15,
		GEN_IPAD3 = 16
	}

	[NonSerialized]
	public static bool IN_EDITOR_MODE;

	[NonSerialized]
	public static bool EDITOR_ACTIVE;

	[NonSerialized]
	public static bool DISABLE_ANIMATIONS = true;

	[NonSerialized]
	public static string LEVEL_PACK_NAME;

	[NonSerialized]
	public static string LEVEL_NAME = "editor_temp";

	[NonSerialized]
	private static DeviceType CURRENT_DEVICE = DeviceType.DEVICE_MAC;

	[NonSerialized]
	private static DeviceGeneration CURRENT_GEN = DeviceGeneration.GEN_MAC;

	[NonSerialized]
	public static float TEXTURE_PIXELS_PER_METER = 256f;

	[NonSerialized]
	public const float SOURCE_PIXELS_PER_METER = 256f;

	[NonSerialized]
	public static readonly float OBJECT_EDITOR_PIXELS_PER_METER = 64f;

	[NonSerialized]
	public const float VISIBLE_METERS = 18f;

	[NonSerialized]
	private static bool INITED;

	[NonSerialized]
	public static string[] devicePaths = new string[7] { "iPhone", "iphone2x", "iPad", "Mac", "MacHD", "Android", "Flash" };

	[NonSerialized]
	public static string[] iconPaths = new string[7] { "iPhone", "iPhone", "iPhone", "Mac", "Mac", "Android", "Android" };

	[NonSerialized]
	public static string[] deviceChars = new string[17]
	{
		"P1", "P3", "3S", "P4", "4S", "O1", "O2", "O3", "O4", "A1",
		"A2", "MS", "MH", "AL", "AH", "FL", "A3"
	};

	[NonSerialized]
	public static string[] deviceGens = new string[17]
	{
		"iPhone", "iPhone 3G", "iPhone 3GS", "iPhone 4", "iPhone 4S", "iPod 1", "iPod 2", "iPod 3", "iPod 4", "iPad 1",
		"iPad 2", "Mac", "Mac HD", "Android Low", "Android High", "Flash", "iPad 3"
	};

	[NonSerialized]
	public static int[] deviceResolution = new int[7] { 480, 960, 1024, 1024, 1680, 960, 1024 };

	[NonSerialized]
	private static string[] iconSizes = new string[7] { "small", "small", "small", "big", "big", "small", "small" };

	[NonSerialized]
	public static readonly float SNAP_FACTOR = (float)deviceResolution[0] / 4608f;

	[NonSerialized]
	public static int[] deviceMaxTexSize = new int[7] { 1024, 1024, 1024, 1024, 2048, 1024, 2048 };

	public static void reinit()
	{
		setDeviceType(DeviceType.DEVICE_ANDROID);
	}

	public static void init()
	{
		if (!INITED)
		{
			reinit();
			INITED = true;
		}
	}

	public static string getIconSize(DeviceType type)
	{
		return iconSizes[(int)type];
	}

	public static void setDeviceType(DeviceType type)
	{
		switch (type)
		{
		case DeviceType.DEVICE_IPHONE:
		case DeviceType.DEVICE_IPHONE2X:
		case DeviceType.DEVICE_IPAD:
			CURRENT_GEN = DeviceGeneration.GEN_IPAD2;
			break;
		case DeviceType.DEVICE_MAC:
			CURRENT_GEN = DeviceGeneration.GEN_MAC;
			break;
		case DeviceType.DEVICE_MAC_HD:
			CURRENT_GEN = DeviceGeneration.GEN_MAC_HD;
			break;
		case DeviceType.DEVICE_ANDROID:
			if (PlayerPrefs.GetInt("ANDROID_LOW_DEF") == 1)
			{
				CURRENT_GEN = DeviceGeneration.GEN_ANDROID_LOW;
			}
			else
			{
				CURRENT_GEN = DeviceGeneration.GEN_ANDROID_HIGH;
			}
			break;
		case DeviceType.DEVICE_FLASH:
			CURRENT_GEN = DeviceGeneration.GEN_FLASH;
			break;
		default:
			Debug.LogError("INVALID DEVICE TYPE! CHECK TYPE LIST AND UPDATE!!!");
			break;
		}
		CURRENT_DEVICE = type;
		TEXTURE_PIXELS_PER_METER = (float)deviceResolution[(int)type] / 18f;
	}

	public static DeviceType getDeviceType()
	{
		return CURRENT_DEVICE;
	}

	public static int getDeviceTypeInt()
	{
		return (int)getDeviceType();
	}

	public static DeviceGeneration getDeviceGeneration()
	{
		return CURRENT_GEN;
	}

	public static int getDeviceGenerationInt()
	{
		return (int)getDeviceGeneration();
	}

	public static string getDevicePath()
	{
		return devicePaths[(int)CURRENT_DEVICE];
	}

	public static string getIconPath()
	{
		return iconPaths[(int)CURRENT_DEVICE];
	}

	public static string getIconPath(DeviceType type)
	{
		return iconPaths[(int)type];
	}

	public static string getDevicePathForDevice(DeviceType type)
	{
		return devicePaths[(int)type];
	}
}
