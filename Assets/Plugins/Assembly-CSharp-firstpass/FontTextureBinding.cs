using UnityEngine;

public class FontTextureBinding
{
	private static int txWidth;

	private static int txHeight;

	private static int realWidth;

	private static int realHeight;

	private static Texture2D texture;

	private static void renderFontToTexture(string text)
	{
	}

	private static void prepareFontTexture(int textureWidth, int textureHeight, int width, int height, int fontPointSize, string name)
	{
		txWidth = textureWidth;
		txHeight = textureHeight;
		realWidth = width;
		realHeight = height;
	}

	public static void initFontTexture(Texture2D tex, int width, int height, int fontPointSize, string name)
	{
		texture = tex;
		prepareFontTexture(texture.width, texture.height, width, height, fontPointSize, name);
	}

	public static void uninitFontTexture()
	{
	}

	public static void refreshFontTexture(string text)
	{
		Color[] array = new Color[txWidth * txHeight];
		for (int i = 0; i < txHeight; i++)
		{
			for (int j = 0; j < txWidth; j++)
			{
				if (j > 0 && i > 0 && j < realWidth - 1 && i < realHeight - 1)
				{
					array[i * txWidth + j] = Color.white;
				}
				else if (j < realWidth && i < realHeight)
				{
					array[i * txWidth + j] = Color.black;
				}
				else
				{
					array[i * txWidth + j] = new Color(0f, 0f, 0f, 0f);
				}
			}
		}
		texture.SetPixels(array, 0);
		texture.Apply(false);
	}
}
