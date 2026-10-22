using System.Collections;
using UnityEngine;

public class MouseGestures : FingerGestures
{
	private const float PinchDeltaEpsilon = 0.0001f;

	public int maxMouseButtons = 3;

	public string pinchGestureAxis = "Mouse ScrollWheel";

	public override int MaxFingers
	{
		get
		{
			return maxMouseButtons;
		}
	}

	protected override void Start()
	{
		base.Start();
		if (trackPinch)
		{
			StartCoroutine(TrackAxisForPinchGesture());
		}
	}

	protected override FingerPhase GetPhase(Finger finger)
	{
		int index = finger.index;
		if (Input.GetMouseButton(index))
		{
			if (Input.GetMouseButtonDown(index))
			{
				return FingerPhase.Began;
			}
			if (((Vector3)(GetPosition(finger) - finger.lastPos)).sqrMagnitude < 1f)
			{
				return FingerPhase.Stationary;
			}
			return FingerPhase.Moved;
		}
		if (Input.GetMouseButtonUp(index))
		{
			return FingerPhase.Ended;
		}
		return FingerPhase.None;
	}

	protected override Vector2 GetPosition(Finger finger)
	{
		return Input.mousePosition;
	}

	private IEnumerator TrackAxisForPinchGesture()
	{
		float lastDelta = 0f;
		while (true)
		{
			float delta = Input.GetAxis(pinchGestureAxis);
			if (Mathf.Abs(delta) >= 0.0001f)
			{
				if (Mathf.Abs(lastDelta) < 0.0001f)
				{
					RaiseOnPinchBegin(Input.mousePosition, Input.mousePosition);
				}
				RaiseOnPinchMove(Input.mousePosition, Input.mousePosition, delta);
			}
			else if (Mathf.Abs(lastDelta) >= 0.0001f)
			{
				RaiseOnPinchEnd(Input.mousePosition, Input.mousePosition);
			}
			lastDelta = delta;
			yield return null;
		}
	}
}
