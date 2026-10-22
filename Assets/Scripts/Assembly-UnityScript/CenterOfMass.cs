using System;
using UnityEngine;

[Serializable]
public class CenterOfMass : MonoBehaviour
{
	public Vector3 position;

	public virtual void Start()
	{
		if (GetComponent<Rigidbody>() != null)
		{
			GetComponent<Rigidbody>().centerOfMass = position;
		}
	}

	public virtual void Main()
	{
	}
}
