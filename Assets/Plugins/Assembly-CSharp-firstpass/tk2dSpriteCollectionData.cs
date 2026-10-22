using UnityEngine;

[AddComponentMenu("2D Toolkit/Backend/tk2dSpriteCollectionData")]
public class tk2dSpriteCollectionData : MonoBehaviour
{
	public const int CURRENT_VERSION = 1;

	[HideInInspector]
	public int version;

	[HideInInspector]
	public tk2dSpriteDefinition[] spriteDefinitions;

	[HideInInspector]
	public bool premultipliedAlpha;

	[HideInInspector]
	public Material material;

	[HideInInspector]
	public Material[] materials;

	[HideInInspector]
	public Texture[] textures;

	[HideInInspector]
	public bool allowMultipleAtlases;

	[HideInInspector]
	public string spriteCollectionGUID;

	[HideInInspector]
	public string spriteCollectionName;

	[HideInInspector]
	public float invOrthoSize = 1f;

	[HideInInspector]
	public int buildKey;

	[HideInInspector]
	public string dataGuid = string.Empty;

	public string largeTexture;

	public int Count
	{
		get
		{
			return spriteDefinitions.Length;
		}
	}
}
