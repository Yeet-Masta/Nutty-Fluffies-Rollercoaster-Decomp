using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Boo.Lang.Runtime;
using UnityScript.Lang;

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
		return (!list.ContainsKey(key)) ? defaultValue : list[key];
	}

	public virtual int getIntValue(string key, int defaultValue)
	{
		return (!list.ContainsKey(key)) ? defaultValue : int.Parse(list[key]);
	}

	public virtual void load(JSONObject j)
	{
		JSONObject fieldSafe = j.GetFieldSafe("settings", null);
		if (RuntimeServices.EqualityOperator(fieldSafe, null))
		{
			return;
		}
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(fieldSafe.list);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is JSONObject))
			{
				obj = RuntimeServices.Coerce(obj, typeof(JSONObject));
			}
			JSONObject jSONObject = (JSONObject)obj;
			string str = jSONObject.GetFieldSafe("key", new JSONObject(0f)).str;
			UnityRuntimeServices.Update(enumerator, jSONObject);
			string str2 = jSONObject.GetFieldSafe("value", new JSONObject(string.Empty)).str;
			UnityRuntimeServices.Update(enumerator, jSONObject);
			setValue(str, str2);
		}
	}

	public virtual void loadBinary(BinaryReader rs)
	{
		int num = rs.ReadByte();
		for (int i = 0; i < num; i++)
		{
			string key = rs.ReadString();
			string value = rs.ReadString();
			setValue(key, value);
		}
	}
}
