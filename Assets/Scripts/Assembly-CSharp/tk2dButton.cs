using System.Collections;
using UnityEngine;

[AddComponentMenu("2D Toolkit/GUI/tk2dButton")]
public class tk2dButton : MonoBehaviour
{
	public delegate void ButtonHandlerDelegate(tk2dButton source);

	public Camera viewCamera;

	public string buttonDownSprite = "button_down";

	public string buttonUpSprite = "button_up";

	public string buttonPressedSprite = "button_up";

	private int buttonDownSpriteId = -1;

	private int buttonUpSpriteId = -1;

	private int buttonPressedSpriteId = -1;

	public AudioClip buttonDownSound;

	public AudioClip buttonUpSound;

	public AudioClip buttonPressedSound;

	public GameObject targetObject;

	public string messageName = string.Empty;

	private tk2dBaseSprite sprite;

	private bool buttonDown;

	public float targetScale = 1.1f;

	public float scaleTime = 0.05f;

	public float pressedWaitTime = 0.3f;

	public event ButtonHandlerDelegate ButtonPressedEvent;

	public event ButtonHandlerDelegate ButtonAutoFireEvent;

	public event ButtonHandlerDelegate ButtonDownEvent;

	public event ButtonHandlerDelegate ButtonUpEvent;

	private void Start()
	{
		if (viewCamera == null)
		{
			Transform parent = transform;
			while ((bool)parent && parent.GetComponent<Camera>() == null)
			{
				parent = parent.parent;
			}
			if ((bool)parent && parent.GetComponent<Camera>() != null)
			{
				viewCamera = parent.GetComponent<Camera>();
			}
			if (viewCamera == null)
			{
				viewCamera = Camera.main;
			}
		}
		sprite = GetComponent<tk2dBaseSprite>();
		if ((bool)sprite)
		{
			if (buttonDownSprite.Length > 0)
			{
				buttonDownSpriteId = sprite.GetSpriteIdByName(buttonDownSprite);
			}
			if (buttonUpSprite.Length > 0)
			{
				buttonUpSpriteId = sprite.GetSpriteIdByName(buttonUpSprite);
			}
			if (buttonPressedSprite.Length > 0)
			{
				buttonPressedSpriteId = sprite.GetSpriteIdByName(buttonPressedSprite);
			}
		}
		if (GetComponent<Collider>() == null)
		{
			BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
			// NFPort: BoxCollider.extents (half size) is gone; size is the full size.
			Vector3 extents = boxCollider.size * 0.5f;
			extents.z = 0.2f;
			boxCollider.size = extents * 2f;
		}
	}

	private void PlaySound(AudioClip source)
	{
		if ((bool)GetComponent<AudioSource>() && (bool)source)
		{
			GetComponent<AudioSource>().PlayOneShot(source);
		}
	}

	private IEnumerator coScale(Vector3 defaultScale, float startScale, float endScale)
	{
		Vector3 scale = defaultScale;
		float s = 0f;
		while (s < scaleTime)
		{
			float t = Mathf.Clamp01(s / scaleTime);
			float scl = Mathf.Lerp(startScale, endScale, t);
			scale = defaultScale * scl;
			transform.localScale = scale;
			s += Time.deltaTime;
			yield return 0;
		}
		transform.localScale = defaultScale * endScale;
	}

	private IEnumerator coHandleButtonPress()
	{
		buttonDown = true;
		bool buttonPressed = true;
		Vector3 defaultScale = transform.localScale;
		if (targetScale != 1f)
		{
			yield return StartCoroutine(coScale(defaultScale, 1f, targetScale));
		}
		PlaySound(buttonDownSound);
		if (buttonDownSpriteId != -1)
		{
			sprite.spriteId = buttonDownSpriteId;
		}
		while (Input.GetMouseButton(0))
		{
			Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			bool colliderHit = GetComponent<Collider>().Raycast(ray, out hitInfo, 100000000f);
			if (buttonPressed && !colliderHit)
			{
				if (targetScale != 1f)
				{
					yield return StartCoroutine(coScale(defaultScale, targetScale, 1f));
				}
				PlaySound(buttonUpSound);
				if (buttonUpSpriteId != -1)
				{
					sprite.spriteId = buttonUpSpriteId;
				}
				if (ButtonDownEvent != null)
				{
					ButtonDownEvent(this);
				}
				buttonPressed = false;
			}
			else if (!buttonPressed & colliderHit)
			{
				if (targetScale != 1f)
				{
					yield return StartCoroutine(coScale(defaultScale, 1f, targetScale));
				}
				PlaySound(buttonDownSound);
				if (buttonDownSpriteId != -1)
				{
					sprite.spriteId = buttonDownSpriteId;
				}
				if (ButtonUpEvent != null)
				{
					ButtonUpEvent(this);
				}
				buttonPressed = true;
			}
			if (buttonPressed && ButtonAutoFireEvent != null)
			{
				ButtonAutoFireEvent(this);
			}
			yield return 0;
		}
		if (buttonPressed)
		{
			if (targetScale != 1f)
			{
				yield return StartCoroutine(coScale(defaultScale, targetScale, 1f));
			}
			PlaySound(buttonPressedSound);
			if (buttonPressedSpriteId != -1)
			{
				sprite.spriteId = buttonPressedSpriteId;
			}
			if ((bool)targetObject)
			{
				targetObject.SendMessage(messageName);
			}
			if (ButtonUpEvent != null)
			{
				ButtonUpEvent(this);
			}
			if (ButtonPressedEvent != null)
			{
				ButtonPressedEvent(this);
			}
			yield return new WaitForSeconds(pressedWaitTime);
			if (buttonUpSpriteId != -1)
			{
				sprite.spriteId = buttonUpSpriteId;
			}
		}
		buttonDown = false;
	}

	private void Update()
	{
		if (!buttonDown && Input.GetMouseButtonDown(0))
		{
			Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			if (GetComponent<Collider>().Raycast(ray, out hitInfo, 100000000f))
			{
				StartCoroutine(coHandleButtonPress());
			}
		}
	}
}
