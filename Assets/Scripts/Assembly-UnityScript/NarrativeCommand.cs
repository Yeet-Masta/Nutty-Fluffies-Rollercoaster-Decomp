using System;
using UnityScript.Lang;

[Serializable]
public class NarrativeCommand
{
	public string name;

	public string[] parameters;

	public int position;

	public NarrativeCommand(int position, string text)
	{
		this.position = position;
		string[] array = text.Split(" "[0]);
		name = array[0];
		parameters = new string[array.Length - 1];
		for (int i = 1; i < array.Length; i++)
		{
			parameters[i - 1] = array[i];
		}
	}

	public virtual int getIntParameter(int i)
	{
		return int.Parse(parameters[i]);
	}
}
