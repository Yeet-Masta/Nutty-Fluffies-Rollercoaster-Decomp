using System;
using UnityEngine;

[Serializable]
public class LoadingMenu : BaseMenu
{
	public GameObject graphic;

	public AlphaAnimator animator;

	private bool activated;

	[NonSerialized]
	public static bool isLoading;

	public override void Awake()
	{
	}

	public override void activate()
	{
		base.activate();
		animator.from = 0f;
		animator.to = 1f;
		animator.duration = 0.1f;
		animator.activationDelay = 0f;
		animator.activate();
		activated = true;
		isLoading = true;
	}

	public override void deactivate()
	{
		animator.from = 1f;
		animator.to = 0f;
		animator.duration = 0.2f;
		animator.activationDelay = 0.2f;
		animator.activate();
		activated = false;
		isLoading = false;
	}

	public virtual void deactivateMenu(bool hideLoadingAnim)
	{
	}

	public virtual void onAnimationComplete()
	{
		if (!activated)
		{
			base.deactivate();
			if (BaseMenuManager.getInstance() != null)
			{
				BaseMenuManager.getInstance().onLoadingMenuDeactivation();
			}
		}
		else if (BaseMenuManager.getInstance() != null)
		{
			BaseMenuManager.getInstance().onLoadingMenuActivation();
		}
	}

	public override void onBackButton()
	{
	}

	public override void Update()
	{
		Input.ResetInputAxes();
	}
}
