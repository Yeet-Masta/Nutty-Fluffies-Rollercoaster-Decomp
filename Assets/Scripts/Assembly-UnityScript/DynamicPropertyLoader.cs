using System.Collections;
using UnityEngine;

public class DynamicPropertyLoader : MonoBehaviour
{
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
		WWW download = new WWW(getPropertyURL());
		if (GameData.LOG)
		{
			Debug.Log("Getting dynamic properties: " + getPropertyURL());
		}
		Invoke("handleTimeout", 30f);
		yield return download;
		CancelInvoke();
		if (!string.IsNullOrEmpty(download.error))
		{
			print("Error downloading dynamic properties: " + download.error);
			yield break;
		}
		JSONObject j = new JSONObject(download.text);
		print("got jsonobject with " + j.keys.Count + " keys");
		foreach (object key in j.keys)
		{
			string skey = key as string;
			JSONObject obj = j.GetField(skey);
			if (obj == null)
			{
				continue;
			}
			print("PROPERTY KEY: " + skey);
			switch (obj.type)
			{
			case JSONObject.Type.STRING:
				if (obj.str.Length != 0)
				{
					PlayerPrefs.SetString(skey, obj.str);
				}
				else
				{
					PlayerPrefs.DeleteKey(skey);
					print("DELETING KEY: " + skey);
				}
				break;
			case JSONObject.Type.NUMBER:
				if (isInteger(obj.n))
				{
					int vi = (int)obj.n;
					if (vi != 0)
					{
						PlayerPrefs.SetInt(skey, (int)obj.n);
					}
					else
					{
						PlayerPrefs.DeleteKey(skey);
						print("DELETING KEY: " + skey);
					}
					print("updating key " + skey + " for integer " + vi);
				}
				else
				{
					float vf = (float)obj.n;
					if (vf != 0f)
					{
						PlayerPrefs.SetFloat(skey, (float)obj.n);
					}
					else
					{
						PlayerPrefs.DeleteKey(skey);
						print("DELETING KEY: " + skey);
					}
					print("updating key " + skey + " for float " + vf);
				}
				break;
			case JSONObject.Type.BOOL:
				PlayerPrefs.SetInt(skey, obj.b ? 1 : 0);
				break;
			}
		}
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
		if (timeoutRequest != null)
		{
			timeoutRequest.Dispose();
		}
	}
}
