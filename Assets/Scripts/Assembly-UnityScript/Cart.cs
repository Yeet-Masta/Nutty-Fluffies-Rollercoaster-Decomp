using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Cart : MonoBehaviour
{
	public ConfigurableJoint joint;

	public Transform animalRoot;

	public Animal animal;

	public int heartValue;

	public Transform tr;

	public Rigidbody rb;

	public Wheel[] wheels;

	public Rigidbody[] wheelsRb;

	public AnimalTrigger animalTrigger;

	public bool crashed;

	public GameObject jumpRocket;

	public Transform jumpRocketFlame;

	public GameObject magnet;

	public Transform raycastPoint;

	public Transform sparkPoint;

	public float height;

	public float sqHeight;

	public bool sqHeightAvailable;

	public bool hasContact;

	public int airTime;

	private int contactCounter;

	private float prevVY;

	[HideInInspector]
	public Joint toyJoint;

	[HideInInspector]
	public int animalIndex;

	[HideInInspector]
	public Vehicle trainRef;

	[HideInInspector]
	public Transform itemTr;

	[HideInInspector]
	public int indexInVehicle;

	private int currentSegment;

	private int currentTriangle;

	[HideInInspector]
	public float targetZ;

	[HideInInspector]
	public Vector2 cachedUpVector;

	private List<int> visitedTriggers;

	private MeshRenderer[] renderers;

	private float animalDropTime;

	[HideInInspector]
	public int currentRenderQueue;

	[HideInInspector]
	public int wheelSparcCounter;

	[NonSerialized]
	private static Level level;

	[NonSerialized]
	private static EffectManager effectMgr;

	private int totalFinishHeartsDelivered;

	public Cart()
	{
		contactCounter = -1;
		animalIndex = -1;
		currentSegment = -1;
		currentTriangle = -1;
		targetZ = -1f;
		visitedTriggers = new List<int>();
	}

	public virtual void prepare()
	{
		level = GameManager.getInstance().getLevel();
		effectMgr = EffectManager.getInstance();
		float z = ZLevel.getInstance().getZ(tr.position);
		Vector3 position = tr.position;
		float num = (position.z = z);
		Vector3 vector = (tr.position = position);
		object[] componentsInChildren = gameObject.GetComponentsInChildren(typeof(MeshRenderer), true);
		renderers = new MeshRenderer[componentsInChildren.Length];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			renderers[i] = componentsInChildren[i] as MeshRenderer;
		}
		int j = 0;
		MeshRenderer[] array = renderers;
		for (int length = array.Length; j < length; j++)
		{
		}
		useGUILayout = false;
		if (animalRoot != null && !animalRoot.gameObject.activeInHierarchy)
		{
			animalRoot.gameObject.SetActive(true);
		}
	}

	public virtual void emitHearts(int amount)
	{
		emitHeart();
		if (amount > 1)
		{
			for (int i = 1; i < amount; i++)
			{
				Invoke("emitHeart", (0.1f + UnityEngine.Random.value * 0.05f) * (float)i);
			}
		}
	}

	public virtual void emitHeart()
	{
		Vector3 position = tr.position;
		if (animal != null)
		{
			position = animal.tr.position;
		}
		emitHeartFromPosition(tr.position, GameData.ANIMAL_NONE);
	}

	public virtual void emitHeartFromPosition(Vector3 pos, int animalIndex)
	{
		if (animalIndex == GameData.ANIMAL_NONE)
		{
			trainRef.heartEmitter.emitAt(pos.x, pos.y, 0.4f, 1);
			RollerCoasterSounds.getInstance().playScore();
		}
		else
		{
			trainRef.animalHeartEmitter[animalIndex].emitAt(pos.x, pos.y, 0.4f, 1);
			RollerCoasterSounds.getInstance().playSpecialScore();
		}
	}

	public virtual void setCrashed()
	{
		if (!crashed)
		{
			if (animal != null)
			{
				animal.releaseJoint();
			}
			crashed = true;
			if (animalTrigger != null)
			{
				animalTrigger.deactivate();
			}
			if (animal != null)
			{
				animal.onCrash();
			}
			effectMgr.emitStars(tr.position, 6);
			GetComponent<Collider>().material.dynamicFriction = 0.4f;
			GetComponent<Collider>().material.staticFriction = 0.4f;
			GetComponent<Collider>().material.frictionCombine = PhysicMaterialCombine.Maximum;
		}
	}

	public virtual void OnTriggerEnter(Collider other)
	{
		if (crashed || !other.isTrigger)
		{
			return;
		}
		int instanceID = other.gameObject.GetInstanceID();
		if (!visitedTriggers.Contains(instanceID))
		{
			RaycastHit raycastHit = default(RaycastHit);
			visitedTriggers.Add(instanceID);
			TriggerCheckPoint triggerCheckPoint = other.gameObject.GetComponent<TriggerCheckPoint>() as TriggerCheckPoint;
			if (!hasContact)
			{
				height = trainRef.heightBonusTreshold + 1f;
			}
			else
			{
				height = 0f;
			}
			if (triggerCheckPoint != null)
			{
				int kind = triggerCheckPoint.kind;
				GameManager.getInstance().onCartInCheckPoint(this, kind);
			}
		}
	}

	public virtual void raycastTrack()
	{
		cachedUpVector = tr.up;
		RaycastHit hitInfo = default(RaycastHit);
		Vector3 position = raycastPoint.position;
		if (Physics.Raycast(position, -cachedUpVector, out hitInfo, 16f, LevelSpline.SPLINE_LAYER) || Physics.Raycast(position, -Vector3.up, out hitInfo, 160f, LevelSpline.SPLINE_LAYER))
		{
			MeshCollider meshCollider = hitInfo.collider as MeshCollider;
			if (!(meshCollider != null) || currentTriangle == hitInfo.triangleIndex)
			{
				return;
			}
			currentTriangle = hitInfo.triangleIndex;
			Mesh sharedMesh = meshCollider.sharedMesh;
			float zForIndex = getZForIndex(currentTriangle, sharedMesh);
			targetZ = zForIndex;
			if (!(Mathf.Abs(tr.position.z - targetZ) <= 0.1f))
			{
				float z = targetZ;
				Vector3 position2 = tr.position;
				float num = (position2.z = z);
				Vector3 vector = (tr.position = position2);
			}
			Vector3 vector3 = tr.position - hitInfo.point;
			int num2 = hitInfo.triangleIndex >> 1;
			sqHeight = vector3.sqrMagnitude;
			sqHeightAvailable = true;
			if (currentSegment == num2)
			{
				return;
			}
			LevelSpline component = hitInfo.transform.gameObject.GetComponent<LevelSpline>();
			if (component != null)
			{
				int renderQueueLevel = component.getRenderQueueLevel(num2);
				if (renderQueueLevel != currentRenderQueue)
				{
					int i = 0;
					MeshRenderer[] array = renderers;
					for (int length = array.Length; i < length; i++)
					{
						array[i].sharedMaterial.renderQueue = renderQueueLevel + 1;
					}
					currentRenderQueue = renderQueueLevel;
				}
			}
			currentSegment = num2;
		}
		else
		{
			float z2 = ZLevel.getInstance().getZ(tr.position);
			Vector3 position3 = tr.position;
			float num3 = (position3.z = z2);
			Vector3 vector4 = (tr.position = position3);
			sqHeightAvailable = false;
		}
	}

	public virtual void updateCart()
	{
		cachedUpVector = tr.up;
		raycastTrack();
		if (animal != null)
		{
			animal.updateAnimal();
		}
		if (wheels[0].onTerrain)
		{
			contactCounter = 3;
		}
		else
		{
			contactCounter--;
		}
		bool flag = contactCounter > 0;
		if (!hasContact && flag)
		{
			float maxImpactSpeed = wheels[0].maxImpactSpeed;
			wheels[0].maxImpactSpeed = 0f;
			if (!(maxImpactSpeed <= Wheel.MAX_COLLISION_IMPACT) && !(wheels[0].speedBeforeImpact <= Wheel.MIN_SPEED_LIMIT_BEFORE_IMPACT) && !(rb.velocity.sqrMagnitude >= Wheel.MAX_COLLISION_STOP_SPEED_LIMIT * Wheel.MAX_COLLISION_STOP_SPEED_LIMIT) && (!trainRef.isSpecialActivated() || !(trainRef.special is Magnet)))
			{
				setCrashed();
			}
			wheelSparcCounter = 5;
		}
		hasContact = flag;
		if (!hasContact)
		{
			airTime++;
		}
		else
		{
			airTime = 0;
		}
		if (wheelSparcCounter > 0)
		{
			wheelSparcCounter--;
			if (!(rb.velocity.sqrMagnitude <= 25f))
			{
				effectMgr.emitSparks(sparkPoint.position, rb.velocity, wheelSparcCounter);
			}
		}
		if (hasContact && !(tr.InverseTransformDirection(wheels[0].impactNormal).y >= -0.8f))
		{
			setCrashed();
		}
		if (hasContact && !(UnityEngine.Random.value <= 0.8f))
		{
			float num = Mathf.Min(20f, rb.velocity.magnitude) * 5f;
			rb.AddForceAtPosition(tr.up * num, tr.position + tr.right * 4f);
			rb.AddForceAtPosition(tr.right * ((0f - num) * 0.25f), tr.position);
		}
	}

	public virtual void onAnimalJointBreak()
	{
		if (GameManager.getInstance().isPlaying())
		{
			GameManager.getInstance().onAnimalDrop(this);
			RollerCoasterSounds.getInstance().playFallSound(animalIndex);
			heartValue = 0;
			animalTrigger.activate();
			animalDropTime = Time.time;
		}
		animal = null;
	}

	public virtual void applyDefaultMagnet()
	{
		if (!rb.isKinematic && !crashed && !(tr.up.y <= -0.4f))
		{
			float x = rb.velocity.x * 0.95f;
			Vector3 velocity = rb.velocity;
			float num = (velocity.x = x);
			Vector3 vector = (rb.velocity = velocity);
		}
	}

	private float getZForIndex(int index, Mesh mesh)
	{
		float result;
		if (level.splineMeshVerticesCache.Count == 0 || !level.splineMeshVerticesCache.ContainsKey(mesh))
		{
			result = 0f;
		}
		else
		{
			Vector3[] array = level.splineMeshVerticesCache[mesh];
			index &= -2;
			float num = (array[index].z + array[index + 1].z + array[index + 2].z + array[index + 3].z) * 0.25f;
			result = num;
		}
		return result;
	}

	public virtual bool hasAnimal()
	{
		return animal != null;
	}

	public virtual int getAnimalType()
	{
		return (!(animal != null) && Time.time - animalDropTime >= 3f) ? GameData.ANIMAL_NONE : animalIndex;
	}

	public virtual void onFinish()
	{
		hasContact = true;
		Invoke("finishInvoke", 0.5f + (float)indexInVehicle * 0.15f);
		if (animalTrigger != null)
		{
			animalTrigger.deactivate();
		}
	}

	private void finishInvoke()
	{
		if (animal != null)
		{
			animal.releaseJoint();
			animal.rb.velocity = animal.rb.velocity * 0f;
			giveFinishHearts();
		}
	}

	private void giveFinishHearts()
	{
		GameManager.getInstance().addAnimalScore((int)GameData.BONUS_VALUES[animalIndex + 1], animalIndex, this);
	}
}
