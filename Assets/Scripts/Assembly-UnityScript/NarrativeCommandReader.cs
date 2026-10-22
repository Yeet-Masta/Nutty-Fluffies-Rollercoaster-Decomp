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
		object result;
		while (true)
		{
			if (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				if (!(obj is NarrativeCommand))
				{
					obj = RuntimeServices.Coerce(obj, typeof(NarrativeCommand));
				}
				NarrativeCommand narrativeCommand = (NarrativeCommand)obj;
				if (narrativeCommand.name == name)
				{
					result = narrativeCommand;
					break;
				}
				continue;
			}
			result = null;
			break;
		}
		return (NarrativeCommand)result;
	}

	public static UnityScript.Lang.Array findAllCommands(string from)
	{
		UnityScript.Lang.Array array = new UnityScript.Lang.Array();
		for (int num = from.IndexOf("{"); num > -1; num = from.IndexOf("{", num))
		{
			int num2 = from.IndexOf("}", num);
			array.Push(new NarrativeCommand(num + 1, from.Substring(num + 1, num2 - num - 1)));
			from = from.Remove(num, num2 - num + 1);
		}
		return array;
	}

	public static string removeAllCommands(string from)
	{
		for (int num = from.IndexOf("{"); num > -1; num = from.IndexOf("{", num))
		{
			int num2 = from.IndexOf("}", num);
			from = from.Remove(num, num2 - num + 1);
		}
		return from;
	}
}
