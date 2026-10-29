using UnityEngine;

public class MasterVolume : MonoBehaviour
{
	private static float masterVolume;

	private static bool gotVolume;

	public virtual void Start()
	{
		getVolume();
		updateListeners();
	}

	public static float getVolume()
	{
		masterVolume = PlayerPrefs.GetFloat("MASTER_VOLUME", 1f);
		gotVolume = true;
		return masterVolume;
	}

	public static void setVolume(float to)
	{
		PlayerPrefs.SetFloat("MASTER_VOLUME", to);
		masterVolume = to;
		updateListeners();
	}

	public static void updateListeners()
	{
		AudioListener[] listeners = (AudioListener[])UnityEngine.Object.FindObjectsOfType(typeof(AudioListener));
		if (gotVolume)
		{
			foreach (AudioListener listener in listeners)
			{
				AudioListener.volume = masterVolume;
			}
		}
	}
}
