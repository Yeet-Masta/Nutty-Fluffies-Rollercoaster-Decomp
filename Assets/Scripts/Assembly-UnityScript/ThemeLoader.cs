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
		JSONObject jSONObject = new JSONObject(str);
		themeData.settings.name = name;
		themeData.settings.musicNames.Clear();
		JSONObject field = jSONObject.GetField("music");
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(field.list);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is JSONObject))
			{
				obj = RuntimeServices.Coerce(obj, typeof(JSONObject));
			}
			JSONObject jSONObject2 = (JSONObject)obj;
			if (jSONObject2.str != null && jSONObject2.str.Length > 0 && !themeData.settings.musicNames.Contains(jSONObject2.str))
			{
				themeData.settings.musicNames.Add(jSONObject2.str);
				UnityRuntimeServices.Update(enumerator, jSONObject2);
			}
		}
		themeData.settings.allowedObjectPacks.Clear();
		JSONObject field2 = jSONObject.GetField("objectpacks");
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
				JSONObject jSONObject3 = (JSONObject)obj2;
				if (jSONObject3.str != null && jSONObject3.str.Length > 0 && !themeData.settings.allowedObjectPacks.Contains(jSONObject3.str))
				{
					themeData.settings.allowedObjectPacks.Add(jSONObject3.str);
					UnityRuntimeServices.Update(enumerator2, jSONObject3);
				}
			}
		}
		themeData.settings.customSettings.load(jSONObject);
		JSONObject field3 = jSONObject.GetField("layers");
		LevelLoader.getInstance().loadLayers(field3, themeData.layerList, null);
		return themeData;
	}

	public virtual ThemeData loadTheme(string name)
	{
		string text = null;
		try
		{
			string path = FileManager.getInstance().locateTheme(name);
			text = FileManager.getInstance().loadFileText(path);
		}
		catch (Exception ex)
		{
			Debug.LogWarning("Could not open theme '" + name + "': " + ex);
			return null;
		}
		return loadThemeFromString(text, name);
	}
}
