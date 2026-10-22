using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class VGUIImages : VGUIImage
{
	public Material[] images;

	public Renderer rend;

	public virtual void setImage(int index)
	{
		if (index >= 0 && index < Extensions.get_length((System.Array)images))
		{
			rend.material = images[index];
		}
	}

	public virtual void setImageTo(Material mat)
	{
		rend.material = mat;
	}
}
