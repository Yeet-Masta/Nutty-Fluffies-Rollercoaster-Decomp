using UnityEngine;

[AddComponentMenu("FingerGestures/Toolbox/Input Manager")]
public class TBInputManager : MonoBehaviour
{
	public enum DragPlaneType
	{
		XY = 0,
		XZ = 1,
		ZY = 2,
		UseCollider = 3
	}

	public bool trackFingerUp = true;

	public bool trackFingerDown = true;

	public bool trackDrag = true;

	public bool trackTap = true;

	public bool trackLongPress = true;

	public bool trackSwipe = true;

	public Camera raycastCamera;

	public LayerMask ignoreLayers = 0;

	public DragPlaneType dragPlaneType;

	public Collider dragPlaneCollider;

	public float dragPlaneOffset;

	private void Start()
	{
		if (!raycastCamera)
		{
			raycastCamera = Camera.main;
		}
	}

	private void OnEnable()
	{
		if (trackFingerDown)
		{
			FingerGestures.OnFingerDown += FingerGestures_OnFingerDown;
		}
		if (trackFingerUp)
		{
			FingerGestures.OnFingerUp += FingerGestures_OnFingerUp;
		}
		if (trackDrag)
		{
			FingerGestures.OnDragBegin += FingerGestures_OnDragBegin;
		}
		if (trackTap)
		{
			FingerGestures.OnTap += FingerGestures_OnTap;
		}
		if (trackLongPress)
		{
			FingerGestures.OnLongPress += FingerGestures_OnLongPress;
		}
		if (trackSwipe)
		{
			FingerGestures.OnSwipe += FingerGestures_OnSwipe;
		}
	}

	private void OnDisable()
	{
		FingerGestures.OnFingerDown -= FingerGestures_OnFingerDown;
		FingerGestures.OnFingerUp -= FingerGestures_OnFingerUp;
		FingerGestures.OnDragBegin -= FingerGestures_OnDragBegin;
		FingerGestures.OnTap -= FingerGestures_OnTap;
		FingerGestures.OnLongPress -= FingerGestures_OnLongPress;
		FingerGestures.OnSwipe -= FingerGestures_OnSwipe;
	}

	private void FingerGestures_OnFingerUp(int fingerIndex, Vector2 fingerPos, float timeHeldDown)
	{
		TBFingerUp tBFingerUp = PickComponent<TBFingerUp>(fingerPos);
		if ((bool)tBFingerUp)
		{
			tBFingerUp.RaiseFingerUp(fingerIndex, fingerPos, timeHeldDown);
		}
	}

	private void FingerGestures_OnFingerDown(int fingerIndex, Vector2 fingerPos)
	{
		TBFingerDown tBFingerDown = PickComponent<TBFingerDown>(fingerPos);
		if ((bool)tBFingerDown)
		{
			tBFingerDown.RaiseFingerDown(fingerIndex, fingerPos);
		}
	}

	private void FingerGestures_OnDragBegin(int fingerIndex, Vector2 fingerPos, Vector2 startPos)
	{
		TBDrag tBDrag = PickComponent<TBDrag>(startPos);
		if ((bool)tBDrag && !tBDrag.Dragging)
		{
			tBDrag.BeginDrag(fingerIndex, fingerPos);
			tBDrag.OnDragMove += draggable_OnDragMove;
			tBDrag.OnDragEnd += draggable_OnDragEnd;
		}
	}

	private bool ProjectScreenPointOnDragPlane(Transform dragObj, Vector2 screenPos, out Vector3 worldPos)
	{
		worldPos = dragObj.position;
		switch (dragPlaneType)
		{
		case DragPlaneType.XY:
			worldPos = raycastCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Mathf.Abs(dragObj.position.z - raycastCamera.transform.position.z)));
			return true;
		case DragPlaneType.XZ:
			worldPos = raycastCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Mathf.Abs(dragObj.position.y - raycastCamera.transform.position.y)));
			return true;
		case DragPlaneType.ZY:
			worldPos = raycastCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, Mathf.Abs(dragObj.position.x - raycastCamera.transform.position.x)));
			return true;
		case DragPlaneType.UseCollider:
		{
			Ray ray = raycastCamera.ScreenPointToRay(screenPos);
			RaycastHit hitInfo;
			if (!dragPlaneCollider.Raycast(ray, out hitInfo, float.MaxValue))
			{
				return false;
			}
			worldPos = hitInfo.point + dragPlaneOffset * hitInfo.normal;
			return true;
		}
		default:
			return false;
		}
	}

	private void draggable_OnDragMove(TBDrag sender)
	{
		Vector3 worldPos;
		if (ProjectScreenPointOnDragPlane(sender.transform, sender.FingerPos, out worldPos))
		{
			sender.transform.position = worldPos;
		}
	}

	private void draggable_OnDragEnd(TBDrag source)
	{
		source.OnDragMove -= draggable_OnDragMove;
		source.OnDragEnd -= draggable_OnDragEnd;
	}

	private void FingerGestures_OnTap(int fingerIndex, Vector2 fingerPos, int tapCount)
	{
		TBTap tBTap = PickComponent<TBTap>(fingerPos);
		if ((bool)tBTap)
		{
			tBTap.RaiseTap(fingerIndex, fingerPos, tapCount);
		}
	}

	private void FingerGestures_OnLongPress(int fingerIndex, Vector2 fingerPos)
	{
		TBLongPress tBLongPress = PickComponent<TBLongPress>(fingerPos);
		if ((bool)tBLongPress)
		{
			tBLongPress.RaiseLongPress(fingerIndex, fingerPos);
		}
	}

	private void FingerGestures_OnSwipe(int fingerIndex, Vector2 startPos, FingerGestures.SwipeDirection direction, float velocity)
	{
		TBSwipe tBSwipe = PickComponent<TBSwipe>(startPos);
		if ((bool)tBSwipe)
		{
			tBSwipe.RaiseSwipe(fingerIndex, startPos, direction, velocity);
		}
	}

	private GameObject PickObject(Vector2 screenPos)
	{
		Ray ray = Camera.main.ScreenPointToRay(screenPos);
		RaycastHit hitInfo;
		if (Physics.Raycast(ray, out hitInfo, float.MaxValue, ~(int)ignoreLayers))
		{
			return hitInfo.collider.gameObject;
		}
		return null;
	}

	private T PickComponent<T>(Vector2 screenPos) where T : TBComponent
	{
		GameObject gameObject = PickObject(screenPos);
		if (!gameObject)
		{
			return (T)null;
		}
		return gameObject.GetComponent<T>();
	}
}
