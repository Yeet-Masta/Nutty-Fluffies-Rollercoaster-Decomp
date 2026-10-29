using UnityEngine;

public class Controller : MonoBehaviour
{
	public float throttle;

	public float brake;

	public float throttleSensitivity = 0.2f;

	public float brakeSensitivity = 0.05f;

	public float returnSensitivity = 0.2f;

	public float centerSize = 0.1f;

	private bool acceleratePressed = true;

	private bool deacceleratePressed = true;

	protected Vector3 lastTouchDirection = new Vector3(0f, 0f, 0f);

	public virtual void Awake()
	{
		reset();
		useGUILayout = false;
	}

	public virtual void reset()
	{
		throttle = 0f;
		brake = 0f;
		acceleratePressed = false;
		deacceleratePressed = false;
	}

	public virtual void setAcceleratePressed(bool to)
	{
		acceleratePressed = to;
	}

	protected virtual void setDeacceleratePressed(bool to)
	{
		deacceleratePressed = to;
	}

	private void accelerate()
	{
		throttle += (1f - throttle) * throttleSensitivity;
		brake = 0f;
	}

	private void deaccelerate()
	{
		brake += (1f - brake) * brakeSensitivity;
		throttle = 0f;
	}

	public virtual void resetGame()
	{
		GameManager.getInstance().restart();
	}

	public virtual void endGame()
	{
		GameManager.getInstance().endGame();
	}

	public virtual void resetToCheckPoint()
	{
	}

	public virtual void FixedUpdate()
	{
		if (GameManager.getInstance().isControllingEnabled())
		{
			if (acceleratePressed)
			{
				accelerate();
				return;
			}
			if (deacceleratePressed)
			{
				deaccelerate();
				return;
			}
			throttle *= 1f - returnSensitivity;
			brake *= 1f - returnSensitivity;
			if (!(throttle >= centerSize))
			{
				throttle = 0f;
			}
			if (!(brake >= centerSize))
			{
				brake = 0f;
			}
		}
		else
		{
			reset();
			brake = 4f;
		}
	}

	public virtual void Update()
	{
		if (GameManager.getInstance().isControllingEnabled())
		{
			updateGameControls();
		}
		updateGenericControls();
	}

	public virtual void updateGameControls()
	{
	}

	public virtual void updateGenericControls()
	{
	}

	public virtual bool isCurrentControlTouchBased()
	{
		return false;
	}

	public virtual Vector3 getLastTouchDirection()
	{
		return lastTouchDirection;
	}

	public virtual bool isTouchInsideSpecialArea(Vector2 pos)
	{
		return pos.y < (float)Screen.height * 0.25f && pos.x < (float)Screen.width * 0.3f;
	}
}
