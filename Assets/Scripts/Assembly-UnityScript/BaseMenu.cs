using System;

[Serializable]
public class BaseMenu : VGUIElement
{
	[NonSerialized]
	public static BaseMenuManager mmgr;

	[NonSerialized]
	public static AnimationManager anim;

	public BaseMenu nextMenu;

	public BaseMenu previousMenu;

	public string header;

	public override void Awake()
	{
		useGUILayout = false;
	}

	public override void activate()
	{
		base.activate();
		VGUIElement.blockingButtonPressed = false;
	}

	public virtual void reactivate()
	{
		base.activate();
		VGUIElement.blockingButtonPressed = false;
	}

	public virtual void onMessageBoxClosed(bool dismissed)
	{
	}

	public virtual void onCommunityFeatureCompleted()
	{
	}

	public override void onBackButton()
	{
	}

	public override void Update()
	{
	}
}
