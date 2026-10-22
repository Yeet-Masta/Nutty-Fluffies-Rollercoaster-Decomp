using System;
using UnityEngine;

[Serializable]
public class FadeOut : FadeIn
{
	public override void Awake()
	{
		base.Awake();
		float a = 0f;
		Color color = material.color;
		float num = (color.a = a);
		Color color2 = (material.color = color);
	}

	public override void Update()
	{
		frames++;
		if (frames >= 2)
		{
			if (!(material.color.a >= 1f))
			{
				float a = material.color.a + Time.deltaTime / duration;
				Color color = material.color;
				float num = (color.a = a);
				Color color2 = (material.color = color);
			}
			else if (invokeObject != null)
			{
				invokeObject.SendMessage(invokeMethod);
				enabled = false;
			}
		}
	}

	public override void start()
	{
		Awake();
		base.start();
		float a = 0f;
		Color color = material.color;
		float num = (color.a = a);
		Color color2 = (material.color = color);
	}
}
