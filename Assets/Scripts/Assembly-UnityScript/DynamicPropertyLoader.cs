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
			JSONObject property = j.GetField(skey);
			if (property == null)
			{
				continue;
			}
			print("PROPERTY KEY: " + skey);
			switch (property.type)
			{
			case JSONObject.Type.STRING:
				if (property.str.Length != 0)
				{
					PlayerPrefs.SetString(skey, property.str);
				}
				else
				{
					PlayerPrefs.DeleteKey(skey);
					print("DELETING KEY: " + skey);
				}
				break;
			case JSONObject.Type.NUMBER:
				if (isInteger(property.n))
				{
					int vi = (int)property.n;
					if (vi != 0)
					{
						PlayerPrefs.SetInt(skey, (int)property.n);
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
					float vf = (float)property.n;
					if (vf != 0f)
					{
						PlayerPrefs.SetFloat(skey, (float)property.n);
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
				PlayerPrefs.SetInt(skey, property.b ? 1 : 0);
				break;
			}
		}
	}

	private bool isInteger(double d)
	{
		int truncated = (int)d;
		double truncatedValue = truncated;
		return d == truncatedValue;
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
