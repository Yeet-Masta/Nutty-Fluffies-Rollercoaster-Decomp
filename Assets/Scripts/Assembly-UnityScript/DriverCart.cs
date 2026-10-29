using UnityEngine;

public class DriverCart : Cart
{
	public Transform brakeHandle;

	public GameObject rocket;

	public Transform rocketFlame;

	public float handleAngleFactor;

	public DriverAnimal driver;

	private float downPull;

	public DriverCart()
	{
		handleAngleFactor = 22f;
		downPull = 25f;
	}

	public virtual void calculateAverageDownpull()
	{
		for (int i = 1; i < trainRef.carts.Length; i++)
		{
			if (trainRef.carts[i].animal != null)
			{
				downPull += trainRef.carts[i].animal.downPull;
			}
		}
		downPull /= trainRef.carts.Length;
	}

	public virtual void Update()
	{
		if (crashed)
		{
			return;
		}
		float num = 90f;
		if (!rb.isKinematic)
		{
			float num2 = trainRef.controller.brake;
			if (!(num2 <= 1f))
			{
				num2 = 1f;
			}
			if (!(num2 <= 0.1f))
			{
				num += num2 * handleAngleFactor;
			}
			else if (!(trainRef.controller.throttle <= 0.1f))
			{
				num -= trainRef.controller.throttle * handleAngleFactor;
			}
			if (!(num <= 180f))
			{
				num = 180f;
			}
			else if (!(num >= 45f))
			{
				num = 45f;
			}
		}
		float z = brakeHandle.localEulerAngles.z + (num - brakeHandle.localEulerAngles.z) * 0.2f;
		Vector3 localEulerAngles = brakeHandle.localEulerAngles;
		localEulerAngles.z = z;
		brakeHandle.localEulerAngles = localEulerAngles;
		float z2 = brakeHandle.localEulerAngles.z;
		Vector3 localEulerAngles2 = driver.tr.localEulerAngles;
		localEulerAngles2.z = z2;
		driver.tr.localEulerAngles = localEulerAngles2;
	}

	public override void OnTriggerEnter(Collider other)
	{
		base.OnTriggerEnter(other);
	}

	public override void setCrashed()
	{
		base.setCrashed();
		if (driver.GetComponent<Rigidbody>() == null)
		{
			driver.gameObject.AddComponent(typeof(Rigidbody));
			driver.GetComponent<Rigidbody>().constraints = (RigidbodyConstraints)56;
			float x = driver.GetComponent<Rigidbody>().velocity.x + (UnityEngine.Random.value - 0.5f);
			Vector3 velocity = driver.GetComponent<Rigidbody>().velocity;
			velocity.x = x;
			driver.GetComponent<Rigidbody>().velocity = velocity;
			float y = driver.GetComponent<Rigidbody>().velocity.y + (UnityEngine.Random.value - 0.5f);
			Vector3 velocity2 = driver.GetComponent<Rigidbody>().velocity;
			velocity2.y = y;
			driver.GetComponent<Rigidbody>().velocity = velocity2;
			float z = driver.GetComponent<Rigidbody>().angularVelocity.z + UnityEngine.Random.value * 4f;
			Vector3 angularVelocity = driver.GetComponent<Rigidbody>().angularVelocity;
			angularVelocity.z = z;
			driver.GetComponent<Rigidbody>().angularVelocity = angularVelocity;
			driver.GetComponent<Rigidbody>().mass = 2f;
			CapsuleCollider capsuleCollider = (CapsuleCollider)driver.gameObject.AddComponent(typeof(CapsuleCollider));
			float x2 = 1.75f;
			Vector3 center = capsuleCollider.center;
			center.x = x2;
			capsuleCollider.center = center;
			float y2 = 0.25f;
			Vector3 center2 = capsuleCollider.center;
			center2.y = y2;
			capsuleCollider.center = center2;
			int num6 = 0;
			Vector3 center3 = capsuleCollider.center;
			center3.z = num6;
			capsuleCollider.center = center3;
			capsuleCollider.radius = 0.75f;
			capsuleCollider.height = 4f;
			capsuleCollider.direction = 0;
			capsuleCollider.material.staticFriction = 0.8f;
			capsuleCollider.material.dynamicFriction = 0.8f;
			capsuleCollider.material.frictionCombine = PhysicMaterialCombine.Maximum;
			brakeHandle.gameObject.AddComponent(typeof(Rigidbody));
			brakeHandle.GetComponent<Rigidbody>().constraints = (RigidbodyConstraints)56;
			float x3 = brakeHandle.GetComponent<Rigidbody>().velocity.x + (UnityEngine.Random.value - 0.5f);
			Vector3 velocity3 = brakeHandle.GetComponent<Rigidbody>().velocity;
			velocity3.x = x3;
			brakeHandle.GetComponent<Rigidbody>().velocity = velocity3;
			float y3 = brakeHandle.GetComponent<Rigidbody>().velocity.y + (UnityEngine.Random.value - 0.5f);
			Vector3 velocity4 = brakeHandle.GetComponent<Rigidbody>().velocity;
			velocity4.y = y3;
			brakeHandle.GetComponent<Rigidbody>().velocity = velocity4;
			float z2 = brakeHandle.GetComponent<Rigidbody>().angularVelocity.z + UnityEngine.Random.value * 10f;
			Vector3 angularVelocity2 = brakeHandle.GetComponent<Rigidbody>().angularVelocity;
			angularVelocity2.z = z2;
			brakeHandle.GetComponent<Rigidbody>().angularVelocity = angularVelocity2;
		}
		driver.onCrash();
	}
}
