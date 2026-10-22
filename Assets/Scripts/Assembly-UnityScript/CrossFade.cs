using System;
using UnityEngine;

[Serializable]
public class CrossFade : MonoBehaviour
{
	[Serializable]
	public enum Mode
	{
		MODE_1_TO_2 = 0,
		MODE_2_TO_1 = 1
	}

	public Renderer object1;

	public Renderer object2;

	public bool initialActive;

	public float duration;

	public Mode mode;

	public float ob1Alpha;

	public float ob2Alpha;

	private float startTime;

	public CrossFade()
	{
		duration = 0.3f;
	}

	public virtual void Awake()
	{
		enabled = initialActive;
		if (enabled)
		{
			start(mode);
		}
	}

	public static void start(GameObject go, Mode mode)
	{
		CrossFade component = go.GetComponent<CrossFade>();
		component.start(mode);
	}

	public virtual void start(Mode mode)
	{
		this.mode = mode;
		if (!enabled)
		{
			startTime = Time.time;
		}
		enabled = true;
	}

	public virtual void Update()
	{
		float num = (Time.time - startTime) / duration;
		if (!(num < 1f))
		{
			num = 1f;
			enabled = false;
		}
		Color color = default(Color);
		if (mode == Mode.MODE_1_TO_2)
		{
			color = object1.material.GetColor("_LayerColor");
			color.a = 1f - num;
			object1.material.SetColor("_LayerColor", color);
			color = object2.material.GetColor("_LayerColor");
			color.a = num;
			object2.material.SetColor("_LayerColor", color);
		}
		else
		{
			color = object2.material.GetColor("_LayerColor");
			color.a = 1f - num;
			object2.material.SetColor("_LayerColor", color);
			color = object1.material.GetColor("_LayerColor");
			color.a = num;
			object1.material.SetColor("_LayerColor", color);
		}
	}
}
