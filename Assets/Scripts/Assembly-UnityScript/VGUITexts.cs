using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class VGUITexts : VGUIText
{
	public string[] texts;

	public int selectedText;

	public bool randomTextOnAwake;

	public override void Awake()
	{
		base.Awake();
		setText(selectedText);
	}

	public virtual void setText(int to)
	{
		selectedText = to;
		if (selectedText >= Extensions.get_length((System.Array)texts))
		{
			selectedText = Extensions.get_length((System.Array)texts) - 1;
		}
		if (selectedText < 0)
		{
			selectedText = 0;
		}
		setText(texts[selectedText]);
	}

	public virtual void setRandomText()
	{
		setText(UnityEngine.Random.Range(0, Extensions.get_length((System.Array)texts)));
	}
}
