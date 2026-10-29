using System;
using UnityEngine;

[Serializable]
public class StoreItem
{
	public static DefaultPrice[] DEFAULT_PRICES = new DefaultPrice[0];

	public int price;

	public StoreItemButton button;

	public string itemKey;

	[HideInInspector]
	public int originalPrice;

	public StoreItem()
	{
		originalPrice = -1;
		originalPrice = -1;
	}

	public virtual void init()
	{
		int defaultPrice = getDefaultPrice(itemKey);
		if (defaultPrice == -1)
		{
			defaultPrice = price;
		}
		price = PlayerPrefs.GetInt(itemKey, defaultPrice);
		if (originalPrice == -1)
		{
			originalPrice = price;
		}
		updatePriceText();
	}

	public virtual void updatePriceText()
	{
		if (button != null)
		{
			if (price == 0)
			{
				button.setPrice(LocalizationManager.localize("ITEM_TEXT_FREE"));
			}
			else
			{
				button.setPrice(GameData.formatTokenValue(string.Empty + price));
			}
		}
	}

	public static int getDefaultPrice(string id)
	{
		int num = 0;
		DefaultPrice[] dEFAULT_PRICES = DEFAULT_PRICES;
		int length = dEFAULT_PRICES.Length;
		while (num < length)
		{
			if (dEFAULT_PRICES[num].id == id)
			{
				return dEFAULT_PRICES[num].price;
			}
			num++;
		}
		if (GameData.LOG)
		{
			Debug.Log(id + " NOT FOUND");
		}
		return -1;
	}
}
