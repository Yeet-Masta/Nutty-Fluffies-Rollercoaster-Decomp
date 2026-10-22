using System;
using UnityEngine;

[Serializable]
public class StoreBaseMenu : BaseMenu
{
	[NonSerialized]
	public static float COMMON_PRICE_FACTOR = 1f;

	public StoreItem[] items;

	public bool updatePricesInActivation;

	public float priceFactor;

	public bool ignoreCommonPriceFactor;

	public StoreBaseMenu()
	{
		priceFactor = 1f;
	}

	public override void Awake()
	{
		StoreItem.DEFAULT_PRICES = GameData.DEFAULT_PRICES;
		int i = 0;
		StoreItem[] array = items;
		for (int length = array.Length; i < length; i++)
		{
			array[i].init();
		}
	}

	public override void activate()
	{
		base.activate();
		int i = 0;
		StoreItem[] array = items;
		for (int length = array.Length; i < length; i++)
		{
			if (updatePricesInActivation)
			{
				array[i].init();
			}
			if (!ignoreCommonPriceFactor)
			{
				array[i].price = (int)((float)array[i].originalPrice * priceFactor * COMMON_PRICE_FACTOR);
			}
			else
			{
				array[i].price = (int)((float)array[i].originalPrice * priceFactor);
			}
			array[i].updatePriceText();
		}
	}

	public virtual StoreItemButton[] instantiateStoreButtons()
	{
		StoreItemButton[] array = new StoreItemButton[items.Length];
		for (int i = 0; i < items.Length; i++)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(items[i].button.gameObject, Vector3.zero, Quaternion.identity);
			array[i] = gameObject.GetComponent<StoreItemButton>() as StoreItemButton;
		}
		return array;
	}

	public virtual void setPurchaseButtonsEnabled(bool enable)
	{
		for (int i = 0; i < items.Length; i++)
		{
			items[i].button.purchaseButton.setLocked(!enable);
		}
	}

	public virtual StoreItem getItem(string key)
	{
		int num = 0;
		StoreItem[] array = items;
		int length = array.Length;
		object result;
		while (true)
		{
			if (num < length)
			{
				if (array[num].itemKey == key)
				{
					result = array[num];
					break;
				}
				num++;
				continue;
			}
			if (GameData.LOG)
			{
				Debug.Log("StoreBaseMenu: ITEM " + key + " WAS NULL");
			}
			result = null;
			break;
		}
		return (StoreItem)result;
	}
}
