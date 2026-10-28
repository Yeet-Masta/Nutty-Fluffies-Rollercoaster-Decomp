using System;
using System.Collections.Generic;
using UnityEngine;

// NFPort: the Facebook, Twitter and e-mail SDKs behind this are gone. Facebook
// and Twitter posts can no longer complete (the requester's own timeout reports
// the failure, as it did before); e-mail still reports success straight away,
// which is what the original did after opening the mail composer.
[Serializable]
public class SharingManager : MonoBehaviour
{
	[Serializable]
	public enum Service
	{
		Facebook = 0,
		Twitter = 1,
		Email = 2
	}

	[NonSerialized]
	private static SharingManager instance;

	public string shareLinkURL;

	public string shareLinkTitle;

	public List<GameObject> shareListeners;

	public SharingManager()
	{
		shareListeners = new List<GameObject>();
	}

	public static SharingManager getInstance()
	{
		return instance;
	}

	public virtual void addListener(GameObject listener)
	{
		if (!shareListeners.Contains(listener))
		{
			shareListeners.Add(listener);
		}
	}

	public virtual void Awake()
	{
		if (instance != null)
		{
			UnityEngine.Object.Destroy(gameObject);
			return;
		}
		instance = this;
		UnityEngine.Object.DontDestroyOnLoad(this);
		shareLinkURL = GameData.APPSTORE_PURCHASE_PAGE_GOOGLE;
	}

	private void sendResponse(string resp)
	{
		foreach (GameObject listener in shareListeners)
		{
			if (listener != null)
			{
				listener.SendMessage(resp);
			}
		}
		shareListeners.Clear();
	}

	public virtual void shareMessage(Service toService, string message)
	{
		shareMessage(toService, string.Empty, message);
	}

	public virtual void shareMessage(Service toService, string title, string message)
	{
		if (toService == Service.Email)
		{
			sendResponse("onEmailPostCompleted");
		}
	}
}
