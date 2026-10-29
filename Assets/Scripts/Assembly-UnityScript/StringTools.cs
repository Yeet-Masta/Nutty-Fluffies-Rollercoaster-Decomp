using System;

[Serializable]
public class StringTools
{
	public static bool stringContainsAny(string str, string[] arr)
	{
		int num = 0;
		int length = arr.Length;
		while (num < length)
		{
			if (str.IndexOf(arr[num]) != -1)
			{
				return true;
			}
			num++;
		}
		return false;
	}

	public static UnityScript.Lang.Array getLines(string str, int lineLength)
	{
		UnityScript.Lang.Array array = new UnityScript.Lang.Array();
		while (str.Length > lineLength)
		{
			bool flag = false;
			for (int num = lineLength; num >= 0; num--)
			{
				if (str.Substring(num, 1) == " ")
				{
					array.Add(str.Substring(0, num));
					str = str.Substring(num + 1);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				array.Add(str.Substring(0, lineLength));
				str = str.Substring(lineLength + 1);
			}
		}
		array.Add(str);
		return array;
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
