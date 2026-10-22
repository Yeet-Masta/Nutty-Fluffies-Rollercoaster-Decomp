using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class VGUIButtonImagesTK2D : VGUIButton
{
	public GameObject[] images;

	public virtual void setImage(int index)
	{
		if (index >= 0 && index < Extensions.get_length((System.Array)images))
		{
			int i = 0;
			GameObject[] array = images;
			for (int length = array.Length; i < length; i++)
			{
				array[i].SetActiveRecursively(false);
			}
			images[index].SetActiveRecursively(true);
		}
	}
}
