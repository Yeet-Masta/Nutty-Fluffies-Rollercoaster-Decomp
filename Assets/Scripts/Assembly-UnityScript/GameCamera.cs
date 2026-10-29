using UnityEngine;

public class GameCamera : Camera2D
{
	public Transform cameraTarget;

	public Vector2 viewOffset;

	public float viewStiffness = 0.1f;

	public float zoomStiffness = 0.01f;

	public float speedZoomFactor = 1.1f;

	public float speedOffsetFactor = 1.5f;

	public float offsetStiffness = 0.01f;

	public float zoomOutPerCart = 0.3f;

	public bool useDynamicY = true;

	public float minOffsetAboveTarget = 4f;

	[HideInInspector]
	public Transform lastCart;

	private float halfWidth;

	private float halfHeight;

	private Vector2 speedOffset;

	private Transform tr;

	private float zoomAdjust;

	private ZLevel zlevel;

	private float jumpZoom;

	public float dynamicYLimit;

	public override void Awake()
	{
		base.Awake();
		useGUILayout = false;
		speedOffset = Vector2.zero;
		tr = transform;
		jumpZoom = 0f;
		zlevel = ZLevel.getInstance();
	}

	public override void FixedUpdate()
	{
		base.FixedUpdate();
		Vehicle vehicle = GameManager.getInstance().getVehicle();
		if (cameraTarget != null && (GameManager.getInstance().isPlaying() || GameManager.getInstance().isFading() || TutorialManager.getInstance().isVisible()))
		{
			float magnitude = cameraTarget.GetComponent<Rigidbody>().velocity.magnitude;
			Cart cart = vehicle.carts[vehicle.carts.Length >> 1];
			if (vehicle.isFlying)
			{
				jumpZoom = Mathf.Sqrt(cart.sqHeight);
				if (!cart.sqHeightAvailable)
				{
					jumpZoom = 15f;
				}
				if (!(jumpZoom <= 20f))
				{
					jumpZoom = 20f;
				}
			}
			else
			{
				jumpZoom *= 0.9f;
			}
			float speedZoom = magnitude * speedZoomFactor;
			float orthographicSize = cameraRef.orthographicSize;
			orthographicSize += (initialViewSize + initialZoomAdjust + zoomAdjust + speedZoom + jumpZoom - orthographicSize) * zoomStiffness;
			if (!(orthographicSize <= limitedSize.y * 0.5f))
			{
				orthographicSize = limitedSize.y * 0.5f;
			}
			Vector3 speedOffsetTarget = cameraTarget.right * speedOffsetFactor * magnitude;
			speedOffset += (Vector2)((speedOffsetTarget - (Vector3)speedOffset) * offsetStiffness);
			if (vehicle != null && !cart.sqHeightAvailable && vehicle.isFlying)
			{
				speedOffset.y -= jumpZoom * 0.05f;
			}
			halfHeight = orthographicSize;
			halfWidth = orthographicSize * cameraRef.aspect;
			float y = cameraTarget.position.y;
			float viewHeight = halfHeight * 2f;
			float x = tr.position.x;
			float y2 = tr.position.y;
			float targetX = cameraTarget.position.x + viewOffset.x + speedOffset.x;
			float targetY = y + viewOffset.y + speedOffset.y;
			x += (targetX - x) * viewStiffness;
			y2 += (targetY - y2) * viewStiffness;
			float margin = 3.5f;
			float x2 = minLimit.x;
			float rightLimit = lastCart.position.x - margin;
			if (!(rightLimit >= x2))
			{
				rightLimit = x2;
			}
			rightLimit += 2f * halfWidth;
			if (!(rightLimit <= maxLimit.x))
			{
				rightLimit = maxLimit.x;
			}
			if (useDynamicY)
			{
				float dynamicLimit = calculateDynamicLimit();
				dynamicYLimit += (dynamicLimit - dynamicYLimit) * 0.01f;
				if (!(dynamicYLimit >= y + minOffsetAboveTarget))
				{
					dynamicYLimit = y + minOffsetAboveTarget;
				}
			}
			float y3 = lastCart.position.y;
			float topLimit = dynamicYLimit;
			float bottomLimit = y3 - viewHeight + margin;
			if (!(topLimit <= y3 + viewHeight - margin))
			{
				topLimit = y3 + viewHeight - margin;
			}
			if (!(topLimit <= maxLimit.y))
			{
				topLimit = maxLimit.y;
				if (!(bottomLimit <= topLimit - viewHeight))
				{
					bottomLimit = topLimit - viewHeight;
				}
			}
			if (!(bottomLimit >= minLimit.y))
			{
				bottomLimit = minLimit.y;
			}
			if (!(topLimit + viewHeight >= bottomLimit))
			{
				topLimit = bottomLimit + viewHeight;
			}
			float maxTop = y3 + viewHeight - margin;
			if (!(topLimit <= maxTop))
			{
				topLimit = maxTop;
			}
			if (!(topLimit >= bottomLimit + viewHeight))
			{
				topLimit = bottomLimit + viewHeight;
			}
			if (!(x >= x2 + halfWidth))
			{
				x = x2 + halfWidth;
			}
			else if (!(x <= rightLimit - halfWidth))
			{
				x = rightLimit - halfWidth;
			}
			if (!(y2 >= bottomLimit + halfHeight))
			{
				y2 = bottomLimit + halfHeight;
			}
			else if (!(y2 <= topLimit - halfHeight))
			{
				y2 = topLimit - halfHeight;
			}
			cameraRef.orthographicSize = orthographicSize;
			tr.position = new Vector2(x, y2);
		}
		if (cameraTarget != null && !GameManager.getInstance().isPlaying() && !vehicle.hasCrashed)
		{
			float blend = 0.01f;
			cameraRef.orthographicSize = cameraRef.orthographicSize * (1f - blend) + 12f * blend;
			tr.position = tr.position * (1f - blend) + blend * cameraTarget.position;
			float y4 = tr.position.y + 0.05f;
			Vector3 position = tr.position;
			position.y = y4;
			tr.position = position;
			tr.position = checkLimits(tr.position);
		}
	}

