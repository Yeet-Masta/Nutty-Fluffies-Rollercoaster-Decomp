using UnityEngine;

[AddComponentMenu("FingerGestures/Toolbox/FingerDown")]
public class TBFingerDown : TBComponent
{
	public Message message = new Message("OnFingerDown");

	public event EventHandler<TBFingerDown> OnFingerDown;

	public bool RaiseFingerDown(int fingerIndex, Vector2 fingerPos)
	{
		FingerIndex = fingerIndex;
		FingerPos = fingerPos;
		if (OnFingerDown != null)
		{
			OnFingerDown(this);
		}
		Send(message);
		return true;
	}
}
