using System;
using UnityEngine;

// NFPort: in-app purchases went through Google Play billing (the Amazon store
// was never enabled), and that SDK is gone. The product list was only accepted
// once billing reported itself supported, which can no longer happen, so
// nothing is ever purchasable; a purchase that is started still times out and
// reports failure, as before.
[Serializable]
[DefaultExecutionOrder(-100)]
public class StoreManager : MonoBehaviour
{
	[NonSerialized]
	public static StoreManager instance;

	[NonSerialized]
	private static bool waitingForCallback;

	public static StoreManager getInstance()
	{
		return instance;
	}

	public virtual void Awake()
	{
		if (instance == null)
		{
			instance = this;
			UnityEngine.Object.DontDestroyOnLoad(this);
		}
	}

	public virtual void purchaseProduct(string id)
	{
		waitingForCallback = true;
		Invoke("onPurchaseTimeOut", 120f);
	}

	private void onPurchaseTimeOut()
	{
		if (waitingForCallback)
		{
			waitingForCallback = false;
			BaseMenuManager.getInstance().onPurchaseFailed();
		}
	}
}
