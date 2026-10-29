using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelLayerData
{
	[Serializable]
	public class ThemeLayerInfo
	{
		public string layerName;

		public Vector2 parallaxSpeed;

		public Vector3 colorMultiplier;

		public int flags;

		public ThemeLayerInfo(string name, Vector2 parallax, Vector3 color, int flgs)
		{
			layerName = name;
			parallaxSpeed = parallax;
			colorMultiplier = color;
			flags = flgs;
		}
	}

	public List<LevelEntityData> objectData;

	public int layer;

	public ThemeLayerInfo themeInfo;

	public int deviceList;

	public LevelLayerData(int layerId)
	{
		objectData = new List<LevelEntityData>();
		themeInfo = new ThemeLayerInfo("<untitled>", Vector2.one, new Vector3(0.5f, 0.5f, 0.5f), 0);
		layer = layerId;
	}

	public virtual bool checkDevice(int dev)
	{
		return (deviceList & dev) != 0;
	}

	public virtual void setDevice(int dev, bool state)
	{
		deviceList &= ~dev;
		if (state)
		{
			deviceList |= dev;
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
