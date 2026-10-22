using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class VGUITextList : VGUIImage
{
	private VGUITextListLine[] lines;

	public override void Awake()
	{
		Component[] componentsInChildren = gameObject.GetComponentsInChildren(typeof(VGUITextListLine), true);
		lines = new VGUITextListLine[Extensions.get_length((System.Array)componentsInChildren)];
		int i = 0;
		Component[] array = componentsInChildren;
		for (int length = array.Length; i < length; i++)
		{
			string s = ((VGUITextListLine)array[i]).name.Substring(Extensions.get_length("Line"));
			try
			{
				int num = int.Parse(s);
				lines[num - 1] = (VGUITextListLine)array[i];
			}
			catch (Exception)
			{
				Debug.LogError("VGUITExtList: Line named " + ((VGUITextListLine)array[i]).name + " could not be placed to list, please rename!");
			}
		}
	}

	public virtual VGUITextListLine getLine(int index)
	{
		return (index >= 0 && index < Extensions.get_length((System.Array)lines)) ? lines[index] : null;
	}

	public virtual int getLineAmount()
	{
		return Extensions.get_length((System.Array)lines);
	}

	public virtual void setAllTexts(UnityScript.Lang.Array to)
	{
		for (int i = 0; i < getLineAmount(); i++)
		{
			VGUITextListLine line = getLine(i);
			if (to.length > i)
			{
				line.setText(to[i] as string);
			}
			else
			{
				line.deactivate();
			}
		}
	}

	public virtual void setAllTexts(string[] to)
	{
		for (int i = 0; i < getLineAmount(); i++)
		{
			VGUITextListLine line = getLine(i);
			if (Extensions.get_length((System.Array)to) > i)
			{
				line.setText(to[i]);
			}
			else
			{
				line.deactivate();
			}
		}
	}

	public virtual void setAllLineColors(Color to)
	{
		for (int i = 0; i < getLineAmount(); i++)
		{
			getLine(i).setColor(to);
		}
	}

	public virtual void deactivateAllLines()
	{
		for (int i = 0; i < getLineAmount(); i++)
		{
			getLine(i).deactivate();
		}
	}
}
