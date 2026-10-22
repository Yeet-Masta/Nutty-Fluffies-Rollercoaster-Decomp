using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class CollisionAudioPlayer : MonoBehaviour
{
	[NonSerialized]
	protected static float replayMinimumWait = 0.25f;

	[NonSerialized]
	protected static float minimumSpeed = 3f;

	[NonSerialized]
	protected static float maxSpeedVolume = 10f;

	public AudioClip[] clips;

	protected float lastPlayTime;

	public virtual void Awake()
	{
		int i = 0;
		AudioClip[] array = clips;
		for (int length = array.Length; i < length; i++)
		{
			OneShotAudio.preload(array[i]);
		}
	}

	public virtual void OnCollisionEnter(Collision collision)
	{
		float magnitude = collision.relativeVelocity.magnitude;
		if (Time.time >= lastPlayTime + replayMinimumWait && magnitude >= minimumSpeed)
		{
			int num = UnityEngine.Random.Range(0, Extensions.get_length((System.Array)clips));
			GeneralSoundEffects.playSound(clips[num], getVolumeFromColPower(magnitude));
			lastPlayTime = Time.time;
		}
	}

	protected virtual float getVolumeFromColPower(float pow)
	{
		float num = pow / maxSpeedVolume;
		return (num <= 1f) ? num : 1f;
	}
}
