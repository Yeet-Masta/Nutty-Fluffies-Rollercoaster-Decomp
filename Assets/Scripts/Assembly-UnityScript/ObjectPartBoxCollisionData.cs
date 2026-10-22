using System;
using UnityEngine;

[Serializable]
public class ObjectPartBoxCollisionData : ObjectPartData
{
	public Vector2 size;

	public ObjectPartBoxCollisionData(Rect bounds, float rotation)
		: base(PartType.PART_BOX_COLLISION, new Vector2(bounds.x, bounds.y), rotation)
	{
		size = new Vector2(bounds.width, bounds.height);
	}
}
