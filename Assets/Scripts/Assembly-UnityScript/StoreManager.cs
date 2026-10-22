using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using Boo.Lang.Runtime;
using Prime31;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
[DefaultExecutionOrder(-100)]
public class StoreManager : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024requestAndroidProducts_00241492 : GenericGenerator<WWW>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WWW>, IEnumerator
		{
			internal WWW _0024download_00241493;

			internal JSONObject _0024j_00241494;

			internal JSONObject _0024iaps_00241495;

			internal JSONObject _0024iap_00241496;

			internal string _0024identifier_00241497;

			internal string _0024title_00241498;

			internal string _0024description_00241499;

			internal string _0024price_00241500;

			internal string _0024currencySymbol_00241501;

			internal string _0024currencyCode_00241502;

			internal string _0024formattedPrice_00241503;

			internal StoreKitProduct _0024p_00241504;

			internal IEnumerator _0024_0024iterator_0024353_00241505;

			internal StoreManager _0024self__00241506;

			public _0024(StoreManager self_)
			{
				_0024self__00241506 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024download_00241493 = new WWW(GameData.ANDROID_IAP_LIST_URL);
					if (GameData.LOG)
					{
						Debug.Log("Getting IAP list: " + GameData.ANDROID_IAP_LIST_URL);
					}
					_0024self__00241506.setTimeoutHandler(_0024download_00241493);
					_0024self__00241506.Invoke("handleTimeout", 30f);
					result = (Yield(2, _0024download_00241493) ? 1 : 0);
					break;
				case 2:
					_0024self__00241506.CancelInvoke();
					if (!string.IsNullOrEmpty(_0024download_00241493.error))
					{
						if (GameData.LOG)
						{
							Debug.Log("Error downloading android list: " + _0024download_00241493.error);
						}
						_0024self__00241506.Invoke("checkAndRequestProducts", 5f);
					}
					else
					{
						if (GameData.LOG)
						{
							Debug.Log("Downloaded JSON success ");
						}
						_0024self__00241506.androidProductList = new System.Collections.Generic.List<StoreKitProduct>();
						_0024j_00241494 = new JSONObject(_0024download_00241493.text);
						_0024iaps_00241495 = _0024j_00241494.GetField("iaps");
						if (GameData.LOG)
						{
							Debug.Log("IAPS: " + _0024iaps_00241495);
						}
						// NFPort: the IAP list URL now serves a web page, which has no "iaps"
						// field; the NullReferenceException skipped didRequest = false below.
						_0024_0024iterator_0024353_00241505 = UnityRuntimeServices.GetEnumerator((_0024iaps_00241495 != null) ? ((object)_0024iaps_00241495.list) : new object[0]);
						while (_0024_0024iterator_0024353_00241505.MoveNext())
						{
							object obj = _0024_0024iterator_0024353_00241505.Current;
							if (!(obj is JSONObject))
							{
								obj = RuntimeServices.Coerce(obj, typeof(JSONObject));
							}
							_0024iap_00241496 = (JSONObject)obj;
							_0024identifier_00241497 = _0024iap_00241496.GetFieldSafe("identifier", new JSONObject(string.Empty)).str;
							UnityRuntimeServices.Update(_0024_0024iterator_0024353_00241505, _0024iap_00241496);
							_0024title_00241498 = _0024iap_00241496.GetFieldSafe("title", new JSONObject(string.Empty)).str;
							UnityRuntimeServices.Update(_0024_0024iterator_0024353_00241505, _0024iap_00241496);
							_0024description_00241499 = _0024iap_00241496.GetFieldSafe("description", new JSONObject(string.Empty)).str;
							UnityRuntimeServices.Update(_0024_0024iterator_0024353_00241505, _0024iap_00241496);
							_0024price_00241500 = _0024iap_00241496.GetFieldSafe("price", new JSONObject(string.Empty)).str;
							UnityRuntimeServices.Update(_0024_0024iterator_0024353_00241505, _0024iap_00241496);
							_0024currencySymbol_00241501 = _0024iap_00241496.GetFieldSafe("currencySymbol", new JSONObject(string.Empty)).str;
							UnityRuntimeServices.Update(_0024_0024iterator_0024353_00241505, _0024iap_00241496);
							_0024currencyCode_00241502 = _0024iap_00241496.GetFieldSafe("currencyCode", new JSONObject(string.Empty)).str;
							UnityRuntimeServices.Update(_0024_0024iterator_0024353_00241505, _0024iap_00241496);
							_0024formattedPrice_00241503 = _0024iap_00241496.GetFieldSafe("formattedPrice", new JSONObject(string.Empty)).str;
							UnityRuntimeServices.Update(_0024_0024iterator_0024353_00241505, _0024iap_00241496);
							_0024p_00241504 = new StoreKitProduct();
							_0024p_00241504.productIdentifier = _0024identifier_00241497;
							_0024p_00241504.title = _0024title_00241498;
							_0024p_00241504.description = _0024description_00241499;
							_0024p_00241504.price = _0024price_00241500;
							_0024p_00241504.currencySymbol = _0024currencySymbol_00241501;
							_0024p_00241504.currencyCode = _0024currencyCode_00241502;
							if (GameData.LOG)
							{
								Debug.Log("IAP: " + _0024p_00241504.productIdentifier);
							}
							_0024self__00241506.androidProductList.Add(_0024p_00241504);
						}
						if (GameData.USE_AMAZON_IAP)
						{
							if (!RuntimeServices.EqualityOperator(_0024self__00241506.androidProductList, null))
							{
								ProductsReceivedCallback(_0024self__00241506.androidProductList);
							}
						}
						else if (!RuntimeServices.EqualityOperator(_0024self__00241506.androidProductList, null) && _0024self__00241506.canMakeAndroidIAPs)
						{
							ProductsReceivedCallback(_0024self__00241506.androidProductList);
						}
						didRequest = false;
					}
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal StoreManager _0024self__00241507;

		public _0024requestAndroidProducts_00241492(StoreManager self_)
		{
			_0024self__00241507 = self_;
		}

		public override IEnumerator<WWW> GetEnumerator()
		{
			return new _0024(_0024self__00241507);
		}
	}

	[NonSerialized]
	public static StoreManager instance;

	private string tempID;

	[NonSerialized]
	private static bool waitingForCallback;

	[NonSerialized]
	private static bool initiateDateRequest;

	private bool canMakeAndroidIAPs;

	private System.Collections.Generic.List<StoreKitProduct> androidProductList;

	[NonSerialized]
	private static bool didRequest;

	private WWW timeoutRequest;

	public static void ProductsReceivedCallback(System.Collections.Generic.List<StoreKitProduct> productList)
	{
		GameData.INAPP_PURCHASE_LIST = productList;
		BaseMenuManager baseMenuManager = BaseMenuManager.getInstance();
		if (baseMenuManager != null)
		{
			if (GameData.LOG)
			{
				Debug.Log("GameData.INAPP_PURCHASE_AVAILABLE = true");
			}
			if (!GameData.INAPP_PURCHASE_AVAILABLE)
			{
				GeneralSoundEffects.playSound(baseMenuManager.notificationSound);
			}
			GameData.INAPP_PURCHASE_AVAILABLE = true;
			baseMenuManager.onProductListReceived();
		}
		if (GameData.LOG)
		{
			Debug.Log("RECEIVED PRODUCTS");
		}
		if (initiateDateRequest)
		{
			return;
		}
		string[] array = new string[GameData.INAPP_PURCHASE_LIST.Count];
		int num = 0;
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(GameData.INAPP_PURCHASE_LIST);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is StoreKitProduct))
			{
				obj = RuntimeServices.Coerce(obj, typeof(StoreKitProduct));
			}
			StoreKitProduct storeKitProduct = (StoreKitProduct)obj;
			if (GameData.LOG)
			{
				Debug.Log("ProductsReceivedCallback " + storeKitProduct.productIdentifier);
				UnityRuntimeServices.Update(enumerator, storeKitProduct);
			}
			array[num] = storeKitProduct.productIdentifier;
			UnityRuntimeServices.Update(enumerator, storeKitProduct);
			num++;
		}
		initiateDateRequest = true;
		if (GameData.USE_AMAZON_IAP)
		{
			AmazonIAP.initiateItemDataRequest(array);
		}
		else
		{
			GoogleIAB.queryInventory(array);
		}
	}

	public static void ProductsRequestFailedCallback()
	{
		didRequest = false;
	}

	public virtual void billingSupportedEvent()
	{
		if (GameData.LOG)
		{
			Debug.Log("billingSupportedEvent: True");
		}
		canMakeAndroidIAPs = true;
		if (!RuntimeServices.EqualityOperator(androidProductList, null) && canMakeAndroidIAPs)
		{
			ProductsReceivedCallback(androidProductList);
		}
	}

	public virtual void billingNotSupportedEvent(string error)
	{
		if (GameData.LOG)
		{
			Debug.Log("billingNotSupportedEvent: " + error);
		}
		canMakeAndroidIAPs = false;
	}

	public virtual void purchaseSignatureVerifiedEvent(string signedData, string signature)
	{
		if (GameData.LOG)
		{
			Debug.Log("purchaseSignatureVerifiedEvent. signedData: " + signedData + ", signature: " + signature);
		}
	}

	public virtual void consumePurchaseSucceededEvent(GooglePurchase purchase)
	{
		if (GameData.LOG)
		{
			Debug.Log("consumePurchaseSucceededEvent: " + purchase);
		}
		purchaseSucceededEvent(purchase.productId);
	}

	public virtual void consumePurchaseFailedEvent(string error)
	{
		if (GameData.LOG)
		{
			Debug.Log("consumePurchaseFailedEvent: " + error);
		}
		purchaseFailedEvent(error);
	}

	public virtual void purchaseSucceededEvent2(GooglePurchase purchase)
	{
		if (GameData.LOG)
		{
			Debug.Log("purchaseSucceededEvent: " + purchase);
		}
		GoogleIAB.consumeProduct(purchase.productId);
	}

	public virtual void queryInventorySucceededEvent(System.Collections.Generic.List<GooglePurchase> purchases, System.Collections.Generic.List<GoogleSkuInfo> skus)
	{
		Debug.Log("queryInventorySucceededEvent");
		Utils.logObject(purchases);
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(purchases);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is GooglePurchase))
			{
				obj = RuntimeServices.Coerce(obj, typeof(GooglePurchase));
			}
			GooglePurchase googlePurchase = (GooglePurchase)obj;
			Debug.Log("anji in loop....");
			if (googlePurchase.purchaseState == GooglePurchase.GooglePurchaseState.Purchased)
			{
				Debug.Log("anji " + googlePurchase.productId + " is purchased and not updated..");
				UnityRuntimeServices.Update(enumerator, googlePurchase);
				GoogleIAB.consumeProduct(googlePurchase.productId);
				UnityRuntimeServices.Update(enumerator, googlePurchase);
				PurchasedCallback(googlePurchase.productId);
				UnityRuntimeServices.Update(enumerator, googlePurchase);
			}
		}
	}

	public virtual void purchaseSucceededEvent(string productId)
	{
		if (GameData.LOG)
		{
			Debug.Log("purchaseSucceededEvent: " + productId);
		}
		PurchasedCallback(productId);
		if (BaseMenuManager.getInstance() != null)
		{
			BaseMenuManager.getInstance().logEventWithParameter("[mntz] CONFIRM IAP TRANSACTION", "Package", string.Empty + productId);
			BaseMenuManager.getInstance().logEventWithParameter("CONFIRM IAP TRANSACTION", "Package", string.Empty + productId);
		}
	}

	public virtual void purchaseCancelledEvent(string productId)
	{
		if (GameData.LOG)
		{
			Debug.Log("purchaseCancelledEvent: " + productId);
		}
		PurchaseCancelCallback(productId);
		if (BaseMenuManager.getInstance() != null)
		{
			BaseMenuManager.getInstance().logEventWithParameter("[mntz] CANCEL IAP TRANSACTION", "Package", string.Empty + productId);
			BaseMenuManager.getInstance().logEventWithParameter("CANCEL IAP TRANSACTION", "Package", string.Empty + productId);
		}
		if (GameData.IS_TAPJOY_ENABLED)
		{
			TapjoyAdController.getInstance().DisplayFullScreenAd("RC_IAP_CANCEL");
		}
	}

	public virtual void purchaseRefundedEvent(string productId)
	{
		if (GameData.LOG)
		{
			Debug.Log("purchaseRefundedEvent: " + productId);
		}
	}

	public virtual void purchaseFailedEvent(string productId)
	{
		if (GameData.LOG)
		{
			Debug.Log("purchaseFailedEvent: " + productId);
		}
		PurchaseFailCallback(productId);
		if (BaseMenuManager.getInstance() != null)
		{
			BaseMenuManager.getInstance().logEventWithParameter("[mntz] FAIL IAP TRANSACTION", "Package", string.Empty + productId);
			BaseMenuManager.getInstance().logEventWithParameter("FAIL IAP TRANSACTION", "Package", string.Empty + productId);
		}
	}

	public virtual void itemDataRequestFailedEvent()
	{
		if (GameData.LOG)
		{
			Debug.Log("itemDataRequestFailedEvent");
		}
	}

	public virtual void itemDataRequestFinishedEvent(System.Collections.Generic.List<string> unavailableSkus, System.Collections.Generic.List<AmazonItem> availableItems)
	{
		if (GameData.LOG)
		{
			Debug.Log("itemDataRequestFinishedEvent. unavailable skus: " + unavailableSkus.Count + ", avaiable items: " + availableItems.Count);
		}
		if (availableItems.Count > 0)
		{
			canMakeAndroidIAPs = true;
			if (!RuntimeServices.EqualityOperator(androidProductList, null))
			{
				ProductsReceivedCallback(androidProductList);
			}
		}
	}

	public virtual void purchaseSuccessfulEvent(AmazonReceipt receipt)
	{
		if (GameData.LOG)
		{
			Debug.Log("purchaseSuccessfulEvent: " + receipt);
		}
		PurchasedCallback(receipt.sku);
	}

	public virtual void purchaseUpdatesRequestFailedEvent()
	{
		if (GameData.LOG)
		{
			Debug.Log("purchaseUpdatesRequestFailedEvent");
		}
	}

	public virtual void purchaseUpdatesRequestSuccessfulEvent(System.Collections.Generic.List<string> revokedSkus, System.Collections.Generic.List<AmazonReceipt> receipts)
	{
		if (GameData.LOG)
		{
			Debug.Log("purchaseUpdatesRequestSuccessfulEvent. revoked skus: " + revokedSkus.Count);
		}
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(receipts);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is AmazonReceipt))
			{
				obj = RuntimeServices.Coerce(obj, typeof(AmazonReceipt));
			}
			AmazonReceipt amazonReceipt = (AmazonReceipt)obj;
			if (GameData.LOG)
			{
				Debug.Log(amazonReceipt);
				UnityRuntimeServices.Update(enumerator, amazonReceipt);
			}
		}
	}

	public virtual void onSdkAvailableEvent(bool isTestMode)
	{
		if (GameData.LOG)
		{
			Debug.Log("onSdkAvailableEvent. isTestMode: " + isTestMode);
		}
	}

	public virtual void onGetUserIdResponseEvent(string userId)
	{
		if (GameData.LOG)
		{
			Debug.Log("onGetUserIdResponseEvent: " + userId);
		}
	}

	public static void PurchasedCallback(string productId)
	{
		if (GameData.LOG)
		{
			MonoBehaviour.print("StoreManager - Purchase ok callback");
		}
		GameData.setInAppPurchased(productId);
		BaseMenuManager.getInstance().onPurchaseComplete(productId);
		waitingForCallback = false;
	}

	public static void PurchaseFailCallback(string productId)
	{
		if (GameData.LOG)
		{
			MonoBehaviour.print("StoreManager - Purchase failed callback");
		}
		BaseMenuManager.getInstance().onPurchaseFailed();
		waitingForCallback = false;
	}

	public static void PurchaseCancelCallback(string productId)
	{
		if (GameData.LOG)
		{
			MonoBehaviour.print("StoreManager - Purchase canceled callback");
		}
		BaseMenuManager.getInstance().onPurchaseCancel();
		waitingForCallback = false;
	}

	public static StoreManager getInstance()
	{
		return instance;
	}

	public virtual void Awake()
	{
		if (instance == null)
		{
			instance = this;
			StoreKitEventListener.SetCallProductCallback(ProductsReceivedCallback);
			StoreKitEventListener.SetPurchaseCallback(PurchasedCallback);
			StoreKitEventListener.SetPurchaseFailedCallback(PurchaseFailCallback);
			StoreKitEventListener.SetPurchaseCancelCallback(PurchaseCancelCallback);
			StoreKitEventListener.SetProductListFailedCallback(ProductsRequestFailedCallback);
			if (GameData.USE_AMAZON_IAP)
			{
				AmazonIAPManager.itemDataRequestFailedEvent += itemDataRequestFailedEvent;
				AmazonIAPManager.itemDataRequestFinishedEvent += itemDataRequestFinishedEvent;
				AmazonIAPManager.purchaseFailedEvent += purchaseFailedEvent;
				AmazonIAPManager.purchaseSuccessfulEvent += purchaseSuccessfulEvent;
				AmazonIAPManager.purchaseUpdatesRequestFailedEvent += purchaseUpdatesRequestFailedEvent;
				AmazonIAPManager.purchaseUpdatesRequestSuccessfulEvent += purchaseUpdatesRequestSuccessfulEvent;
				AmazonIAPManager.onSdkAvailableEvent += onSdkAvailableEvent;
				AmazonIAPManager.onGetUserIdResponseEvent += onGetUserIdResponseEvent;
			}
			else
			{
				GoogleIAB.init(GameData.ANDROID_IAP_PUBLIC_KEY);
				GoogleIABManager.billingSupportedEvent += billingSupportedEvent;
				GoogleIABManager.billingNotSupportedEvent += billingNotSupportedEvent;
				GoogleIABManager.purchaseSucceededEvent += purchaseSucceededEvent2;
				GoogleIABManager.purchaseFailedEvent += purchaseFailedEvent;
				GoogleIABManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
				GoogleIABManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
				GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
			}
			UnityEngine.Object.DontDestroyOnLoad(this);
		}
	}

	public virtual void callback(Hashtable ht)
	{
		if (GameData.LOG)
		{
			Debug.Log("VALIDATE RESULT: " + ht);
		}
		if (!RuntimeServices.EqualityOperator(ht, null))
		{
			ResultLogger.logHashtable(ht);
		}
	}

	public virtual void purchaseProduct(string id)
	{
		waitingForCallback = true;
		if (GameData.USE_AMAZON_IAP)
		{
			if (GameData.LOG)
			{
				Debug.Log(id);
			}
			AmazonIAP.initiatePurchaseRequest(id);
		}
		else
		{
			GoogleIAB.purchaseProduct(id);
		}
		if (BaseMenuManager.getInstance() != null)
		{
			BaseMenuManager.getInstance().logEventWithParameter("[mntz] START IAP TRANSACTION", "Package", string.Empty + id);
			BaseMenuManager.getInstance().logEventWithParameter("START IAP TRANSACTION", "Package", string.Empty + id);
		}
		Invoke("onPurchaseTimeOut", 120f);
	}

	private void onPurchaseTimeOut()
	{
		if (waitingForCallback)
		{
			waitingForCallback = false;
			PurchaseFailCallback(string.Empty);
		}
	}

	public virtual void requestProducts(string products)
	{
		if (!didRequest)
		{
			didRequest = true;
			StartCoroutine(requestAndroidProducts());
		}
	}

	private void checkAndRequestProducts()
	{
		if (GameData.getInAppPurchaseProductCount() == 0)
		{
			StartCoroutine(requestAndroidProducts());
		}
	}

	public virtual IEnumerator requestAndroidProducts()
	{
		return new _0024requestAndroidProducts_00241492(this).GetEnumerator();
	}

	public virtual void setTimeoutHandler(WWW request)
	{
		timeoutRequest = request;
	}

	public virtual void handleTimeout()
	{
		if (!RuntimeServices.EqualityOperator(timeoutRequest, null))
		{
			timeoutRequest.Dispose();
		}
	}
}
