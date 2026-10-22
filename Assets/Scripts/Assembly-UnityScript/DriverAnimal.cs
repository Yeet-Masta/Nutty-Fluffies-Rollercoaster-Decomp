using System;
using UnityEngine;

[Serializable]
public class DriverAnimal : MonoBehaviour
{
	public GameObject headNormal;

	public GameObject headFail;

	public Transform tr;

	public virtual void Awake()
	{
		headFail.SetActiveRecursively(false);
	}

	public virtual void onCrash()
	{
		headNormal.SetActiveRecursively(false);
		headFail.SetActiveRecursively(true);
	}
}
