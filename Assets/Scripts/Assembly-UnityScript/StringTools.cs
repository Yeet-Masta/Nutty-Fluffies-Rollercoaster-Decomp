using System;

[Serializable]
public class StringTools
{
	public static bool stringContainsAny(string str, string[] arr)
	{
		int index = 0;
		int length = arr.Length;
		while (index < length)
		{
			if (str.IndexOf(arr[index]) != -1)
			{
				return true;
			}
			index++;
		}
		return false;
	}

	public static UnityScript.Lang.Array getLines(string str, int lineLength)
	{
		UnityScript.Lang.Array lines = new UnityScript.Lang.Array();
		while (str.Length > lineLength)
		{
			bool flag = false;
			for (int pos = lineLength; pos >= 0; pos--)
			{
				if (str.Substring(pos, 1) == " ")
				{
					lines.Add(str.Substring(0, pos));
					str = str.Substring(pos + 1);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				lines.Add(str.Substring(0, lineLength));
				str = str.Substring(lineLength + 1);
			}
		}
		lines.Add(str);
		return lines;
	}

	public static string wrapToLines(string str, int lineLength, int maxLines)
	{
		if (str.Length <= lineLength)
		{
			return str;
		}
		else
		{
			UnityScript.Lang.Array lines = getLines(str, lineLength);
			string lhs = lines[0] as string;
			for (int i = 1; i < lines.length; i++)
			{
				lhs = i >= maxLines ? (lhs + (" " + lines[i])) : (lhs + ("\n" + lines[i]));
			}
			lhs += "\n";
			return lhs;
		}
	}

	public static int countLines(string str)
	{
		string[] a = str.Split("\n"[0]);
		return a.Length;
	}
}
