using System;

[Serializable]
public class NarrativeCommand
{
	public string name;

	public string[] parameters;

	public int position;

	public NarrativeCommand(int position, string text)
	{
		this.position = position;
		string[] words = text.Split(" "[0]);
		name = words[0];
		parameters = new string[words.Length - 1];
		for (int i = 1; i < words.Length; i++)
		{
			parameters[i - 1] = words[i];
		}
	}

	public virtual int getIntParameter(int i)
	{
		return int.Parse(parameters[i]);
	}
}
