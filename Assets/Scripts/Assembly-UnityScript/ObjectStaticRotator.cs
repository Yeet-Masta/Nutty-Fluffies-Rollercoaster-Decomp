using System;
using UnityEngine;

[Serializable]
[ExecuteInEditMode]
public class ObjectStaticRotator : MonoBehaviour
{
	public Vector3 amountPerSecond;

	public ObjectStaticRotator()
	{
		amountPerSecond = new Vector3(0f, 0f, 0f);
	}

	public virtual void Update()
	{
		transform.Rotate(amountPerSecond * Time.deltaTime);
	}

	public virtual void Main()
	{
	}
}
