using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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
		foreach (string path in levelPackPaths)
		{
			levelPackNames.Add(getFileName(path));
		}
		foreach (string path in objectPackPaths)
		{
			if (GameData.LOG)
			{
				Debug.Log("Object pack found: " + getFileName(path));
			}
			objectPackNames.Add(getFileName(path));
		}
		foreach (string path in themePackPaths)
		{
			themePackNames.Add(getFileName(path));
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
		string path = null;
		foreach (string packPath in objectPackPaths)
		{
			path = packPath + "/textures/" + CommonData.getDevicePath() + "/" + TextureManager.texturePathForType(type) + "/" + name;
			if (loadWWWFile(path) != null)
			{
				return path;
			}
		}
		Debug.LogWarning("Did not find texture (" + TextureManager.texturePathForType(type) + ") '" + name + "'!");
		return string.Empty;
	}

	public virtual string locateObjectPack(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return string.Empty;
		}
		foreach (string path in objectPackPaths)
		{
			if (getFileName(path) == name)
			{
				return path;
			}
		}
		Debug.LogWarning("Did not find object pack '" + name + "'!");
		return string.Empty;
	}

	public virtual string locateLevelPack(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return string.Empty;
		}
		foreach (string path in levelPackPaths)
		{
			if (getFileName(path) == name)
			{
				return path;
			}
		}
		Debug.LogWarning("Did not find level pack '" + name + "'!");
		return string.Empty;
	}

	public virtual string locateLevel(string pack, string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return string.Empty;
		}
		string packPath = locateLevelPack(pack);
		return string.IsNullOrEmpty(packPath) ? string.Empty : (packPath + "/levels/" + name + ".json");
	}

	public virtual string locateTheme(string name)
	{
		if (string.IsNullOrEmpty(name))
		{
			return string.Empty;
		}
		foreach (string path in themePackPaths)
		{
			if (getFileName(path) == name)
			{
				return path + "/theme.json";
			}
		}
		Debug.LogWarning("Did not find theme '" + name + "'!");
		return string.Empty;
	}

	public virtual string loadFileText(string path)
	{
		NFPortAssetFile www = loadWWWFile(path);
		return www == null ? null : www.text;
	}

	public virtual byte[] loadFileBytes(string path)
	{
		NFPortAssetFile www = loadWWWFile(path);
		return www == null ? null : www.bytes;
	}

	private NFPortAssetFile loadWWWFile(string path)
	{
		NFPortAssetFile www = NFPortAssetFile.Open("" + path);
		while (!www.isDone)
		{
		}
		if (www.error != null)
		{
			Debug.Log(www.error);
			return null;
		}
		return www;
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
		NFPortAssetFile www = NFPortAssetFile.Open("GameResources/" + dir + "/packs.txt");
		while (!www.isDone)
		{
		}
		StringReader stringReader = new StringReader(www.text);
		string line = stringReader.ReadLine();
		while (line != null)
		{
			packList.Add("GameResources/" + dir + "/" + line);
			line = stringReader.ReadLine();
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
