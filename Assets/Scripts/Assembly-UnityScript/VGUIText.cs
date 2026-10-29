using Boo.Lang.Runtime;
using UnityEngine;

public class VGUIText : VGUIElement
{
	public string text;

	private TextMesh textMesh;

	public bool convertToCapital;

	protected string template;

	private string preAwakeSetText;

	private string[] preAwakeFillTemplateValues;

	protected virtual void findTextMesh()
	{
		template = text;
		textMesh = (TextMesh)GetComponent(typeof(TextMesh));
		if (textMesh == null)
		{
			textMesh = (TextMesh)GetComponentInChildren(typeof(TextMesh));
		}
		if (textMesh == null)
		{
			Debug.LogError("VGUIText does not have textMesh! " + textMesh + ", this: " + gameObject.name);
		}
	}

	public override void Awake()
	{
		if (textMesh == null)
		{
			findTextMesh();
		}
		if (preAwakeSetText != null)
		{
			setText(preAwakeSetText);
		}
		if (preAwakeFillTemplateValues != null && preAwakeFillTemplateValues.Length > 0)
		{
			fillTemplate(preAwakeFillTemplateValues);
		}
		base.Awake();
	}

	public virtual void setText(string to)
	{
		if (textMesh == null)
		{
			MonoBehaviour.print("MESH NOT FOUND");
			preAwakeSetText = to;
			return;
		}
		if (convertToCapital)
		{
			to = to.ToUpper();
		}
		if (to != text)
		{
			text = to;
			setTextToMesh(text);
		}
	}

	protected virtual void setTextToMesh(string text)
	{
		textMesh.text = text;
	}

	public virtual void setColor(Color to)
	{
		if (textMesh != null)
		{
			textMesh.GetComponent<Renderer>().material.color = to;
		}
	}

	public virtual void fillTemplate(string value)
	{
		if (textMesh == null)
		{
			preAwakeFillTemplateValues = new string[1];
			preAwakeFillTemplateValues[0] = value;
		}
		else
		{
			setText(TextTemplate.getString(template, value));
		}
	}

	public virtual void fillTemplate(string value1, string value2)
	{
		if (textMesh == null)
		{
			preAwakeFillTemplateValues = new string[2];
			preAwakeFillTemplateValues[0] = value1;
			preAwakeFillTemplateValues[1] = value2;
		}
		else
		{
			setText(TextTemplate.getString(template, value1, value2));
		}
	}

	public virtual void fillTemplate(string value1, string value2, string value3)
	{
		if (textMesh == null)
		{
			preAwakeFillTemplateValues = new string[3];
			preAwakeFillTemplateValues[0] = value1;
			preAwakeFillTemplateValues[1] = value2;
			preAwakeFillTemplateValues[2] = value3;
		}
		else
		{
			setText(TextTemplate.getString(template, value1, value2, value3));
		}
	}

	public virtual void fillTemplate(string[] values)
	{
		if (textMesh == null)
		{
			preAwakeFillTemplateValues = values;
		}
		else
		{
			setText(TextTemplate.getString(template, values));
		}
	}

	public virtual string getText()
	{
		return text;
	}
}
