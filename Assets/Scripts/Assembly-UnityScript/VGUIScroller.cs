using UnityEngine;

public class VGUIScroller : VGUIElement
{
	public bool horizontalScroll;

	public bool verticalScroll = true;

	public Vector2 minLimit = new Vector2(-1f, -1f);

	public Vector2 maxLimit = new Vector2(1f, 1f);

	public float stiffness = 0.1f;

	private bool dragging;

	private Vector2 startPos;

	private Vector2 initialPos;

	private Transform tr;

	private Vector2 scrollSpeed;

	public override void Awake()
	{
		tr = transform;
	}

	public virtual void OnEnable()
	{
		FingerGestures.OnDragBegin += FingerGestureOnDragBegin;
		FingerGestures.OnDragEnd += FingerGestureOnDragEnd;
	}

	public virtual void OnDisable()
	{
		FingerGestures.OnDragBegin -= FingerGestureOnDragBegin;
		FingerGestures.OnDragEnd -= FingerGestureOnDragEnd;
	}

	public virtual void FingerGestureOnDragBegin(int fingerIndex, Vector2 fingerPos, Vector2 st)
	{
		initialPos = tr.localPosition;
		dragging = true;
		startPos = GetWorldPos(fingerPos);
		scrollSpeed = Vector2.zero;
	}

	public virtual void FingerGestureOnDragEnd(int fingerIndex, Vector2 fingerPos)
	{
		dragging = false;
	}

	public override void Update()
	{
		Vector2 pos = tr.localPosition;
		if (!dragging)
		{
			pos += scrollSpeed;
			scrollSpeed *= 1f - stiffness;
			if (!(pos.x >= minLimit.x))
			{
				pos.x += (minLimit.x - pos.x) * stiffness;
			}
			else if (!(pos.x <= maxLimit.x))
			{
				pos.x += (maxLimit.x - pos.x) * stiffness;
			}
			if (!(pos.y >= minLimit.y))
			{
				pos.y += (minLimit.y - pos.y) * stiffness;
			}
			else if (!(pos.y <= maxLimit.y))
			{
				pos.y += (maxLimit.y - pos.y) * stiffness;
			}
		}
		else
		{
			float overshoot = 0f;
			Vector3 worldPos = GetWorldPos(Input.mousePosition);
			if (horizontalScroll)
			{
				float dx = worldPos.x - startPos.x;
				pos.x = initialPos.x + dx;
				if (!(pos.x >= minLimit.x))
				{
					overshoot = pos.x - minLimit.x;
					pos.x = minLimit.x + overshoot * 0.5f;
				}
				else if (!(pos.x <= maxLimit.x))
				{
					overshoot = pos.x - maxLimit.x;
					pos.x = maxLimit.x + overshoot * 0.5f;
				}
			}
			if (verticalScroll)
			{
				float dy = worldPos.y - startPos.y;
				pos.y = initialPos.y + dy;
				if (!(pos.y >= minLimit.y))
				{
					overshoot = pos.y - minLimit.y;
					pos.y = minLimit.y + overshoot * 0.5f;
				}
				else if (!(pos.y <= maxLimit.y))
				{
					overshoot = pos.y - maxLimit.y;
					pos.y = maxLimit.y + overshoot * 0.5f;
				}
			}
			scrollSpeed += (pos - (Vector2)tr.localPosition) * stiffness;
		}
		tr.localPosition = pos;
	}

	public static Vector3 GetWorldPos(Vector2 screenPos)
	{
		Ray ray = Camera.main.ScreenPointToRay(screenPos);
		float distance = (0f - ray.origin.z) / ray.direction.z;
		return ray.GetPoint(distance);
	}
}
