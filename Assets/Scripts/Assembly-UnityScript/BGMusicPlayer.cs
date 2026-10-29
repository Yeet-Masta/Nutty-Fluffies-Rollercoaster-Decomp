using UnityEngine;

public class BGMusicPlayer : MonoBehaviour
{
	private static float musicVolume;

	public virtual void Awake()
	{
		getMusicVolume();
		GetComponent<AudioSource>().volume = musicVolume;
	}

	public static float getMusicVolume()
	{
		musicVolume = PlayerPrefs.GetFloat("MUSIC_VOLUME", 0.3f);
		return musicVolume;
	}

	public static void setMusicVolume(float to)
	{
		PlayerPrefs.SetFloat("MUSIC_VOLUME", to);
		musicVolume = to;
		BGMusicPlayer[] array = ((BGMusicPlayer[])UnityEngine.Object.FindObjectsOfType(typeof(BGMusicPlayer))) as BGMusicPlayer[];
		int i = 0;
		BGMusicPlayer[] array2 = array;
		for (int length = array2.Length; i < length; i++)
		{
			array2[i].GetComponent<AudioSource>().volume = to;
		}
	}
}
