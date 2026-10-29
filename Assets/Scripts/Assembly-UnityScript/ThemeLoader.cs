using System;
using UnityEngine;

[Serializable]
public class ThemeLoader
{
	public static ThemeLoader instance;

	public static ThemeLoader getInstance()
	{
		if (instance == null)
		{
			instance = new ThemeLoader();
		}
		return instance;
	}

	public virtual ThemeData createTestTheme(string name)
	{
		ThemeData themeData = new ThemeData();
		LevelLoader.getInstance().createTestLayerAndContents(themeData.layerList);
		return themeData;
	}

	public virtual ThemeData loadThemeFromString(string str, string name)
	{
		ThemeData themeData = new ThemeData();
		JSONObject json = new JSONObject(str);
		themeData.settings.name = name;
		themeData.settings.musicNames.Clear();
		JSONObject field = json.GetField("music");
		foreach (JSONObject json2 in field.list)
		{
			if (json2.str != null && json2.str.Length > 0 && !themeData.settings.musicNames.Contains(json2.str))
			{
				themeData.settings.musicNames.Add(json2.str);
			}
		}
		themeData.settings.allowedObjectPacks.Clear();
		JSONObject field2 = json.GetField("objectpacks");
		if (field2 != null)
		{
			foreach (JSONObject json3 in field2.list)
			{
				if (json3.str != null && json3.str.Length > 0 && !themeData.settings.allowedObjectPacks.Contains(json3.str))
				{
					themeData.settings.allowedObjectPacks.Add(json3.str);
				}
			}
		}
		themeData.settings.customSettings.load(json);
		JSONObject field3 = json.GetField("layers");
		LevelLoader.getInstance().loadLayers(field3, themeData.layerList, null);
		return themeData;
	}

	public virtual ThemeData loadTheme(string name)
	{
		string json = null;
		try
		{
			string path = FileManager.getInstance().locateTheme(name);
			json = FileManager.getInstance().loadFileText(path);
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Could not open theme '" + name + "': " + ex);
			return null;
		}
		return loadThemeFromString(json, name);
	}
}
