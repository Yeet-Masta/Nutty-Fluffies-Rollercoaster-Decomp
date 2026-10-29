using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class FileManager
{
	private List<string> levelPackPaths;

	private List<string> objectPackPaths;

	private List<string> themePackPaths;

	private List<string> levelPackNames;

	private List<string> objectPackNames;

	private List<string> themePackNames;

	private static FileManager instance;

	protected FileManager()
	{
		levelPackPaths = new List<string>();
		objectPackPaths = new List<string>();
		themePackPaths = new List<string>();
		levelPackNames = new List<string>();
		objectPackNames = new List<string>();
		themePackNames = new List<string>();
		addPacks("levelpacks", levelPackPaths);
		addPacks("objectpacks", objectPackPaths);
		addPacks("themepacks", themePackPaths);
		foreach (string text in levelPackPaths)
		{
			levelPackNames.Add(getFileName(text));
		}
		foreach (string text2 in objectPackPaths)
		{
			if (GameData.LOG)
			{
				Debug.Log("Object pack found: " + getFileName(text2));
			}
			objectPackNames.Add(getFileName(text2));
		}
		foreach (string text3 in themePackPaths)
		{
			themePackNames.Add(getFileName(text3));
		}
	}

	public static FileManager getInstance()
	{
		if (instance == null)
		{
			instance = new FileManager();
		}
		return instance;
	}

	public virtual string locateAnyFile(string filename)
	{
		return "GameResources/" + filename;
	}

	public static string getDataDirectory()
	{
		return Application.dataPath.Replace("/Data", "/GameResources");
	}

	public static string getEditorLocalDataDirectory()
	{
		return "GameResources";
	}

	public static string getEditorPackExportDirectory()
	{
		return "ExportedPackages";
	}

	public static string getEditorTempPath()
	{
		return getEditorLocalDataDirectory() + "/EditorTemp";
	}

	public static string getGameMusicPath()
	{
		return "Music/Game";
	}

	public static string getPersistentDataDirectory()
	{
		return Application.persistentDataPath + "/GameResources";
	}

	public virtual string[] getObjectPackNames()
	{
		return objectPackNames.ToArray();
	}

	public virtual string[] getLevelPackNames()
	{
		return levelPackNames.ToArray();
	}

	public virtual string[] getThemePackNames()
	{
		return themePackNames.ToArray();
	}

	public virtual string locateTexture(string name, TextureManager.TextureType type)
	{
		string text = null;
		foreach (string text2 in objectPackPaths)
		{
			text = text2 + "/textures/" + CommonData.getDevicePath() + "/" + TextureManager.texturePathForType(type) + "/" + name;
			if (loadWWWFile(text) != null)
			{
				return text;
			}
		}
		Debug.LogWarning("Did not find texture (" + TextureManager.texturePathForType(type) + ") '" + name + "'!");
		return string.Empty;
	}

	public virtual string locateObjectPack(string name)
	{
		string result;
		if (string.IsNullOrEmpty(name))
		{
			return string.Empty;
		}
		else
		{
			IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(objectPackPaths);
			while (true)
			{
				if (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					if (!(obj is string))
					{
						obj = RuntimeServices.Coerce(obj, typeof(string));
					}
					string text = (string)obj;
					if (getFileName(text) == name)
					{
						result = text;
						break;
					}
					continue;
				}
				Debug.LogWarning("Did not find object pack '" + name + "'!");
				result = string.Empty;
				break;
			}
		}
		return result;
	}

	public virtual string locateLevelPack(string name)
	{
		string result;
		if (string.IsNullOrEmpty(name))
		{
			return string.Empty;
		}
		else
		{
			IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(levelPackPaths);
			while (true)
			{
				if (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					if (!(obj is string))
					{
						obj = RuntimeServices.Coerce(obj, typeof(string));
					}
					string text = (string)obj;
					if (getFileName(text) == name)
					{
						result = text;
						break;
					}
					continue;
				}
				Debug.LogWarning("Did not find level pack '" + name + "'!");
				result = string.Empty;
				break;
			}
		}
		return result;
	}

	public virtual string locateLevel(string pack, string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return string.Empty;
		}
		else
		{
			string text = locateLevelPack(pack);
			return string.IsNullOrEmpty(text) ? string.Empty : (text + "/levels/" + name + ".json");
		}
	}

	public virtual string locateTheme(string name)
	{
		string result;
		if (string.IsNullOrEmpty(name))
		{
			return string.Empty;
		}
		else
		{
			IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(themePackPaths);
			while (true)
			{
				if (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					if (!(obj is string))
					{
						obj = RuntimeServices.Coerce(obj, typeof(string));
					}
					string text = (string)obj;
					if (getFileName(text) == name)
					{
						result = text + "/theme.json";
						break;
					}
					continue;
				}
				Debug.LogWarning("Did not find theme '" + name + "'!");
				result = string.Empty;
				break;
			}
		}
		return result;
	}

	public virtual string loadFileText(string path)
	{
		NFPortAssetFile wWW = loadWWWFile(path);
		return wWW == null ? null : wWW.text;
	}

	public virtual byte[] loadFileBytes(string path)
	{
		NFPortAssetFile wWW = loadWWWFile(path);
		return wWW == null ? null : wWW.bytes;
	}

	private NFPortAssetFile loadWWWFile(string path)
	{
		NFPortAssetFile wWW = NFPortAssetFile.Open("" + path);
		while (!wWW.isDone)
		{
		}
		if (wWW.error != null)
		{
			Debug.Log(wWW.error);
			return null;
		}
		else
		{
			return wWW;
		}
	}

	private string[] getFoldersFullPaths(string dir)
	{
		if (Directory.Exists(dir))
		{
			return Directory.GetDirectories(dir);
		}
		return new string[0];
	}

	private void addPacks(string dir, List<string> packList)
	{
		NFPortAssetFile wWW = NFPortAssetFile.Open("GameResources/" + dir + "/packs.txt");
		while (!wWW.isDone)
		{
		}
		StringReader stringReader = new StringReader(wWW.text);
		string text = stringReader.ReadLine();
		while (text != null)
		{
			packList.Add("GameResources/" + dir + "/" + text);
			text = stringReader.ReadLine();
		}
		packList.AddRange(getFoldersFullPaths(getDataDirectory() + "/" + dir));
		packList.AddRange(getFoldersFullPaths(getPersistentDataDirectory() + "/" + dir));
		packList.AddRange(getFoldersFullPaths(getEditorLocalDataDirectory() + "/" + dir));
	}

	public static string getDirectoryName(string path)
	{
		return Path.GetDirectoryName(path);
	}

	public static string getFileName(string path)
	{
		return Path.GetFileName(path);
	}
}
