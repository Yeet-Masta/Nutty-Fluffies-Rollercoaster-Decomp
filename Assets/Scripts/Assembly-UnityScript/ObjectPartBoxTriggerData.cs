using System;
using UnityEngine;

[Serializable]
public class ObjectPartBoxTriggerData : ObjectPartTriggerData
{
	public Vector2 size;

	public ObjectPartBoxTriggerData(Rect bounds, float rotation)
		: base(5, new Vector2(bounds.x, bounds.y), rotation)
	{
		size = new Vector2(bounds.width, bounds.height);
	}
}
