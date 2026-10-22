using System;
using UnityEngine;

[Serializable]
public class VGUINotify : VGUIImageInvoker
{
	public float duration;

	public bool showOnAwake;

	public float showOnAwakeDelay;

	public bool mouseclickSkips;

	public bool skippingImmediatesInvoke;

	public AnimationClip showAnimation;

	public AnimationClip hideAnimation;

	public AudioClip showSoundFX;

	public bool lockedSound;

	public int minimumShowRepeatLimit;

	private bool visible;

	private bool inPlace;

	private int lastShowTime;

	public VGUINotify()
	{
		mouseclickSkips = true;
		skippingImmediatesInvoke = true;
	}

	public override void Awake()
	{
		if (showAnimation != null)
		{
			GetComponent<Animation>().AddClip(showAnimation, "show");
		}
		if (hideAnimation != null)
		{
			GetComponent<Animation>().AddClip(hideAnimation, "hide");
		}
		if (showSoundFX != null)
		{
			OneShotAudio.preload(showSoundFX);
		}
		GetComponent<Animation>().wrapMode = WrapMode.ClampForever;
	}

	public virtual void Start()
	{
		if (!visible)
		{
			deactivate();
		}
		if (showOnAwake)
		{
			if (showOnAwakeDelay == 0f)
			{
				show();
			}
			else
			{
				showAfter(showOnAwakeDelay);
			}
		}
	}

	public virtual void show()
	{
		if (Time.time - (float)lastShowTime >= (float)minimumShowRepeatLimit)
		{
			CancelInvoke("hide");
			CancelInvoke("deactivate");
			activate();
			GetComponent<Animation>().Stop();
			float num = 0f;
			if (showAnimation != null)
			{
				GetComponent<Animation>().Play("show");
				num = showAnimation.length;
			}
			Invoke("isShownNow", num);
			Invoke("hide", duration + num);
			visible = true;
			inPlace = false;
			lastShowTime = (int)Time.time;
		}
	}

	public virtual void cancelHide()
	{
		CancelInvoke("hide");
		CancelInvoke("deactivate");
	}

	public virtual void isShownNow()
	{
		inPlace = true;
		activateInvoke(false);
		if (showSoundFX != null)
		{
			GeneralSoundEffects.playSound(showSoundFX, lockedSound ? 1 : 0);
		}
	}

	public virtual void showAfter(float time)
	{
		Invoke("show", time);
	}

	public virtual void hide()
	{
		if (hideAnimation != null)
		{
			GetComponent<Animation>().Play("hide");
		}
		inPlace = false;
		if (runningCoRoutinesAmount == 0)
		{
			if (hideAnimation == null)
			{
				deactivate();
			}
			else
			{
				Invoke("deactivate", hideAnimation.length);
			}
			visible = false;
		}
	}

	public override void Update()
	{
		if (visible && inPlace && mouseclickSkips && Input.GetMouseButton(0))
		{
			hide();
			if (skippingImmediatesInvoke)
			{
				immediatePendingInvoke();
			}
			else
			{
				cancelPendingInvoke();
			}
		}
	}
}
