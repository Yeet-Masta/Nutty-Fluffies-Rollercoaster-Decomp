using System;
using System.Collections.Generic;

[Serializable]
public class ThemeData
{
	public const string DEFAULT_THEME_NAME = "woodTrackTheme";

	public ThemeSettings settings = new ThemeSettings();

	public List<LevelLayerData> layerList = new List<LevelLayerData>();

	public virtual LevelLayerData getLayerForNumber(int num)
	{
		foreach (LevelLayerData levelLayerData in layerList)
		{
			if (levelLayerData.layer == num)
			{
				return levelLayerData;
			}
		}
		return null;
	}
}
