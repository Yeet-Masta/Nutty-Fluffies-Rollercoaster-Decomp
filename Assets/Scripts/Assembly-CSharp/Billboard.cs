using UnityEngine;

public class Billboard : MonoBehaviour
{
	public Camera theCamera;

	public bool lockX = true;

	public bool lockY;

	public bool lockZ = true;

	private void Start()
	{
		if (theCamera == null)
		{
			theCamera = Camera.main;
		}
	}

	private void Update()
	{
		float x = ((!lockX) ? theCamera.transform.localEulerAngles.x : transform.localEulerAngles.x);
		float y = ((!lockY) ? theCamera.transform.localEulerAngles.y : transform.localEulerAngles.y);
		float z = ((!lockZ) ? theCamera.transform.localEulerAngles.z : transform.localEulerAngles.z);
		transform.localEulerAngles = new Vector3(x, y, z);
	}
}
