using System;
using UnityEngine;

[Serializable]
public class PlayOnEnable : MonoBehaviour
{
	public virtual void OnEnable()
	{
		GetComponent<AudioSource>().Play();
	}

	public virtual void OnDisable()
	{
		GetComponent<AudioSource>().Stop();
	}

	public virtual void stateResetted()
	{
		OnEnable();
	}

	public virtual void Main()
	{
	}
}
