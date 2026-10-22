using System;
using UnityEngine;

[Serializable]
public class VGUILoadingKeepalive : MonoBehaviour
{
	public bool killAfterLoad;

	public virtual void Start()
	{
		UnityEngine.Object.DontDestroyOnLoad(gameObject);
		transform.parent = null;
	}

	public virtual void OnLevelWasLoaded(int level)
	{
		if (killAfterLoad)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
	}
}
