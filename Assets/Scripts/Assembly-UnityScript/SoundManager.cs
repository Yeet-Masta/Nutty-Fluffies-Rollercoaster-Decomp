using System;
using UnityEngine;

[Serializable]
public class SoundManager : MonoBehaviour
{
	public AudioSource musicSource;

	protected AudioClip music;

	[NonSerialized]
	protected static SoundManager instance;

	public static SoundManager getInstance()
	{
		return instance;
	}

	public virtual void Awake()
	{
		instance = this;
	}

	public virtual void playMusic(string name)
	{
		playMusic(Resources.Load(name) as AudioClip);
	}

	public virtual void playMusic(AudioClip music)
	{
		this.music = music;
		musicSource.clip = music;
		musicSource.loop = true;
		musicSource.Play();
	}

	public virtual void stopMusic()
	{
		musicSource.Stop();
		musicSource.clip = null;
	}

	public virtual void pauseMusic()
	{
		if (musicSource.clip != null)
		{
			musicSource.Pause();
		}
	}

	public virtual void continueMusic()
	{
		if (musicSource.clip != null)
		{
			musicSource.Play();
		}
	}

	public virtual AudioClip playRandomSound(AudioClip[] clips)
	{
		int num = (int)(UnityEngine.Random.value * 123456f);
		num %= clips.Length;
		GeneralSoundEffects.playSound(clips[num]);
		return clips[num];
	}
}
