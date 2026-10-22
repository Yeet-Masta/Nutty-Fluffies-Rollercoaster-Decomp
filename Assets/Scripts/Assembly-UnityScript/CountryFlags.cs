using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class CountryFlags : MonoBehaviour
{
	public Material[] materials;

	public string materialTexturePrefix;

	public string materialTextureSuffix;

	public Material notFoundMaterial;

	private int nextMaterialIndex;

	public virtual void startNewFlagSet()
	{
		nextMaterialIndex = 0;
	}

	public virtual Material getFlagMaterial(string forCountryCode)
	{
		Material material = materials[nextMaterialIndex];
		string path = materialTexturePrefix + forCountryCode + materialTextureSuffix;
		Texture2D texture2D = (Texture2D)Resources.Load(path, typeof(Texture2D));
		if (texture2D == null)
		{
			material = notFoundMaterial;
		}
		else
		{
			material.mainTexture = texture2D;
		}
		nextMaterialIndex = (nextMaterialIndex + 1) % Extensions.get_length((System.Array)materials);
		return material;
	}
}
