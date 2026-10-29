using UnityEngine;

public class KeyboardController : Controller
{
	private Vector2 lastMousePos = new Vector2(0f, 0f);

	private Vector2 mouseDelta = new Vector2(0f, 0f);

	private bool lastControlWasSwipe;

	private bool usingSpecial;

	public override void Awake()
	{
		base.Awake();
		lastMousePos = Input.mousePosition;
	}

	public override void updateGameControls()
	{
		setAcceleratePressed(false);
		setDeacceleratePressed(false);
		Vehicle vehicle = GameManager.getInstance().getVehicle();
		if (vehicle == null)
		{
			return;
		}
		if (Input.GetMouseButton(0))
		{
			Transform transform = GameManager.getInstance().getVehicle().getMiddleCart()
				.gameObject.transform;
			Vector3 right = transform.right;
			Vector2 lhs = new Vector2(right.x, right.y);
			float num = Vector2.Dot(lhs, mouseDelta);
			float magnitude = mouseDelta.magnitude;
			if (num > 0f || (vehicle.isInLoop() && !vehicle.isReversing()))
			{
				throttle = Mathf.Min(1f, magnitude * 0.1f);
				brake = 0f;
			}
			else if (!(num >= 0f))
			{
				brake = Mathf.Min(1f, magnitude * 0.1f);
				throttle = 0f;
			}
			lastControlWasSwipe = true;
			lastTouchDirection = mouseDelta;
			lastTouchDirection.Normalize();
			if (usingSpecial && !(mouseDelta.sqrMagnitude <= 150f))
			{
				CancelInvoke("useSpecial");
				usingSpecial = false;
			}
		}
		if (Input.GetMouseButtonDown(0) && isTouchInsideSpecialArea(Input.mousePosition))
		{
			usingSpecial = true;
			if (!TutorialManager.isTutorialEnable && !PauseMenu.isPauseMenuEnable)
			{
				Invoke("useSpecial", 0.075f);
				return;
			}
			CancelInvoke("useSpecial");
			usingSpecial = false;
		}
	}

	public virtual void useSpecial()
	{
		GameManager.getInstance().useSpecial();
		usingSpecial = false;
	}

	public override void updateGenericControls()
	{
		mouseDelta = (Input.mousePosition - (Vector3)lastMousePos) / Time.deltaTime / 30f;
		lastMousePos = Input.mousePosition;
	}

	public override bool isCurrentControlTouchBased()
	{
		return lastControlWasSwipe;
	}
}
