using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
	public static string[] LANGUAGES = new string[7] { "en", "fr", "it", "de", "es", "br", "fi" };

	public tk2dTextMesh[] textEntry;

	private static LocalizationManager instance;

	private static Dictionary<string, string> translation;

	private string currentLanguage;

	private bool inited;

	public static LocalizationManager getInstance()
	{
		return instance;
	}

	public virtual void Awake()
	{
		if (inited)
		{
			return;
		}
		instance = this;
		if (GameData.LOCALIZATION_TEST)
		{
			setLanguage(PlayerPrefs.GetString("LANGUAGE_ID", "en"));
		}
		else
		{
			SystemLanguage systemLanguage = Application.systemLanguage;
			string language = "en";
			switch (systemLanguage)
			{
			case SystemLanguage.French:
				language = "fr";
				break;
			case SystemLanguage.German:
				language = "de";
				break;
			case SystemLanguage.Italian:
				language = "it";
				break;
			case SystemLanguage.Spanish:
				language = "es";
				break;
			case SystemLanguage.Portuguese:
				language = "br";
				break;
			case SystemLanguage.Finnish:
				language = "fi";
				break;
			}
			setLanguage(language);
		}
		inited = true;
	}

	public virtual string getLanguage()
	{
		return currentLanguage;
	}

	public virtual void setLanguage(string language)
	{
		loadLanguage(language);
		if (GameData.LOCALIZATION_TEST)
		{
			PlayerPrefs.SetString("LANGUAGE_ID", language);
		}
	}

	public virtual void Start()
	{
		translateReferences();
	}

	public static string localize(string key)
	{
		return translation.ContainsKey(key) ? translation[key] : key;
	}

	private void translateReferences()
	{
		for (int i = 0; i < textEntry.Length; i++)
		{
			// NFPort: some entries are missing in the ripped scene; skip them
			// instead of stopping with the remaining texts untranslated.
			if (textEntry[i] != null)
			{
				textEntry[i].setText(localize(textEntry[i].text));
			}
		}
	}

	private void loadLanguage(string language)
	{
		if (translation == null || currentLanguage != language)
		{
			translation = new Dictionary<string, string>();
			currentLanguage = language;
			string text = FileManager.getInstance().locateAnyFile("language_" + language + ".txt");
			if (text != null)
			{
				string text2 = FileManager.getInstance().loadFileText(text);
				string[] array = text2.Split("\n"[0]);
				int i = 0;
				string[] array2 = array;
				for (int length = array2.Length; i < length; i++)
				{
					string[] array3 = array2[i].Split("="[0]);
					if (array3.Length == 2)
					{
						translation[array3[0].Trim()] = array3[1].Replace("\\n", "\n").Trim();
					}
					else if (array3.Length > 2)
					{
						Debug.LogError("ILLEGAL LINE: " + array2[i]);
					}
				}
			}
			else
			{
				Debug.LogError("LANGUAGE " + language + " NOT FOUND");
			}
		}
		TextData.init();
	}
}
