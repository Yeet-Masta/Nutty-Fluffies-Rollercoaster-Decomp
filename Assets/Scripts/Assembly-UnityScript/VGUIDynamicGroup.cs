using System;
using UnityEngine;

public class VGUIDynamicGroup : VGUIElement
{
	[Serializable]
	public enum HorizontalLock
	{
		LEFT = 0,
		CENTER = 1,
		RIGHT = 2
	}

	[Serializable]
	public enum VerticalLock
	{
		TOP = 0,
		CENTER = 1,
		BOTTOM = 2
	}

	public static bool DYNAMIC_RESIZE = true;

	public HorizontalLock horizontalLock = HorizontalLock.CENTER;

	public VerticalLock verticalLock = VerticalLock.CENTER;

	public bool keepRelativePosition = true;

	public bool constantScale;

	public bool constantScaleOnRetina;

	public bool constantScaleOnIPad;

	public bool dontScale;

	public Camera cameraRef;

	public Vector3 coordInViewPort;

	public Vector2 referenceSize = new Vector2(480f, 320f);

	private Vector3 initialPosition;

	private Vector2 screenSize;

	private float referenceAspectRatio;

	public override void Awake()
	{
		if (cameraRef == null)
		{
			cameraRef = Camera.main;
		}
		getAnchorRefPoint();
		initialPosition = transform.localPosition;
		referenceAspectRatio = referenceSize.y / referenceSize.x;
		onResize();
	}

	public virtual void onResize()
	{
		// NFPort: Camera.pixelWidth/pixelHeight were floats in Unity 4 and are
		// ints now, which turned this into integer division (2000 / 3200 = 0).
		// The resulting x of +-Infinity/NaN hid every edge-anchored button and,
		// through their NaN bounds, scrambled the draw order of all sprites.
		if (cameraRef != null && cameraRef.pixelWidth > 0 && cameraRef.pixelHeight > 0 && (horizontalLock == HorizontalLock.LEFT || horizontalLock == HorizontalLock.RIGHT))
		{
			float num = (float)cameraRef.pixelHeight / (float)cameraRef.pixelWidth;
			float num2 = referenceAspectRatio / num;
			float x = initialPosition.x * num2;
			Vector3 localPosition = transform.localPosition;
			localPosition.x = x;
			transform.localPosition = localPosition;
		}
	}

	public override void Update()
	{
		if (screenSize.x != cameraRef.pixelWidth || screenSize.y != cameraRef.pixelHeight)
		{
			onResize();
		}
	}

	private Vector2 getAnchorRefPoint()
	{
		Vector2 result = default(Vector2);
		switch (horizontalLock)
		{
		case HorizontalLock.LEFT:
			result.x = -0.5f;
			break;
		case HorizontalLock.CENTER:
			result.x = 0f;
			break;
		case HorizontalLock.RIGHT:
			result.x = 0.5f;
			break;
		}
		switch (verticalLock)
		{
		case VerticalLock.TOP:
			result.y = -0.5f;
			break;
		case VerticalLock.CENTER:
			result.y = 0f;
			break;
		case VerticalLock.BOTTOM:
			result.y = 0.5f;
			break;
		}
		return result;
	}
}
