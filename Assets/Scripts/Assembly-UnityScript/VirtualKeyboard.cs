using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class VirtualKeyboard
{
	[NonSerialized]
	public static int BUTTON_HEIGHT = 35;

	[NonSerialized]
	public static int BUTTON_WIDTH = 40;

	[NonSerialized]
	public static string DELETE_CHARACTER = "\b";

	[NonSerialized]
	public static string READY_CHARACTER = "\n";

	[NonSerialized]
	public static bool IGNORE_FIRST_EVENT = true;

	public Texture keyboardImage;

	private string text;

	private bool done;

	private int rowHeight;

	private bool visible;

	private int maxLength;

	private Rect keyboardBounds;

	private Rect textBounds;

	private string textStyle;

	private bool openingFlag;

	private RowData[] rowData;

	public VirtualKeyboard()
	{
		text = string.Empty;
		rowHeight = 1;
		maxLength = 20;
		keyboardBounds = default(Rect);
		textBounds = new Rect(0f, 0f, 200f, 32f);
		visible = false;
	}

	public virtual void setImage(Texture image)
	{
		keyboardImage = image;
	}

	public virtual void setRows(int top, int bottom, int rows)
	{
		keyboardBounds.y = top;
		keyboardBounds.height = bottom - top;
		rowData = new RowData[rows];
		for (int i = 0; i < rows; i++)
		{
			rowData[i] = new RowData();
		}
		rowHeight = (int)(keyboardBounds.height / (float)rows);
	}

	public virtual void setButtons(int rowIndex, int rowStart, int rowEnd, string characters)
	{
		rowData[rowIndex].rowStart = rowStart;
		rowData[rowIndex].rowWidth = rowEnd - rowStart;
		rowData[rowIndex].characters = characters;
	}

	public virtual void setTextArea(Rect area, string style)
	{
		textBounds = area;
		textStyle = style;
	}

	public virtual void show(string txt, int max)
	{
		text = txt;
		done = false;
		maxLength = max;
		visible = true;
		openingFlag = IGNORE_FIRST_EVENT;
	}

	public virtual void hide()
	{
		done = true;
		visible = false;
	}

	public virtual void draw()
	{
		GUI.DrawTexture(new Rect((Screen.width - keyboardImage.width) / 2, (Screen.height - keyboardImage.height) / 2, keyboardImage.width, keyboardImage.height), keyboardImage);
		GUI.Label(textBounds, text, textStyle);
	}

	public virtual bool click(Vector2 point)
	{
		int num = (int)((float)Screen.height - point.y - keyboardBounds.y);
		int num2 = default(int);
		int result;
		if (openingFlag)
		{
			openingFlag = false;
			result = 0;
		}
		else
		{
			if (num > 0 && !((float)num >= keyboardBounds.height))
			{
				num2 = num / rowHeight;
				float num3 = point.x - (float)rowData[num2].rowStart;
				if (!(num3 <= 0f) && !(num3 >= (float)rowData[num2].rowWidth))
				{
					int num4 = (int)(num3 / (float)BUTTON_WIDTH);
					if (num4 >= Extensions.get_length(rowData[num2].characters))
					{
						num4 = Extensions.get_length(rowData[num2].characters) - 1;
					}
					char c = rowData[num2].characters[num4];
					if (c.ToString() == DELETE_CHARACTER)
					{
						backspace();
					}
					else if (c.ToString() == READY_CHARACTER)
					{
						hide();
					}
					else if (Extensions.get_length(text) < maxLength)
					{
						text += c;
					}
					result = 1;
					goto IL_0153;
				}
			}
			result = 0;
		}
		goto IL_0153;
		IL_0153:
		return (byte)result != 0;
	}

	public virtual bool isReady()
	{
		return done;
	}

	public virtual bool isVisible()
	{
		return visible;
	}

	public virtual string getText()
	{
		return text;
	}

	private void backspace()
	{
		if (Extensions.get_length(text) > 0)
		{
			text = text.Remove(text.Length - 1, 1);
		}
	}
}
