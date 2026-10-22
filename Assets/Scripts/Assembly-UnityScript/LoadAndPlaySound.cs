using System;
using UnityEngine;

[Serializable]
public class LoadAndPlaySound : MonoBehaviour
{
	public string audioClipName;

	public bool playAfterLoad;

	public LoadAndPlaySound()
	{
		playAfterLoad = true;
	}

	public virtual void Awake()
	{
		AudioClip audioClip = (AudioClip)Resources.Load(audioClipName);
		if (audioClip == null)
		{
			Debug.LogWarning("Audio \"" + audioClipName + "\" not found, using silence...");
			return;
		}
		GetComponent<AudioSource>().clip = audioClip;
		if (playAfterLoad)
		{
			GetComponent<AudioSource>().Play();
		}
	}

	public virtual void Main()
	{
	}
}
