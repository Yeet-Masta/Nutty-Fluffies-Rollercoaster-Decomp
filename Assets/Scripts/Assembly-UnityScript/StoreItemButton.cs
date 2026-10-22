using System;
using UnityEngine;

[Serializable]
public class StoreItemButton : MonoBehaviour
{
	public tk2dTextMesh priceText;

	public MenuButton purchaseButton;

	public tk2dTextMesh savePercentageText;

	public tk2dTextMesh savePercentageValueText;

	public tk2dTextMesh bestValueText1;

	public tk2dTextMesh bestValueText2;

	public tk2dTextMesh mostPopularText1;

	public tk2dTextMesh mostPopularText2;

	public virtual void setPrice(string price)
	{
		priceText.setText(price);
		if (bestValueText1 != null)
		{
			bestValueText1.setText(TextData.COINSTORE_NOTIFY_VALUE1);
		}
		if (bestValueText2 != null)
		{
			bestValueText2.setText(TextData.COINSTORE_NOTIFY_VALUE2);
		}
		if (mostPopularText1 != null)
		{
			mostPopularText1.setText(TextData.COINSTORE_NOTIFY_POPULAR1);
		}
		if (mostPopularText2 != null)
		{
			mostPopularText2.setText(TextData.COINSTORE_NOTIFY_POPULAR2);
		}
	}

	public virtual void setSavePercentage(int to, int sale)
	{
		if (savePercentageValueText != null)
		{
			savePercentageValueText.setText(to + sale + "%");
		}
		if (savePercentageText != null)
		{
			if (sale == 0)
			{
				savePercentageText.setText(TextData.COINSTORE_NOTIFY_BONUS);
			}
			else
			{
				savePercentageText.setText(TextData.COINSTORE_NOTIFY_SALE);
			}
		}
	}
}
