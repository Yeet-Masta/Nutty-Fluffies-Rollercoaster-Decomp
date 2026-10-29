using System.Collections;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

public class VGUIButton : VGUIImageInvoker
{
	public bool runAnimation;

	public float animationSpeed = 1f;

	public AudioClip pressSound;

	public AudioClip lockedPressSound;

	public bool invokeOnMouseUp;

	public VGUIElement lockedElement;

	protected bool locked;

	protected bool pressed;

	protected bool mouseDownInsideThisButton;

	protected VGUIScrollingContainer scrollingContainer;

	public virtual void Start()
	{
		if (transform.parent != null)
		{
			scrollingContainer = (VGUIScrollingContainer)transform.parent.gameObject.GetComponent(typeof(VGUIScrollingContainer));
			if (scrollingContainer == null && transform.parent.parent != null)
			{
				scrollingContainer = (VGUIScrollingContainer)transform.parent.parent.gameObject.GetComponent(typeof(VGUIScrollingContainer));
			}
		}
	}

	public virtual void setLocked(bool to)
	{
		locked = to;
		if (lockedElement != null)
		{
			lockedElement.setActive(to);
		}
	}

	public virtual void reactivate()
	{
		pressed = false;
	}

	public virtual void setLocked()
	{
		setLocked(true);
	}

	public virtual void setUnlocked()
	{
		setLocked(false);
	}

	public virtual bool isLocked()
	{
		return locked;
	}

	public virtual void OnEnable()
	{
		if (runAnimation)
		{
			IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(GetComponent<Animation>());
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				if (!(obj is AnimationState))
				{
					obj = RuntimeServices.Coerce(obj, typeof(AnimationState));
				}
				AnimationState animationState = (AnimationState)obj;
				animationState.speed = animationSpeed;
				UnityRuntimeServices.Update(enumerator, animationState);
			}
			GetComponent<Animation>().Stop();
			GetComponent<Animation>().clip.SampleAnimation(gameObject, 0f);
		}
		reactivate();
	}

	public virtual bool hasScrollingContainer()
	{
		return scrollingContainer != null;
	}

	public virtual bool isContainerDragging()
	{
		return (scrollingContainer != null && scrollingContainer.isDraggingOverMinimumAmount()) ? true : false;
	}

	public virtual void OnMouseDown()
	{
		if (!isContainerDragging())
		{
			if (!invokeOnMouseUp)
			{
				clickButton();
			}
			else
			{
				mouseDownInsideThisButton = true;
			}
		}
	}

	public virtual void OnMouseUp()
	{
		if (!isContainerDragging())
		{
			if (invokeOnMouseUp && mouseDownInsideThisButton)
			{
				clickButton();
			}
			mouseDownInsideThisButton = false;
		}
	}

	protected virtual void playPressSound()
	{
		if (pressSound != null)
		{
			GeneralSoundEffects.playSound(pressSound);
		}
	}

	public virtual void clickButton()
	{
		if (!locked && !pressed)
		{
			playPressSound();
			if (activateInvoke(true) && runAnimation)
			{
				GetComponent<Animation>().Play();
				pressed = true;
			}
		}
		else if (lockedPressSound != null)
		{
			GeneralSoundEffects.playSound(lockedPressSound);
		}
	}
}
