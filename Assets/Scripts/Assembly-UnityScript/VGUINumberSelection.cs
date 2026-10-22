using System;
using UnityEngine;

[Serializable]
public class VGUINumberSelection : VGUIButton
{
	protected float selectedValue;

	public float maxValue;

	public float minValue;

	public float step;

	public VGUIText showText;

	public bool saveToPrefs;

	public string prefsName;

	public float defaultValue;

	public VGUINumberSelection()
	{
		maxValue = 10f;
		minValue = 1f;
		step = 1f;
		defaultValue = 5f;
	}

	public override void Start()
	{
		setSelectedValue(defaultValue);
		if (saveToPrefs)
		{
			setSelectedValue(PlayerPrefs.GetFloat(prefsName, selectedValue));
		}
	}

	public virtual void setSelectedValue(float to)
	{
		selectedValue = to;
		showText.setText(string.Empty + selectedValue);
	}

	public virtual int getSelectedValue()
	{
		return (int)selectedValue;
	}

	public virtual void nextValue()
	{
		int num = (int)(selectedValue + step);
		if (!((float)num <= maxValue))
		{
			num = (int)maxValue;
		}
		setSelectedValue(num);
		save();
		activateInvoke(false);
	}

	public virtual void previousValue()
	{
		int num = (int)(selectedValue - step);
		if (!((float)num >= minValue))
		{
			num = (int)minValue;
		}
		setSelectedValue(num);
		save();
		activateInvoke(false);
	}

	private void save()
	{
		if (saveToPrefs)
		{
			PlayerPrefs.SetFloat(prefsName, selectedValue);
		}
	}

	public static float getSavedValue(string id, float def)
	{
		return PlayerPrefs.GetFloat(id, def);
	}
}
