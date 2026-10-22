using System;
using UnityEngine;

[Serializable]
public class VGUIValueSlider : VGUIImageInvoker
{
	private float selectedValue;

	public VGUIImage scrollThumb;

	public Transform limitMinPos;

	public Transform limitMaxPos;

	private Vector3 railNormal;

	private float railLength;

	public float minimumValue;

	public float maximumValue;

	public float defaultValue;

	public float stepSize;

	public VGUIText showText;

	public string textPrefix;

	public bool saveToPrefs;

	public string prefsName;

	public bool truncateValueToInt;

	private bool setted;

	public AudioClip dragSound;

	public string[] valueTexts;

	public bool dragging;

	public bool savePrefsValueAsNormalized;

	public VGUIValueSlider()
	{
		stepSize = 1f;
		textPrefix = string.Empty;
	}

	public virtual void OnEnable()
	{
		setted = false;
	}

	public override void Awake()
	{
		if (saveToPrefs)
		{
			float num = getSavedValue(prefsName, (int)defaultValue);
			if (savePrefsValueAsNormalized)
			{
				num *= maximumValue;
			}
			setSelectedValue(num, false);
		}
		if (!setted)
		{
			setSelectedValue(defaultValue, false);
		}
		initRail();
	}

	public virtual void initRail()
	{
		railNormal = limitMaxPos.position - limitMinPos.position;
		railLength = railNormal.magnitude;
		railNormal = railNormal.normalized;
	}

	public virtual void invokeCurrentValue()
	{
		setSelectedValue(selectedValue, true);
	}

	public virtual void setSelectedValue(float to, bool withInvoke)
	{
		if (truncateValueToInt)
		{
			to = Mathf.Floor(to);
		}
		to = Mathf.Floor(to / stepSize) * stepSize;
		if (!(to >= minimumValue))
		{
			to = minimumValue;
		}
		else if (!(to <= maximumValue))
		{
			to = maximumValue;
		}
		selectedValue = to;
		if (valueTexts == null)
		{
			showText.setText(string.Empty + selectedValue + textPrefix);
		}
		else
		{
			showText.setText(valueTexts[(int)to]);
		}
		if (withInvoke && invokeObject != null && invokeMethod != null)
		{
			valueType = VGUIImageInvokerValueType.integer;
			intValue = (int)to;
			activateInvoke(true);
		}
		setted = true;
		setThumbPositionBetweenLimits((to - minimumValue) / (maximumValue - minimumValue));
	}

	public virtual void setSelectedValueAsFraction(float to, bool withInvoke)
	{
		setSelectedValue((maximumValue - minimumValue) * to + minimumValue, withInvoke);
	}

	public virtual void setThumbPositionBetweenLimits(float to)
	{
		if (scrollThumb != null)
		{
			scrollThumb.transform.position = Vector3.Lerp(limitMinPos.position, limitMaxPos.position, to);
		}
	}

	public virtual float getSelectedValue()
	{
		return selectedValue;
	}

	public virtual float getSelectedValueAsFraction()
	{
		return (selectedValue - minimumValue) / (maximumValue - minimumValue);
	}

	public virtual void stepUp()
	{
		float num = selectedValue + stepSize;
		if (!(num <= maximumValue))
		{
			num = maximumValue;
		}
		setSelectedValue(num, true);
		save();
	}

	public virtual void stepDown()
	{
		float num = selectedValue - stepSize;
		if (!(num >= minimumValue))
		{
			num = minimumValue;
		}
		setSelectedValue(num, true);
		save();
	}

	private void save()
	{
		if (saveToPrefs)
		{
			float selectedValueAsFraction = selectedValue;
			if (savePrefsValueAsNormalized)
			{
				selectedValueAsFraction = getSelectedValueAsFraction();
			}
			PlayerPrefs.SetFloat(prefsName, selectedValueAsFraction);
		}
	}

	public static float getSavedValue(string id, int def)
	{
		return PlayerPrefs.GetFloat(id, def);
	}

	public virtual void OnMouseDown()
	{
		OnMouseDrag();
	}

	public virtual void OnMouseDrag()
	{
		dragging = true;
		Vector3 vector = Camera.main.WorldToScreenPoint(transform.position);
		Vector3 vector2 = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, vector.z));
		vector2.z = limitMinPos.position.z;
		vector2 -= limitMinPos.position;
		float to = Vector4.Dot(railNormal, vector2) / railLength;
		setSelectedValueAsFraction(to, true);
		if (dragSound != null && !GetComponent<AudioSource>().isPlaying)
		{
			GetComponent<AudioSource>().loop = true;
			GetComponent<AudioSource>().Play();
		}
	}

	public virtual void OnMouseUp()
	{
		if (dragging)
		{
			save();
		}
		dragging = false;
		if (dragSound != null)
		{
			GetComponent<AudioSource>().Stop();
		}
	}
}
