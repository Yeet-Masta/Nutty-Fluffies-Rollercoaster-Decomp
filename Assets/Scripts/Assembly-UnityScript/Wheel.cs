using System;
using UnityEngine;

[Serializable]
public class Wheel : MonoBehaviour
{
	[NonSerialized]
	public static float MIN_SOUND_COLLISION_IMPACT = 6f;

	[NonSerialized]
	public static float MAX_COLLISION_IMPACT = 8f;

	[NonSerialized]
	public static float MAX_COLLISION_STOP_SPEED_LIMIT = 5f;

	[NonSerialized]
	public static float MIN_SPEED_LIMIT_BEFORE_IMPACT = 25f;

	public bool onTerrain;

	public Rigidbody rb;

	public Transform tr;

	public float impactSpeed;

	public float maxImpactSpeed;

	public float speedBeforeImpact;

	public Vector3 impactNormal;

	public Cart cartRef;

	public Wheel()
	{
		impactSpeed = -1f;
		impactNormal = Vector3.up;
	}

	public virtual void Awake()
	{
		useGUILayout = false;
		tr = transform;
	}

	public virtual void OnCollisionEnter(Collision collisionInfo)
	{
		onTerrain = true;
		speedBeforeImpact = collisionInfo.relativeVelocity.magnitude;
		impactSpeed = speedBeforeImpact - rb.velocity.magnitude;
		if (!(maxImpactSpeed >= impactSpeed))
		{
			maxImpactSpeed = impactSpeed;
		}
		if (!(impactSpeed <= MIN_SOUND_COLLISION_IMPACT))
		{
			if (!(impactSpeed <= MAX_COLLISION_IMPACT))
			{
				impactSpeed = MAX_COLLISION_IMPACT;
			}
			float impactPower = impactSpeed / MAX_COLLISION_IMPACT;
			RollerCoasterSounds.getInstance().playTrackCollision(impactPower);
		}
		impactNormal = collisionInfo.contacts[0].normal;
		if (cartRef.animal != null)
		{
			cartRef.animal.gotBump(impactSpeed);
		}
	}

	public virtual void OnCollisionStay(Collision collisionInfo)
	{
		if (collisionInfo.contacts.Length > 0)
		{
			impactNormal = collisionInfo.contacts[0].normal;
		}
	}

	public virtual void OnCollisionExit()
	{
		onTerrain = false;
	}
}
