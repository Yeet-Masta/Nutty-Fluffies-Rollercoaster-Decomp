using System;
using System.Collections.Generic;
using System.IO;

[Serializable]
public class CustomSettings
{
	public Dictionary<string, string> list = new Dictionary<string, string>();

	public virtual void setValue(string key, string value)
	{
		list[key] = value;
	}

	public virtual string getValue(string key, string defaultValue)
	{
		return list.ContainsKey(key) ? list[key] : defaultValue;
	}

	public virtual int getIntValue(string key, int defaultValue)
	{
		return list.ContainsKey(key) ? int.Parse(list[key]) : defaultValue;
	}

	public virtual void load(JSONObject j)
	{
		JSONObject fieldSafe = j.GetFieldSafe("settings", null);
		if (fieldSafe == null)
		{
			return;
		}
		foreach (JSONObject json in fieldSafe.list)
		{
			string str = json.GetFieldSafe("key", new JSONObject(0f)).str;
			string str2 = json.GetFieldSafe("value", new JSONObject(string.Empty)).str;
			setValue(str, str2);
		}
	}

	public virtual void loadBinary(BinaryReader rs)
	{
		int count = rs.ReadByte();
		for (int i = 0; i < count; i++)
		{
			string key = rs.ReadString();
			string value = rs.ReadString();
			setValue(key, value);
		}
	}
}
