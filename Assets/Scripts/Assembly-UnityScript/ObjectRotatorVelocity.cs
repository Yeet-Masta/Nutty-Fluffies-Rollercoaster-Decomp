using System;
using UnityEngine;

[Serializable]
public class ObjectRotatorVelocity : CachedMonoBehaviour
{
	public Transform getVelocityFrom;

	private Vector3 lastPosition;

	public Renderer targetRenderer;

	public Vector3 lookUp;

	public float scaleFromVelocity;

	public float minimumShowSpeed;

	public float rotationTurnSpeed;

	private Vector3 lastVelocity;

	private bool visib;

	public ObjectRotatorVelocity()
	{
		visib = true;
	}

	public virtual void Start()
	{
		lastPosition = cTransform.position;
		lastVelocity = new Vector3(0f, 0f, 0f);
	}

	public virtual void Update()
	{
		Vector3 to = (cTransform.position - lastPosition) / Time.deltaTime;
		lastPosition = cTransform.position;
		if (to.magnitude == 0f)
		{
			return;
		}
		if (!(minimumShowSpeed <= 0f) && !(to.magnitude >= minimumShowSpeed))
		{
			targetRenderer.enabled = false;
			visib = false;
			return;
		}
		if (!visib)
		{
			targetRenderer.enabled = true;
			visib = true;
		}
		to = Vector3.Lerp(lastVelocity, to, rotationTurnSpeed);
		cTransform.LookAt(cTransform.position + to);
		lastVelocity = to;
		if (!(scaleFromVelocity <= 0f))
		{
			float z = scaleFromVelocity * (to.magnitude - minimumShowSpeed);
			Vector3 localScale = cTransform.localScale;
			float num = (localScale.z = z);
			Vector3 vector = (cTransform.localScale = localScale);
		}
	}
}
