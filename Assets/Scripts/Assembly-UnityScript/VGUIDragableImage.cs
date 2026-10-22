using System;
using UnityEngine;

[Serializable]
public class VGUIDragableImage : VGUIImageInvoker
{
	public Transform limitMinPos;

	public Transform limitMaxPos;

	public bool invokeWhenMoved;

	private Vector3 thisPositionAtMouseDown;

	private Vector3 screenSpace;

	private Vector3 offset;

	public AudioClip dragSound;

	public virtual void Start()
	{
		valueType = VGUIImageInvokerValueType.@float;
		if (dragSound != null)
		{
			GetComponent<AudioSource>().clip = dragSound;
		}
	}

	public virtual float getPositionBetweenLimits()
	{
		Vector3 vector = limitMaxPos.position - limitMinPos.position;
		float magnitude = vector.magnitude;
		vector = vector.normalized;
		Vector3 lhs = transform.position - limitMinPos.position;
		float magnitude2 = lhs.magnitude;
		return Vector3.Dot(lhs, vector) / magnitude;
	}

	public virtual void setPositionBetweenLimits(float to)
	{
		transform.position = Vector3.Lerp(limitMinPos.position, limitMaxPos.position, to);
	}

	public virtual void OnMouseDown()
	{
		thisPositionAtMouseDown = transform.position;
		screenSpace = Camera.main.WorldToScreenPoint(transform.position);
		offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
	}

	public virtual void OnMouseUp()
	{
		if (dragSound != null)
		{
			GetComponent<AudioSource>().Stop();
		}
	}

	public virtual void OnMouseDrag()
	{
		Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);
		Vector3 position2 = Camera.main.ScreenToWorldPoint(position) + offset;
		position2 = transform.parent.InverseTransformPoint(position2);
		Vector3 vector = default(Vector3);
		if (limitMaxPos != null)
		{
			vector = limitMaxPos.position;
			vector = transform.parent.InverseTransformPoint(vector);
			if (!(position2.x <= vector.x))
			{
				position2.x = vector.x;
			}
			if (!(position2.y <= vector.y))
			{
				position2.y = vector.y;
			}
			if (!(position2.z <= vector.z))
			{
				position2.y = vector.z;
			}
		}
		if (limitMinPos != null)
		{
			vector = limitMinPos.position;
			vector = transform.parent.InverseTransformPoint(vector);
			if (!(position2.x >= vector.x))
			{
				position2.x = vector.x;
			}
			if (!(position2.y >= vector.y))
			{
				position2.y = vector.y;
			}
			if (!(position2.z >= vector.z))
			{
				position2.z = vector.z;
			}
		}
		transform.localPosition = position2;
		if (invokeWhenMoved)
		{
			floatValue = getPositionBetweenLimits();
			activateInvoke(false);
		}
		if (dragSound != null && !GetComponent<AudioSource>().isPlaying)
		{
			GetComponent<AudioSource>().loop = true;
			GetComponent<AudioSource>().Play();
		}
	}
}
