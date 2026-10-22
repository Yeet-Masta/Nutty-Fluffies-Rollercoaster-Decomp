using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
[ExecuteInEditMode]
public class ObjectRotatorBillboard : MonoBehaviour
{
	private Camera cam;

	public Vector3 rotateBias;

	public virtual void Awake()
	{
		object[] array = (Camera[])UnityEngine.Object.FindObjectsOfType(typeof(Camera));
		if (Extensions.get_length((System.Array)array) > 0)
		{
			cam = array[0] as Camera;
		}
	}

	public virtual void OnBecameVisible()
	{
		enabled = true;
	}

	public virtual void OnBecameInvisible()
	{
		enabled = false;
	}

	public virtual void Update()
	{
		if (cam != null)
		{
			MonoBehaviour.print("billboard rotating");
			transform.rotation = Quaternion.identity;
			transform.LookAt(cam.transform.forward + transform.position, transform.up);
			if (rotateBias.x != 0f)
			{
				transform.Rotate(Vector3.left * rotateBias.x);
			}
			if (rotateBias.y != 0f)
			{
				transform.Rotate(Vector3.forward * rotateBias.y);
			}
			if (rotateBias.z != 0f)
			{
				transform.Rotate(Vector3.up * rotateBias.z);
			}
		}
	}

	public virtual void Main()
	{
	}
}
