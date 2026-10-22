using System;
using UnityEngine;

[Serializable]
public class PowerBar : VGUIElement
{
	public float power;

	public Transform barObject;

	public VGUIText valueText;

	public bool clampToNormalRange;

	private Vector3 originalLocalScale;

	public bool scaleX;

	public bool scaleY;

	public bool scaleZ;

	public PowerBar()
	{
		clampToNormalRange = true;
	}

	public override void Awake()
	{
		originalLocalScale = barObject.localScale;
		setPower(power);
	}

	public virtual void setPower(float to)
	{
		power = to;
		if (clampToNormalRange)
		{
			power = Mathf.Min(Mathf.Max(to, 0f), 1f);
		}
		if (scaleX)
		{
			float x = originalLocalScale.x * power;
			Vector3 localScale = barObject.localScale;
			float num = (localScale.x = x);
			Vector3 vector = (barObject.localScale = localScale);
		}
		if (scaleY)
		{
			float y = originalLocalScale.y * power;
			Vector3 localScale2 = barObject.localScale;
			float num2 = (localScale2.y = y);
			Vector3 vector3 = (barObject.localScale = localScale2);
		}
		if (scaleZ)
		{
			float z = originalLocalScale.z * power;
			Vector3 localScale3 = barObject.localScale;
			float num3 = (localScale3.z = z);
			Vector3 vector5 = (barObject.localScale = localScale3);
		}
	}

	public virtual void slidePower(float to, float speed)
	{
		setPower(to * speed + power * (1f - speed));
	}

	public virtual void setValueText(string to)
	{
		valueText.setText(to);
	}
}
