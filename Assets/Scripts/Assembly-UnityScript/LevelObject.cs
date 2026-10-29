using UnityEngine;

public class LevelObject : LevelEntity
{
	public int objectId = 1;

	public Vector3 initialPosition = Vector3.zero;

	public virtual void setZstep(int zStep)
	{
		float z = 1f * (float)zStep;
		Vector3 localPosition = transform.localPosition;
		localPosition.z = z;
		transform.localPosition = localPosition;
	}

	public virtual void resetPosition(LevelLayer onLayer)
	{
		Transform transform = onLayer ? onLayer.transform : gameObject.transform.parent;
		float x = initialPosition.x * transform.localScale.x + transform.position.x;
		Vector3 position = this.transform.position;
		position.x = x;
		this.transform.position = position;
		float y = initialPosition.y * transform.localScale.y + transform.position.y;
		Vector3 position2 = this.transform.position;
		position2.y = y;
		this.transform.position = position2;
		float z = initialPosition.z;
		Vector3 eulerAngles = this.transform.eulerAngles;
		eulerAngles.z = z;
		this.transform.eulerAngles = eulerAngles;
	}

	public virtual void savePosition(LevelLayer onLayer)
	{
		Transform transform = onLayer ? onLayer.transform : gameObject.transform.parent;
		initialPosition.x = (this.transform.position.x - transform.position.x) * (1f / transform.localScale.x);
		initialPosition.y = (this.transform.position.y - transform.position.y) * (1f / transform.localScale.y);
		initialPosition.z = this.transform.eulerAngles.z;
	}
}
