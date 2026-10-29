using UnityEngine;

public class OneShotAudio : MonoBehaviour
{
	private static bool PARENT_TO_MAIN_CAMERA = true;

	private static int amount;

	private static int current;

	private static GameObject[] gos;

	private static AudioSource[] sources;

	private static Transform[] transforms;

	private static int[] priorities;

	public static void InitObjects(int amount)
	{
		OneShotAudio.amount = amount;
		gos = new GameObject[amount];
		sources = new AudioSource[amount];
		transforms = new Transform[amount];
		priorities = new int[amount];
		for (int i = 0; i < amount; i++)
		{
			gos[i] = new GameObject("One shot audio " + i);
			sources[i] = (AudioSource)gos[i].AddComponent(typeof(AudioSource));
			sources[i].playOnAwake = false;
			transforms[i] = gos[i].transform;
			priorities[i] = 0;
		}
		current = 0;
	}

	public static void CameraChanged()
	{
		if (PARENT_TO_MAIN_CAMERA)
		{
			for (int i = 0; i < amount; i++)
			{
				transforms[i].parent = Camera.main.transform;
				transforms[i].localPosition = new Vector3(0f, 0f, 0f);
			}
		}
	}

	public static AudioSource Play(AudioClip clip, Vector3 position, float volume, float falloff, float pitch)
	{
		return Play(clip, position, volume, falloff, pitch, 0);
	}

	public static AudioSource Play(AudioClip clip, Vector3 position, float volume, float falloff, float pitch, int priority)
	{
		if (sources == null)
		{
			Debug.Log("OneShot Audio Objects are not created you need to create them!");
		}
		else if (!PARENT_TO_MAIN_CAMERA || transforms[current].parent == null || transforms[current].parent.gameObject.active)
		{
			if (!PARENT_TO_MAIN_CAMERA && !(falloff <= 0f))
			{
				transforms[current].position = position;
			}
			gos[current].active = true;
			priorities[current] = priority;
			AudioSource audioSource = sources[current];
			audioSource.Stop();
			audioSource.clip = clip;
			audioSource.volume = volume * MasterVolume.getVolume();
			audioSource.pitch = pitch;
			audioSource.Play();
			setNext();
			return audioSource;
		}
		return null;
	}

	public static bool isPlaying(AudioClip clip)
	{
		int index = 0;
		while (index < amount)
		{
			if (sources[index].clip == clip && sources[index].isPlaying)
			{
				return true;
			}
			index++;
		}
		return false;
	}

	private static void setNext()
	{
		for (int i = 0; i < amount; i++)
		{
			current = (current + 1) % sources.Length;
			if (priorities[current] == 0)
			{
				return;
			}
			if (priorities[current] > 0 && !sources[current].isPlaying)
			{
				priorities[current] = 0;
				return;
			}
		}
		current = (current + 1) % sources.Length;
		priorities[current] = 0;
	}

	public static void preload(AudioClip clip)
	{
		AudioSource audioSource = sources[current];
		float volume = audioSource.volume;
		audioSource.Stop();
		audioSource.clip = clip;
		audioSource.volume = 0f;
		audioSource.Play();
		audioSource.Stop();
		audioSource.volume = volume;
		setNext();
	}

	public static void Stop(AudioClip clip)
	{
		for (int i = 0; i < amount; i++)
		{
			if (sources[i].clip == clip)
			{
				sources[i].Stop();
				priorities[i] = 0;
			}
		}
	}

	public static void StopAllSounds()
	{
		for (int i = 0; i < amount; i++)
		{
			sources[i].Stop();
			priorities[i] = 0;
		}
	}

	public static void CloseObjects()
	{
		for (int i = 0; i < amount; i++)
		{
			UnityEngine.Object.Destroy(gos[i]);
		}
		gos = null;
		sources = null;
		transforms = null;
		priorities = null;
	}
}
