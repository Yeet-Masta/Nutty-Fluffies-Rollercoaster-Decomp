using UnityEngine;

public class EtceteraTwoManager : MonoBehaviour
{
	public delegate void EtceteraTwoEmptyDelegate();

	public static event EtceteraTwoEmptyDelegate screenMirroringDidStartEvent;

	public static event EtceteraTwoEmptyDelegate screenMirroringDidStopEvent;

	private void Awake()
	{
		gameObject.name = GetType().ToString();
		Object.DontDestroyOnLoad(this);
	}

	public void screenMirroringDidStart(string empty)
	{
		if (screenMirroringDidStartEvent != null)
		{
			screenMirroringDidStartEvent();
		}
	}

	public void screenMirroringDidStop(string empty)
	{
		if (screenMirroringDidStopEvent != null)
		{
			screenMirroringDidStopEvent();
		}
	}
}
