using System;
using Boo.Lang.Runtime;
using UnityScript.Lang;

[Serializable]
public class Subtitle : VGUIText
{
	public int maxChars;

	public int maxLines;

	public bool removeLineBrakes;

	private UnityScript.Lang.Array chunks;

	private int currentChunk;

	public Subtitle()
	{
		maxChars = 100;
		maxLines = 2;
		removeLineBrakes = true;
		currentChunk = -1;
	}

	public virtual void startDisplaying(NarrativeLine line)
	{
		CancelInvoke();
		currentChunk = -1;
		string text = line.textContent;
		if (removeLineBrakes)
		{
			text = text.Replace("\n", " ");
			text = text.Replace("\r", string.Empty);
		}
		chunks = StringTools.getLines(text.Trim(), maxChars);
		float num = line.duration / (float)Extensions.get_length(text);
		showNextChunk();
		float num2 = (float)Extensions.get_length(chunks[0] as string) * num;
		for (int i = 1; i < chunks.length; i++)
		{
			Invoke("showNextChunk", num2 - 0.3f);
			num2 += (float)Extensions.get_length(chunks[i] as string) * num;
		}
		Invoke("clearText", line.duration);
	}

	public virtual void showNextChunk()
	{
		currentChunk++;
		object obj = chunks[currentChunk];
		if (!(obj is string))
		{
			obj = RuntimeServices.Coerce(obj, typeof(string));
		}
		string text = StringTools.wrapToLines((string)obj, (int)((float)(maxChars / maxLines) * 1.05f), maxLines);
		setText(text);
	}

	public virtual void clearText()
	{
		setText(string.Empty);
	}
}
