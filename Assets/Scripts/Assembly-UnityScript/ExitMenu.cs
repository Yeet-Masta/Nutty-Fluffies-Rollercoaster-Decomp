using UnityEngine;

public class ExitMenu : MonoBehaviour
{
	private static ExitMenu instance;

	private MenuManager mmgr;

	public MessageBox messageBox;

	public GameObject[] featureGraphics;

	private bool isMessageEnable;

	public static ExitMenu getInstance()
	{
		return instance;
	}

	public virtual void Awake()
	{
		useGUILayout = false;
		instance = this;
		messageBox.deactivate();
		Invoke("DisableMessage", 0.25f);
	}

	public virtual void Start()
	{
		mmgr = MenuManager.getInstance();
	}

	public virtual void ConfirmExit()
	{
		if (messageBox.isActive())
		{
			isMessageEnable = true;
		}
		if (!isMessageEnable)
		{
			MenuManager.getInstance().versionString.gameObject.SetActiveRecursively(false);
			messageBox.activate(TextData.Exit_title_1, TextData.Exit_text_2, featureGraphics[0], true, false);
			isMessageEnable = true;
		}
	}

	// NFPort: the single Esc handler for the main menu (see MainMenu.onBackButton).
	public virtual void ToggleExitDialog()
	{
		if (messageBox.isActive())
		{
			onDialogClose();
		}
		else if (!isMessageEnable)
		{
			ConfirmExit();
		}
	}

	public virtual void onDialogContinue()
	{
		Invoke("QuitApp", 0.6f);
	}

	private void QuitApp()
	{
		Application.Quit();
	}

	public virtual void onDialogClose()
	{
		MenuManager.getInstance().versionString.gameObject.SetActiveRecursively(true);
		messageBox.onContinue();
		Invoke("DisableMessage", 0.25f);
	}

	public virtual void onResetGame()
	{
		MenuManager.getInstance().versionString.gameObject.SetActiveRecursively(true);
		messageBox.onContinue();
		Invoke("DisableMessage", 0.25f);
	}

	private void DisableMessage()
	{
		isMessageEnable = false;
	}

}
