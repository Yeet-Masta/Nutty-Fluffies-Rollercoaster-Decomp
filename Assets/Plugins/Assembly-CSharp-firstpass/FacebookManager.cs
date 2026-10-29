using System;
using UnityEngine;

public class FacebookManager : MonoBehaviour
{
	private static FacebookManager instance;

	public static event Action loginSucceededEvent;

	public static event Action preLoginSucceededEvent;

	public static event Action<string> loginFailedEvent;

	public static event Action loggedOutEvent;

	public static event Action<DateTime> accessTokenExtendedEvent;

	public static event Action failedToExtendTokenEvent;

	public static event Action sessionInvalidatedEvent;

	public static event Action dialogCompletedEvent;

	public static event Action<string> dialogFailedEvent;

	public static event Action dialogDidNotCompleteEvent;

	public static event Action<string> dialogCompletedWithUrlEvent;

	public static event Action<object> customRequestReceivedEvent;

	public static event Action<string> customRequestFailedEvent;

	public static event Action<bool> facebookComposerCompletedEvent;

	private void Awake()
	{
		if (instance == null)
		{
			gameObject.name = GetType().ToString();
			UnityEngine.Object.DontDestroyOnLoad(this);
			instance = this;
		}
		else
		{
			UnityEngine.Object.Destroy(gameObject);
		}
	}

	public void facebookLoginSucceeded(string empty)
	{
		if (preLoginSucceededEvent != null)
		{
			preLoginSucceededEvent();
		}
		if (loginSucceededEvent != null)
		{
			loginSucceededEvent();
		}
	}

	public void facebookLoginDidFail(string error)
	{
		if (loginFailedEvent != null)
		{
			loginFailedEvent(error);
		}
	}

	public void facebookDidLogout(string empty)
	{
		if (loggedOutEvent != null)
		{
			loggedOutEvent();
		}
	}

	public void facebookDidExtendToken(string secondsSinceEpoch)
	{
		if (accessTokenExtendedEvent != null)
		{
			double value = double.Parse(secondsSinceEpoch);
			accessTokenExtendedEvent(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(value));
		}
	}

	public void facebookFailedToExtendToken(string empty)
	{
		if (failedToExtendTokenEvent != null)
		{
			failedToExtendTokenEvent();
		}
	}

	public void facebookSessionInvalidated(string empty)
	{
		if (sessionInvalidatedEvent != null)
		{
			sessionInvalidatedEvent();
		}
	}

	public void facebookDialogDidComplete(string empty)
	{
		Debug.Log("facebookDialogDidComplete Complete");
		if (dialogCompletedEvent != null)
		{
			dialogCompletedEvent();
		}
	}

	public void facebookDialogDidCompleteWithUrl(string url)
	{
		Debug.Log("facebookDialogDidCompleteWithUrl Complete");
		if (dialogCompletedWithUrlEvent != null)
		{
			dialogCompletedWithUrlEvent(url);
		}
	}

	public void facebookDialogDidNotComplete(string empty)
	{
		if (dialogDidNotCompleteEvent != null)
		{
			dialogDidNotCompleteEvent();
		}
	}

	public void facebookDialogDidFailWithError(string error)
	{
		if (dialogFailedEvent != null)
		{
			dialogFailedEvent(error);
		}
	}

	public void facebookDidReceiveCustomRequest(string result)
	{
		if (customRequestReceivedEvent != null)
		{
			customRequestReceivedEvent(Prime31.MiniJSON.jsonDecode(result));
		}
	}

	public void facebookCustomRequestDidFail(string error)
	{
		if (customRequestFailedEvent != null)
		{
			customRequestFailedEvent(error);
		}
	}

	public void facebookComposerCompleted(string result)
	{
		if (facebookComposerCompletedEvent != null)
		{
			facebookComposerCompletedEvent(result == "1");
		}
	}

	public void graphAPICallback(string result)
	{
		Debug.Log("FACEBOOK graphAPICallback: " + result);
	}

	public void facebookCallback(string result)
	{
		Debug.Log("FACEBOOK facebookCallback: " + result);
	}
}
