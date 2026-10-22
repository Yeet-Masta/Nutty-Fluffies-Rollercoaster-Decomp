using UnityEngine;

[AddComponentMenu("2D Toolkit/Backend/tk2dFontData")]
public class tk2dFontData : MonoBehaviour
{
	public float lineHeight;

	public tk2dFontChar[] chars;

	public tk2dFontKerning[] kerning;

	public float largestWidth;

	public Material material;

	public string largeTexture;

	public Texture2D gradientTexture;

	public bool textureGradients;

	public int gradientCount = 1;
}
