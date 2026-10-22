using System;
using Boo.Lang;
using UnityEngine;

[Serializable]
public class GUIControls : MonoBehaviour
{
	[NonSerialized]
	public const int Command = 1;

	[NonSerialized]
	public const int Shift = 2;

	[NonSerialized]
	public const int Alt = 4;

	public static Color RGBSlider(Color c, string label)
	{
		GUI.color = c;
		GUILayout.Label(label);
		GUI.color = Color.red;
		c.r = GUILayout.HorizontalSlider(c.r, 0f, 1f);
		GUI.color = Color.green;
		c.g = GUILayout.HorizontalSlider(c.g, 0f, 1f);
		GUI.color = Color.blue;
		c.b = GUILayout.HorizontalSlider(c.b, 0f, 1f);
		GUI.color = Color.white;
		return c;
	}

	public static Color RGBCircle(Rect r, Color c, string label, Texture2D colorCircle)
	{
		r.height = (r.width -= 15f);
		Rect position = new Rect(r.x + r.width + 0f, r.y, 20f, r.height);
		HSBColor hSBColor = new HSBColor(c);
		Vector2 vector = new Vector2(r.x + r.width / 2f, r.y + r.height / 2f);
		int controlID = GUIUtility.GetControlID(0, FocusType.Passive, r);
		switch (Event.current.GetTypeForControl(controlID))
		{
		case EventType.MouseDown:
			if (r.Contains(Event.current.mousePosition))
			{
				GUIUtility.hotControl = controlID;
				Event.current.Use();
			}
			break;
		case EventType.MouseUp:
			if (GUIUtility.hotControl == controlID)
			{
				GUIUtility.hotControl = 0;
				Event.current.Use();
			}
			break;
		}
		if (GUIUtility.hotControl == controlID && Event.current.type == EventType.MouseDrag && Event.current.button == 0)
		{
			Vector2 zero = Vector2.zero;
			zero.x = vector.x - Event.current.mousePosition.x;
			zero.y = vector.y - Event.current.mousePosition.y;
			float value = Mathf.Sqrt(zero.x * zero.x + zero.y * zero.y);
			value = Mathf.Clamp(value, 0f, r.width / 2f);
			float num = Vector3.Angle(new Vector3(-1f, 0f, 0f), zero);
			if (!(zero.y >= 0f))
			{
				num = 360f - num;
			}
			hSBColor.h = num / 360f;
			hSBColor.s = value / (r.width / 2f);
			Event.current.Use();
		}
		HSBColor hSBColor2 = new HSBColor(c);
		hSBColor2.b = 1f;
		Color color = hSBColor2.ToColor();
		GUI.color = color;
		hSBColor.b = GUI.VerticalSlider(position, hSBColor.b, 1f, 0f);
		GUI.color = Color.white * hSBColor.b;
		int num2 = 1;
		Color color2 = GUI.color;
		float num3 = (color2.a = num2);
		Color color3 = (GUI.color = color2);
		GUI.Box(r, new GUIContent(colorCircle, "Choose color"), GUIStyle.none);
		Vector2 vector2 = new Vector2(Mathf.Cos(hSBColor.h * 360f * ((float)Math.PI / 180f)), 0f - Mathf.Sin(hSBColor.h * 360f * ((float)Math.PI / 180f))) * r.width * hSBColor.s / 2f;
		GUI.color = c;
		GUI.Box(new Rect(vector2.x - 5f + vector.x, vector2.y - 5f + vector.y, 10f, 10f), string.Empty);
		GUI.color = Color.white;
		c = hSBColor.ToColor();
		return c;
	}

