using UnityEngine;
using UnityScript.Lang;

public class TouchScreenController : Controller
{
	private bool usingSpecial;

	private int specialTouchId = -1;

	private int swipeDetectionRange;

	public override void Awake()
	{
		base.Awake();
		swipeDetectionRange = Screen.width / 25;
		swipeDetectionRange *= swipeDetectionRange;
	}

	public override void updateGameControls()
	{
		setAcceleratePressed(false);
		setDeacceleratePressed(false);
		if (Extensions.get_length((System.Array)Input.touches) == 0)
		{
			return;
		}
		int i = 0;
		Touch[] touches = Input.touches;
		for (int length = touches.Length; i < length; i++)
		{
			float magnitude = touches[i].deltaPosition.magnitude;
			if (touches[i].phase == TouchPhase.Moved)
			{
				Vehicle vehicle = GameManager.getInstance().getVehicle();
				Transform transform = GameManager.getInstance().getVehicle().getMiddleCart()
					.gameObject.transform;
				Vector3 right = transform.right;
				Vector2 lhs = new Vector2(right.x, right.y);
				float num = Vector2.Dot(lhs, touches[i].deltaPosition);
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
				if (usingSpecial && specialTouchId == touches[i].fingerId && !(touches[i].deltaPosition.sqrMagnitude <= (float)swipeDetectionRange))
				{
					CancelInvoke("useSpecial");
					usingSpecial = false;
					specialTouchId = -1;
				}
				lastTouchDirection = touches[i].deltaPosition;
				lastTouchDirection.Normalize();
			}
			else if (isTouchInsideSpecialArea(touches[i].position) && touches[i].phase == TouchPhase.Began)
			{
				special(touches[i].fingerId);
			}
		}
	}

	private void special(int fingerId)
	{
		usingSpecial = true;
		specialTouchId = fingerId;
		if (!TutorialManager.isTutorialEnable && !PauseMenu.isPauseMenuEnable)
		{
			Invoke("useSpecial", 0.075f);
			return;
		}
		CancelInvoke("useSpecial");
		usingSpecial = false;
		specialTouchId = -1;
	}

	public virtual void useSpecial()
	{
		GameManager.getInstance().useSpecial();
		usingSpecial = false;
		specialTouchId = -1;
	}

	public override bool isCurrentControlTouchBased()
	{
		return true;
	}
}
