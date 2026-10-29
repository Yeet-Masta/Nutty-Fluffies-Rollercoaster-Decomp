using System;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;

public class MessageBox : BaseMenu
{
	[Serializable]
	public class MessageQueueEntry
	{
		public string title;

		public string message;

		public GameObject image;

		public bool withClose;

		public bool errorMessage;

		public GameObject callbackObject;

		public string callbackMethod;
	}

	public VGUIText titleText;

	public VGUIText[] messageText;

	public Transform imageRoot;

	public AlphaAnimator animator;

	public ParticleEmitter[] paperEmitters;

	public GameObject closeButton;

	public GameObject continueButton;

	public GameObject backButton;

	public GameObject tapjoyButton;

	private GameObject messageImage;

	private bool activated;

	private List<MessageQueueEntry> queue = new List<MessageQueueEntry>();

	private bool runEmitters;

	private GameObject callbackObject;

	private string callbackMethod;

	public virtual void activate(string title, string message, GameObject image, bool closeButton, bool errorMessage)
	{
		activate(title, message, image, closeButton, errorMessage, null, null);
	}

	public virtual void activate(string title, string message, GameObject image, bool closeButton, bool errorMessage, GameObject callbackObject, string callbackMethod)
	{
		if (activated)
		{
			MessageQueueEntry messageQueueEntry = new MessageQueueEntry();
			messageQueueEntry.title = title;
			messageQueueEntry.message = message;
			messageQueueEntry.image = image;
			messageQueueEntry.withClose = closeButton;
			messageQueueEntry.errorMessage = errorMessage;
			messageQueueEntry.callbackObject = callbackObject;
			messageQueueEntry.callbackMethod = callbackMethod;
			queue.Add(messageQueueEntry);
		}
		else
		{
			internalShowMessage(title, message, image, closeButton, errorMessage);
			setCallBack(callbackObject, callbackMethod);
		}
		animator.reverse = false;
		animator.activate();
	}

	private void internalShowMessage(string title, string message, GameObject image, bool close, bool errorMessage)
	{
		if (paperEmitters != null)
		{
			disablePaperEmitters();
		}
		string[] array = message.Split("\n"[0]);
		base.activate();
		titleText.setText(title);
		continueButton.SetActiveRecursively(!errorMessage);
		if (backButton != null)
		{
			backButton.SetActiveRecursively(errorMessage);
		}
		if (tapjoyButton != null)
		{
			tapjoyButton.SetActiveRecursively(false);
		}
		int i;
		for (i = 0; i < array.Length && i < messageText.Length; i++)
		{
			messageText[i].setText(array[i]);
		}
		for (int j = array.Length; j < messageText.Length; j++)
		{
			messageText[i].setText(string.Empty);
		}
		if (image != null)
		{
			messageImage = (GameObject)UnityEngine.Object.Instantiate(image, Vector3.zero, Quaternion.identity);
			messageImage.transform.parent = imageRoot;
			messageImage.transform.localPosition = Vector3.zero;
		}
		closeButton.SetActiveRecursively(close);
		activated = true;
		if (runEmitters)
		{
			int k = 0;
			ParticleEmitter[] array2 = paperEmitters;
			for (int length = array2.Length; k < length; k++)
			{
				array2[k].emit = true;
			}
			CancelInvoke("disablePaperEmitters");
			Invoke("disablePaperEmitters", 2f);
		}
	}

	public virtual void setCallBack(GameObject @object, string method)
	{
		callbackObject = @object;
		callbackMethod = method;
	}

	public virtual void onContinue()
	{
		close(false);
	}

	public virtual void onDismiss()
	{
		close(true);
	}

	// The Tapjoy offer wall is gone (its button is always hidden); kept because
	// the scene's button calls it by name.
	public virtual void onTapjoy()
	{
	}

	private void close(bool dismissed)
	{
		if (queue.Count > 0)
		{
			if (messageImage != null)
			{
				UnityEngine.Object.Destroy(messageImage);
				messageImage = null;
			}
			MessageQueueEntry messageQueueEntry = queue[0];
			queue.Remove(messageQueueEntry);
			internalShowMessage(messageQueueEntry.title, messageQueueEntry.message, messageQueueEntry.image, messageQueueEntry.withClose, messageQueueEntry.errorMessage);
			setCallBack(messageQueueEntry.callbackObject, messageQueueEntry.callbackMethod);
		}
		else
		{
			activated = false;
			animator.reverse = true;
			animator.activate();
			if (callbackObject != null)
			{
				callbackObject.SendMessage(callbackMethod, dismissed);
			}
			else
			{
				BaseMenuManager.getInstance().onMessageBoxClosed(dismissed);
			}
		}
	}

	public virtual void onAnimationComplete()
	{
		if (activated)
		{
			return;
		}
		if (messageImage != null)
		{
			UnityEngine.Object.Destroy(messageImage);
			messageImage = null;
		}
		CancelInvoke("disablePaperEmitters");
		deactivate();
		if (runEmitters)
		{
			int i = 0;
			ParticleEmitter[] array = paperEmitters;
			for (int length = array.Length; i < length; i++)
			{
				array[i].emit = false;
				array[i].ClearParticles();
			}
		}
		runEmitters = false;
	}

	public virtual void disablePaperEmitters()
	{
		int i = 0;
		ParticleEmitter[] array = paperEmitters;
		for (int length = array.Length; i < length; i++)
		{
			array[i].emit = false;
		}
	}

	public virtual void enablePaperEmitters(bool enabled)
	{
		runEmitters = enabled;
	}

	public override bool isActive()
	{
		return activated;
	}

	public override void onBackButton()
	{
		if (continueButton.active)
		{
			onContinue();
		}
		else if (closeButton.active)
		{
			onDismiss();
		}
		else
		{
			onContinue();
		}
	}
}
