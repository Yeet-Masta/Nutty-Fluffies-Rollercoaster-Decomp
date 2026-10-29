using UnityEngine;

public class VGUIImage : VGUIElement
{
	private Renderer imageRendered;

	private void findRenderer()
	{
		if (!(imageRendered != null))
		{
			imageRendered = (Renderer)GetComponent(typeof(Renderer));
			if (imageRendered == null)
			{
				imageRendered = (Renderer)GetComponentInChildren(typeof(Renderer));
			}
			if (imageRendered == null)
			{
				Debug.LogError("VGUIImage does not have Renderer! " + name);
			}
		}
	}

	public virtual void setMaterial(Material to)
	{
		findRenderer();
		imageRendered.material = to;
	}
}
