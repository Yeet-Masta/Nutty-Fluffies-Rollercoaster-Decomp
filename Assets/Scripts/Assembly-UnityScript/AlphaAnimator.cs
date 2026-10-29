using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

public class AlphaAnimator : VGUIAnimator
{
	public float from;

	public float to;

	private List<Renderer> rendererList;

	private bool complete;

	public float currentValue;

	public float cip;

	protected override void activateInternal()
	{
		rendererList = new List<Renderer>();
		complete = false;
		object[] componentsInChildren = target.gameObject.GetComponentsInChildren(typeof(Renderer), true);
		int i = 0;
		object[] array = componentsInChildren;
		for (int length = array.Length; i < length; i++)
		{
			Renderer item = array[i] as Renderer;
			rendererList.Add(item);
		}
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(rendererList);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is Renderer))
			{
				obj = RuntimeServices.Coerce(obj, typeof(Renderer));
			}
			Renderer renderer = (Renderer)obj;
			renderer.material = new Material(renderer.material);
			UnityRuntimeServices.Update(enumerator, renderer);
			renderer.material.name = "mat";
			UnityRuntimeServices.Update(enumerator, renderer);
		}
	}

	public override void Update()
	{
		float timeInterpolator = getTimeInterpolator();
		currentValue = from + (to - from) * timeInterpolator;
		if (rendererList != null)
		{
			for (int i = 0; i < rendererList.Count; i++)
			{
				SpriteUtil.setAlpha(rendererList[i], currentValue);
			}
		}
		cip = timeInterpolator;
		if (!complete)
		{
			if ((!reverse && timeInterpolator >= 1f) || (reverse && !(timeInterpolator > 0f)))
			{
				complete = true;
			}
		}
		else if (complete)
		{
			checkComplete(timeInterpolator);
		}
	}

	public override void deactivate()
	{
		if (!enabled)
		{
			return;
		}
		if (rendererList != null)
		{
			for (int i = 0; i < rendererList.Count; i++)
			{
				if (!reverse)
				{
					SpriteUtil.setAlpha(rendererList[i], to);
				}
				else
				{
					SpriteUtil.setAlpha(rendererList[i], from);
				}
			}
		}
		base.deactivate();
		complete = false;
	}
}
