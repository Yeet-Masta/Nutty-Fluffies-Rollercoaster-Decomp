using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using Boo.Lang.Runtime;
using UnityEngine;

[Serializable]
public class CopyProtection : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024doNetworkCheck_00241488 : GenericGenerator<WWW>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WWW>, IEnumerator
		{
			internal WWW _0024download_00241489;

			internal CopyProtection _0024self__00241490;

			public _0024(CopyProtection self_)
			{
				_0024self__00241490 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024download_00241489 = new WWW(_0024self__00241490.checkURL);
					if (GameData.LOG)
					{
						Debug.Log("Network check from: " + _0024self__00241490.checkURL);
					}
					_0024self__00241490.Invoke("handleTimeout", 30f);
					result = (Yield(2, _0024download_00241489) ? 1 : 0);
					break;
				case 2:
					_0024self__00241490.CancelInvoke();
					if (!string.IsNullOrEmpty(_0024download_00241489.error))
					{
						Debug.LogError("Error when doing network check: " + _0024download_00241489.error);
					}
					else if (_0024download_00241489.text.Contains("ok"))
					{
						MonoBehaviour.print("NETWORK CHECK OK");
					}
					else
					{
						_0024self__00241490.failCheck();
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

		internal CopyProtection _0024self__00241491;

		public _0024doNetworkCheck_00241488(CopyProtection self_)
		{
			_0024self__00241491 = self_;
		}

		public override IEnumerator<WWW> GetEnumerator()
		{
			return new _0024(_0024self__00241491);
		}
	}

	public string checkURL;

	public bool networkCheck;

	public int expireYear;

	public int expireMonth;

	private WWW timeoutRequest;

	public CopyProtection()
	{
		expireYear = 2012;
		expireMonth = 9;
	}

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
		return new _0024doNetworkCheck_00241488(this).GetEnumerator();
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
			failCheck();
		}
	}

	private void failCheck()
	{
		Debug.LogError("NETWORK CHECK FAILED");
		FlurryAndroid.endSession();
		Application.Quit();
	}
}
