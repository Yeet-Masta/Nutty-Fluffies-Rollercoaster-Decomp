using System;
using Boo.Lang.Runtime;

[Serializable]
public class TextTemplate
{
	private static string[] val = new string[3];

	public static string getString(string template, string value)
	{
		val[0] = value;
		val[1] = null;
		val[2] = null;
		return getString(template, val);
	}

	public static string getString(string template, string value1, string value2)
	{
		val[0] = value1;
		val[1] = value2;
		val[2] = null;
		return getString(template, val);
	}

	public static string getString(string template, string value1, string value2, string value3)
	{
		val[0] = value1;
		val[1] = value2;
		val[2] = value3;
		return getString(template, val);
	}

	public static string getString(string template, string[] values)
	{
		string text = template;
		if (!RuntimeServices.EqualityOperator(values, null))
		{
			for (int i = 0; i < values.Length && values[i] != null; i++)
			{
				string oldValue = "%" + (i + 1);
				if (values.Length > i)
				{
					text = text.Replace(oldValue, values[i]);
				}
			}
		}
		return text;
	}
}
