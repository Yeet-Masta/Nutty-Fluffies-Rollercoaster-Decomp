using System;
using System.Collections;
using Boo.Lang.Runtime;
using UnityScript.Lang;

[Serializable]
public class NarrativeCommandReader
{
	public static NarrativeCommand findCommand(UnityScript.Lang.Array commands, string name)
	{
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(commands);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is NarrativeCommand))
			{
				obj = RuntimeServices.Coerce(obj, typeof(NarrativeCommand));
			}
			NarrativeCommand narrativeCommand = (NarrativeCommand)obj;
			if (narrativeCommand.name == name)
			{
				return narrativeCommand;
			}
		}
		return null;
	}

	public static UnityScript.Lang.Array findAllCommands(string from)
	{
		UnityScript.Lang.Array commands = new UnityScript.Lang.Array();
		for (int start = from.IndexOf("{"); start > -1; start = from.IndexOf("{", start))
		{
			int end = from.IndexOf("}", start);
			commands.Push(new NarrativeCommand(start + 1, from.Substring(start + 1, end - start - 1)));
			from = from.Remove(start, end - start + 1);
		}
		return commands;
	}

	public static string removeAllCommands(string from)
	{
		for (int start = from.IndexOf("{"); start > -1; start = from.IndexOf("{", start))
		{
			int end = from.IndexOf("}", start);
			from = from.Remove(start, end - start + 1);
		}
		return from;
	}
}
