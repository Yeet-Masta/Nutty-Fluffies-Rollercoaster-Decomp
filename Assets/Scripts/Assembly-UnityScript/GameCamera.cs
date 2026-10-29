using UnityEngine;

public class GameCamera : Camera2D
{
	public Transform cameraTarget;

	public Vector2 viewOffset;

	public float viewStiffness;

	public float zoomStiffness;

	public float speedZoomFactor;

	public float speedOffsetFactor;

	public float offsetStiffness;

	public float zoomOutPerCart;

	public bool useDynamicY;

	public float minOffsetAboveTarget;

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

	public GameCamera()
	{
		viewStiffness = 0.1f;
		zoomStiffness = 0.01f;
		speedZoomFactor = 1.1f;
		speedOffsetFactor = 1.5f;
		offsetStiffness = 0.01f;
		zoomOutPerCart = 0.3f;
		useDynamicY = true;
		minOffsetAboveTarget = 4f;
	}

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
			float num = magnitude * speedZoomFactor;
			float orthographicSize = cameraRef.orthographicSize;
			orthographicSize += (initialViewSize + initialZoomAdjust + zoomAdjust + num + jumpZoom - orthographicSize) * zoomStiffness;
			if (!(orthographicSize <= limitedSize.y * 0.5f))
			{
				orthographicSize = limitedSize.y * 0.5f;
			}
			Vector3 vector = cameraTarget.right * speedOffsetFactor * magnitude;
			speedOffset += (Vector2)((vector - (Vector3)speedOffset) * offsetStiffness);
			if (vehicle != null && !cart.sqHeightAvailable && vehicle.isFlying)
			{
				speedOffset.y -= jumpZoom * 0.05f;
			}
			halfHeight = orthographicSize;
			halfWidth = orthographicSize * cameraRef.aspect;
			float y = cameraTarget.position.y;
			float num2 = halfHeight * 2f;
			float x = tr.position.x;
			float y2 = tr.position.y;
			float num3 = cameraTarget.position.x + viewOffset.x + speedOffset.x;
			float num4 = y + viewOffset.y + speedOffset.y;
			x += (num3 - x) * viewStiffness;
			y2 += (num4 - y2) * viewStiffness;
			float num5 = 3.5f;
			float x2 = minLimit.x;
			float num6 = lastCart.position.x - num5;
			if (!(num6 >= x2))
			{
				num6 = x2;
			}
			num6 += 2f * halfWidth;
			if (!(num6 <= maxLimit.x))
			{
				num6 = maxLimit.x;
			}
			if (useDynamicY)
			{
				float num7 = calculateDynamicLimit();
				dynamicYLimit += (num7 - dynamicYLimit) * 0.01f;
				if (!(dynamicYLimit >= y + minOffsetAboveTarget))
				{
					dynamicYLimit = y + minOffsetAboveTarget;
				}
			}
			float y3 = lastCart.position.y;
			float num8 = dynamicYLimit;
			float num9 = y3 - num2 + num5;
			if (!(num8 <= y3 + num2 - num5))
			{
				num8 = y3 + num2 - num5;
			}
			if (!(num8 <= maxLimit.y))
			{
				num8 = maxLimit.y;
				if (!(num9 <= num8 - num2))
				{
					num9 = num8 - num2;
				}
			}
			if (!(num9 >= minLimit.y))
			{
				num9 = minLimit.y;
			}
			if (!(num8 + num2 >= num9))
			{
				num8 = num9 + num2;
			}
			float num10 = y3 + num2 - num5;
			if (!(num8 <= num10))
			{
				num8 = num10;
			}
			if (!(num8 >= num9 + num2))
			{
				num8 = num9 + num2;
			}
			if (!(x >= x2 + halfWidth))
			{
				x = x2 + halfWidth;
			}
			else if (!(x <= num6 - halfWidth))
			{
				x = num6 - halfWidth;
			}
			if (!(y2 >= num9 + halfHeight))
			{
				y2 = num9 + halfHeight;
			}
			else if (!(y2 <= num8 - halfHeight))
			{
				y2 = num8 - halfHeight;
			}
			cameraRef.orthographicSize = orthographicSize;
			tr.position = new Vector2(x, y2);
		}
		if (cameraTarget != null && !GameManager.getInstance().isPlaying() && !vehicle.hasCrashed)
		{
			float num11 = 12f;
			float num12 = 0.01f;
			cameraRef.orthographicSize = cameraRef.orthographicSize * (1f - num12) + num11 * num12;
			tr.position = tr.position * (1f - num12) + num12 * cameraTarget.position;
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
		float num = orthographicSize;
		float num2 = orthographicSize * cameraRef.aspect;
		if (!(currentPosition.x <= x - num2))
		{
			currentPosition.x = x - num2;
		}
		if (!(currentPosition.y >= y2 + num))
		{
			currentPosition.y = y2 + num;
		}
		else if (!(currentPosition.y <= y - num))
		{
			currentPosition.y = y - num;
		}
		return currentPosition;
	}

	public virtual float calculateDynamicLimit()
	{
		float x = tr.position.x;
		float yLimit = zlevel.getYLimit(x - 8f);
		float yLimit2 = zlevel.getYLimit(x);
		float yLimit3 = zlevel.getYLimit(x + 8f);
		float num = yLimit * 0.3f + yLimit2 * 0.2f + yLimit3 * 0.5f;
		if (!(num >= -1000f))
		{
			num = maxLimit.y;
		}
		float num2 = 15f;
		if (!(yLimit3 >= yLimit - 2f))
		{
			num2 = 5f;
		}
		if (!(num >= yLimit + num2))
		{
			num = yLimit + num2;
		}
		if (!(num >= yLimit2 + num2))
		{
			num = yLimit2 + num2;
		}
		return num;
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
