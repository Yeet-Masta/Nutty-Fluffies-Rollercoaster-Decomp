using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityScript.Lang;

[Serializable]
public class ThemeData
{
	[NonSerialized]
	public const string DEFAULT_THEME_NAME = "woodTrackTheme";

	public ThemeSettings settings;

	public List<LevelLayerData> layerList;

	public ThemeData()
	{
		settings = new ThemeSettings();
		layerList = new List<LevelLayerData>();
	}

	public virtual LevelLayerData getLayerForNumber(int num)
	{
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(layerList);
		object result;
		while (true)
		{
			if (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				if (!(obj is LevelLayerData))
				{
					obj = RuntimeServices.Coerce(obj, typeof(LevelLayerData));
				}
				LevelLayerData levelLayerData = (LevelLayerData)obj;
				if (levelLayerData.layer == num)
				{
					result = levelLayerData;
					break;
				}
				continue;
			}
			result = null;
			break;
		}
		return (LevelLayerData)result;
	}
}
