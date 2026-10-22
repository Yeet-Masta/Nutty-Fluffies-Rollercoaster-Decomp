using System;
using UnityEngine;

[Serializable]
public class SpriteUtil
{
	public static void setAlpha(Renderer r, float alpha)
	{
		if (r != null && r.material.HasProperty("_MainColor"))
		{
			Color color = r.material.GetColor("_MainColor");
			color.a = alpha;
			r.material.SetColor("_MainColor", color);
		}
	}

	public static void setColor(Renderer renderer, float r, float g, float b)
	{
		if (renderer.material.HasProperty("_MainColor"))
		{
			Color color = renderer.material.GetColor("_MainColor");
			color.r = r;
			color.g = g;
			color.b = b;
			renderer.material.SetColor("_MainColor", color);
		}
	}
}
