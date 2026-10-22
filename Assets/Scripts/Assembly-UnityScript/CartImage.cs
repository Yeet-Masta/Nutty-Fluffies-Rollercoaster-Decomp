using System;
using UnityEngine;

[Serializable]
public class CartImage : MonoBehaviour
{
	public GameObject[] cartImage;

	public GameObject[] rocketImage;

	public GameObject jumpRocket;

	public GameObject magnet;

	public GameObject removedImage;

	private Renderer[] renderers;

	private bool showRemoveImage;

	public CartImage()
	{
		renderers = new Renderer[4];
	}

	public virtual void activate(int cartType, int special)
	{
		renderers[0] = jumpRocket.GetComponent<Renderer>();
		renderers[1] = magnet.GetComponent<Renderer>();
		renderers[2] = cartImage[cartType].GetComponent<Renderer>();
		renderers[3] = jumpRocket.GetComponent<Renderer>();
		int i = 0;
		GameObject[] array = rocketImage;
		for (int length = array.Length; i < length; i++)
		{
			array[i].SetActiveRecursively(false);
		}
		int j = 0;
		GameObject[] array2 = cartImage;
		for (int length2 = array2.Length; j < length2; j++)
		{
			array2[j].SetActiveRecursively(false);
		}
		jumpRocket.SetActiveRecursively(special == 2);
		magnet.SetActiveRecursively(special == 1);
		cartImage[cartType].SetActiveRecursively(true);
		if (special == 0)
		{
			rocketImage[cartType].SetActiveRecursively(true);
			renderers[3] = rocketImage[cartType].GetComponent<Renderer>();
		}
		setAsUsed(!showRemoveImage);
	}

	public virtual void setAsUsed(bool to)
	{
		showRemoveImage = !to;
		if (removedImage != null)
		{
			removedImage.SetActiveRecursively(showRemoveImage);
		}
	}

	public virtual void setAlpha(float value)
	{
		int i = 0;
		Renderer[] array = renderers;
		for (int length = array.Length; i < length; i++)
		{
			if (array[i] != null)
			{
				SpriteUtil.setAlpha(array[i], value);
			}
		}
	}
}
