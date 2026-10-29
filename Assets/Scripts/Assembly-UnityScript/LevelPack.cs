using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class LevelPack
{
	[Serializable]
	public class LevelInfo
	{
		public string fileName;

		public int flags;

		public CustomSettings settings = new CustomSettings();

		public virtual bool isUnlocked()
		{
			return (flags & 1) != 0;
		}

		public virtual bool testFlag(int flag)
		{
			return (flags & flag) != 0;
		}

		public virtual void setFlag(int flag)
		{
			flags |= flag;
		}
	}

	public const int FLAG_UNLOCKED = 1;

	public const int FLAG_INVISIBLE = 2;

	public const int FLAG_EXTERNAL = 4;

	public const int FLAG_COMPLETED = 8;

	public static readonly int LEVEL_FLAG_MASK = 6;

	public static readonly int FLAG_RESERVED_FLAGS = 2147418112;

	public string fileName;

	public string packName;

	public List<LevelInfo> levelList = new List<LevelInfo>();

	public int flags;

	public int version;

	public float priceMultiplier;

	private static List<LevelPack> levelPacks;

	public static void refreshPacks()
	{
		levelPacks = new List<LevelPack>();
		foreach (string levelPackName in FileManager.getInstance().getLevelPackNames())
		{
			levelPacks.Add(load(levelPackName));
		}
	}

	public virtual int getLevelCount()
	{
		return levelList.Count;
	}

	public virtual LevelInfo getLevelInfo(int index)
	{
		return (index < 0 || index >= levelList.Count) ? null : levelList[index];
	}

	public virtual bool hasLevel(string name)
	{
		foreach (LevelInfo levelInfo in levelList)
		{
			if (levelInfo.fileName == name)
			{
				return true;
			}
		}
		return false;
	}

	public virtual int getLevelIndex(string name)
	{
		int index = 0;
		foreach (LevelInfo levelInfo in levelList)
		{
			if (levelInfo.fileName == name)
			{
				return index;
			}
			index++;
		}
		return -1;
	}

	public virtual LevelInfo[] getLevelList()
	{
		return levelList.ToArray();
	}

	public virtual bool isUnlocked()
	{
		return (flags & 1) != 0;
	}

	public virtual void unlock()
	{
		flags |= 1;
	}

	public virtual bool isExternal()
	{
		return (flags & 4) != 0;
	}

	public virtual void setExternal(bool ext)
	{
		flags &= -5;
		if (ext)
		{
			flags |= 4;
		}
	}

	public virtual void setCompleted()
	{
		flags |= 8;
	}

	public virtual void unlockLevel(int index)
	{
		if (index >= 0 && index < levelList.Count)
		{
			levelList[index].flags = levelList[index].flags | 1;
		}
	}

	public virtual void completeLevel(int index)
	{
		if (index >= 0 && index < levelList.Count)
		{
			levelList[index].flags = levelList[index].flags | 8;
		}
	}

	public virtual int getProgress()
	{
		if (levelList.Count == 0)
		{
			return 0;
		}
		else
		{
			float built = 0f;
			foreach (LevelInfo levelInfo in levelList)
			{
				if (levelInfo.testFlag(8))
				{
					built++;
				}
			}
			float fraction = built / (float)levelList.Count;
			return (int)Mathf.Ceil(fraction * 100f);
		}
	}

	private static LevelPack load(string name)
	{
		string rhs = FileManager.getInstance().locateLevelPack(name);
		LevelPack levelPack = new LevelPack();
		string json = null;
		NFPortAssetFile wWW = NFPortAssetFile.Open("" + rhs + "/levelpack.json");
		while (!wWW.isDone)
		{
		}
		if (wWW.error != null)
		{
			Debug.Log(wWW.error);
			levelPack.fileName = name;
			return levelPack;
		}
		else
		{
			json = wWW.text;
			JSONObject jSONObject = new JSONObject(json);
			levelPack.flags = (int)jSONObject.GetFieldSafe("flags", new JSONObject(0f)).n;
			levelPack.version = (int)jSONObject.GetFieldSafe("version", new JSONObject(0f)).n;
			levelPack.priceMultiplier = (float)jSONObject.GetFieldSafe("priceMultiplier", new JSONObject(1f)).n;
			levelPack.fileName = name;
			levelPack.packName = jSONObject.GetFieldSafe("displayName", new JSONObject("\"noname\"")).str;
			JSONObject field = jSONObject.GetField("levels");
			IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(field.list);
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				if (!(obj is JSONObject))
				{
					obj = RuntimeServices.Coerce(obj, typeof(JSONObject));
				}
				JSONObject jSONObject2 = (JSONObject)obj;
				LevelInfo levelInfo = new LevelInfo();
				levelInfo.fileName = jSONObject2.GetFieldSafe("level", new JSONObject(string.Empty)).str;
				UnityRuntimeServices.Update(enumerator, jSONObject2);
				levelInfo.flags = (int)jSONObject2.GetFieldSafe("flags", new JSONObject(0f)).n;
				UnityRuntimeServices.Update(enumerator, jSONObject2);
				levelInfo.flags &= LEVEL_FLAG_MASK;
				levelInfo.settings.load(jSONObject2);
				UnityRuntimeServices.Update(enumerator, jSONObject2);
				levelPack.levelList.Add(levelInfo);
			}
			return levelPack;
		}
	}

	public static List<LevelPack> getLevelPackList()
	{
		if (levelPacks == null)
		{
			refreshPacks();
		}
		return levelPacks;
	}

	public static LevelPack getNamedLevelPack(string name)
	{
		if (levelPacks == null)
		{
			refreshPacks();
		}
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(levelPacks);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is LevelPack))
			{
				obj = RuntimeServices.Coerce(obj, typeof(LevelPack));
			}
			LevelPack levelPack = (LevelPack)obj;
			if (levelPack.fileName == name)
			{
				return levelPack;
			}
		}
		return null;
	}
}
