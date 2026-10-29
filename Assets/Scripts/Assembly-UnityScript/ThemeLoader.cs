using System;
using System.Collections;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

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
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(field.list);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is JSONObject))
			{
				obj = RuntimeServices.Coerce(obj, typeof(JSONObject));
			}
			JSONObject json2 = (JSONObject)obj;
			if (json2.str != null && json2.str.Length > 0 && !themeData.settings.musicNames.Contains(json2.str))
			{
				themeData.settings.musicNames.Add(json2.str);
				UnityRuntimeServices.Update(enumerator, json2);
			}
		}
		themeData.settings.allowedObjectPacks.Clear();
		JSONObject field2 = json.GetField("objectpacks");
		if (field2 != null)
		{
			IEnumerator enumerator2 = UnityRuntimeServices.GetEnumerator(field2.list);
			while (enumerator2.MoveNext())
			{
				object obj2 = enumerator2.Current;
				if (!(obj2 is JSONObject))
				{
					obj2 = RuntimeServices.Coerce(obj2, typeof(JSONObject));
				}
				JSONObject json3 = (JSONObject)obj2;
				if (json3.str != null && json3.str.Length > 0 && !themeData.settings.allowedObjectPacks.Contains(json3.str))
				{
					themeData.settings.allowedObjectPacks.Add(json3.str);
					UnityRuntimeServices.Update(enumerator2, json3);
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
