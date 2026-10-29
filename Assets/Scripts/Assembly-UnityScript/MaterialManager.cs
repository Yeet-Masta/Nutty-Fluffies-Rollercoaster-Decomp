using System;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;

[Serializable]
public class MaterialManager
{
	private static MaterialManager instance;

	private Dictionary<MaterialKey, Material> materialList = new Dictionary<MaterialKey, Material>();

	private MaterialManager()
	{
	}

	public static MaterialManager getInstance()
	{
		if (instance == null)
		{
			instance = new MaterialManager();
		}
		return instance;
	}

	public virtual Material getMaterial(Texture2D tex, bool alpha)
	{
		return getMaterialForUniqueID(tex, 0, alpha);
	}

	public virtual void purge()
	{
		foreach (Material material in materialList.Values)
		{
			UnityEngine.Object.DestroyImmediate(material);
		}
		materialList.Clear();
	}

	public virtual Material getMaterialForUniqueID(Texture2D tex, int id, bool alpha)
	{
		CustomProfiler.begin("getMaterialForUniqueID");
		MaterialKey key = new MaterialKey(tex, id);
		Material material = null;
		if (!materialList.ContainsKey(key))
		{
			material = new Material(alpha ? LevelDataPrefabs.getInstance().alphaShader : LevelDataPrefabs.getInstance().nonAlphaShader);
			material.mainTexture = tex;
			materialList[key] = material;
		}
		else
		{
			material = materialList[key];
		}
		CustomProfiler.end("getMaterialForUniqueID");
		return material;
	}
}
