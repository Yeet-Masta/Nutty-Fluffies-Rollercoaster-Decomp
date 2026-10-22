using System;
using UnityEngine;

[Serializable]
public class VGUILoadingElement : VGUIElement
{
	[NonSerialized]
	private static VGUILoadingElement instance;

	public virtual void OnLevelWasLoaded()
	{
		transform.parent = null;
		if (instance != null)
		{
			UnityEngine.Object.Destroy(instance);
		}
		if (instance != this)
		{
			instance = this;
			UnityEngine.Object.DontDestroyOnLoad(instance);
			deactivate();
		}
	}
}
