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
		float x = lockX ? transform.localEulerAngles.x : theCamera.transform.localEulerAngles.x;
		float y = lockY ? transform.localEulerAngles.y : theCamera.transform.localEulerAngles.y;
		float z = lockZ ? transform.localEulerAngles.z : theCamera.transform.localEulerAngles.z;
		transform.localEulerAngles = new Vector3(x, y, z);
	}
}
