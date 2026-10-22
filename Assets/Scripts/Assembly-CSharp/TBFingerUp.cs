using UnityEngine;

[AddComponentMenu("FingerGestures/Toolbox/FingerUp")]
public class TBFingerUp : TBComponent
{
	public Message message = new Message("OnFingerUp");

	private float timeHeldDown;

	public float TimeHeldDown
	{
		get
		{
			return timeHeldDown;
		}
		private set
		{
			timeHeldDown = value;
		}
	}

	public event EventHandler<TBFingerUp> OnFingerUp;

	public bool RaiseFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
	{
		FingerIndex = fingerIndex;
		FingerPos = fingerPos;
		TimeHeldDown = timeHeldDown;
		if (OnFingerUp != null)
		{
			OnFingerUp(this);
		}
		Send(message);
		return true;
	}
}
