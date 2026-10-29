using System;
using System.Collections;
using UnityEngine;

public class CopyProtection : MonoBehaviour
{
	public string checkURL;

	public bool networkCheck;

	public int expireYear = 2012;

	public int expireMonth = 9;

	private WWW timeoutRequest;

	public virtual void Start()
	{
		if (GameData.LOCALIZATION_TEST)
		{
			if (networkCheck)
			{
				StartCoroutine(doNetworkCheck());
			}
			else
			{
				doDateCheck();
			}
		}
	}

	private void doDateCheck()
	{
		int month = DateTime.Now.Month;
		int day = DateTime.Now.Day;
		int year = DateTime.Now.Year;
		if (GameData.LOG)
		{
			MonoBehaviour.print("DATE IS: " + day + "/" + month + "/" + year);
		}
		if (year > expireYear || month > expireMonth)
		{
			failCheck();
		}
	}

	public virtual IEnumerator doNetworkCheck()
	{
		WWW download = new WWW(checkURL);
		if (GameData.LOG)
		{
			Debug.Log("Network check from: " + checkURL);
		}
		Invoke("handleTimeout", 30f);
		yield return download;
		CancelInvoke();
		if (!string.IsNullOrEmpty(download.error))
		{
			Debug.LogError("Error when doing network check: " + download.error);
		}
		else if (download.text.Contains("ok"))
		{
			print("NETWORK CHECK OK");
		}
		else
		{
			failCheck();
		}
	}

	public virtual void setTimeoutHandler(WWW request)
	{
		timeoutRequest = request;
	}

	public virtual void handleTimeout()
	{
		if (timeoutRequest != null)
		{
			timeoutRequest.Dispose();
			failCheck();
		}
	}

	private void failCheck()
	{
		Debug.LogError("NETWORK CHECK FAILED");
		Application.Quit();
	}
}
