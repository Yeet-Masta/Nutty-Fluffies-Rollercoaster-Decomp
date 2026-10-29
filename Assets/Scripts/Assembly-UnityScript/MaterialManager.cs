using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

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
		if (RuntimeServices.EqualityOperator(instance, null))
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
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(materialList.Values);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is Material))
			{
				obj = RuntimeServices.Coerce(obj, typeof(Material));
			}
			Material material = (Material)obj;
			UnityEngine.Object.DestroyImmediate(material);
			UnityRuntimeServices.Update(enumerator, material);
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
