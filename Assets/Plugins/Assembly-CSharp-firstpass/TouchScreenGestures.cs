using UnityEngine;

public class TouchScreenGestures : FingerGestures
{
	public int maxFingers = 5;

	public float pinchDOT = -0.7f;

	public float minPinchDistance = 0.5f;

	public float rotationDOT = -0.7f;

	public float minRotationAmount = 0.5f;

	private Touch nullTouch = default(Touch);

	private int[] finger2touchMap;

	private bool pinching;

	private bool rotating;

	private float totalRotation;

	public override int MaxFingers
	{
		get
		{
			return maxFingers;
		}
	}

	protected override void Start()
	{
		finger2touchMap = new int[MaxFingers];
		base.Start();
	}

	private void UpdateFingerTouchMap()
	{
		for (int i = 0; i < finger2touchMap.Length; i++)
		{
			finger2touchMap[i] = -1;
		}
		if (Input.touchCount == 1)
		{
			finger2touchMap[0] = 0;
			return;
		}
		for (int j = 0; j < Input.touchCount; j++)
		{
			int fingerId = Input.touches[j].fingerId;
			if (fingerId < finger2touchMap.Length)
			{
				finger2touchMap[fingerId] = j;
			}
		}
	}

	private bool HasValidTouch(Finger finger)
	{
		return finger2touchMap[finger.index] != -1;
	}

	private Touch GetTouch(Finger finger)
	{
		int num = finger2touchMap[finger.index];
		if (num == -1)
		{
			return nullTouch;
		}
		return Input.touches[num];
	}

	protected override void Update()
	{
		UpdateFingerTouchMap();
		base.Update();
		if (trackPinch || trackRotation)
		{
			TrackTwoFingerGestures();
		}
	}

	private void TrackTwoFingerGestures()
	{
		Finger finger = GetFinger(0);
		Finger finger2 = GetFinger(1);
		if (Input.touchCount == 2)
		{
			if (!finger.moved || !finger2.moved)
			{
				return;
			}
			FingerPhase phase = GetPhase(finger);
			FingerPhase phase2 = GetPhase(finger2);
			if (phase != FingerPhase.Moved || phase2 != FingerPhase.Moved)
			{
				return;
			}
			Touch touch = GetTouch(finger);
			Touch touch2 = GetTouch(finger2);
			Vector2 vector = touch.position - touch.deltaPosition;
			Vector2 vector2 = touch2.position - touch2.deltaPosition;
			Vector2 vector3 = vector - vector2;
			Vector2 vector4 = touch.position - touch2.position;
			float num = vector4.magnitude - vector3.magnitude;
			if (Mathf.Abs(num) < 0.001f)
			{
				return;
			}
			float num2 = Vector2.Dot(touch.deltaPosition.normalized, touch2.deltaPosition.normalized);
			if (trackPinch && num2 < pinchDOT)
			{
				if (!pinching)
				{
					Vector2 vector5 = finger.startPos - finger2.startPos;
					float num3 = vector4.magnitude - vector5.magnitude;
					if (Mathf.Abs(num3) >= minPinchDistance)
					{
						pinching = true;
						RaiseOnPinchBegin(finger.startPos, finger2.startPos);
						RaiseOnPinchMove(touch.position, touch2.position, num3 - Mathf.Sign(num3) * minPinchDistance);
					}
				}
				else
				{
					RaiseOnPinchMove(touch.position, touch2.position, num);
				}
			}
			if (!trackRotation || !(num2 < rotationDOT))
			{
				return;
			}
			Vector2 normalized = vector4.normalized;
			if (!rotating)
			{
				Vector2 normalized2 = (finger.startPos - finger2.startPos).normalized;
				float num4 = 57.29578f * SignedAngle(normalized2, normalized);
				if (Mathf.Abs(num4) >= minRotationAmount)
				{
					rotating = true;
					RaiseOnRotationBegin(finger.startPos, finger2.startPos);
					RaiseOnRotationMove(touch.position, touch2.position, num4 - Mathf.Sign(num4) * minRotationAmount);
					totalRotation = minRotationAmount;
				}
			}
			else
			{
				Vector2 normalized3 = vector3.normalized;
				float num5 = 57.29578f * SignedAngle(normalized3, normalized);
				RaiseOnRotationMove(touch.position, touch2.position, num5);
				totalRotation += num5;
			}
		}
		else
		{
			if (pinching)
			{
				RaiseOnPinchEnd(finger.lastPos, finger2.lastPos);
				pinching = false;
			}
			if (rotating)
			{
				RaiseOnRotationEnd(finger.lastPos, finger2.lastPos, totalRotation);
				rotating = false;
			}
		}
	}

	protected override FingerPhase GetPhase(Finger finger)
	{
		if (HasValidTouch(finger))
		{
			switch (GetTouch(finger).phase)
			{
			case TouchPhase.Began:
				return FingerPhase.Began;
			case TouchPhase.Moved:
				return FingerPhase.Moved;
			case TouchPhase.Stationary:
				return FingerPhase.Stationary;
			default:
				return FingerPhase.Ended;
			}
		}
		return FingerPhase.None;
	}

	protected override Vector2 GetPosition(Finger finger)
	{
		return GetTouch(finger).position;
	}

	public static float SignedAngle(Vector2 from, Vector2 to)
	{
		float y = from.x * to.y - from.y * to.x;
		return Mathf.Atan2(y, Vector2.Dot(from, to));
	}
}