	public static string keyCodeToAscii(KeyCode vk)
	{
		string text = "<invalid>";
		switch (vk)
		{
		case KeyCode.Return:
			text = "RETURN";
			break;
		case KeyCode.Alpha1:
			text = "1";
			break;
		case KeyCode.Alpha2:
			text = "2";
			break;
		case KeyCode.Alpha3:
			text = "3";
			break;
		case KeyCode.Alpha4:
			text = "4";
			break;
		case KeyCode.Alpha5:
			text = "5";
			break;
		case KeyCode.Alpha6:
			text = "6";
			break;
		case KeyCode.G:
			text = "G";
			break;
		case KeyCode.I:
			text = "I";
			break;
		case KeyCode.L:
			text = "L";
			break;
		case KeyCode.N:
			text = "N";
			break;
		case KeyCode.O:
			text = "O";
			break;
		case KeyCode.P:
			text = "P";
			break;
		case KeyCode.R:
			text = "R";
			break;
		case KeyCode.S:
			text = "S";
			break;
		case KeyCode.T:
			text = "T";
			break;
		case KeyCode.W:
			text = "W";
			break;
		case KeyCode.Z:
			text = "Z";
			break;
		case KeyCode.V:
			text = "V";
			break;
		default:
			Debug.LogError("Implement keycode for " + vk + "!!!");
			break;
		}
		return "result";
	}

	public static GUIContent btnTitle(GUIContent cnt, KeyCode key, int mod)
	{
		GUIContent result;
		if (key == KeyCode.None)
		{
			result = cnt;
		}
		else
		{
			string lhs = " [";
			if ((mod & 1) != 0)
			{
				lhs = ((Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.OSXPlayer) ? (lhs + "CTRL + ") : (lhs + "CMD + "));
			}
			if ((mod & 2) != 0)
			{
				lhs += "SHIFT + ";
			}
			if ((mod & 4) != 0)
			{
				lhs = ((Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.OSXPlayer) ? (lhs + "ALT + ") : (lhs + "OPT + "));
			}
			lhs += keyCodeToAscii(key);
			lhs += "]";
			cnt.tooltip += lhs;
			result = cnt;
		}
		return result;
	}

	public static bool matchModifiers(int mod)
	{
		int result;
		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer)
		{
			if (Event.current.command == ((mod & 1) != 0))
			{
				goto IL_0056;
			}
			result = 0;
		}
		else
		{
			if (Event.current.control == ((mod & 1) != 0))
			{
				goto IL_0056;
			}
			result = 0;
		}
		goto IL_0096;
		IL_0096:
		return (byte)result != 0;
		IL_0056:
		result = ((Event.current.alt == ((mod & 4) != 0)) ? ((Event.current.shift == ((mod & 2) != 0)) ? 1 : 0) : 0);
		goto IL_0096;
	}

	public static void Toggle(Rect r, bool state, Texture2D cnt, string inactiveStr, string activeStr, KeyCode key, int mod, ICallable callback)
	{
		Toggle(r, state, cnt, inactiveStr, activeStr, key, mod, GUI.skin.button, callback);
	}

	public static void Toggle(Rect r, bool state, Texture2D cnt, string inactiveStr, string activeStr, KeyCode key, int mod, GUIStyle style, ICallable callback)
	{
		if (state != GUI.Toggle(r, state, btnTitle(new GUIContent(cnt, (!state) ? inactiveStr : activeStr), key, mod), style))
		{
			callback.Call(new object[0]);
		}
		else if (Event.current.type == EventType.KeyDown && Event.current.keyCode == key && key != KeyCode.None && matchModifiers(mod) && GUIUtility.keyboardControl == 0)
		{
			callback.Call(new object[0]);
			Event.current.Use();
		}
	}

	public static void Button(Rect r, Texture2D cnt, string tooptip, KeyCode key, int mod, ICallable callback)
	{
		Button(r, cnt, tooptip, key, mod, GUI.skin.button, callback);
	}

	public static void Button(Rect r, Texture2D cnt, string tooptip, KeyCode key, int mod, GUIStyle style, ICallable callback)
	{
		if (GUI.Button(r, btnTitle(new GUIContent(cnt, tooptip), key, mod), style))
		{
			callback.Call(new object[0]);
		}
		else if (Event.current.type == EventType.KeyDown && Event.current.keyCode == key && key != KeyCode.None && matchModifiers(mod))
		{
			callback.Call(new object[0]);
			Event.current.Use();
		}
	}

	public virtual void Main()
	{
	}
}
