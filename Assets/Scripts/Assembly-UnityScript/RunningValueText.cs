using System;
using UnityEngine;

public class RunningValueText : VGUIRunningValueTK2D
{
	public Transform icon;

	public bool formatToCash;

	public float animationFactor;

	private bool triggered;

	private float phase;

	private Vector3 iconPosition;

	public virtual void Start()
	{
		if (icon != null)
		{
			iconPosition = icon.localPosition;
		}
	}

	public override void updateValue()
	{
		base.updateValue();
		triggered = true;
		phase = 0f;
	}

	public override void Update()
	{
		if (triggered && icon != null)
		{
			float y = iconPosition.y + Mathf.Sin(phase) * animationFactor;
			Vector3 localPosition = icon.localPosition;
			localPosition.y = y;
			icon.localPosition = localPosition;
			phase += Time.deltaTime * (float)Math.PI / stepDuration;
			triggered = phase < (float)Math.PI;
			if (!(phase <= (float)Math.PI))
			{
				triggered = false;
				icon.localPosition = iconPosition;
			}
		}
	}

	public override void setText(string to)
	{
		if (textMeshTK != null && formatToCash)
		{
			textMeshTK.setText(GameData.formatTokenValue(to));
		}
		else
		{
			base.setText(to);
		}
	}
}
