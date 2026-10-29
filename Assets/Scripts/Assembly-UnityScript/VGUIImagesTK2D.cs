using UnityEngine;

public class VGUIImagesTK2D : VGUIImage
{
	public GameObject[] images;

	public virtual void setImage(int index)
	{
		if (index < 0 || index >= images.Length)
		{
			return;
		}
		foreach (GameObject image in images)
		{
			if (image != null)
			{
				image.SetActiveRecursively(false);
			}
		}
		if (images[index] != null)
		{
			images[index].SetActiveRecursively(true);
		}
	}

	public override void setActive(bool active)
	{
		foreach (GameObject image in images)
		{
			image.SetActiveRecursively(active);
		}
	}
}
