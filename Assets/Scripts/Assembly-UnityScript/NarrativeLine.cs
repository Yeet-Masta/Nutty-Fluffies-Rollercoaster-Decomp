using System;
using UnityEngine;

[Serializable]
public class NarrativeLine
{
	public string textContent;

	public AudioClip narrateAudio;

	public UnityScript.Lang.Array commands;

	public float duration;

	public NarrativeLine(string text)
	{
		commands = NarrativeCommandReader.findAllCommands(text);
		textContent = NarrativeCommandReader.removeAllCommands(text);
		NarrativeCommand narrativeCommand = NarrativeCommandReader.findCommand(commands, "narrateAudio");
		if (narrativeCommand != null)
		{
			narrateAudio = (AudioClip)Resources.Load(narrativeCommand.parameters[0]);
			if (narrateAudio == null)
			{
				Debug.LogWarning("Narrative audio " + narrativeCommand.parameters[0] + " not found, using silence...");
			}
		}
		if (narrateAudio == null)
		{
			duration = (float)textContent.Length * 0.1f;
		}
		else
		{
			duration = narrateAudio.length;
		}
	}
}
