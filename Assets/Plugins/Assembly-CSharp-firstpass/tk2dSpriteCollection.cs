using UnityEngine;

[AddComponentMenu("2D Toolkit/Backend/tk2dSpriteCollection")]
public class tk2dSpriteCollection : MonoBehaviour
{
	public enum TextureCompression
	{
		Uncompressed = 0,
		Reduced16Bit = 1,
		Compressed = 2
	}

	[HideInInspector]
	public tk2dSpriteCollectionDefinition[] textures;

	public Texture2D[] textureRefs;

	public tk2dSpriteSheetSource[] spriteSheets;

	public tk2dSpriteCollectionDefault defaults;

	[HideInInspector]
	public int maxTextureSize = 1024;

	[HideInInspector]
	public TextureCompression textureCompression;

	[HideInInspector]
	public int atlasWidth;

	[HideInInspector]
	public int atlasHeight;

	[HideInInspector]
	public float atlasWastage;

	[HideInInspector]
	public bool allowMultipleAtlases;

	[HideInInspector]
	public tk2dSpriteCollectionDefinition[] textureParams;

	public tk2dSpriteCollectionData spriteCollection;

	public bool premultipliedAlpha = true;

	public Material[] atlasMaterials;

	public Texture2D[] atlasTextures;

	public int targetHeight = 640;

	public float targetOrthoSize = 1f;

	public bool pixelPerfectPointSampled;

	public float physicsDepth = 0.1f;

	[HideInInspector]
	public int padAmount = -1;

	[HideInInspector]
	public bool autoUpdate = true;
}
