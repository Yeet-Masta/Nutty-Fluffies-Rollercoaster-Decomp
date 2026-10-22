using System;
using UnityEngine;

[Serializable]
[ExecuteInEditMode]
public class ObjectScalerWobble : CachedMonoBehaviour
{
	private Vector3 origScale;

	public Vector3 frequency;

	public Vector3 amount;

	public Vector3 offset;

	public ObjectScalerWobble()
	{
		frequency = new Vector3(1f, 1f, 1f);
		amount = new Vector3(0f, 0f, 0f);
		offset = new Vector3(0f, 0f, 0f);
	}

	public override void Awake()
	{
		origScale = transform.localScale;
	}

	public virtual void OnBecameVisible()
	{
		enabled = true;
	}

	public virtual void OnBecameInvisible()
	{
		enabled = false;
	}

	public virtual void Update()
	{
		MonoBehaviour.print("wobbling");
		float wobbleScale = getWobbleScale(origScale.x, frequency.x, amount.x, offset.x);
		Vector3 localScale = cTransform.localScale;
		float num = (localScale.x = wobbleScale);
		Vector3 vector = (cTransform.localScale = localScale);
		float wobbleScale2 = getWobbleScale(origScale.y, frequency.y, amount.y, offset.y);
		Vector3 localScale2 = cTransform.localScale;
		float num2 = (localScale2.y = wobbleScale2);
		Vector3 vector3 = (cTransform.localScale = localScale2);
		float wobbleScale3 = getWobbleScale(origScale.z, frequency.z, amount.z, offset.z);
		Vector3 localScale3 = cTransform.localScale;
		float num3 = (localScale3.z = wobbleScale3);
		Vector3 vector5 = (cTransform.localScale = localScale3);
	}

	private float getWobbleScale(float orig, float freq, float amount, float offset)
	{
		return (amount != 0f) ? (orig + Mathf.Sin((Time.time + offset) * freq) * amount) : orig;
	}

	public virtual void Main()
	{
	}
}
