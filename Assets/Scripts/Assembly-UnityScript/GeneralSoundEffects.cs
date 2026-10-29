using UnityEngine;

public class GeneralSoundEffects : MonoBehaviour
{
	public int oneShotSoundAmount = 2;

	public int oneShotSoundAmount2g = 2;

	public Vector3 playPosition = default(Vector3);

	public float playVolume = 1f;

	public float playFalloff;

	protected static GeneralSoundEffects instance;

	public static void playSound(AudioClip a)
	{
		playSound(a, 0);
	}

	public static void playSound(AudioClip a, int priority)
	{
		if (instance != null)
		{
			instance.playSoundInternal(a, priority);
		}
		else
		{
			Debug.LogWarning("There is no instance of General Sound Effects, please place one to scene!");
		}
	}

	public static void stopSound(AudioClip a)
	{
		if (instance != null)
		{
			instance.stopSoundInternal(a);
		}
	}

	public static bool isPlaying(AudioClip a)
	{
		return OneShotAudio.isPlaying(a);
	}

	public static void playSound(AudioClip a, float vol)
	{
		if (instance != null)
		{
			instance.playSoundInternal(a, vol, 0);
		}
		else
		{
			Debug.LogWarning("There is no instance of General Sound Effects, please place one to scene!");
		}
	}

	public static void playSound(AudioClip a, float vol, float pitch)
	{
		if (instance != null)
		{
			instance.playSoundInternal(a, vol, pitch, 0);
		}
		else
		{
			Debug.LogWarning("There is no instance of General Sound Effects, please place one to scene!");
		}
	}

	public static void playSound(AudioClip[] clips, float floatIndex)
	{
		playSound(clips, floatIndex, 1f);
	}

	public static void playSound(AudioClip[] clips, float floatIndex, float vol)
	{
		if (!(floatIndex <= 0.9999f))
		{
			floatIndex = 0.9999f;
		}
		else if (!(floatIndex >= 0f))
		{
			floatIndex = 0f;
		}
		int num = (int)Mathf.Floor(floatIndex * (float)clips.Length);
		playSound(clips[num], vol);
	}

	public static void playSoundIfNotPlaying(AudioClip[] clips, float floatIndex, float vol)
	{
		if (!(floatIndex <= 0.9999f))
		{
			floatIndex = 0.9999f;
		}
		else if (!(floatIndex >= 0f))
		{
			floatIndex = 0f;
		}
		int num = (int)Mathf.Floor(floatIndex * (float)clips.Length);
		if (!isPlaying(clips[num]))
		{
			playSound(clips[num], vol);
		}
	}

	public static void playRandomSound(AudioClip[] clips)
	{
		playSound(clips, UnityEngine.Random.value);
	}

	public static void playRandomSound(AudioClip[] clips, float volume)
	{
		playSoundIfNotPlaying(clips, UnityEngine.Random.value, volume);
	}

	public static void loadAndPlaySound(string name)
	{
		AudioClip audioClip = (AudioClip)Resources.Load(name);
		if (audioClip == null)
		{
			Debug.LogWarning("Audio \"" + name + "\" not found, using silence...");
		}
		else
		{
			playSound(audioClip);
		}
	}

	public static void loadAndPlayRandomSound(string[] clips)
	{
		int num = (int)(UnityEngine.Random.value * (float)clips.Length);
		if (num >= clips.Length)
		{
			num = clips.Length - 1;
		}
		loadAndPlaySound(clips[num]);
	}

	public static void stopAllSounds(AudioClip[] clips)
	{
		int i = 0;
		for (int length = clips.Length; i < length; i++)
		{
			stopSound(clips[i]);
		}
	}

	public static void stopAllSounds()
	{
		OneShotAudio.StopAllSounds();
	}

	private void playSoundInternal(AudioClip a, float vol, float pitch, int priority)
	{
		OneShotAudio.Play(a, playPosition, vol, playFalloff, pitch, priority);
	}

	private void playSoundInternal(AudioClip a, float vol, int priority)
	{
		OneShotAudio.Play(a, playPosition, vol, playFalloff, 1f, priority);
	}

	private void playSoundInternal(AudioClip a, int priority)
	{
		OneShotAudio.Play(a, playPosition, playVolume, playFalloff, 1f, priority);
	}

	private void stopSoundInternal(AudioClip a)
	{
		OneShotAudio.Stop(a);
	}

	public static GeneralSoundEffects getGeneralInstance()
	{
		return instance;
	}

	public virtual void Awake()
	{
		OneShotAudio.InitObjects(oneShotSoundAmount);
		instance = this;
	}

	public virtual void Start()
	{
		OneShotAudio.CameraChanged();
	}

	public static AudioSource createAudioSourceTo(GameObject to, AudioClip clip)
	{
		AudioSource audioSource = (AudioSource)to.AddComponent(typeof(AudioSource));
		audioSource.clip = clip;
		audioSource.playOnAwake = false;
		audioSource.volume = 1f;
		return audioSource;
	}

	public static AudioSource createAudioSourceToNewGO(AudioClip clip)
	{
		GameObject to = new GameObject("NewSoundGO");
		return createAudioSourceTo(to, clip);
	}
}
