using System;
using UnityEngine;

[Serializable]
public class WaterLevel : MonoBehaviour
{
	[NonSerialized]
	public static float NOT_DEFINED = -1f;

	[NonSerialized]
	public static float y = NOT_DEFINED;

	public virtual void Awake()
	{
		y = transform.position.y;
	}

	public static bool isDefined()
	{
		return y != NOT_DEFINED;
	}

	public static bool setNotDefined()
	{
		y = NOT_DEFINED;
		return false;
	}
}
