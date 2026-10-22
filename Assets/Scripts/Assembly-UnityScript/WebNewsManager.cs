using System;
using System.Collections;
using Boo.Lang.Runtime;
using UnityEngine;

[Serializable]
public class WebNewsManager : MonoBehaviour
{
	[Serializable]
	public enum State
	{
		INIT = 0,
		DOWNLOAD_JSON = 1,
		IDLE = 2
	}

	[NonSerialized]
	private static string NEWS_KEY = "WEBNEWS_LOCAL_VERSION";

	[NonSerialized]
	public static int ERROR_RETRY_DELAY = 60;

	[NonSerialized]
	private static WebNewsManager instance;

	public string newsURL;

	private State state;

	private WWW www;

	private string netNewsVersion;

	private string localNewsVersion;

	private bool ready;

	private string tempVersion;

	public WebNewsManager()
	{
		tempVersion = string.Empty;
	}

	public static WebNewsManager getInstance()
	{
		return instance;
	}

	public virtual void Awake()
	{
		if (instance != null)
		{
			UnityEngine.Object.Destroy(gameObject);
			return;
		}
		localNewsVersion = PlayerPrefs.GetString(NEWS_KEY);
		netNewsVersion = localNewsVersion;
		instance = this;
		UnityEngine.Object.DontDestroyOnLoad(this);
		ready = false;
		state = State.INIT;
	}

	public virtual bool hasNews()
	{
		return localNewsVersion != netNewsVersion;
	}

	public virtual void showNews()
	{
		localNewsVersion = netNewsVersion;
		PlayerPrefs.SetString(NEWS_KEY, netNewsVersion);
		Application.OpenURL(newsURL);
	}

	public virtual void Update()
	{
		if (ready)
		{
			return;
		}
		int num = default(int);
		switch (state)
		{
		case State.INIT:
			startNewsDownload();
			break;
		case State.DOWNLOAD_JSON:
			if (!www.isDone)
			{
				break;
			}
			if (www.error != null)
			{
				if (GameData.LOG)
				{
					Debug.Log("Error, trying JSON download later again...");
				}
				Invoke("startNewsDownload", ERROR_RETRY_DELAY);
				state = State.IDLE;
				break;
			}
			netNewsVersion = parseNewsVersionFromHTML(www.text);
			if (GameData.LOG)
			{
				Debug.Log("NEWS VERSION IN HTML IS '" + netNewsVersion + "'");
			}
			if (GameData.LOG)
			{
				Debug.Log("LOCAL VERSION IS '" + localNewsVersion + "'");
			}
			state = State.IDLE;
			ready = true;
			if (GameData.LOG)
			{
				Debug.Log("All done, web news ready");
			}
			break;
		}
	}

	private void startNewsDownload()
	{
		if (GameData.LOG)
		{
			Debug.Log("Downloading webnews JSON from " + newsURL + "...");
		}
		// NFPort: the WWW constructor throws when the request can't even be
		// started (e.g. plain http blocked by Player Settings). Unity 4 never
		// threw here, so Update kept calling this every frame; treat it like a
		// failed download instead.
		try
		{
			www = new WWW(newsURL);
		}
		catch (Exception ex)
		{
			if (GameData.LOG)
			{
				Debug.Log("Could not start webnews download: " + ex.Message);
			}
			Invoke("startNewsDownload", ERROR_RETRY_DELAY);
			state = State.IDLE;
			return;
		}
		state = State.DOWNLOAD_JSON;
	}

	private bool isWhiteSpace(char c)
	{
		bool flag = c == " "[0];
		if (!flag)
		{
			flag = c == "\r"[0];
		}
		if (!flag)
		{
			flag = c == "\n"[0];
		}
		if (!flag)
		{
			flag = c == "\t"[0];
		}
		return flag;
	}

	private void parseVersionText(IEnumerator se)
	{
		while (!RuntimeServices.EqualityOperator(se.Current, ">"[0]))
		{
			se.MoveNext();
		}
		se.MoveNext();
		while (!RuntimeServices.EqualityOperator(se.Current, "<"[0]))
		{
			tempVersion += se.Current;
			se.MoveNext();
		}
	}

	private void parseDiv(IEnumerator se)
	{
		string text = null;
		string text2 = null;
		while (!RuntimeServices.EqualityOperator(se.Current, ">"[0]))
		{
			text = string.Empty;
			text2 = string.Empty;
			if (!isWhiteSpace(RuntimeServices.UnboxChar(se.Current)))
			{
				while (!isWhiteSpace(RuntimeServices.UnboxChar(se.Current)) && !RuntimeServices.EqualityOperator(se.Current, "="[0]) && !RuntimeServices.EqualityOperator(se.Current, ">"[0]))
				{
					text += se.Current;
					se.MoveNext();
				}
				while (isWhiteSpace(RuntimeServices.UnboxChar(se.Current)))
				{
					se.MoveNext();
				}
				if (RuntimeServices.EqualityOperator(se.Current, "="[0]))
				{
					se.MoveNext();
					while (isWhiteSpace(RuntimeServices.UnboxChar(se.Current)))
					{
						se.MoveNext();
					}
					while (!isWhiteSpace(RuntimeServices.UnboxChar(se.Current)) && !RuntimeServices.EqualityOperator(se.Current, ">"[0]))
					{
						text2 += se.Current;
						se.MoveNext();
					}
				}
				if (text == "id" && text2 == "\"version\"")
				{
					parseVersionText(se);
				}
			}
			else
			{
				se.MoveNext();
			}
		}
	}

	private void parseTag(IEnumerator se)
	{
		se.MoveNext();
		while (isWhiteSpace(RuntimeServices.UnboxChar(se.Current)))
		{
			se.MoveNext();
		}
		string text = string.Empty;
		while (!isWhiteSpace(RuntimeServices.UnboxChar(se.Current)) && !RuntimeServices.EqualityOperator(se.Current, ">"[0]))
		{
			text += se.Current;
			se.MoveNext();
		}
		if (text.ToLower() == "div")
		{
			parseDiv(se);
		}
	}

	private string parseNewsVersionFromHTML(string html)
	{
		IEnumerator enumerator = html.GetEnumerator();
		while (enumerator.MoveNext())
		{
			if (RuntimeServices.EqualityOperator(enumerator.Current, "<"[0]))
			{
				parseTag(enumerator);
			}
		}
		return tempVersion;
	}
}
