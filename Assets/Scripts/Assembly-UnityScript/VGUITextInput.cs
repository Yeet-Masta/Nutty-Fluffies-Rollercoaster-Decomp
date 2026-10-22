using System;
using UnityEngine;

[Serializable]
public class VGUITextInput : VGUIButton
{
	private string inputText;

	public VGUIText visibleText;

	public int maxInputLength;

	public bool applyMaxInputLengthOnlyToVisibleText;

	public bool saveToPrefs;

	public string prefsName;

	public bool trimInput;

	public bool lowercase;

	public bool uppercase;

	public bool allowEmptyInput;

	public string acceptedChars;

	public string prompt;

	public string editorDefaultInput;

	public string emptyError;

	public string nonAcceptedCharsError;

	public GameObject loadingIndicator;

	private bool inputting;

	private bool removedChars;

	[NonSerialized]
	private static bool firstKeyboardOpen = true;

	public VGUITextInput()
	{
		maxInputLength = 20;
		trimInput = true;
		acceptedChars = string.Empty;
		editorDefaultInput = "default";
		emptyError = string.Empty;
		nonAcceptedCharsError = string.Empty;
	}

	public virtual bool hasSavedValue()
	{
		return PlayerPrefs.GetString(prefsName, "_NULL_") != "_NULL_";
	}

	public virtual void saveCurrentValue()
	{
		if (saveToPrefs)
		{
			PlayerPrefs.SetString(prefsName, inputText);
		}
	}

	public static string getSavedValue(string id, string def)
	{
		return PlayerPrefs.GetString(id, def);
	}

	public static string setSavedValue(string id, string to)
	{
		PlayerPrefs.SetString(id, to);
		return null;
	}
}
