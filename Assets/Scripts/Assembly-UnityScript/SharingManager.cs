using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using CompilerGenerated;
using UnityEngine;
using UnityScript.Lang;

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

	public string facebookAppID;

	public string twitterConsumerKey;

	public string twitterConsumerSecret;

	public string shareLinkURL;

	public string shareLinkTitle;

	public string shareImageURL;

	public string shareImageCaption;

	public List<GameObject> shareListeners;

	[HideInInspector]
	private string pendingMessage;

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
		if (GameData.USE_AMAZON_IAP)
		{
			shareLinkURL = GameData.APPSTORE_PURCHASE_PAGE_AMAZON;
		}
		else
		{
			shareLinkURL = GameData.APPSTORE_PURCHASE_PAGE_GOOGLE;
		}
		FacebookManager.loginSucceededEvent += facebookLogin;
		FacebookManager.dialogCompletedEvent += facebookDialogCompleted;
		FacebookManager.loginFailedEvent += loginFailedFB;
		FacebookManager.loggedOutEvent += loggedOut;
		FacebookManager.sessionInvalidatedEvent += sessionInvalidated;
		FacebookManager.dialogFailedEvent += dialogFailed;
		FacebookManager.dialogDidNotCompleteEvent += dialogDidNotComplete;
		TwitterAndroidManager.loginDidSucceedEvent += _0024adaptor_0024__SharingManager_Awake_0024callable0_002487_63___0024Action_00240.Adapt(twitterLogin);
		TwitterAndroidManager.requestSucceededEvent += _0024adaptor_0024__SharingManager_Awake_0024callable0_002487_63___0024Action_00241.Adapt(twitterPost);
		TwitterAndroidManager.loginDidFailEvent += _0024adaptor_0024__SharingManager_Awake_0024callable0_002487_63___0024Action_00240.Adapt(loginDidFailEvent);
		TwitterAndroidManager.requestFailedEvent += _0024adaptor_0024__SharingManager_Awake_0024callable0_002487_63___0024Action_00240.Adapt(requestFailedEvent);
		EtceteraAndroidManager.mailComposerFinished += mailComposerFinished;
		FacebookAndroid.init(facebookAppID);
		TwitterAndroid.init(twitterConsumerKey, twitterConsumerSecret);
	}

	private void twitterLogin()
	{
		if (GameData.LOG)
		{
			Debug.Log("Logged in to twitter");
		}
		Invoke("postTwitterMessage", 2f);
	}

	private void facebookLogin()
	{
		if (GameData.LOG)
		{
			Debug.Log("Logged in to facebook");
		}
		postFacebookMessage();
	}

	private void sendResponse(string resp)
	{
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(shareListeners);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is GameObject))
			{
				obj = RuntimeServices.Coerce(obj, typeof(GameObject));
			}
			GameObject gameObject = (GameObject)obj;
			if (gameObject != null)
			{
				gameObject.SendMessage(resp);
				UnityRuntimeServices.Update(enumerator, gameObject);
			}
		}
		shareListeners.Clear();
	}

	private void twitterPost()
	{
		if (GameData.LOG)
		{
			Debug.Log("posted to twitter");
		}
		sendResponse("onTwitterPostCompleted");
	}

	private void facebookDialogCompleted()
	{
		if (GameData.LOG)
		{
			Debug.Log("posted to facebook");
		}
		sendResponse("onFacebookPostCompleted");
	}

	private void mailComposerFinished(string result)
	{
		if (GameData.LOG)
		{
			Debug.Log("DDDDDDDDDDDDDDDDDDDD " + result);
		}
		if (result == "Sent")
		{
			sendResponse("onEmailPostCompleted");
		}
		else
		{
			sendResponse("onSharingFailed");
		}
	}

	public virtual void shareMessage(Service toService, string message)
	{
		shareMessage(toService, string.Empty, message);
	}

	public virtual void shareMessage(Service toService, string title, string message)
	{
		pendingMessage = message;
		switch (toService)
		{
		case Service.Facebook:
			FacebookAndroid.loginWithRequestedPermissions(new string[2] { "publish_stream", "offline_access" });
			break;
		case Service.Twitter:
			if (!TwitterAndroid.isLoggedIn())
			{
				TwitterAndroid.showLoginDialog();
			}
			else
			{
				postTwitterMessage();
			}
			break;
		case Service.Email:
			EtceteraAndroid.showEmailComposer(string.Empty, title, pendingMessage, false);
			mailComposerFinished("Sent");
			break;
		}
	}

	private void postFacebookMessage()
	{
		if (pendingMessage != null)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary["link"] = shareLinkURL;
			dictionary["name"] = shareLinkTitle;
			if (!string.IsNullOrEmpty(shareImageURL) && shareImageURL.Length > 0)
			{
				dictionary["picture"] = shareImageURL;
			}
			if (!string.IsNullOrEmpty(shareImageCaption) && shareImageCaption.Length > 0)
			{
				dictionary["caption"] = shareImageCaption;
			}
			dictionary["description"] = pendingMessage;
			FacebookAndroid.showDialog("stream.publish", dictionary);
		}
		pendingMessage = null;
	}

	private void postTwitterMessage()
	{
		if (pendingMessage != null)
		{
			TwitterAndroid.postUpdate(pendingMessage + " " + shareLinkURL);
		}
		pendingMessage = null;
	}

	private void loginFailedFB(string error)
	{
		if (GameData.LOG)
		{
			Debug.Log("FACEBOOK LOGIN FAILED: " + error);
		}
		sendResponse("onSharingFailed");
	}

	private void loggedOut()
	{
		if (GameData.LOG)
		{
			Debug.Log("FACEBOOK LOGGED OUT");
		}
		sendResponse("onSharingFailed");
	}

	private void sessionInvalidated()
	{
		if (GameData.LOG)
		{
			Debug.Log("FACEBOOK SESSION INVALIDATED");
		}
		sendResponse("onSharingFailed");
	}

	private void dialogFailed(string error)
	{
		if (GameData.LOG)
		{
			Debug.Log("FACEBOOK DIALOG FAILED: " + error);
		}
		sendResponse("onSharingFailed");
	}

	private void dialogDidNotComplete()
	{
		if (GameData.LOG)
		{
			Debug.Log("FACEBOOK DIALOG DIDN'T COMPLETE");
		}
		sendResponse("onSharingFailed");
	}

	private void loginDidFailEvent()
	{
		if (GameData.LOG)
		{
			Debug.Log("WTITTER LOGIN FAILED");
		}
		sendResponse("onSharingFailed");
	}

	private void requestFailedEvent()
	{
		if (GameData.LOG)
		{
			Debug.Log("TRWITTER REQUEST FIALED");
		}
		sendResponse("onSharingFailed");
	}

	private void loginFailedTW()
	{
		if (GameData.LOG)
		{
			Debug.Log("TRWITTER LOGIN FIALED");
		}
		sendResponse("onSharingFailed");
	}

	private void postFailed()
	{
		if (GameData.LOG)
		{
			Debug.Log("TRWITTER POST FIALED");
		}
		sendResponse("onSharingFailed");
	}

	private void tweetSheetCompletedEvent(bool success)
	{
		if (success)
		{
			twitterPost();
		}
		else
		{
			sendResponse("onSharingFailed");
		}
	}

	private void homeTimelineFailed()
	{
		if (GameData.LOG)
		{
			Debug.Log("TRWITTER TIMELINE FIALED");
		}
		sendResponse("onSharingFailed");
	}

	private void requestDidFailEvent()
	{
		if (GameData.LOG)
		{
			Debug.Log("TRWITTER REQUEST DID FIALED");
		}
		sendResponse("onSharingFailed");
	}
}
