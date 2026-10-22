using System;
using UnityEngine;

[Serializable]
public class CachedMonoBehaviour : MonoBehaviour
{
	protected Transform cTransform;

	protected Renderer cRenderer;

	protected AudioSource cAudio;

	protected Rigidbody cRigidbody;

	public virtual void Awake()
	{
		cRenderer = GetComponent<Renderer>();
		cTransform = transform;
		cAudio = GetComponent<AudioSource>();
		cRigidbody = GetComponent<Rigidbody>();
	}
}
