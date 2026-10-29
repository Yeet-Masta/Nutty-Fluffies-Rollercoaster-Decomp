using UnityEngine;

public class RemoveItemButton : MenuButton
{
	public GameObject xGraphic;

	public GameObject minusGraphic;

	public virtual void setMode(bool useX)
	{
		xGraphic.SetActiveRecursively(useX);
		minusGraphic.SetActiveRecursively(!useX);
	}
}