	private Vector3 checkLimits(Vector3 currentPosition)
	{
		float y = maxLimit.y;
		float x = maxLimit.x;
		float y2 = minLimit.y;
		float orthographicSize = cameraRef.orthographicSize;
		float extentY = orthographicSize;
		float extentX = orthographicSize * cameraRef.aspect;
		if (!(currentPosition.x <= x - extentX))
		{
			currentPosition.x = x - extentX;
		}
		if (!(currentPosition.y >= y2 + extentY))
		{
			currentPosition.y = y2 + extentY;
		}
		else if (!(currentPosition.y <= y - extentY))
		{
			currentPosition.y = y - extentY;
		}
		return currentPosition;
	}

	public virtual float calculateDynamicLimit()
	{
		float x = tr.position.x;
		float yLimit = zlevel.getYLimit(x - 8f);
		float yLimit2 = zlevel.getYLimit(x);
		float yLimit3 = zlevel.getYLimit(x + 8f);
		float limit = yLimit * 0.3f + yLimit2 * 0.2f + yLimit3 * 0.5f;
		if (!(limit >= -1000f))
		{
			limit = maxLimit.y;
		}
		float margin = 15f;
		if (!(yLimit3 >= yLimit - 2f))
		{
			margin = 5f;
		}
		if (!(limit >= yLimit + margin))
		{
			limit = yLimit + margin;
		}
		if (!(limit >= yLimit2 + margin))
		{
			limit = yLimit2 + margin;
		}
		return limit;
	}

	public virtual void setLimits(Vector2 topLeft, Vector2 bottomRight)
	{
		minLimit = topLeft;
		maxLimit = bottomRight;
		limitedSize = maxLimit - minLimit;
		dynamicYLimit = calculateDynamicLimit();
	}

	public virtual void setPosition(Vector2 position, float zoom)
	{
		float x = position.x;
		Vector3 position2 = transform.position;
		position2.x = x;
		transform.position = position2;
		float y = position.y;
		Vector3 position3 = transform.position;
		position3.y = y;
		transform.position = position3;
		speedOffset = Vector2.zero;
		cameraRef.orthographicSize = initialViewSize + zoom + initialZoomAdjust + zoomAdjust;
		if (!(cameraRef.orthographicSize <= limitedSize.y * 0.5f))
		{
			cameraRef.orthographicSize = limitedSize.y * 0.5f;
		}
		jumpZoom = 0f;
	}

	public virtual void adjustForTrainLength(int cartNum)
	{
		zoomAdjust = zoomOutPerCart * (float)cartNum;
	}
}
