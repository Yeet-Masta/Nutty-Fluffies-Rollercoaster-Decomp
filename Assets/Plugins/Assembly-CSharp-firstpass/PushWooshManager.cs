using UnityEngine;

public class PushWooshManager : MonoBehaviour
{
	private static PushWooshManager instance;

	private void Awake()
	{
		if (instance == null)
		{
			gameObject.name = GetType().ToString();
			Object.DontDestroyOnLoad(this);
			instance = this;
		}
		else
		{
			Object.Destroy(gameObject);
		}
	}
}
