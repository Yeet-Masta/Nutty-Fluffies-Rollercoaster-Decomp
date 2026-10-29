using UnityEngine;

public class FreeCoinStoreMenu : CoinStoreMenu
{
	private bool wasFreeCoinStoreActive;

	public override void Awake()
	{
		requstProducts = false;
		base.Awake();
	}

	public virtual void Start()
	{
		updateCommunityPurchases();
	}

	public override void activate()
	{
		base.activate();
		wasFreeCoinStoreActive = true;
		updateCommunityPurchases();
	}

	public override void deactivate()
	{
		base.deactivate();
		wasFreeCoinStoreActive = false;
	}

	public override void reSetTitle()
	{
	}

	private void updateCommunityPurchases()
	{
		items = iapItems;
		for (int i = 0; i < 6; i++)
		{
			string key = "DP_PURCHASE_" + CommunityFeatures.FEATURE_KEY[i];
			CommunityFeatures.FEATURE_VALUES[i] = StoreItem.getDefaultPrice(key);
			StoreItem item = getItem(key);
			if (item == null)
			{
				continue;
			}
			bool available = CommunityFeatures.getInstance().isAvailable(i);
			StoreItemButton button = item.button;
			if (button == null)
			{
				continue;
			}
			if (available)
			{
				// Without the Tapjoy offer wall, two buttons move into the freed slots.
				if (CommunityFeatures.FEATURE_KEY[i] == "TWITTER_POST")
				{
					float x = getItem("DP_PURCHASE_FACEBOOK_LIKE").button.transform.position.x;
					Vector3 position = button.transform.position;
					position.x = x;
					button.transform.position = position;
				}
				if (CommunityFeatures.FEATURE_KEY[i] == "FACEBOOK_POST")
				{
					float x2 = getItem("DP_PURCHASE_SEND_EMAIL").button.transform.position.x;
					Vector3 position2 = button.transform.position;
					position2.x = x2;
					button.transform.position = position2;
				}
				item.price = CommunityFeatures.FEATURE_VALUES[i];
				item.updatePriceText();
				CommunityFeatures.getInstance().isEnabled(i);
				button.priceText.gameObject.SetActiveRecursively(CommunityFeatures.getInstance().isEnabled(i));
			}
			else
			{
				button.gameObject.SetActiveRecursively(false);
			}
		}
	}

	public override void onCommunityFeatureCompleted()
	{
		updateCommunityPurchases();
	}

	public virtual void onPostTwitter()
	{
		CommunityFeatures.getInstance().showOffer(0);
	}

	public virtual void onFaceBookLike()
	{
		CommunityFeatures.getInstance().showOffer(2);
	}

	public virtual void onFaceBookAdvert()
	{
		CommunityFeatures.getInstance().showOffer(1);
	}

	public virtual void onSendEmail()
	{
		CommunityFeatures.getInstance().showOffer(3);
	}

	// The Tapjoy offer wall is gone (its button is hidden); kept because the
	// scene's button calls it by name.
	public virtual void onShowTapjoyOffers()
	{
	}

	public override bool isActive()
	{
		return wasFreeCoinStoreActive;
	}

	public override void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && CommunityFeatures.getInstance().isDialogBoxClosed())
		{
			wasFreeCoinStoreActive = false;
			BaseMenuManager.getInstance().onPreviousMenu();
		}
	}
}
