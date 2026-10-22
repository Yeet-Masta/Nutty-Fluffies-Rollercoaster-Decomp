using System;
using UnityEngine;

[Serializable]
public class AudioTriggerScript : MonoBehaviour
{
	public Collider colTo;

	public AudioSource stopSource;

	public virtual void OnTriggerEnter(Collider other)
	{
		if (colTo == null || other == colTo)
		{
			GetComponent<AudioSource>().Play();
			if (stopSource != null)
			{
				stopSource.Stop();
			}
		}
	}

	public virtual void Main()
	{
	}
}
