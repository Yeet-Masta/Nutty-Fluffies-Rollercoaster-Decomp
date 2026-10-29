using UnityEngine;

public class StoreBaseMenu : BaseMenu
{
	public static float COMMON_PRICE_FACTOR = 1f;

	public StoreItem[] items;

	public bool updatePricesInActivation;

	public float priceFactor = 1f;

	public bool ignoreCommonPriceFactor;

	public override void Awake()
	{
		StoreItem.DEFAULT_PRICES = GameData.DEFAULT_PRICES;
		foreach (StoreItem item in items)
		{
			item.init();
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
		StoreItemButton[] buttons = new StoreItemButton[items.Length];
		for (int i = 0; i < items.Length; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(items[i].button.gameObject, Vector3.zero, Quaternion.identity);
			buttons[i] = gameObject.GetComponent<StoreItemButton>();
		}
		return buttons;
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
		int index = 0;
		StoreItem[] array = items;
		int length = array.Length;
		while (index < length)
		{
			if (array[index].itemKey == key)
			{
				return array[index];
			}
			index++;
		}
		if (GameData.LOG)
		{
			Debug.Log("StoreBaseMenu: ITEM " + key + " WAS NULL");
		}
		return null;
	}
}
