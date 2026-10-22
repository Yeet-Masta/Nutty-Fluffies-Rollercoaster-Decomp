using System;
using UnityEngine;

[Serializable]
public class VGUIToggleButton : VGUIImageInvoker
{
	public AudioClip pressSound;

	private bool mouseOver;

	public bool @checked;

	public VGUISwitchObject switchLayer;

	public virtual void OnEnable()
	{
		switchLayer.Awake();
	}

	private void toggle()
	{
		@checked = !@checked;
		switchLayer.switchNext();
	}

	public virtual void OnMouseDown()
	{
		toggle();
		activateInvoke(false);
		runPressActions();
	}

	public virtual void OnMouseUp()
	{
	}

	public virtual void OnMouseOver()
	{
		mouseOver = true;
	}

	public virtual void OnMouseExit()
	{
		mouseOver = false;
	}

	public virtual void runPressActions()
	{
		if (pressSound != null)
		{
			GeneralSoundEffects.playSound(pressSound);
		}
	}
}
