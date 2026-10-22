using UnityEngine;

public class CSharpSkeleton : MonoBehaviour
{
	private void OnEnable()
	{
		FingerGestures.OnFingerDown += FingerGestures_OnFingerDown;
		FingerGestures.OnFingerStationaryBegin += FingerGestures_OnFingerStationaryBegin;
		FingerGestures.OnFingerStationary += FingerGestures_OnFingerStationary;
		FingerGestures.OnFingerStationaryEnd += FingerGestures_OnFingerStationaryEnd;
		FingerGestures.OnFingerMoveBegin += FingerGestures_OnFingerMoveBegin;
		FingerGestures.OnFingerMove += FingerGestures_OnFingerMove;
		FingerGestures.OnFingerMoveEnd += FingerGestures_OnFingerMoveEnd;
		FingerGestures.OnFingerUp += FingerGestures_OnFingerUp;
		FingerGestures.OnLongPress += FingerGestures_OnLongPress;
		FingerGestures.OnTap += FingerGestures_OnTap;
		FingerGestures.OnSwipe += FingerGestures_OnSwipe;
		FingerGestures.OnDragBegin += FingerGestures_OnDragBegin;
		FingerGestures.OnDragMove += FingerGestures_OnDragMove;
		FingerGestures.OnDragEnd += FingerGestures_OnDragEnd;
		FingerGestures.OnPinchBegin += FingerGestures_OnPinchBegin;
		FingerGestures.OnPinchMove += FingerGestures_OnPinchMove;
		FingerGestures.OnPinchEnd += FingerGestures_OnPinchEnd;
		FingerGestures.OnRotationBegin += FingerGestures_OnRotationBegin;
		FingerGestures.OnRotationMove += FingerGestures_OnRotationMove;
		FingerGestures.OnRotationEnd += FingerGestures_OnRotationEnd;
	}

	private void OnDisable()
	{
		FingerGestures.OnFingerDown -= FingerGestures_OnFingerDown;
		FingerGestures.OnFingerStationaryBegin -= FingerGestures_OnFingerStationaryBegin;
		FingerGestures.OnFingerStationary -= FingerGestures_OnFingerStationary;
		FingerGestures.OnFingerStationaryEnd -= FingerGestures_OnFingerStationaryEnd;
		FingerGestures.OnFingerMoveBegin -= FingerGestures_OnFingerMoveBegin;
		FingerGestures.OnFingerMove -= FingerGestures_OnFingerMove;
		FingerGestures.OnFingerMoveEnd -= FingerGestures_OnFingerMoveEnd;
		FingerGestures.OnFingerUp -= FingerGestures_OnFingerUp;
		FingerGestures.OnLongPress -= FingerGestures_OnLongPress;
		FingerGestures.OnTap -= FingerGestures_OnTap;
		FingerGestures.OnSwipe -= FingerGestures_OnSwipe;
		FingerGestures.OnDragBegin -= FingerGestures_OnDragBegin;
		FingerGestures.OnDragMove -= FingerGestures_OnDragMove;
		FingerGestures.OnDragEnd -= FingerGestures_OnDragEnd;
		FingerGestures.OnPinchBegin -= FingerGestures_OnPinchBegin;
		FingerGestures.OnPinchMove -= FingerGestures_OnPinchMove;
		FingerGestures.OnPinchEnd -= FingerGestures_OnPinchEnd;
		FingerGestures.OnRotationBegin -= FingerGestures_OnRotationBegin;
		FingerGestures.OnRotationMove -= FingerGestures_OnRotationMove;
		FingerGestures.OnRotationEnd -= FingerGestures_OnRotationEnd;
	}

	private void FingerGestures_OnFingerDown(int fingerIndex, Vector2 fingerPos)
	{
	}

	private void FingerGestures_OnFingerMoveBegin(int fingerIndex, Vector2 fingerPos)
	{
	}

	private void FingerGestures_OnFingerMove(int fingerIndex, Vector2 fingerPos)
	{
	}

	private void FingerGestures_OnFingerMoveEnd(int fingerIndex, Vector2 fingerPos)
	{
	}

	private void FingerGestures_OnFingerStationaryBegin(int fingerIndex, Vector2 fingerPos)
	{
	}

	private void FingerGestures_OnFingerStationary(int fingerIndex, Vector2 fingerPos, float elapsedTime)
	{
	}

	private void FingerGestures_OnFingerStationaryEnd(int fingerIndex, Vector2 fingerPos, float elapsedTime)
	{
	}

	private void FingerGestures_OnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
	{
	}

	private void FingerGestures_OnLongPress(int fingerIndex, Vector2 fingerPos)
	{
	}

	private void FingerGestures_OnTap(int fingerIndex, Vector2 fingerPos, int tapCount)
	{
	}

	private void FingerGestures_OnSwipe(int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity)
	{
	}

	private void FingerGestures_OnDragBegin(int fingerIndex, Vector2 fingerPos, Vector2 startPos)
	{
	}

	private void FingerGestures_OnDragMove(int fingerIndex, Vector2 fingerPos, Vector2 delta)
	{
	}

	private void FingerGestures_OnDragEnd(int fingerIndex, Vector2 fingerPos)
	{
	}

	private void FingerGestures_OnPinchBegin(Vector2 fingerPos1, Vector2 fingerPos2)
	{
	}

	private void FingerGestures_OnPinchMove(Vector2 fingerPos1, Vector2 fingerPos2, float delta)
	{
	}

	private void FingerGestures_OnPinchEnd(Vector2 fingerPos1, Vector2 fingerPos2)
	{
	}

	private void FingerGestures_OnRotationBegin(Vector2 fingerPos1, Vector2 fingerPos2)
	{
	}

	private void FingerGestures_OnRotationMove(Vector2 fingerPos1, Vector2 fingerPos2, float rotationAngleDelta)
	{
	}

	private void FingerGestures_OnRotationEnd(Vector2 fingerPos1, Vector2 fingerPos2, float totalRotationAngle)
	{
	}
}
