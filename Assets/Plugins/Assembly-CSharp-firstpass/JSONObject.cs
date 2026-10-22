using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JSONObject : Nullable
{
	public enum Type
	{
		NULL = 0,
		STRING = 1,
		NUMBER = 2,
		OBJECT = 3,
		ARRAY = 4,
		BOOL = 5
	}

	private const int MAX_DEPTH = 1000;

	private const string INFINITY = "\"INFINITY\"";

	private const string NEGINFINITY = "\"NEGINFINITY\"";

	public JSONObject parent;

	public Type type;

	public ArrayList list = new ArrayList();

	public ArrayList keys = new ArrayList();

	public string str;

	public double n;

	public bool b;

	public bool isContainer
	{
		get
		{
			return type == Type.ARRAY || type == Type.OBJECT;
		}
	}

	public int Count
	{
		get
		{
			return list.Count;
		}
	}

	public static JSONObject nullJO
	{
		get
		{
			return new JSONObject(Type.NULL);
		}
	}

	public static JSONObject obj
	{
		get
		{
			return new JSONObject(Type.OBJECT);
		}
	}

	public static JSONObject arr
	{
		get
		{
			return new JSONObject(Type.ARRAY);
		}
	}

	public JSONObject this[int index]
	{
		get
		{
			if (list.Count > index)
			{
				return (JSONObject)list[index];
			}
			return null;
		}
		set
		{
			if (list.Count > index)
			{
				list[index] = value;
			}
		}
	}

	public JSONObject this[string index]
	{
		get
		{
			return GetField(index);
		}
		set
		{
			SetField(index, value);
		}
	}

	public JSONObject(Type t)
	{
		type = t;
		switch (t)
		{
		case Type.ARRAY:
			list = new ArrayList();
			break;
		case Type.OBJECT:
			list = new ArrayList();
			keys = new ArrayList();
			break;
		}
	}

	public JSONObject(bool b)
	{
		type = Type.BOOL;
		this.b = b;
	}

	public JSONObject(float f)
	{
		type = Type.NUMBER;
		n = f;
	}

	public JSONObject(Dictionary<string, string> dic)
	{
		type = Type.OBJECT;
		foreach (KeyValuePair<string, string> item in dic)
		{
			keys.Add(item.Key);
			list.Add(new JSONObject
			{
				type = Type.STRING,
				str = item.Value
			});
		}
	}

	public JSONObject()
	{
	}

	public JSONObject(string str)
	{
		if (str != null)
		{
			str = str.Replace("\t", string.Empty);
			str = str.Replace("\n", string.Empty);
			str = str.Replace("\r", string.Empty);
			while (str.Length > 0 && str[0] == ' ')
			{
				str = str.Substring(1);
			}
			while (str.Length > 0 && str[str.Length - 1] == ' ')
			{
				str = str.Substring(0, str.Length - 1);
			}
			if (str.Length <= 0)
			{
				return;
			}
			if (string.Compare(str.ToLower(), "true") == 0)
			{
				type = Type.BOOL;
				b = true;
				return;
			}
			if (string.Compare(str.ToLower(), "false") == 0)
			{
				type = Type.BOOL;
				b = false;
				return;
			}
			switch (str)
			{
			case "null":
				type = Type.NULL;
				return;
			case "\"INFINITY\"":
				type = Type.NUMBER;
				n = double.PositiveInfinity;
				return;
			case "\"NEGINFINITY\"":
				type = Type.NUMBER;
				n = double.NegativeInfinity;
				return;
			}
			if (str[0] == '"')
			{
				type = Type.STRING;
				this.str = str.Substring(1, str.Length - 2);
				return;
			}
			try
			{
				n = float.Parse(str);
				type = Type.NUMBER;
				return;
			}
			catch (Exception)
			{
				int i = 0;
				switch (str[0])
				{
				case '{':
					type = Type.OBJECT;
					keys = new ArrayList();
					list = new ArrayList();
					break;
				case '[':
					type = Type.ARRAY;
					list = new ArrayList();
					break;
				default:
					type = Type.NULL;
					Debug.LogWarning("improper JSON formatting:" + str);
					return;
				}
				int num = 0;
				bool flag = false;
				bool flag2 = false;
				for (int j = 1; j < str.Length; j++)
				{
					if (str[j] == ' ')
					{
						continue;
					}
					if (str[j] == '\\' || str[j] == '\t' || str[j] == '\n' || str[j] == '\r')
					{
						j++;
						continue;
					}
					if (str[j] == '"')
					{
						flag = !flag;
					}
					else if (str[j] == '[' || str[j] == '{')
					{
						num++;
					}
					if (num == 0 && !flag)
					{
						if (str[j] == ':' && !flag2)
						{
							flag2 = true;
							for (; str[i + 1] == ' '; i++)
							{
							}
							try
							{
								string text = str.Substring(i + 2, j - i - 2);
								while (text[text.Length - 1] != '"')
								{
									text = text.Substring(0, text.Length - 1);
								}
								text = text.Substring(0, text.Length - 1);
								keys.Add(text);
							}
							catch
							{
								Debug.Log(j + " - " + str.Length + " - " + str);
							}
							i = j;
						}
						if (str[j] == ',')
						{
							flag2 = false;
							for (; str[i] == ' '; i++)
							{
							}
							list.Add(new JSONObject(str.Substring(i + 1, j - i - 1)));
							i = j;
						}
						if (str[j] == ']' || str[j] == '}')
						{
							for (; str[i] == ' '; i++)
							{
							}
							list.Add(new JSONObject(str.Substring(i + 1, j - i - 1)));
						}
					}
					if (str[j] == ']' || str[j] == '}')
					{
						num--;
					}
				}
				return;
			}
		}
		type = Type.NULL;
	}

	public void Absorb(JSONObject obj)
	{
		list.AddRange(obj.list);
		keys.AddRange(obj.keys);
		str = obj.str;
		n = obj.n;
		b = obj.b;
		type = obj.type;
	}

	public void Add(bool val)
	{
		Add(new JSONObject(val));
	}

	public void Add(float val)
	{
		Add(new JSONObject(val));
	}

	public void Add(int val)
	{
		Add(new JSONObject(val));
	}

	public void Add(JSONObject obj)
	{
		if ((bool)obj)
		{
			if (type != Type.ARRAY)
			{
				type = Type.ARRAY;
				Debug.LogWarning("tried to add an object to a non-array JSONObject.  We'll do it for you, but you might be doing something wrong.");
			}
			list.Add(obj);
		}
	}

	public void AddField(string name, bool val)
	{
		AddField(name, new JSONObject(val));
	}

	public void AddField(string name, float val)
	{
		AddField(name, new JSONObject(val));
	}

	public void AddField(string name, int val)
	{
		AddField(name, new JSONObject(val));
	}

	public void AddField(string name, string val)
	{
		AddField(name, new JSONObject
		{
			type = Type.STRING,
			str = val
		});
	}

	public void AddField(string name, JSONObject obj)
	{
		if ((bool)obj)
		{
			if (type != Type.OBJECT)
			{
				type = Type.OBJECT;
				Debug.LogWarning("tried to add a field to a non-object JSONObject.  We'll do it for you, but you might be doing something wrong.");
			}
			keys.Add(name);
			list.Add(obj);
		}
	}

	public void SetField(string name, JSONObject obj)
	{
		if (HasField(name))
		{
			list.Remove(this[name]);
			keys.Remove(name);
		}
		AddField(name, obj);
	}

	public JSONObject GetField(string name)
	{
		if (type == Type.OBJECT)
		{
			for (int i = 0; i < keys.Count; i++)
			{
				if ((string)keys[i] == name)
				{
					return (JSONObject)list[i];
				}
			}
		}
		return null;
	}

	public bool HasField(string name)
	{
		if (type == Type.OBJECT)
		{
			for (int i = 0; i < keys.Count; i++)
			{
				if ((string)keys[i] == name)
				{
					return true;
				}
			}
		}
		return false;
	}

	public JSONObject GetFieldSafe(string name, JSONObject defValue)
	{
		if (HasField(name))
		{
			return GetField(name);
		}
		return defValue;
	}

	public void Clear()
	{
		type = Type.NULL;
		list.Clear();
		keys.Clear();
		str = string.Empty;
		n = 0.0;
		b = false;
	}

	public JSONObject Copy()
	{
		return new JSONObject(print());
	}

	public void Merge(JSONObject obj)
	{
		MergeRecur(this, obj);
	}

	private static void MergeRecur(JSONObject left, JSONObject right)
	{
		if (left.type == Type.NULL)
		{
			left.Absorb(right);
		}
		else if (left.type == Type.OBJECT && right.type == Type.OBJECT)
		{
			for (int i = 0; i < right.list.Count; i++)
			{
				string text = (string)right.keys[i];
				if (right[i].isContainer)
				{
					if (left.HasField(text))
					{
						MergeRecur(left[text], right[i]);
					}
					else
					{
						left.AddField(text, right[i]);
					}
				}
				else if (left.HasField(text))
				{
					left.SetField(text, right[i]);
				}
				else
				{
					left.AddField(text, right[i]);
				}
			}
		}
		else
		{
			if (left.type != Type.ARRAY || right.type != Type.ARRAY)
			{
				return;
			}
			if (right.Count > left.Count)
			{
				Debug.LogError("Cannot merge arrays when right object has more elements");
				return;
			}
			for (int j = 0; j < right.list.Count; j++)
			{
				if (left[j].type == right[j].type)
				{
					if (left[j].isContainer)
					{
						MergeRecur(left[j], right[j]);
					}
					else
					{
						left[j] = right[j];
					}
				}
			}
		}
	}

	public string print()
	{
		float time = Time.time;
		string result = print(0);
		float time2 = Time.time;
		Debug.Log("JSON Save time: " + (time2 - time) + "s");
		return result;
	}

	public string print(int depth)
	{
		if (depth++ > 1000)
		{
			Debug.Log("reached max depth!");
			return string.Empty;
		}
		string text = string.Empty;
		switch (type)
		{
		case Type.STRING:
			text = "\"" + str + "\"";
			break;
		case Type.NUMBER:
			if (n != double.PositiveInfinity)
			{
				text = ((n != double.NegativeInfinity) ? (text + n) : "\"NEGINFINITY\"");
			}
			else
			{
				text = "\"INFINITY\"";
			}
			break;
		case Type.OBJECT:
			if (list.Count > 0)
			{
				text = "{";
				text += "\n";
				depth++;
				for (int i = 0; i < list.Count; i++)
				{
					string text2 = (string)keys[i];
					JSONObject jSONObject = (JSONObject)list[i];
					if ((bool)jSONObject)
					{
						for (int j = 0; j < depth; j++)
						{
							text += "  ";
						}
						text = text + "\"" + text2 + "\" : ";
						text = text + jSONObject.print(depth) + ",";
						text += "\n";
					}
				}
				text = text.Substring(0, text.Length - 1);
				text = text.Substring(0, text.Length - 1);
				text += "\n";
				for (int k = 0; k < depth - 2; k++)
				{
					text += "  ";
				}
				text += "}";
			}
			else
			{
				text = "null";
			}
			break;
		case Type.ARRAY:
			if (list.Count > 0)
			{
				text = "[";
				text += "\n";
				depth++;
				foreach (JSONObject item in list)
				{
					if ((bool)item)
					{
						for (int l = 0; l < depth; l++)
						{
							text += "  ";
						}
						text = text + item.print(depth) + ",";
						text += "\n";
					}
				}
				text = text.Substring(0, text.Length - 1);
				text = text.Substring(0, text.Length - 1);
				text += "\n";
				for (int m = 0; m < depth - 2; m++)
				{
					text += "  ";
				}
				text += "]";
			}
			else
			{
				text = "null";
			}
			break;
		case Type.BOOL:
			text = ((!b) ? "false" : "true");
			break;
		case Type.NULL:
			text = "null";
			break;
		}
		return text;
	}

	public Dictionary<string, string> ToDictionary()
	{
		if (type == Type.OBJECT)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			for (int i = 0; i < list.Count; i++)
			{
				JSONObject jSONObject = (JSONObject)list[i];
				switch (jSONObject.type)
				{
				case Type.STRING:
					dictionary.Add((string)keys[i], jSONObject.str);
					break;
				case Type.NUMBER:
					dictionary.Add((string)keys[i], jSONObject.n + string.Empty);
					break;
				case Type.BOOL:
					dictionary.Add((string)keys[i], jSONObject.b + string.Empty);
					break;
				default:
					Debug.LogWarning("Omitting object: " + (string)keys[i] + " in dictionary conversion");
					break;
				}
			}
			return dictionary;
		}
		Debug.LogWarning("Tried to turn non-Object JSONObject into a dictionary");
		return null;
	}

	public Vector2 DecodeVector2(string vecName)
	{
		JSONObject field = GetField(vecName);
		if (field == null)
		{
			return new Vector2(0f, 0f);
		}
		float x = (float)field.GetField("x").n;
		float y = (float)field.GetField("y").n;
		return new Vector2(x, y);
	}

	public JSONObject EncodeVector2(string fieldName, Vector2 value)
	{
		JSONObject jSONObject = obj;
		JSONObject jSONObject2 = arr;
		jSONObject.type = Type.ARRAY;
		jSONObject.Add(jSONObject2);
		jSONObject.type = Type.ARRAY;
		jSONObject2.type = Type.OBJECT;
		jSONObject2.AddField("x", value.x);
		jSONObject2.AddField("y", value.y);
		jSONObject2.type = Type.OBJECT;
		AddField(fieldName, jSONObject2);
		return jSONObject;
	}

	public Vector3 DecodeColor3(string cName)
	{
		JSONObject field = GetField(cName);
		if (field == null)
		{
			return new Vector3(0f, 0f, 0f);
		}
		float x = (float)field.GetField("r").n;
		float y = (float)field.GetField("g").n;
		float z = (float)field.GetField("b").n;
		return new Vector3(x, y, z);
	}

	public JSONObject EncodeColor3(string fieldName, Vector3 value)
	{
		JSONObject jSONObject = obj;
		JSONObject jSONObject2 = arr;
		jSONObject.type = Type.ARRAY;
		jSONObject.Add(jSONObject2);
		jSONObject.type = Type.ARRAY;
		jSONObject2.type = Type.OBJECT;
		jSONObject2.AddField("r", value.x);
		jSONObject2.AddField("g", value.y);
		jSONObject2.AddField("b", value.z);
		jSONObject2.type = Type.OBJECT;
		AddField(fieldName, jSONObject2);
		return jSONObject;
	}
}
