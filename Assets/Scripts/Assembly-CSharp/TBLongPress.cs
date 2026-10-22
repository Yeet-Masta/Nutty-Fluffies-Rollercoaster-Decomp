using UnityEngine;

[AddComponentMenu("FingerGestures/Toolbox/LongPress")]
public class TBLongPress : TBComponent
{
	public Message message = new Message("OnLongPress");

	public event EventHandler<TBLongPress> OnLongPress;

	public bool RaiseLongPress(int fingerIndex, Vector2 fingerPos)
	{
		FingerIndex = fingerIndex;
		FingerPos = fingerPos;
		if (OnLongPress != null)
		{
			OnLongPress(this);
		}
		Send(message);
		return true;
	}
}
