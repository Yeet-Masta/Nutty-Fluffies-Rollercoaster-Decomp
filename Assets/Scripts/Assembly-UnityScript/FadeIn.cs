using System;
using UnityEngine;

[Serializable]
public class FadeIn : MonoBehaviour
{
	public Material material;

	public float duration;

	public GameObject invokeObject;

	public string invokeMethod;

	protected int frames;

	public FadeIn()
	{
		duration = 1f;
	}

	public virtual void Awake()
	{
		material = new Material(material);
		gameObject.GetComponent<Renderer>().material = material;
		float a = 1f;
		Color color = material.color;
		float num = (color.a = a);
		Color color2 = (material.color = color);
	}

	public virtual void Update()
	{
		frames++;
		if (frames >= 2)
		{
			if (!(material.color.a <= 0f))
			{
				float a = material.color.a - Time.deltaTime / duration;
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

	public virtual void setVisible(bool to)
	{
		gameObject.SetActiveRecursively(to);
	}

	public virtual void start()
	{
		Awake();
		setVisible(true);
		frames = 0;
		enabled = true;
	}

	public virtual void setInvokeCallBack(GameObject target, string method)
	{
		invokeObject = target;
		invokeMethod = method;
	}
}
