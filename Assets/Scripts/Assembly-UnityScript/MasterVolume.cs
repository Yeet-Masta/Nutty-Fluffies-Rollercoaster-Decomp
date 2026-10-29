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
		AudioListener[] array = ((AudioListener[])UnityEngine.Object.FindObjectsOfType(typeof(AudioListener))) as AudioListener[];
		if (gotVolume)
		{
			int i = 0;
			AudioListener[] array2 = array;
			for (int length = array2.Length; i < length; i++)
			{
				AudioListener.volume = masterVolume;
			}
		}
	}
}
