using System;
using UnityEngine;

[Serializable]
public class MenuSwitcher : MonoBehaviour
{
	public AlphaAnimator fader;

	public BaseMenu activeMenu;

	public BaseMenu nextMenu;

	public BaseMenu previousMenu;

	private bool goBack;

	private bool switching;

	public virtual void Awake()
	{
		switching = false;
	}

	public virtual void activateMenu(BaseMenu menu)
	{
		activeMenu = menu;
		BaseMenuManager.getInstance().setTokenCounterVisible(true);
		BaseMenuManager.getInstance().setStoreButtonVisible(true);
		BaseMenuManager.getInstance().setMenuTitle(activeMenu.header);
		activeMenu.activate();
	}

	public virtual void switchMenu(BaseMenu to)
	{
		if (!switching)
		{
			nextMenu = to;
			fader.reverse = false;
			fader.activate();
			fader.target.GetComponent<Renderer>().enabled = true;
			switching = true;
		}
	}

	public virtual void switchMenuForced(BaseMenu to)
	{
		nextMenu = to;
		fader.reverse = false;
		fader.activate();
		fader.target.GetComponent<Renderer>().enabled = true;
		switching = true;
	}

	public virtual void switchBack()
	{
		if (!switching)
		{
			goBack = true;
			fader.reverse = false;
			fader.activate();
			switching = true;
			fader.target.GetComponent<Renderer>().enabled = true;
		}
	}

	public virtual void onFadeOut()
	{
		if (!fader.reverse)
		{
			BaseMenuManager.getInstance().setStoreButtonVisible(true);
			BaseMenuManager.getInstance().setMenuTitle(activeMenu.header);
			if (nextMenu != null && nextMenu != activeMenu)
			{
				previousMenu = activeMenu;
				activeMenu.deactivate();
				activateMenu(nextMenu);
			}
			else if (goBack && previousMenu != null)
			{
				activeMenu.deactivate();
				previousMenu.reactivate();
				activeMenu = previousMenu;
				nextMenu = activeMenu;
				goBack = false;
			}
			fader.reverse = true;
			fader.activate();
		}
		else
		{
			switching = false;
			fader.target.GetComponent<Renderer>().enabled = false;
		}
	}

	public virtual void onFadeIn()
	{
	}

	public virtual bool isSwitching()
	{
		return switching;
	}
}
