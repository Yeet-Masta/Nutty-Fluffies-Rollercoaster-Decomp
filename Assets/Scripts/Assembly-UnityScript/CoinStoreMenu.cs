using UnityEngine;
using UnityScript.Lang;

public class CoinStoreMenu : StoreBaseMenu
{
	public VGUIScrollingContainer packContainer;

	public IAPStoreItem[] iapItems;

	public GameObject nextPageButton;

	public GameObject resetButton;

	private bool wasActive;

	private bool sale;

	private int foundProducts;

	protected bool requstProducts = true;

	public tk2dTextMesh doubleCoinInfoText;

	public GameObject doubleCoinStar;

	private bool bugi = true;

	private bool offerWallMessageDisplayed;

	private bool back;

	public override void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Input.ResetInputAxes();
			onExitStore();
		}
		else if (offerWallMessageDisplayed && CommunityFeatures.getInstance().isDialogBoxClosed() && back)
		{
			offerWallMessageDisplayed = false;
			GameData.lockFeature(8192);
			BaseMenuManager.getInstance().onPreviousMenu();
		}
		if (CommunityFeatures.showingOffer)
		{
			back = false;
		}
		else
		{
			back = true;
		}
	}

	public override void Awake()
	{
		foundProducts = 0;
		base.Awake();
		if (bugi && requstProducts)
		{
			// Google Play product ids.
			for (int i = 0; i < iapItems.Length; i++)
			{
				iapItems[i].productID = iapItems[i].productID + ".managed";
			}
		}
		bugi = false;
		if (!GameData.TEST_BUILD && resetButton != null)
		{
			UnityEngine.Object.Destroy(resetButton);
		}
		// In-app purchases never become available any more, so the coin store
		// always shows the default coin amounts.
		if (requstProducts)
		{
			for (int j = 0; j < iapItems.Length; j++)
			{
				GameData.INAPP_PURCHASE_TOKENS[iapItems[j].productID] = StoreItem.getDefaultPrice(iapItems[j].itemKey);
			}
		}
		else
		{
			onPurchasePricesReceived();
		}
	}

	public override void activate()
	{
		BaseMenuManager.getInstance().freeCoinsMenu.deactivate();
		items = iapItems;
		base.activate();
		BaseMenu.mmgr.setStoreButtonVisible(false);
		onPurchasePricesReceived();
		BaseMenu baseMenu = BaseMenuManager.getInstance().menuSwitcher.previousMenu;
		if (baseMenu != nextMenu)
		{
			previousMenu = baseMenu;
		}
		if (nextPageButton != null)
		{
			nextPageButton.SetActiveRecursively(CommunityFeatures.getInstance().hasSupport());
		}
		wasActive = true;
		if (doubleCoinStar != null)
		{
			doubleCoinStar.SetActiveRecursively(false);
		}
		reSetTitle();
	}

	public virtual void reSetTitle()
	{
		if (GameData.isFeatureUnlocked(8192))
		{
			BaseMenuManager.getInstance().setMenuTitle(TextData.COINSTORE_DOUBLE_COINS_TITLE);
			if (doubleCoinStar != null)
			{
				doubleCoinStar.SetActiveRecursively(true);
			}
			if (doubleCoinInfoText != null)
			{
				doubleCoinInfoText.setText(TextData.COINSTORE_DOUBLE_COINS_INFO);
			}
		}
		else if (sale && doubleCoinStar != null)
		{
			BaseMenuManager.getInstance().setMenuTitle(TextData.COINSTORE_SALE_TITLE);
		}
	}

	public override void deactivate()
	{
		base.deactivate();
		if (wasActive)
		{
			wasActive = false;
		}
	}

	public override bool isActive()
	{
		return wasActive;
	}

	public virtual void onPurchaseItem(int index)
	{
		if (iapItems.Length > index)
		{
			if (GameData.LOG)
			{
				print("purchasing item " + iapItems[index].productID);
			}
			StoreManager.getInstance().purchaseProduct(iapItems[index].productID);
		}
	}

	public virtual void onPurchasePricesReceived()
	{
		StoreKitProduct storeKitProduct = null;
		float num = 0f;
		float num2 = 1E+11f;
		int i = 0;
		IAPStoreItem[] array = iapItems;
		for (int length = array.Length; i < length; i++)
		{
			storeKitProduct = GameData.getInAppPurchaseProduct(array[i].productID);
			if (GameData.LOG)
			{
				Debug.Log("ITEM ID >>> :::: " + array[i].productID);
			}
			if (storeKitProduct != null)
			{
				parseIAPDescription(storeKitProduct.description, array[i].productID);
				array[i].price = GameData.getTokenValue(array[i].productID);
				array[i].originalPrice = array[i].price;
				num = UnityBuiltins.parseFloat(storeKitProduct.price);
				if (num != float.NaN)
				{
					float num3 = (float)array[i].price / num;
					if (!(num3 >= num2))
					{
						num2 = num3;
					}
				}
			}
			else
			{
				array[i].price = GameData.getTokenValue(array[i].productID);
				array[i].originalPrice = array[i].price;
			}
		}
		int j = 0;
		IAPStoreItem[] array2 = iapItems;
		for (int length2 = array2.Length; j < length2; j++)
		{
			storeKitProduct = GameData.getInAppPurchaseProduct(array2[j].productID);
			if (storeKitProduct != null)
			{
				array2[j].iapPriceText.setText(string.Empty + storeKitProduct.currencySymbol + " " + storeKitProduct.price);
				array2[j].updatePriceText();
				num = UnityBuiltins.parseFloat(storeKitProduct.price);
				if (num != float.NaN)
				{
					float num4 = (float)array2[j].price / num;
					int num5 = (int)((num4 - num2) / num2 * 100f);
					num5 = (int)(Mathf.Ceil((float)num5 / 10f) * 10f);
					if (sale)
					{
						array2[j].button.setSavePercentage(0, 50);
					}
					else
					{
						array2[j].button.setSavePercentage(num5, 0);
					}
				}
				foundProducts++;
			}
			else if (array2[j].productID.Length > 0)
			{
				packContainer.setPageVisible(array2[j].button.gameObject, false, false);
			}
		}
	}

	private void parseIAPDescription(string text, string productId)
	{
		string[] array = null;
		if (text != null)
		{
			array = text.Split(" "[0]);
		}
		if (array == null)
		{
			return;
		}
		int i = 0;
		string[] array2 = array;
		for (int length = array2.Length; i < length; i++)
		{
			int result = 0;
			if (array2[i] != null && int.TryParse(array2[i], out result))
			{
				GameData.INAPP_PURCHASE_TOKENS[productId] = result;
				if (GameData.LOG)
				{
					print("FOUND TOKEN VALUE: " + result);
				}
			}
			else if (array2[i] == "SALE")
			{
				if (GameData.LOG)
				{
					print("FOUND SALE");
				}
				sale = true;
			}
		}
	}

	public virtual void onExitStore()
	{
		GameData.lockFeature(8192);
		if (offerWallMessageDisplayed)
		{
			offerWallMessageDisplayed = false;
			CommunityFeatures.getInstance().onBackButtonCloseDialog();
		}
		BaseMenuManager.getInstance().onPreviousMenu();
	}
}
