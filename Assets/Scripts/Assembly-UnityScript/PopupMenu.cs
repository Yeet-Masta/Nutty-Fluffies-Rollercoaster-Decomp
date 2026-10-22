using System;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class PopupMenu
{
	public int listEntry;

	private bool showList;

	private GUIContent[] list;

	private GUIStyle listStyle;

	private GUIContent selected;

	public PopupMenu()
	{
		selected = new GUIContent(string.Empty);
	}

	public virtual void setListStyle(GUIStyle style)
	{
		listStyle = style;
	}

	public virtual void setItems(string[] whom)
	{
		int num = 0;
		list = new GUIContent[Extensions.get_length((System.Array)whom)];
		int i = 0;
		for (int length = whom.Length; i < length; i++)
		{
			list[num++] = new GUIContent(whom[i]);
		}
		listEntry = Mathf.Clamp(listEntry, 0, list.Length - 1);
	}

	public virtual string[] getItems()
	{
		int num = 0;
		string[] array = new string[Extensions.get_length((System.Array)list)];
		int i = 0;
		GUIContent[] array2 = list;
		for (int length = array2.Length; i < length; i++)
		{
			array[num++] = array2[i].text;
		}
		return array;
	}

	public virtual void setSelectedIndex(int ind)
	{
		listEntry = ind;
		selected = list[listEntry];
	}

	public virtual void setSelected(string what)
	{
		if (list[listEntry].text == what)
		{
			return;
		}
		int num = 0;
		int i = 0;
		GUIContent[] array = list;
		for (int length = array.Length; i < length; i++)
		{
			if (array[i].text == what)
			{
				setSelectedIndex(num);
				break;
			}
			num++;
		}
	}

	public virtual bool List(Rect bounds, CustomLayoutOption[] options)
	{
		if (RuntimeServices.EqualityOperator(listStyle, null))
		{
			listStyle = GUI.skin.button;
		}
		Vector2 vector = GUIUtility.GUIToScreenPoint(new Vector2(0f, 0f));
		bool result = false;
		GUI.EndGroup();
		if (Popup.List(new Rect(bounds.x + vector.x, bounds.y + vector.y, bounds.width, bounds.height), ref showList, ref listEntry, selected, list, listStyle, null, options))
		{
			listEntry = Mathf.Clamp(listEntry, 0, list.Length - 1);
			selected = list[listEntry];
			result = true;
		}
		GUI.BeginGroup(new Rect(vector.x, vector.y, 4096f, 4096f));
		return result;
	}
}
