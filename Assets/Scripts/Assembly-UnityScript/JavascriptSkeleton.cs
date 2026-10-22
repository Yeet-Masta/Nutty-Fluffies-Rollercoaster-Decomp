using System;
using UnityEngine;

[Serializable]
public class JavascriptSkeleton : MonoBehaviour
{
	public virtual void OnEnable()
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

	public virtual void OnDisable()
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

	public virtual void FingerGestures_OnFingerDown(int fingerIndex, Vector2 fingerPos)
	{
	}

	public virtual void FingerGestures_OnFingerMoveBegin(int fingerIndex, Vector2 fingerPos)
	{
	}

	public virtual void FingerGestures_OnFingerMove(int fingerIndex, Vector2 fingerPos)
	{
	}

	public virtual void FingerGestures_OnFingerMoveEnd(int fingerIndex, Vector2 fingerPos)
	{
	}

	public virtual void FingerGestures_OnFingerStationaryBegin(int fingerIndex, Vector2 fingerPos)
	{
	}

	public virtual void FingerGestures_OnFingerStationary(int fingerIndex, Vector2 fingerPos, float elapsedTime)
	{
	}

	public virtual void FingerGestures_OnFingerStationaryEnd(int fingerIndex, Vector2 fingerPos, float elapsedTime)
	{
	}

	public virtual void FingerGestures_OnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
	{
	}

	public virtual void FingerGestures_OnLongPress(int fingerIndex, Vector2 fingerPos)
	{
	}

	public virtual void FingerGestures_OnTap(int fingerIndex, Vector2 fingerPos, int tapCount)
	{
	}

	public virtual void FingerGestures_OnSwipe(int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity)
	{
	}

	public virtual void FingerGestures_OnDragBegin(int fingerIndex, Vector2 fingerPos, Vector2 startPos)
	{
	}

	public virtual void FingerGestures_OnDragMove(int fingerIndex, Vector2 fingerPos, Vector2 delta)
	{
	}

	public virtual void FingerGestures_OnDragEnd(int fingerIndex, Vector2 fingerPos)
	{
	}

	public virtual void FingerGestures_OnPinchBegin(Vector2 fingerPos1, Vector2 fingerPos2)
	{
	}

	public virtual void FingerGestures_OnPinchMove(Vector2 fingerPos1, Vector2 fingerPos2, float delta)
	{
	}

	public virtual void FingerGestures_OnPinchEnd(Vector2 fingerPos1, Vector2 fingerPos2)
	{
	}

	public virtual void FingerGestures_OnRotationBegin(Vector2 fingerPos1, Vector2 fingerPos2)
	{
	}

	public virtual void FingerGestures_OnRotationMove(Vector2 fingerPos1, Vector2 fingerPos2, float rotationAngleDelta)
	{
	}

	public virtual void FingerGestures_OnRotationEnd(Vector2 fingerPos1, Vector2 fingerPos2, float totalRotationAngle)
	{
	}

	public virtual void Main()
	{
	}
}
