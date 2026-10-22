using System;
using UnityEngine;

[Serializable]
public class IAPStoreItem : StoreItem
{
	[HideInInspector]
	public string storePrice;

	public string productID;

	public VGUIText iapPriceText;

	public bool active;

	public IAPStoreItem()
	{
		active = true;
	}
}
