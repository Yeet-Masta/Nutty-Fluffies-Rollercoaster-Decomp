using System;
using Boo.Lang.Runtime;
using UnityEngine;

[Serializable]
public class FreeCoinStoreMenu : CoinStoreMenu
{
	[NonSerialized]
	public static bool tapjoyOWOpened;

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
			string text = "DP_PURCHASE_" + CommunityFeatures.FEATURE_KEY[i];
			CommunityFeatures.FEATURE_VALUES[i] = StoreItem.getDefaultPrice(text);
			StoreItem item = getItem(text);
			if (RuntimeServices.EqualityOperator(item, null))
			{
				continue;
			}
			bool flag = CommunityFeatures.getInstance().isAvailable(i);
			StoreItemButton button = item.button;
			if (!(button != null))
			{
				continue;
			}
			if (flag)
			{
				if (!GameData.IS_TAPJOY_ENABLED)
				{
					if (CommunityFeatures.FEATURE_KEY[i] == "TWITTER_POST")
					{
						float x = getItem("DP_PURCHASE_FACEBOOK_LIKE").button.transform.position.x;
						Vector3 position = button.transform.position;
						float num = (position.x = x);
						Vector3 vector = (button.transform.position = position);
					}
					if (CommunityFeatures.FEATURE_KEY[i] == "FACEBOOK_POST")
					{
						float x2 = getItem("DP_PURCHASE_SEND_EMAIL").button.transform.position.x;
						Vector3 position2 = button.transform.position;
						float num2 = (position2.x = x2);
						Vector3 vector3 = (button.transform.position = position2);
					}
				}
				item.price = CommunityFeatures.FEATURE_VALUES[i];
				item.updatePriceText();
				bool flag2 = CommunityFeatures.getInstance().isEnabled(i);
				button.priceText.gameObject.SetActiveRecursively(CommunityFeatures.getInstance().isEnabled(i));
			}
			else
			{
				button.gameObject.SetActiveRecursively(false);
			}
		}
	}

	private void enableTapjoyOfferwall()
	{
		tapjoyOWOpened = true;
	}

	public override void onCommunityFeatureCompleted()
	{
		updateCommunityPurchases();
	}

	public virtual void onPostTwitter()
	{
		CommunityFeatures.getInstance().showOffer(0);
		BaseMenuManager.getInstance().logEvent("Selected free coins with twitter");
	}

	public virtual void onFaceBookLike()
	{
		CommunityFeatures.getInstance().showOffer(2);
		BaseMenuManager.getInstance().logEvent("Selected free coins with Facebook like");
	}

	public virtual void onFaceBookAdvert()
	{
		CommunityFeatures.getInstance().showOffer(1);
		BaseMenuManager.getInstance().logEvent("Selected free coins with Facebook post");
	}

	public virtual void onSendEmail()
	{
		CommunityFeatures.getInstance().showOffer(3);
		BaseMenuManager.getInstance().logEvent("Selected free coins with email");
	}

	public virtual void onShowTapjoyOffers()
	{
		if (GameData.hasInternet())
		{
			CommunityFeatures.getInstance().showTapjoyOfferPage();
			Invoke("enableTapjoyOfferwall", 2f);
			BaseMenuManager.getInstance().logEvent("Selected Tapjoy Offer Wall");
		}
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
		else if (tapjoyOWOpened)
		{
			BaseMenuManager.getInstance().showLoadingScreen(false);
			tapjoyOWOpened = false;
			TapjoyAdController.getInstance().GetTapPoints();
		}
	}
}
