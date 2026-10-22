using System;
using UnityEngine;

[Serializable]
public class LevelObject : LevelEntity
{
	public int objectId;

	public Vector3 initialPosition;

	public LevelObject()
	{
		objectId = 1;
		initialPosition = Vector3.zero;
	}

	public virtual void setZstep(int zStep)
	{
		float z = 1f * (float)zStep;
		Vector3 localPosition = transform.localPosition;
		float num = (localPosition.z = z);
		Vector3 vector = (transform.localPosition = localPosition);
	}

	public virtual void resetPosition(LevelLayer onLayer)
	{
		Transform transform = ((!onLayer) ? gameObject.transform.parent : onLayer.transform);
		float x = initialPosition.x * transform.localScale.x + transform.position.x;
		Vector3 position = this.transform.position;
		float num = (position.x = x);
		Vector3 vector = (this.transform.position = position);
		float y = initialPosition.y * transform.localScale.y + transform.position.y;
		Vector3 position2 = this.transform.position;
		float num2 = (position2.y = y);
		Vector3 vector3 = (this.transform.position = position2);
		float z = initialPosition.z;
		Vector3 eulerAngles = this.transform.eulerAngles;
		float num3 = (eulerAngles.z = z);
		Vector3 vector5 = (this.transform.eulerAngles = eulerAngles);
	}

	public virtual void savePosition(LevelLayer onLayer)
	{
		Transform transform = ((!onLayer) ? gameObject.transform.parent : onLayer.transform);
		initialPosition.x = (this.transform.position.x - transform.position.x) * (1f / transform.localScale.x);
		initialPosition.y = (this.transform.position.y - transform.position.y) * (1f / transform.localScale.y);
		initialPosition.z = this.transform.eulerAngles.z;
	}
}
