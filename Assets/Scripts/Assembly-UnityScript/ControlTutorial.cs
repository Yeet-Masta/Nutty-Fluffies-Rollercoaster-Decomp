using Boo.Lang.Runtime;
using UnityEngine;

public class ControlTutorial : TutorialPage
{
	public GameObject[] specialGraphics;

	public GameObject[] specialExtra;

	public override void activate()
	{
		base.activate();
		int specialCartType = GameManager.getInstance().getSpecialCartType();
		if (!RuntimeServices.EqualityOperator(specialGraphics, null))
		{
			for (int i = 0; i < specialGraphics.Length; i++)
			{
				specialGraphics[i].SetActiveRecursively(i == specialCartType);
			}
		}
		if (specialCartType == -1 && !RuntimeServices.EqualityOperator(specialExtra, null))
		{
			int j = 0;
			GameObject[] array = specialExtra;
			for (int length = array.Length; j < length; j++)
			{
				array[j].SetActiveRecursively(false);
			}
		}
		shown = false;
	}
}
