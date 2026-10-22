using System;
using UnityEngine;

[Serializable]
public class TouchSensor : MonoBehaviour
{
	public bool pressed;

	public virtual void Awake()
	{
		useGUILayout = false;
		pressed = false;
	}

	public virtual void OnMouseDown()
	{
		pressed = true;
	}

	public virtual void OnMouseUp()
	{
		pressed = false;
	}

	public virtual void OnMouseExit()
	{
		pressed = false;
	}

	public virtual void OnMouseEnter()
	{
		pressed = true;
	}
}
