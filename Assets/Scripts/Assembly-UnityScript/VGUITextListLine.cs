using System;
using UnityEngine;

[Serializable]
public class VGUITextListLine : VGUIElement
{
	private int index;

	public VGUIText textObject;

	public VGUITextListLine(int index)
	{
		this.index = index;
	}

	public virtual void setText(string to)
	{
		activate();
		if (textObject != null)
		{
			textObject.setText(to);
		}
	}

	public virtual void setTextVisible(bool to)
	{
		textObject.setActive(to);
	}

	public virtual void setColor(Color to)
	{
		if (textObject != null)
		{
			textObject.setColor(to);
		}
	}
}
