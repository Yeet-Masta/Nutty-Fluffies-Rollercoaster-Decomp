namespace UnityEngine
{
	public abstract class FingerGestures : MonoBehaviour
	{
		public enum SwipeDirection
		{
			Right = 0,
			Left = 1,
			Up = 2,
			Down = 3
		}

		protected enum FingerPhase
		{
			None = 0,
			Began = 1,
			Moved = 2,
			Stationary = 3,
			Ended = 4
		}

		protected enum FingerMotionState
		{
			None = 0,
			Stationary = 1,
			Moving = 2
		}

		protected class Finger
		{
			public int index;

			public FingerPhase lastPhase;

			public bool active;

			public bool moved;

			public FingerMotionState motionState;

			public Vector2 startPos = Vector2.zero;

			public float startTime;

			public Vector2 moveStartPos = Vector2.zero;

			public bool longPressFired;

			public Vector2 stationaryStartPos = Vector2.zero;

			public float stationaryStartTime;

			public Vector2 lastPos = Vector2.zero;

			public bool dragging;

			public float dragStartTime;

			public int tapCount;

			public float tapStartTime;

			public Vector2 tapStartPos = Vector2.zero;

			public Finger(int index)
			{
				this.index = index;
			}

			public virtual void OnActivate(Vector2 position)
			{
				active = true;
				moved = false;
				motionState = FingerMotionState.None;
				startTime = Time.time;
				moveStartPos = position;
				stationaryStartTime = Time.time;
				stationaryStartPos = position;
				startPos = position;
				lastPos = position;
				dragging = false;
				longPressFired = false;
			}

			public virtual void OnRelease()
			{
				active = false;
			}
		}

		public delegate void FingerDownEventHandler(int fingerIndex, Vector2 fingerPos);

		public delegate void FingerStationaryBeginEventHandler(int fingerIndex, Vector2 fingerPos);

		public delegate void FingerStationaryEventHandler(int fingerIndex, Vector2 fingerPos, float elapsedTime);

		public delegate void FingerStationaryEndEventHandler(int fingerIndex, Vector2 fingerPos, float elapsedTime);

		public delegate void FingerUpEventHandler(int fingerIndex, Vector2 fingerPos, float timeHeldDown);

		public delegate void FingerMoveEventHandler(int fingerIndex, Vector2 fingerPos);

		public delegate void LongPressEventHandler(int fingerIndex, Vector2 fingerPos);

		public delegate void DragBeginEventHandler(int fingerIndex, Vector2 fingerPos, Vector2 startPos);

		public delegate void DragMoveEventHandler(int fingerIndex, Vector2 fingerPos, Vector2 delta);

		public delegate void DragEndEventHandler(int fingerIndex, Vector2 fingerPos);

		public delegate void PinchEventHandler(Vector2 fingerPos1, Vector2 fingerPos2);

		public delegate void PinchMoveEventHandler(Vector2 fingerPos1, Vector2 fingerPos2, float delta);

		public delegate void TapEventHandler(int fingerIndex, Vector2 fingerPos, int tapCount);

		public delegate void SwipeEventHandler(int fingerIndex, Vector2 startPos, SwipeDirection direction, float velocity);

		public delegate void RotationBeginEventHandler(Vector2 fingerPos1, Vector2 fingerPos2);

		public delegate void RotationMoveEventHandler(Vector2 fingerPos1, Vector2 fingerPos2, float rotationAngleDelta);

		public delegate void RotationEndEventHandler(Vector2 fingerPos1, Vector2 fingerPos2, float totalRotationAngle);

		public float moveThreshold = 5f;

		public bool trackPinch = true;

		public float pinchDeltaScale = 1f;

		public float maxTapTime = 0.75f;

		public float tapMoveTolerance = 2f;

		public float minSwipeDistance = 2f;

		public float swipeDirectionTolerance = 0.2f;

		public bool trackRotation = true;

		public float longPressDuration = 1.5f;

		private static FingerGestures instance;

		private Finger[] fingers;

		public static FingerGestures Instance
		{
			get
			{
				return instance;
			}
		}

		public abstract int MaxFingers { get; }

		public static event FingerDownEventHandler OnFingerDown;

		public static event FingerStationaryBeginEventHandler OnFingerStationaryBegin;

		public static event FingerStationaryEventHandler OnFingerStationary;

		public static event FingerStationaryEndEventHandler OnFingerStationaryEnd;

		public static event FingerMoveEventHandler OnFingerMoveBegin;

		public static event FingerMoveEventHandler OnFingerMove;

		public static event FingerMoveEventHandler OnFingerMoveEnd;

		public static event FingerUpEventHandler OnFingerUp;

		public static event LongPressEventHandler OnLongPress;

		public static event DragBeginEventHandler OnDragBegin;

		public static event DragMoveEventHandler OnDragMove;

		public static event DragEndEventHandler OnDragEnd;

		public static event PinchEventHandler OnPinchBegin;

		public static event PinchMoveEventHandler OnPinchMove;

		public static event PinchEventHandler OnPinchEnd;

		public static event TapEventHandler OnTap;

		public static event SwipeEventHandler OnSwipe;

		public static event RotationBeginEventHandler OnRotationBegin;

		public static event RotationMoveEventHandler OnRotationMove;

		public static event RotationEndEventHandler OnRotationEnd;

		protected Finger GetFinger(int index)
		{
			return fingers[index];
		}

		private void InitFingers(int count)
		{
			fingers = new Finger[count];
			for (int i = 0; i < fingers.Length; i++)
			{
				fingers[i] = CreateFinger(i);
			}
		}

		protected virtual void Awake()
		{
			instance = this;
			InitFingers(MaxFingers);
		}

		protected virtual void Start()
		{
		}

		protected virtual void Update()
		{
			if (fingers != null)
			{
				Finger[] array = fingers;
				foreach (Finger finger in array)
				{
					FingerPhase phase = GetPhase(finger);
					if (phase == FingerPhase.None)
					{
						if (finger.active && finger.lastPhase != FingerPhase.Ended)
						{
							Debug.LogWarning(string.Concat("Unexpected phase transition: ", finger.lastPhase, " to ", phase, ". Forcing finger release."));
							Release(finger);
						}
					}
					else
					{
						Vector2 position = GetPosition(finger);
						UpdateFinger(finger, phase, position);
					}
					finger.lastPhase = phase;
				}
			}
			else
			{
				Debug.LogWarning("Fingers list was destroyed. Forcing reinitialization.");
				InitFingers(MaxFingers);
			}
		}

		protected virtual Finger CreateFinger(int index)
		{
			return new Finger(index);
		}

		protected abstract FingerPhase GetPhase(Finger finger);

		protected abstract Vector2 GetPosition(Finger finger);

		protected virtual void UpdateFinger(Finger finger, FingerPhase phase, Vector2 fingerPos)
		{
			if (phase == FingerPhase.Ended)
			{
				finger.lastPos = fingerPos;
				Release(finger);
				return;
			}
			if (phase == FingerPhase.Began || !finger.active)
			{
				finger.OnActivate(fingerPos);
				RaiseOnFingerDown(finger.index, finger.startPos);
			}
			else
			{
				switch (phase)
				{
				case FingerPhase.Moved:
					if (finger.motionState != FingerMotionState.Moving)
					{
						if ((fingerPos - finger.moveStartPos).sqrMagnitude > moveThreshold * moveThreshold)
						{
							if (finger.motionState == FingerMotionState.Stationary)
							{
								StopStationary(finger, finger.lastPos);
							}
							finger.motionState = FingerMotionState.Moving;
							RaiseOnFingerMoveBegin(finger.index, finger.moveStartPos);
							if (!finger.moved)
							{
								finger.dragging = true;
								finger.dragStartTime = Time.time;
								RaiseOnDragBegin(finger.index, fingerPos, finger.startPos);
								finger.moved = true;
							}
						}
						else
						{
							HandleStationaryFinger(finger, fingerPos);
						}
					}
					else
					{
						Vector2 delta = fingerPos - finger.lastPos;
						if (delta.sqrMagnitude > 0f)
						{
							RaiseOnFingerMove(finger.index, fingerPos);
							RaiseOnDragMove(finger.index, fingerPos, delta);
						}
					}
					break;
				case FingerPhase.Stationary:
					HandleStationaryFinger(finger, fingerPos);
					break;
				}
			}
			finger.lastPos = fingerPos;
		}

		private void StopMoving(Finger finger, Vector2 fingerPos)
		{
			finger.motionState = FingerMotionState.None;
			RaiseOnFingerMoveEnd(finger.index, fingerPos);
			finger.moveStartPos = fingerPos;
		}

		private void StopStationary(Finger finger, Vector2 fingerPos)
		{
			finger.motionState = FingerMotionState.None;
			RaiseOnFingerStationaryEnd(finger.index, fingerPos, Time.time - finger.stationaryStartTime);
		}

		protected void HandleStationaryFinger(Finger finger, Vector2 fingerPos)
		{
			if (finger.motionState != FingerMotionState.Stationary)
			{
				if (finger.motionState == FingerMotionState.Moving)
				{
					StopMoving(finger, fingerPos);
				}
				finger.motionState = FingerMotionState.Stationary;
				finger.stationaryStartTime = Time.time;
				finger.stationaryStartPos = fingerPos;
				RaiseOnFingerStationaryBegin(finger.index, fingerPos);
			}
			float num = Time.time - finger.stationaryStartTime;
			if (!finger.longPressFired && !finger.moved && num >= longPressDuration)
			{
				RaiseOnLongPress(finger.index, finger.startPos);
				finger.longPressFired = true;
			}
			RaiseOnFingerStationary(finger.index, finger.stationaryStartPos, num);
		}

		protected virtual void Release(Finger finger)
		{
			if (finger.moved || finger.dragging)
			{
				finger.tapCount = 0;
				RaiseOnDragEnd(finger.index, finger.lastPos);
				SwipeDirection direction;
				float velocity;
				if (GetSwipeGesture(finger, out direction, out velocity))
				{
					RaiseOnSwipe(finger.index, finger.startPos, direction, velocity);
				}
			}
			else
			{
				if (ShouldResetTap(finger))
				{
					finger.tapCount = 0;
				}
				if (finger.tapCount == 0)
				{
					finger.tapStartTime = Time.time;
					finger.tapStartPos = finger.lastPos;
				}
				RaiseOnTap(finger.index, finger.lastPos, ++finger.tapCount);
			}
			if (finger.motionState == FingerMotionState.Stationary)
			{
				StopStationary(finger, finger.lastPos);
			}
			else if (finger.motionState == FingerMotionState.Moving)
			{
				StopMoving(finger, finger.lastPos);
			}
			float timeHeldDown = Time.time - finger.startTime;
			RaiseOnFingerUp(finger.index, finger.lastPos, timeHeldDown);
			finger.OnRelease();
		}

		private bool GetSwipeGesture(Finger finger, out SwipeDirection direction, out float velocity)
		{
			direction = SwipeDirection.Right;
			velocity = 0f;
			Vector3 vector = finger.lastPos - finger.startPos;
			float magnitude = vector.magnitude;
			if (magnitude < minSwipeDistance)
			{
				return false;
			}
			float num = Mathf.Clamp01(1f - swipeDirectionTolerance);
			Vector2 lhs = vector.normalized;
			if (Vector2.Dot(lhs, Vector2.right) >= num)
			{
				direction = SwipeDirection.Right;
			}
			else if (Vector2.Dot(lhs, -Vector2.right) >= num)
			{
				direction = SwipeDirection.Left;
			}
			else if (Vector2.Dot(lhs, Vector2.up) >= num)
			{
				direction = SwipeDirection.Up;
			}
			else
			{
				if (!(Vector2.Dot(lhs, -Vector2.up) >= num))
				{
					return false;
				}
				direction = SwipeDirection.Down;
			}
			float num2 = Time.time - finger.dragStartTime;
			velocity = magnitude / num2;
			return true;
		}

		private bool ShouldResetTap(Finger finger)
		{
			if (finger.tapCount > 0)
			{
				float num = Time.time - finger.tapStartTime;
				if (num > maxTapTime)
				{
					return true;
				}
				if ((finger.lastPos - finger.tapStartPos).SqrMagnitude() > tapMoveTolerance * tapMoveTolerance)
				{
					return true;
				}
			}
			return false;
		}

		protected void RaiseOnFingerDown(int fingerIndex, Vector2 fingerPos)
		{
			if (OnFingerDown != null)
			{
				OnFingerDown(fingerIndex, fingerPos);
			}
		}

		protected void RaiseOnFingerStationaryBegin(int fingerIndex, Vector2 fingerPos)
		{
			if (OnFingerStationaryBegin != null)
			{
				OnFingerStationaryBegin(fingerIndex, fingerPos);
			}
		}

		protected void RaiseOnFingerStationary(int fingerIndex, Vector2 fingerPos, float elapsedTime)
		{
			if (OnFingerStationary != null)
			{
				OnFingerStationary(fingerIndex, fingerPos, elapsedTime);
			}
		}

		protected void RaiseOnFingerStationaryEnd(int fingerIndex, Vector2 fingerPos, float elapsedTime)
		{
			if (OnFingerStationaryEnd != null)
			{
				OnFingerStationaryEnd(fingerIndex, fingerPos, elapsedTime);
			}
		}

		protected void RaiseOnFingerMoveBegin(int fingerIndex, Vector2 fingerPos)
		{
			if (OnFingerMoveBegin != null)
			{
				OnFingerMoveBegin(fingerIndex, fingerPos);
			}
		}

		protected void RaiseOnFingerMove(int fingerIndex, Vector2 fingerPos)
		{
			if (OnFingerMove != null)
			{
				OnFingerMove(fingerIndex, fingerPos);
			}
		}

		protected void RaiseOnFingerMoveEnd(int fingerIndex, Vector2 fingerPos)
		{
			if (OnFingerMoveEnd != null)
			{
				OnFingerMoveEnd(fingerIndex, fingerPos);
			}
		}

		protected void RaiseOnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
		{
			if (OnFingerUp != null)
			{
				OnFingerUp(fingerIndex, fingerPos, timeHeldDown);
			}
		}

		protected void RaiseOnLongPress(int fingerIndex, Vector2 fingerPos)
		{
			if (OnLongPress != null)
			{
				OnLongPress(fingerIndex, fingerPos);
			}
		}

		protected void RaiseOnDragBegin(int fingerIndex, Vector2 fingerPos, Vector2 startPos)
		{
			if (OnDragBegin != null)
			{
				OnDragBegin(fingerIndex, fingerPos, startPos);
			}
		}

		protected void RaiseOnDragMove(int fingerIndex, Vector2 fingerPos, Vector2 delta)
		{
			if (OnDragMove != null)
			{
				OnDragMove(fingerIndex, fingerPos, delta);
			}
		}

		protected void RaiseOnDragEnd(int fingerIndex, Vector2 fingerPos)
		{
			if (OnDragEnd != null)
			{
				OnDragEnd(fingerIndex, fingerPos);
			}
		}

		protected void RaiseOnPinchBegin(Vector2 fingerPos1, Vector2 fingerPos2)
		{
			if (OnPinchBegin != null)
			{
				OnPinchBegin(fingerPos1, fingerPos2);
			}
		}

		protected void RaiseOnPinchMove(Vector2 fingerPos1, Vector2 fingerPos2, float delta)
		{
			if (OnPinchMove != null)
			{
				OnPinchMove(fingerPos1, fingerPos2, pinchDeltaScale * delta);
			}
		}

		protected void RaiseOnPinchEnd(Vector2 fingerPos1, Vector2 fingerPos2)
		{
			if (OnPinchEnd != null)
			{
				OnPinchEnd(fingerPos1, fingerPos2);
			}
		}

		protected void RaiseOnTap(int fingerIndex, Vector2 fingerPos, int tapCount)
		{
			if (OnTap != null)
			{
				OnTap(fingerIndex, fingerPos, tapCount);
			}
		}

		protected void RaiseOnSwipe(int fingerIndex, Vector2 startPos, SwipeDirection direction, float velocity)
		{
			if (OnSwipe != null)
			{
				OnSwipe(fingerIndex, startPos, direction, velocity);
			}
		}

		protected void RaiseOnRotationBegin(Vector2 fingerPos1, Vector2 fingerPos2)
		{
			if (OnRotationBegin != null)
			{
				OnRotationBegin(fingerPos1, fingerPos2);
			}
		}

		protected void RaiseOnRotationMove(Vector2 fingerPos1, Vector2 fingerPos2, float rotationAngleDelta)
		{
			if (OnRotationMove != null)
			{
				OnRotationMove(fingerPos1, fingerPos2, rotationAngleDelta);
			}
		}

		protected void RaiseOnRotationEnd(Vector2 fingerPos1, Vector2 fingerPos2, float totalRotationAngle)
		{
			if (OnRotationEnd != null)
			{
				OnRotationEnd(fingerPos1, fingerPos2, totalRotationAngle);
			}
		}
	}
}
