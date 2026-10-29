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
		BGMusicPlayer[] players = ((BGMusicPlayer[])UnityEngine.Object.FindObjectsOfType(typeof(BGMusicPlayer))) as BGMusicPlayer[];
		foreach (BGMusicPlayer bgMusicPlayer in players)
		{
			bgMusicPlayer.GetComponent<AudioSource>().volume = to;
		}
	}
}
