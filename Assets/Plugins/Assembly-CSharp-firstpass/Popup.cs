using UnityEngine;

public class Popup
{
	public delegate void ListCallBack();

	private static int popupListHash = "PopupList".GetHashCode();

	private static int initialListIndex = 0;

	public static bool List(Rect position, ref bool showList, ref int listEntry, GUIContent buttonContent, GUIContent[] list, GUIStyle listStyle, ListCallBack callBack, params CustomLayoutOption[] options)
	{
		return List(position, ref showList, ref listEntry, buttonContent, list, "button", "box", listStyle, callBack, options);
	}

	public static bool List(Rect position, ref bool showList, ref int listEntry, string buttonContent, string[] list, GUIStyle listStyle, ListCallBack callBack, params CustomLayoutOption[] options)
	{
		GUIContent[] array = new GUIContent[list.Length];
		int num = 0;
		foreach (string text in list)
		{
			array[num++] = new GUIContent(text);
		}
		return List(position, ref showList, ref listEntry, new GUIContent(buttonContent), array, "button", "box", listStyle, callBack, options);
	}

	public static bool List(Rect position, ref bool showList, ref int listEntry, GUIContent buttonContent, GUIContent[] list, GUIStyle buttonStyle, GUIStyle boxStyle, GUIStyle listStyle, ListCallBack callBack, params CustomLayoutOption[] options)
	{
		int controlID = GUIUtility.GetControlID(popupListHash, FocusType.Passive);
		bool flag = false;
		float num = 0f;
		float num2 = 20f;
		Rect position2 = new Rect(position);
		bool flag2 = false;
		if (Event.current.GetTypeForControl(controlID) == EventType.MouseDown && position.Contains(Event.current.mousePosition))
		{
			GUIUtility.hotControl = controlID;
			showList = true;
		}
		if (showList)
		{
			if (initialListIndex == -1)
			{
				initialListIndex = listEntry;
			}
			string[] array = new string[list.Length];
			for (int i = 0; i < list.Length; i++)
			{
				array[i] = list[i].text;
				Vector2 vector = listStyle.CalcSize(list[i]);
				if (vector.x > num)
				{
					num = vector.x;
				}
				num2 = vector.y;
			}
			foreach (CustomLayoutOption customLayoutOption in options)
			{
				if (customLayoutOption.type == CustomLayoutOption.Type.minWidth && (float)customLayoutOption.value > num)
				{
					num = (float)customLayoutOption.value;
				}
			}
			position2 = new Rect(position.x, position.y - (float)initialListIndex * num2 - 4f, num, (float)list.Length * num2);
			position2.width += 8f;
			position2.height += 8f;
		}
		if (Event.current.GetTypeForControl(controlID) == EventType.MouseUp && showList)
		{
			flag = true;
			initialListIndex = -1;
			if (position2.Contains(Event.current.mousePosition))
			{
				flag2 = true;
			}
		}
		GUI.Label(position, buttonContent, buttonStyle);
		if (showList)
		{
			GUI.Box(position2, string.Empty, boxStyle);
			position2.x += 4f;
			position2.y += 4f;
			position2.width -= 8f;
			position2.height -= 8f;
			listEntry = GUI.SelectionGrid(position2, listEntry, list, 1, listStyle);
		}
		if (flag2 && callBack != null)
		{
			callBack();
		}
		if (flag)
		{
			showList = false;
		}
		return flag;
	}
}
