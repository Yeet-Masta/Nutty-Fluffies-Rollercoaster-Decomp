using System;
using UnityEngine;

[Serializable]
public class VGUIMasterVolumeSelection : VGUINumberSelection
{
	private bool valueChanged;

	public override void Start()
	{
		base.Start();
		setSelectedValue(Mathf.Min(maxValue, MasterVolume.getVolume() * (maxValue - minValue) + minValue));
		valueChanged = false;
	}

	public override void setSelectedValue(float to)
	{
		base.setSelectedValue(to);
		valueChanged = true;
	}

	public override void Update()
	{
		if (valueChanged)
		{
			MasterVolume.setVolume(selectedValue / (maxValue - minValue));
			valueChanged = false;
		}
	}
}
