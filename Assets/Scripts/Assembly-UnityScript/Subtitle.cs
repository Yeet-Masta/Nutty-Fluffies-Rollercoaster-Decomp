using Boo.Lang.Runtime;

public class Subtitle : VGUIText
{
	public int maxChars = 100;

	public int maxLines = 2;

	public bool removeLineBrakes = true;

	private UnityScript.Lang.Array chunks;

	private int currentChunk = -1;

	public virtual void startDisplaying(NarrativeLine line)
	{
		CancelInvoke();
		currentChunk = -1;
		string content = line.textContent;
		if (removeLineBrakes)
		{
			content = content.Replace("\n", " ");
			content = content.Replace("\r", string.Empty);
		}
		chunks = StringTools.getLines(content.Trim(), maxChars);
		float timePerChar = line.duration / (float)content.Length;
		showNextChunk();
		float delay = (float)(chunks[0] as string).Length * timePerChar;
		for (int i = 1; i < chunks.length; i++)
		{
			Invoke("showNextChunk", delay - 0.3f);
			delay += (float)(chunks[i] as string).Length * timePerChar;
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
		setText(StringTools.wrapToLines((string)obj, (int)((float)(maxChars / maxLines) * 1.05f), maxLines));
	}

	public virtual void clearText()
	{
		setText(string.Empty);
	}
}
