using System;
using UnityEngine;

[Serializable]
public class TextureTools
{
	public static Texture2D DrawLine(Texture2D tex, int x0, int y0, int x1, int y1, Color col)
	{
		int num = y1 - y0;
		int num2 = x1 - x0;
		int num3;
		if (num < 0)
		{
			num = -num;
			num3 = -1;
		}
		else
		{
			num3 = 1;
		}
		int num4;
		if (num2 < 0)
		{
			num2 = -num2;
			num4 = -1;
		}
		else
		{
			num4 = 1;
		}
		num <<= 1;
		num2 <<= 1;
		tex.SetPixel(x0, y0, col);
		if (num2 > num)
		{
			int num5 = num - (num2 >> 1);
			while (x0 != x1)
			{
				if (num5 >= 0)
				{
					y0 += num3;
					num5 -= num2;
				}
				x0 += num4;
				num5 += num;
				tex.SetPixel(x0, y0, col);
			}
		}
		else
		{
			int num5 = num2 - (num >> 1);
			while (y0 != y1)
			{
				if (num5 >= 0)
				{
					x0 += num4;
					num5 -= num;
				}
				y0 += num3;
				num5 += num2;
				tex.SetPixel(x0, y0, col);
			}
		}
		return tex;
	}

	public static Texture2D DrawCircle(Texture2D tex, int cx, int cy, int r, Color col)
	{
		int num = r;
		int num2 = -r;
		float num3 = Mathf.Ceil((float)r / Mathf.Sqrt(2f));
		for (int i = 0; (float)i <= num3; i++)
		{
			tex.SetPixel(cx + i, cy + num, col);
			tex.SetPixel(cx + i, cy - num, col);
			tex.SetPixel(cx - i, cy + num, col);
			tex.SetPixel(cx - i, cy - num, col);
			tex.SetPixel(cx + num, cy + i, col);
			tex.SetPixel(cx - num, cy + i, col);
			tex.SetPixel(cx + num, cy - i, col);
			tex.SetPixel(cx - num, cy - i, col);
			num2 += 2 * i + 1;
			if (num2 > 0)
			{
				num2 += 2 - 2 * num--;
			}
		}
		return tex;
	}
}
