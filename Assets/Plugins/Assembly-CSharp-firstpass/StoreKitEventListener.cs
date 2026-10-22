using System.Collections.Generic;
using UnityEngine;

public class StoreKitEventListener : MonoBehaviour
{
	public delegate void ProductsReceivedCallback(List<StoreKitProduct> productList);

	public delegate void ProductPurchasedCallback(string productId);

	public delegate void PurchaseFailedCallback(string productId);

	public delegate void PurchaseCancelCallback(string productId);

	public delegate void ReceiptValidationFailedCallback(string error);

	public delegate void ReceiptValidationRawResponseReceivedCallback(string result);

	public delegate void ReceiptValidationSuccessfulCallback();

	public delegate void ProductsRequestFailedCallback();

	public static ProductsReceivedCallback productCallback;

	public static ProductPurchasedCallback purchaseCallback;

	public static PurchaseFailedCallback failCallback;

	public static PurchaseCancelCallback cancelCallback;

	public static ReceiptValidationFailedCallback receiptValidationFailedCallback;

	public static ReceiptValidationRawResponseReceivedCallback receiptValidationRawResponseReceivedCallback;

	public static ReceiptValidationSuccessfulCallback receiptValidationSuccessfulCallback;

	public static ProductsRequestFailedCallback listfailCallback;

	public static void SetProductListFailedCallback(ProductsRequestFailedCallback callback)
	{
		listfailCallback = callback;
	}

	public static void SetCallProductCallback(ProductsReceivedCallback callBack)
	{
		productCallback = callBack;
	}

	public static void SetPurchaseCallback(ProductPurchasedCallback callBack)
	{
		purchaseCallback = callBack;
	}

	public static void SetPurchaseFailedCallback(PurchaseFailedCallback callBack)
	{
		failCallback = callBack;
	}

	public static void SetPurchaseCancelCallback(PurchaseCancelCallback callBack)
	{
		cancelCallback = callBack;
	}
}
