using System;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class VGUITrophyImage : VGUIImage
{
	public string trophyPrefsName;

	public VGUIElement lockedElement;

	public bool modifyAlpha;

	public float lockedAlpha;

	public Renderer imageElement;

	public VGUITrophyImage()
	{
		trophyPrefsName = string.Empty;
		modifyAlpha = true;
		lockedAlpha = 0.5f;
	}

	public virtual void OnEnable()
	{
		if (Extensions.get_length(trophyPrefsName) > 0)
		{
			Trophy trophy = Trophy.loadFromPrefs(trophyPrefsName);
			if (!RuntimeServices.EqualityOperator(trophy, null) && !(trophy.value <= 0f))
			{
				lockedElement.deactivate();
				int num = 1;
				Color color = imageElement.GetComponent<Renderer>().material.color;
				float num2 = (color.a = num);
				Color color2 = (imageElement.GetComponent<Renderer>().material.color = color);
			}
			else
			{
				lockedElement.activate();
				float a = lockedAlpha;
				Color color4 = imageElement.GetComponent<Renderer>().material.color;
				float num3 = (color4.a = a);
				Color color5 = (imageElement.GetComponent<Renderer>().material.color = color4);
				Debug.Log("mat " + imageElement.GetComponent<Renderer>().material.name);
			}
		}
	}
}
