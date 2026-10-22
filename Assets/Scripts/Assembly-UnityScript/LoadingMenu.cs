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
		if (GameData.IS_TAPJOY_ENABLED && TapjoyAdController.getInstance() != null)
		{
			TapjoyAdController.getInstance().HideAd();
		}
		if (GameData.LOG)
		{
			Debug.Log("RKN tushar loaidng activate");
		}
		animator.from = 0f;
		animator.to = 1f;
		animator.duration = 0.1f;
		animator.activationDelay = 0f;
		animator.activate();
		activated = true;
		isLoading = true;
		LoadingDialog.showLoadingAnimation();
	}

	public override void deactivate()
	{
		if (GameData.LOG)
		{
			Debug.Log("RKN tushar loaidng deactivate");
		}
		animator.from = 1f;
		animator.to = 0f;
		animator.duration = 0.2f;
		animator.activationDelay = 0.2f;
		animator.activate();
		activated = false;
		isLoading = false;
		LoadingDialog.hideLoadingAnimation();
	}

	public virtual void deactivateMenu(bool hideLoadingAnim)
	{
	}

	public virtual void onAnimationComplete()
	{
		if (GameData.LOG)
		{
			Debug.Log("RKN tushar loaidng onAnimationComplete activated : " + activated);
		}
		if (!activated)
		{
			if (GameData.LOG)
			{
				Debug.Log("RKN loaidng onAnimationComplete activated false");
			}
			base.deactivate();
			if (!activated && BaseMenuManager.getInstance() != null)
			{
				BaseMenuManager.getInstance().onLoadingMenuDeactivation();
			}
		}
		else if (BaseMenuManager.getInstance() != null)
		{
			if (GameData.LOG)
			{
				Debug.Log("RKN tushar loaidng onAnimationComplete activated true");
			}
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
