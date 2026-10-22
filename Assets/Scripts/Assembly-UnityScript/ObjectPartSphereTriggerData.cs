using System;
using UnityEngine;

[Serializable]
public class ObjectPartSphereTriggerData : ObjectPartTriggerData
{
	public float radius;

	public ObjectPartSphereTriggerData(Vector2 origin, float radius)
		: base(6, origin, 0f)
	{
		this.radius = radius;
	}
}
