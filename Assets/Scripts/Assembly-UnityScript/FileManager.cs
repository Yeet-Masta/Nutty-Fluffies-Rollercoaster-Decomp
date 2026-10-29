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
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(levelPackPaths);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is string))
			{
				obj = RuntimeServices.Coerce(obj, typeof(string));
			}
			string text = (string)obj;
			levelPackNames.Add(getFileName(text));
			UnityRuntimeServices.Update(enumerator, text);
		}
		IEnumerator enumerator2 = UnityRuntimeServices.GetEnumerator(objectPackPaths);
		while (enumerator2.MoveNext())
		{
			object obj2 = enumerator2.Current;
			if (!(obj2 is string))
			{
				obj2 = RuntimeServices.Coerce(obj2, typeof(string));
			}
			string text2 = (string)obj2;
			if (GameData.LOG)
			{
				Debug.Log("Object pack found: " + getFileName(text2));
				UnityRuntimeServices.Update(enumerator2, text2);
			}
			objectPackNames.Add(getFileName(text2));
			UnityRuntimeServices.Update(enumerator2, text2);
		}
		IEnumerator enumerator3 = UnityRuntimeServices.GetEnumerator(themePackPaths);
		while (enumerator3.MoveNext())
		{
			object obj3 = enumerator3.Current;
			if (!(obj3 is string))
			{
				obj3 = RuntimeServices.Coerce(obj3, typeof(string));
			}
			string text3 = (string)obj3;
			themePackNames.Add(getFileName(text3));
			UnityRuntimeServices.Update(enumerator3, text3);
		}
	}

	public static FileManager getInstance()
	{
		if (RuntimeServices.EqualityOperator(instance, null))
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
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(objectPackPaths);
		string result;
		while (true)
		{
			if (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				if (!(obj is string))
				{
					obj = RuntimeServices.Coerce(obj, typeof(string));
				}
				string text2 = (string)obj;
				text = text2 + "/textures/" + CommonData.getDevicePath() + "/" + TextureManager.texturePathForType(type) + "/" + name;
				UnityRuntimeServices.Update(enumerator, text2);
				if (!RuntimeServices.EqualityOperator(loadWWWFile(text), null))
				{
					result = text;
					break;
				}
				continue;
			}
			Debug.LogWarning("Did not find texture (" + TextureManager.texturePathForType(type) + ") '" + name + "'!");
			result = string.Empty;
			break;
		}
		return result;
	}

	public virtual string locateObjectPack(string name)
	{
		string result;
		if (string.IsNullOrEmpty(name))
		{
			result = string.Empty;
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
			result = string.Empty;
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
		string result;
		if (string.IsNullOrEmpty(name))
		{
			result = string.Empty;
		}
		else
		{
			string text = locateLevelPack(pack);
			result = string.IsNullOrEmpty(text) ? string.Empty : (text + "/levels/" + name + ".json");
		}
		return result;
	}

	public virtual string locateTheme(string name)
	{
		string result;
		if (string.IsNullOrEmpty(name))
		{
			result = string.Empty;
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
		return RuntimeServices.EqualityOperator(wWW, null) ? null : wWW.text;
	}

	public virtual byte[] loadFileBytes(string path)
	{
		NFPortAssetFile wWW = loadWWWFile(path);
		return RuntimeServices.EqualityOperator(wWW, null) ? null : wWW.bytes;
	}

	private NFPortAssetFile loadWWWFile(string path)
	{
		NFPortAssetFile wWW = NFPortAssetFile.Open("" + path);
		while (!wWW.isDone)
		{
		}
		object result;
		if (wWW.error != null)
		{
			Debug.Log(wWW.error);
			result = null;
		}
		else
		{
			result = wWW;
		}
		return (NFPortAssetFile)result;
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
