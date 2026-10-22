using UnityEngine;

public class EtceteraTwoEventListener : MonoBehaviour
{
	private void OnEnable()
	{
		EtceteraTwoManager.screenMirroringDidStartEvent += screenMirroringDidStartEvent;
		EtceteraTwoManager.screenMirroringDidStopEvent += screenMirroringDidStopEvent;
	}

	private void OnDisable()
	{
		EtceteraTwoManager.screenMirroringDidStartEvent -= screenMirroringDidStartEvent;
		EtceteraTwoManager.screenMirroringDidStopEvent -= screenMirroringDidStopEvent;
	}

	private void screenMirroringDidStartEvent()
	{
		Debug.Log("screenMirroringDidStartEvent");
	}

	private void screenMirroringDidStopEvent()
	{
		Debug.Log("screenMirroringDidStopEvent");
	}
}
