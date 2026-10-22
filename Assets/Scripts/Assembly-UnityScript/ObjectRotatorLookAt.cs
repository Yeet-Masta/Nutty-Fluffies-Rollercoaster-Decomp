using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
[ExecuteInEditMode]
public class ObjectRotatorLookAt : CachedMonoBehaviour
{
	public Transform[] centerTo;

	public Transform lookAt;

	public Transform lookUp;

	public Vector3 rotateBias;

	private Vector3 center;

	public ObjectRotatorLookAt()
	{
		center = default(Vector3);
	}

	public virtual void Update()
	{
		center.x = 0f;
		center.y = 0f;
		center.z = 0f;
		for (int i = 0; i < Extensions.get_length((System.Array)centerTo); i++)
		{
			center += centerTo[i].position;
		}
		center /= (float)Extensions.get_length((System.Array)centerTo);
		cTransform.position = center;
		cTransform.rotation = Quaternion.identity;
		Vector3 worldUp = ((!(lookUp != null)) ? cTransform.up : lookUp.up);
		cTransform.LookAt(lookAt.position, worldUp);
		if (rotateBias.x != 0f)
		{
			cTransform.Rotate(Vector3.left * rotateBias.x);
		}
		if (rotateBias.y != 0f)
		{
			cTransform.Rotate(Vector3.forward * rotateBias.y);
		}
		if (rotateBias.z != 0f)
		{
			cTransform.Rotate(Vector3.up * rotateBias.z);
		}
	}

	public virtual void Main()
	{
	}
}
