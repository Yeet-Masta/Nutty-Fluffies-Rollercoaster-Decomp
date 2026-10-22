using System;
using UnityEngine;

[Serializable]
public class MenuButton : VGUIButton
{
	[NonSerialized]
	public static bool menuButtonPressed;

	[NonSerialized]
	public static ButtonPressAnimator clickAnimator;

	public GameObject graphic;

	private Vector3 initialCollisionSize;

	private bool dimmed;

	public override void Awake()
	{
		base.Awake();
		useGUILayout = false;
		invokeOnMouseUp = true;
		initialCollisionSize = gameObject.GetComponent<BoxCollider>().size;
	}

	public virtual void setDimmed(bool dim)
	{
		if (graphic != null && graphic.GetComponent<Renderer>() != null)
		{
			float num = default(float);
			num = ((!dim) ? 1f : 0.2f);
			SpriteUtil.setAlpha(graphic.GetComponent<Renderer>(), num);
		}
		dimmed = dim;
	}

	public override void setLocked(bool @lock)
	{
		base.setLocked(@lock);
		setDimmed(@lock);
	}

	public override void clickButton()
	{
		menuButtonPressed = true;
		bool switching = BaseMenuManager.getInstance() != null && BaseMenuManager.getInstance().menuSwitcher.isSwitching();
		if (GameData.LOG)
		{
			Debug.Log("NFPort MenuButton.clickButton '" + name + "' locked=" + locked + " pressed=" + pressed + " dimmed=" + dimmed + " menuSwitching=" + switching);
		}
		if (!switching)
		{
			base.clickButton();
		}
	}

	public override void OnMouseUp()
	{
		if (GameData.LOG)
		{
			Debug.Log("NFPort MenuButton.OnMouseUp '" + name + "' downInside=" + mouseDownInsideThisButton + " locked=" + locked + " pressed=" + pressed + " dimmed=" + dimmed + " dragging=" + isContainerDragging());
		}
		menuButtonPressed = false;
		if (!locked && !pressed && !isContainerDragging() && !dimmed)
		{
			doAnimation(true);
		}
		base.OnMouseUp();
	}

	public override void OnMouseDown()
	{
		if (GameData.LOG)
		{
			Debug.Log("NFPort MenuButton.OnMouseDown '" + name + "' clickAnimator=" + (clickAnimator != null));
		}
		if (clickAnimator != null)
		{
			(clickAnimator as ButtonPressAnimator).restore();
		}
		base.OnMouseDown();
		if (!locked && !pressed && !hasScrollingContainer() && !dimmed)
		{
			doAnimation(false);
		}
		else
		{
			(clickAnimator as ButtonPressAnimator).setTarget(transform);
		}
	}

	public virtual void OnMouseExit()
	{
		menuButtonPressed = false;
	}

	public virtual void doAnimation(bool scaleDown)
	{
		if (VGUIElement.blockingButtonPressed)
		{
			clickAnimator.playSound(scaleDown);
		}
		if (clickAnimator.enabled)
		{
			clickAnimator.deactivate();
		}
		(clickAnimator as ButtonPressAnimator).restore();
		clickAnimator.setTarget(transform);
		clickAnimator.reverse = scaleDown;
		clickAnimator.activate();
	}

	protected override void playPressSound()
	{
		if (!dimmed && pressSound != null)
		{
			base.playPressSound();
		}
	}

	public virtual bool isDimmed()
	{
		return dimmed;
	}
}
