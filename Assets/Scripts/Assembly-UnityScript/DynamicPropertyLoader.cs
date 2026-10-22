using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class DynamicPropertyLoader : MonoBehaviour
{
	[Serializable]
	internal sealed class _0024loadProperties_00241517 : GenericGenerator<WWW>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WWW>, IEnumerator
		{
			internal WWW _0024download_00241518;

			internal JSONObject _0024j_00241519;

			internal object _0024key_00241520;

			internal string _0024skey_00241521;

			internal JSONObject _0024obj_00241522;

			internal JSONObject.Type _0024_0024switch_0024218_00241523;

			internal int _0024vi_00241524;

			internal float _0024vf_00241525;

			internal IEnumerator _0024_0024iterator_0024364_00241526;

			internal DynamicPropertyLoader _0024self__00241527;

			public _0024(DynamicPropertyLoader self_)
			{
				_0024self__00241527 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024download_00241518 = new WWW(_0024self__00241527.getPropertyURL());
					if (GameData.LOG)
					{
						Debug.Log("Getting dynamic properties: " + _0024self__00241527.getPropertyURL());
					}
					_0024self__00241527.Invoke("handleTimeout", 30f);
					result = (Yield(2, _0024download_00241518) ? 1 : 0);
					break;
				case 2:
					_0024self__00241527.CancelInvoke();
					if (!string.IsNullOrEmpty(_0024download_00241518.error))
					{
						MonoBehaviour.print("Error downloading dynamic properties: " + _0024download_00241518.error);
					}
					else
					{
						_0024j_00241519 = new JSONObject(_0024download_00241518.text);
						MonoBehaviour.print("got jsonobject with " + _0024j_00241519.keys.Count + " keys");
						_0024_0024iterator_0024364_00241526 = UnityRuntimeServices.GetEnumerator(_0024j_00241519.keys);
						while (_0024_0024iterator_0024364_00241526.MoveNext())
						{
							_0024key_00241520 = _0024_0024iterator_0024364_00241526.Current;
							_0024skey_00241521 = _0024key_00241520 as string;
							UnityRuntimeServices.Update(_0024_0024iterator_0024364_00241526, _0024key_00241520);
							_0024obj_00241522 = _0024j_00241519.GetField(_0024key_00241520 as string);
							UnityRuntimeServices.Update(_0024_0024iterator_0024364_00241526, _0024key_00241520);
							if (RuntimeServices.EqualityOperator(_0024obj_00241522, null))
							{
								continue;
							}
							MonoBehaviour.print("PROPERTY KEY: " + _0024skey_00241521);
							_0024_0024switch_0024218_00241523 = _0024obj_00241522.type;
							if (_0024_0024switch_0024218_00241523 == JSONObject.Type.STRING)
							{
								if (_0024obj_00241522.str.Length != 0)
								{
									PlayerPrefs.SetString(_0024skey_00241521, _0024obj_00241522.str);
									continue;
								}
								PlayerPrefs.DeleteKey(_0024skey_00241521);
								MonoBehaviour.print("DELETING KEY: " + _0024skey_00241521);
							}
							else if (_0024_0024switch_0024218_00241523 == JSONObject.Type.NUMBER)
							{
								if (_0024self__00241527.isInteger(_0024obj_00241522.n))
								{
									_0024vi_00241524 = (int)_0024obj_00241522.n;
									if (_0024vi_00241524 != 0)
									{
										PlayerPrefs.SetInt(_0024skey_00241521, (int)_0024obj_00241522.n);
									}
									else
									{
										PlayerPrefs.DeleteKey(_0024skey_00241521);
										MonoBehaviour.print("DELETING KEY: " + _0024skey_00241521);
									}
									MonoBehaviour.print("updating key " + _0024skey_00241521 + " for integer " + _0024vi_00241524);
								}
								else
								{
									_0024vf_00241525 = (float)_0024obj_00241522.n;
									if (_0024vf_00241525 != 0f)
									{
										PlayerPrefs.SetFloat(_0024skey_00241521, (float)_0024obj_00241522.n);
									}
									else
									{
										PlayerPrefs.DeleteKey(_0024skey_00241521);
										MonoBehaviour.print("DELETING KEY: " + _0024skey_00241521);
									}
									MonoBehaviour.print("updating key " + _0024skey_00241521 + " for float " + _0024vf_00241525);
								}
							}
							else if (_0024_0024switch_0024218_00241523 == JSONObject.Type.BOOL)
							{
								if (_0024obj_00241522.b)
								{
									PlayerPrefs.SetInt(_0024skey_00241521, 1);
								}
								else
								{
									PlayerPrefs.SetInt(_0024skey_00241521, 0);
								}
							}
						}
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

		internal DynamicPropertyLoader _0024self__00241528;

		public _0024loadProperties_00241517(DynamicPropertyLoader self_)
		{
			_0024self__00241528 = self_;
		}

		public override IEnumerator<WWW> GetEnumerator()
		{
			return new _0024(_0024self__00241528);
		}
	}

	public string propertyURL;

	private WWW timeoutRequest;

	public virtual void Start()
	{
		StartCoroutine(loadProperties());
	}

	private string getPropertyURL()
	{
		return propertyURL.Replace(".properties", "_android.properties");
	}

	public virtual IEnumerator loadProperties()
	{
		return new _0024loadProperties_00241517(this).GetEnumerator();
	}

	private bool isInteger(double d)
	{
		int num = (int)d;
		double num2 = num;
		return d == num2;
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
