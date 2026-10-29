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
		Vector2 vector = tr.localPosition;
		if (!dragging)
		{
			vector += scrollSpeed;
			scrollSpeed *= 1f - stiffness;
			if (!(vector.x >= minLimit.x))
			{
				vector.x += (minLimit.x - vector.x) * stiffness;
			}
			else if (!(vector.x <= maxLimit.x))
			{
				vector.x += (maxLimit.x - vector.x) * stiffness;
			}
			if (!(vector.y >= minLimit.y))
			{
				vector.y += (minLimit.y - vector.y) * stiffness;
			}
			else if (!(vector.y <= maxLimit.y))
			{
				vector.y += (maxLimit.y - vector.y) * stiffness;
			}
		}
		else
		{
			float num = default(float);
			Vector3 worldPos = GetWorldPos(Input.mousePosition);
			if (horizontalScroll)
			{
				float num2 = worldPos.x - startPos.x;
				vector.x = initialPos.x + num2;
				if (!(vector.x >= minLimit.x))
				{
					num = vector.x - minLimit.x;
					vector.x = minLimit.x + num * 0.5f;
				}
				else if (!(vector.x <= maxLimit.x))
				{
					num = vector.x - maxLimit.x;
					vector.x = maxLimit.x + num * 0.5f;
				}
			}
			if (verticalScroll)
			{
				float num3 = worldPos.y - startPos.y;
				vector.y = initialPos.y + num3;
				if (!(vector.y >= minLimit.y))
				{
					num = vector.y - minLimit.y;
					vector.y = minLimit.y + num * 0.5f;
				}
				else if (!(vector.y <= maxLimit.y))
				{
					num = vector.y - maxLimit.y;
					vector.y = maxLimit.y + num * 0.5f;
				}
			}
			scrollSpeed += (vector - (Vector2)tr.localPosition) * stiffness;
		}
		tr.localPosition = vector;
	}

	public static Vector3 GetWorldPos(Vector2 screenPos)
	{
		Ray ray = Camera.main.ScreenPointToRay(screenPos);
		float distance = (0f - ray.origin.z) / ray.direction.z;
		return ray.GetPoint(distance);
	}
}
