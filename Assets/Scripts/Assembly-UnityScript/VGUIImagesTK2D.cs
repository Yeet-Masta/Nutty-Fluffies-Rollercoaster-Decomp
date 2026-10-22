using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class VGUIImagesTK2D : VGUIImage
{
	public GameObject[] images;

	public virtual void setImage(int index)
	{
		if (index < 0 || index >= Extensions.get_length((System.Array)images))
		{
			return;
		}
		int i = 0;
		GameObject[] array = images;
		for (int length = array.Length; i < length; i++)
		{
			if (array[i] != null)
			{
				array[i].SetActiveRecursively(false);
			}
		}
		if (images[index] != null)
		{
			images[index].SetActiveRecursively(true);
		}
	}

	public override void setActive(bool active)
	{
		int i = 0;
		GameObject[] array = images;
		for (int length = array.Length; i < length; i++)
		{
			array[i].SetActiveRecursively(active);
		}
	}
}
