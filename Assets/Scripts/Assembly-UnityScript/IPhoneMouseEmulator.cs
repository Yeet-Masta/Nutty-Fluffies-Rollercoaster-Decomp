using UnityEngine;

public class IPhoneMouseEmulator : MonoBehaviour
{
	public float rayDepth;

	public int layerMaskOfColliders;

	public bool simulateDraggingWithUpdateRays;

	public bool simulateDraggingWith3DRays;

	public bool fixedUpdateDragging;

	public bool updateOnlyOnMobile;

	public Camera cameraRef;

	public GameObject pressedObject;

	public Transform markerLeftTop;

	public Transform markerRightBottom;

	private Vector2 lastMousePosition;

	private Vector2 lastMouseUpPosition;

	private Vector2 lastMouseDownPosition;

	private Vector3 sim3DFrom;

	private Vector3 sim3DTo;

	public IPhoneMouseEmulator()
	{
		rayDepth = 100f;
		layerMaskOfColliders = -1;
		fixedUpdateDragging = true;
		sim3DFrom = default(Vector3);
		sim3DTo = default(Vector3);
	}

	public virtual void Awake()
	{
		lastMousePosition = Input.mousePosition;
		lastMouseUpPosition = Input.mousePosition;
		lastMouseDownPosition = Input.mousePosition;
		pressedObject = null;
		if (cameraRef == null)
		{
			cameraRef = Camera.main;
		}
	}

	public virtual void Update()
	{
		if (updateOnlyOnMobile && Application.platform != RuntimePlatform.IPhonePlayer && Application.platform != RuntimePlatform.Android)
		{
			return;
		}
		if (Input.GetMouseButtonDown(0))
		{
			simulateMouseDownAt(Input.mousePosition);
		}
		else
		{
			if (Input.GetMouseButtonUp(0))
			{
				simulateMouseUpAt(lastMousePosition);
			}
			if (!fixedUpdateDragging)
			{
				simulateDragging();
			}
		}
		lastMousePosition = Input.mousePosition;
	}

	public virtual void FixedUpdate()
	{
		if (fixedUpdateDragging)
		{
			simulateDragging();
		}
	}

	private void simulateDragging()
	{
		if (Input.GetMouseButton(0))
		{
			if (simulateDraggingWithUpdateRays)
			{
				simulateMouseDragAt(Input.mousePosition);
			}
			else if (simulateDraggingWith3DRays)
			{
				simulateMouseDrag3DRay(lastMousePosition, Input.mousePosition);
			}
		}
	}

	public virtual void simulateMouseDragAt(Vector2 point)
	{
		RayCastTools.ifHitInvokeNearest(cameraRef.ScreenPointToRay(point), rayDepth, "OnMouseDrag", layerMaskOfColliders);
	}

	public virtual void simulateMouseUpAt(Vector2 point)
	{
		lastMouseUpPosition = point;
		if (pressedObject != null)
		{
			pressedObject.SendMessage("OnMouseUp", point, SendMessageOptions.DontRequireReceiver);
			pressedObject = null;
		}
		else
		{
			RayCastTools.ifHitInvokeNearest(cameraRef.ScreenPointToRay(point), rayDepth, "OnMouseUp", layerMaskOfColliders);
		}
	}

	public virtual void simulateMouseDownAt(Vector2 point)
	{
		if (pressedObject == null)
		{
			lastMouseDownPosition = point;
			pressedObject = RayCastTools.ifHitInvokeNearest(cameraRef.ScreenPointToRay(point), rayDepth, "OnMouseDown", layerMaskOfColliders);
		}
	}

	public virtual void simulateMouseDrag3DRay(Vector2 from2, Vector2 to2)
	{
		sim3DFrom = get3DCoordinates(from2);
		sim3DTo = get3DCoordinates(to2);
		sim3DTo.x -= sim3DFrom.x;
		sim3DTo.y -= sim3DFrom.y;
		sim3DTo.z -= sim3DFrom.z;
		simulateMouseDrag3DRay(new Ray(sim3DFrom, sim3DTo), sim3DTo.magnitude);
	}

	private void simulateMouseDrag3DRay(Ray ray, float depth)
	{
		if (depth != 0f)
		{
			RayCastTools.ifHitInvokeNearest(ray, depth, "OnMouseDrag", layerMaskOfColliders);
		}
	}

	private Vector3 get3DCoordinates(Vector2 from)
	{
		from = HUDUVTools.fromScreenCoordinatesToHUDUV(from, new Vector2(Screen.width, Screen.height), true);
		Vector3 position = markerLeftTop.position;
		Vector3 position2 = markerRightBottom.position;
		return new Vector3((position2.x - position.x) * from.x + position.x, position.y, (position2.z - position.z) * from.y + position.z);
	}

	public virtual Vector2 getLastMousePosition()
	{
		return lastMousePosition;
	}

	public virtual Vector2 getLastMouseUpPosition()
	{
		return lastMouseUpPosition;
	}

	public virtual Vector2 getLastMouseDownPosition()
	{
		return lastMouseDownPosition;
	}
}
