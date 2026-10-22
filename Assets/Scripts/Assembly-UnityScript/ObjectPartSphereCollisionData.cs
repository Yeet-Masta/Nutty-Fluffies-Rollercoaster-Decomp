using System;
using UnityEngine;

[Serializable]
public class ObjectPartSphereCollisionData : ObjectPartData
{
	public float radius;

	public ObjectPartSphereCollisionData(Vector2 origin, float radius)
		: base(PartType.PART_SPHERE_COLLISION, origin, 0f)
	{
		this.radius = radius;
	}
}
