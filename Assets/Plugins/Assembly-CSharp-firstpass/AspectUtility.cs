using UnityEngine;

public class AspectUtility : MonoBehaviour
{
	public float _wantedAspectRatio = 1.3333333f;

	private float wantedAspectRatio;

	private Camera cam;

	private Camera backgroundCam;

	public Camera attachedCamera;

	private float oldHeight;

	private float oldWidth;

	public static int screenHeight
	{
		get
		{
			return (int)((float)Screen.height * Camera.main.GetComponent<AspectUtility>().cam.rect.height);
		}
	}

	public static int screenWidth
	{
		get
		{
			return (int)((float)Screen.width * Camera.main.GetComponent<AspectUtility>().cam.rect.width);
		}
	}

	public static Vector3 mousePosition
	{
		get
		{
			Vector3 result = Input.mousePosition;
			result.y -= (int)(Camera.main.GetComponent<AspectUtility>().cam.rect.y * (float)Screen.height);
			result.x -= (int)(Camera.main.GetComponent<AspectUtility>().cam.rect.x * (float)Screen.width);
			result.y += Camera.main.GetComponent<AspectUtility>().cam.pixelRect.y;
			result.x += Camera.main.GetComponent<AspectUtility>().cam.pixelRect.x;
			return result;
		}
	}

	private void Awake()
	{
		cam = attachedCamera;
		if (!cam)
		{
			cam = Camera.main;
		}
		if (!cam)
		{
			Debug.LogError("No camera available");
			return;
		}
		wantedAspectRatio = _wantedAspectRatio;
		SetCamera();
	}

	public void SetCamera()
	{
		cam = attachedCamera;
		if (!cam)
		{
			cam = Camera.main;
		}
		float num = (float)Screen.width / (float)Screen.height;
		if ((float)(int)(num * 100f) / 100f == (float)(int)(wantedAspectRatio * 100f) / 100f)
		{
			cam.rect = new Rect(0f, 0f, 1f, 1f);
			if ((bool)backgroundCam)
			{
				Object.Destroy(backgroundCam.gameObject);
			}
			return;
		}
		if (num > wantedAspectRatio)
		{
			float num2 = 1f - wantedAspectRatio / num;
			cam.rect = new Rect(num2 / 2f, 0f, 1f - num2, 1f);
		}
		else
		{
			float num3 = 1f - num / wantedAspectRatio;
			cam.rect = new Rect(0f, num3 / 2f, 1f, 1f - num3);
		}
		if (!backgroundCam)
		{
			backgroundCam = new GameObject("BackgroundCam", typeof(Camera)).GetComponent<Camera>();
			backgroundCam.depth = -2.1474836E+09f;
			backgroundCam.clearFlags = CameraClearFlags.Color;
			backgroundCam.backgroundColor = Color.black;
			backgroundCam.cullingMask = 0;
		}
	}

	public void setRatio(float ratio)
	{
		wantedAspectRatio = ratio;
		_wantedAspectRatio = ratio;
		SetCamera();
	}

	public void Update()
	{
		if ((float)Screen.height != oldHeight || (float)Screen.width != oldWidth)
		{
			SetCamera();
			oldWidth = Screen.width;
			oldHeight = Screen.height;
		}
	}
}
