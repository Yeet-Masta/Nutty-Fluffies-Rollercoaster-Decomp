using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class VGUITextSelection : VGUIButton
{
	private int selectedIndex;

	public string[] selections;

	public VGUIText showText;

	public string[] infos;

	public VGUIText showInfo;

	public bool saveToPrefs;

	public string prefsName;

	public int defaultIndex;

	public override void Start()
	{
		setSelectedIndex(defaultIndex);
		if (saveToPrefs)
		{
			setSelectedIndex(PlayerPrefs.GetInt(prefsName, selectedIndex));
		}
	}

	public virtual void setSelectedIndex(int to)
	{
		selectedIndex = to;
		if (showText != null)
		{
			showText.setText(selections[to]);
		}
		if (showInfo != null && Extensions.get_length((System.Array)infos) > 0)
		{
			showInfo.setText(infos[to]);
		}
	}

	public virtual int getSelectedIndex()
	{
		return selectedIndex;
	}

	public virtual string getSelectedText()
	{
		return selections[selectedIndex];
	}

	public virtual void nextSelection()
	{
		int num = (selectedIndex + 1) % Extensions.get_length((System.Array)selections);
		setSelectedIndex(num);
		save();
		activateInvoke(false);
	}

	public virtual void previousSelection()
	{
		int num = (selectedIndex + Extensions.get_length((System.Array)selections) - 1) % Extensions.get_length((System.Array)selections);
		setSelectedIndex(num);
		save();
		activateInvoke(false);
	}

	private void save()
	{
		if (saveToPrefs)
		{
			PlayerPrefs.SetInt(prefsName, selectedIndex);
		}
	}

	public static int getSavedValue(string id, int def)
	{
		return PlayerPrefs.GetInt(id, def);
	}
}
