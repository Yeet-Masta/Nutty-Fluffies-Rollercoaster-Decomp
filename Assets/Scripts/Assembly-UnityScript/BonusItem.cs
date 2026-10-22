using System;
using UnityEngine;

[Serializable]
public class BonusItem : MonoBehaviour
{
	public VGUITextTK2D multiplier;

	public VGUITextTK2D heartAmount;

	public virtual void setMultiplier(int mult)
	{
		if (multiplier != null)
		{
			if (mult <= 1)
			{
				multiplier.gameObject.SetActiveRecursively(false);
			}
			else
			{
				multiplier.setText("x" + mult);
			}
		}
	}

	public virtual void setHeartAmount(int amount)
	{
		if (heartAmount != null)
		{
			if (amount <= -1)
			{
				heartAmount.gameObject.SetActiveRecursively(false);
			}
			else
			{
				heartAmount.setText(string.Empty + amount);
			}
		}
	}
}
