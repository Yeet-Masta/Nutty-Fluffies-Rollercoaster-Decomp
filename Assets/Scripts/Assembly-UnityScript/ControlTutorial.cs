using UnityEngine;

public class ControlTutorial : TutorialPage
{
	public GameObject[] specialGraphics;

	public GameObject[] specialExtra;

	public override void activate()
	{
		base.activate();
		int specialCartType = GameManager.getInstance().getSpecialCartType();
		if (specialGraphics != null)
		{
			for (int i = 0; i < specialGraphics.Length; i++)
			{
				specialGraphics[i].SetActiveRecursively(i == specialCartType);
			}
		}
		if (specialCartType == -1 && specialExtra != null)
		{
			foreach (GameObject item in specialExtra)
			{
				item.SetActiveRecursively(false);
			}
		}
		shown = false;
	}
}
