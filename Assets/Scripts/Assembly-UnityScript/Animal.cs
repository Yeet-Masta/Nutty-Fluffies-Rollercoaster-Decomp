using System;
using UnityEngine;

[Serializable]
public class Animal : MonoBehaviour
{
	[NonSerialized]
	public const int EXPRESSION_NORMAL = 0;

	[NonSerialized]
	public const int EXPRESSION_COOL = 1;

	[NonSerialized]
	public const int EXPRESSION_GRIN = 2;

	[NonSerialized]
	public const int EXPRESSION_SCARED = 3;

	[NonSerialized]
	public static float JUMP_DELAY = 0.25f;

	[NonSerialized]
	public static float JUMP_START_MAX_SPEED = 4f;

	[NonSerialized]
	public static float JUMP_START_MAX_SPEED_SQUARED = JUMP_START_MAX_SPEED * JUMP_START_MAX_SPEED;

	[NonSerialized]
	public static int PICKUP_START_TIME = 1;

	[NonSerialized]
	public static float MIN_EXPRESSION_DURATION = 1.5f;

	[NonSerialized]
	public static int EXTRA_HEART_FROM_AIR_START_TIME = 2;

	[NonSerialized]
	public static int EXTRA_HEART_FROM_AIR_GIVE_RATE = 6;

	[NonSerialized]
	public static int MAXIMUM_EXTRA_HEARTS_FROM_DROPPING = 50;

	[NonSerialized]
	public static int MAXIMUM_EXTRA_HEARTS_FROM_BUMPS = 60;

	[NonSerialized]
	public static int MAXIMUM_EXTRA_HEARTS_FROM_AIR = 50;

	[NonSerialized]
	public static int MAXIMUM_EXTRA_HEARTS_FROM_SPEEDING = 40;

	[NonSerialized]
	public static float EXTRA_HEART_FROM_BUMP_LIMIT = 2f;

	[NonSerialized]
	public static int EXTRA_HEART_FROM_BUMP_AIRTIME_LIMIT = 10;

	public bool upsideDownBrakeForceModify;

	public bool givesExtraHeartsFromBumps;

	public bool givesExtraHeartsWhenInAir;

	public bool givesExtraHeartsWhenDropped;

	public bool givesExtraHeartsWhenSpeeding;

	public float givesExtraHeartsXPos;

	private int extraHeartGiveCounter;

	private int extraHeartsGiven;

	private float lastVelocity;

	private bool givingExtraHearts;

	private float gaveExtraHeartsTime;

	public Transform head;

	public Transform body;

	public float jumpForce;

	public float turnTorque;

	public float downPull;

	public float forwardForce;

	public int typeIndex;

	public Rigidbody rb;

	public Transform tr;

	public Transform armTransform;

	public Vector3 startArmPosition;

	public GameObject[] expression;

	public float colliderSize;

	[HideInInspector]
	public Cart cartRef;

	private FixedJoint joint;

	private float startBreakForce;

	private bool setBreakForce;

	public bool canBePickedUp;

	private SphereCollider collisionTrigger;

	public bool jumpEnabled;

	private int stayCounter;

	private float bodyScale;

	private float headScale;

	private float expressionSetTime;

	private Vector3 headInitialPosition;

	private Vector3 prevV;

	[NonSerialized]
	private static Vehicle trainRef;

	[NonSerialized]
	private static Level level;

	private float droppedTime;

	private int speedingCounter;

	private int nextExp;

	private float headDisplacer;

	private float headDisplacerX;

	private float headAngle;

	public Animal()
	{
		upsideDownBrakeForceModify = true;
		gaveExtraHeartsTime = -100f;
		colliderSize = 0.5f;
		expressionSetTime = -1000f;
		nextExp = -1;
	}

	public virtual void Awake()
	{
		level = GameManager.getInstance().getLevel();
		useGUILayout = false;
		joint = gameObject.GetComponent<FixedJoint>();
		setBreakForce = false;
		canBePickedUp = false;
		jumpEnabled = false;
		bodyScale = body.localScale.x;
		headScale = head.localScale.x;
		setExpression(1, true);
		headInitialPosition = head.localPosition;
		startArmPosition = armTransform.localPosition;
		int num = 90;
		Vector3 localEulerAngles = armTransform.localEulerAngles;
		float num2 = (localEulerAngles.z = num);
		Vector3 vector = (armTransform.localEulerAngles = localEulerAngles);
		extraHeartGiveCounter = 1;
		lastVelocity = rb.velocity.magnitude;
		droppedTime = 0f;
		extraHeartsGiven = 0;
		givingExtraHearts = false;
		gaveExtraHeartsTime = -100f;
		givesExtraHeartsXPos = transform.position.x - 10000f;
	}

	public virtual void Start()
	{
		trainRef = GameManager.getInstance().getVehicle();
		startBreakForce = joint.breakForce * 100f * GetComponent<Rigidbody>().mass;
		joint.breakForce = 100000f;
		Invoke("returnBreakForce", 4f);
		bool flag = typeIndex == 1;
		if (!flag)
		{
			flag = typeIndex == 4;
		}
		givesExtraHeartsWhenSpeeding = flag;
		bool flag2 = typeIndex == 2;
		if (!flag2)
		{
			flag2 = typeIndex == 4;
		}
		givesExtraHeartsWhenInAir = flag2;
		givesExtraHeartsFromBumps = typeIndex == 3;
	}

	public virtual void returnBreakForce()
	{
		setBreakForce = true;
		updateBrakeForce();
	}

	public virtual void updateBrakeForce()
	{
		if (joint != null)
		{
			if (setBreakForce && !cartRef.hasContact)
			{
				joint.breakForce = startBreakForce;
			}
			else
			{
				joint.breakForce = 100000f;
			}
		}
	}

	public virtual void gotBump(float power)
	{
		if (givesExtraHeartsFromBumps && GameManager.getInstance().isPlaying() && !(givesExtraHeartsXPos >= transform.position.x) && (power > EXTRA_HEART_FROM_BUMP_LIMIT || cartRef.airTime > EXTRA_HEART_FROM_BUMP_AIRTIME_LIMIT))
		{
			int num = 2;
			if (extraHeartsGiven + num - 1 < MAXIMUM_EXTRA_HEARTS_FROM_BUMPS && !(Time.timeScale <= 0f))
			{
				giveSpecialHearts(num);
			}
		}
	}

	public virtual void updateAnimal()
	{
		givingExtraHearts = false;
		if (joint != null)
		{
			Vehicle vehicle = cartRef.trainRef;
			if (!(cartRef.tr.up.y >= 0f) && !((float)cartRef.airTime >= 0.25f))
			{
				givesExtraHeartsXPos = transform.position.x;
			}
			if (givesExtraHeartsWhenInAir && !cartRef.hasContact && !((float)vehicle.getCartsInAir() <= (float)vehicle.getCartCount() * 0.75f) && cartRef.airTime > EXTRA_HEART_FROM_AIR_START_TIME && !(givesExtraHeartsXPos >= transform.position.x) && (cartRef.airTime - EXTRA_HEART_FROM_AIR_START_TIME) % EXTRA_HEART_FROM_AIR_GIVE_RATE == 0 && extraHeartsGiven < MAXIMUM_EXTRA_HEARTS_FROM_AIR && !(Time.timeScale <= 0f))
			{
				giveSpecialHearts(1);
			}
			if (givesExtraHeartsWhenSpeeding && cartRef.hasContact && !(givesExtraHeartsXPos >= transform.position.x - 12f) && !(cartRef.rb.velocity.y >= -4f) && extraHeartsGiven < MAXIMUM_EXTRA_HEARTS_FROM_SPEEDING && !(Time.timeScale <= 0f))
			{
				giveSpecialHearts(1);
			}
		}
		updateBrakeForce();
	}

	public virtual void LateUpdate()
	{
		if (joint != null)
		{
			tr.localPosition = Vector3.zero;
		}
	}

	public virtual void addCollider()
	{
		SphereCollider sphereCollider = null;
		sphereCollider = (SphereCollider)gameObject.GetComponent(typeof(SphereCollider));
		if (sphereCollider == null)
		{
			sphereCollider = (SphereCollider)gameObject.AddComponent(typeof(SphereCollider));
			float y = 1f;
			Vector3 center = sphereCollider.center;
			float num = (center.y = y);
			Vector3 vector = (sphereCollider.center = center);
			sphereCollider.radius = colliderSize;
			collisionTrigger = sphereCollider;
			sphereCollider.material.staticFriction = 0.75f;
			sphereCollider.material.dynamicFriction = 0.75f;
			sphereCollider.material.frictionCombine = PhysicMaterialCombine.Maximum;
			sphereCollider.material.bounciness = 0.25f;
			sphereCollider.material.bounceCombine = PhysicMaterialCombine.Maximum;
		}
	}

	public virtual void releaseJoint()
	{
		if (joint != null)
		{
			UnityEngine.Object.Destroy(joint);
			OnJointBreak();
			joint = null;
		}
	}

	public virtual void OnJointBreak()
	{
		tr.position -= rb.velocity * 0.05f;
		disconnectFromCart();
	}

	public virtual void OnCollisionStay()
	{
		if (jumpEnabled)
		{
			stayCounter++;
			if (stayCounter > 2)
			{
				jump();
			}
		}
	}

	public virtual void disconnectFromCart()
	{
		addCollider();
		tr.parent = GameManager.getInstance().getVehicle().transform;
		float y = headInitialPosition.y;
		Vector3 localPosition = head.localPosition;
		float num = (localPosition.y = y);
		Vector3 vector = (head.localPosition = localPosition);
		int num2 = 180;
		Vector3 localEulerAngles = armTransform.localEulerAngles;
		float num3 = (localEulerAngles.z = num2);
		Vector3 vector3 = (armTransform.localEulerAngles = localEulerAngles);
		float y2 = startArmPosition.y;
		Vector3 localPosition2 = armTransform.localPosition;
		float num4 = (localPosition2.y = y2);
		Vector3 vector5 = (armTransform.localPosition = localPosition2);
		int num5 = 0;
		Vector3 localEulerAngles2 = head.localEulerAngles;
		float num6 = (localEulerAngles2.z = num5);
		Vector3 vector7 = (head.localEulerAngles = localEulerAngles2);
		int num7 = 0;
		Vector3 localEulerAngles3 = body.localEulerAngles;
		float num8 = (localEulerAngles3.z = num7);
		Vector3 vector9 = (body.localEulerAngles = localEulerAngles3);
		rb.angularDrag = 0.5f;
		if (GameManager.getInstance().isPlaying())
		{
			cartRef.onAnimalJointBreak();
			setExpression(3, true);
			EffectManager.getInstance().emitStars(tr.position, 6);
			if (givesExtraHeartsWhenDropped && !(Time.timeScale <= 0f))
			{
				int amount = 3;
				giveSpecialHearts(amount);
			}
		}
		else
		{
			setExpression(1, true);
		}
		droppedTime = 0f;
	}

	public virtual void pickUpTest()
	{
		if (!canBePickedUp)
		{
			if (!(droppedTime >= (float)PICKUP_START_TIME))
			{
				return;
			}
			canBePickedUp = true;
		}
		if (rb.velocity.sqrMagnitude <= JUMP_START_MAX_SPEED_SQUARED)
		{
			jumpEnabled = true;
			jump();
		}
	}

	public virtual void reconnectToCart(Cart cart)
	{
		if (canBePickedUp && !cart.hasAnimal())
		{
			UnityEngine.Object.Destroy(collisionTrigger);
			tr.parent = cart.animalRoot;
			tr.localPosition = Vector3.zero;
			float z = 0f;
			Vector3 localEulerAngles = tr.localEulerAngles;
			float num = (localEulerAngles.z = z);
			Vector3 vector = (tr.localEulerAngles = localEulerAngles);
			float x = 1f;
			Vector3 localScale = tr.localScale;
			float num2 = (localScale.x = x);
			Vector3 vector3 = (tr.localScale = localScale);
			rb.freezeRotation = false;
			rb.constraints = (RigidbodyConstraints)56;
			rb.angularDrag = 0f;
			joint = gameObject.AddComponent<FixedJoint>();
			joint.connectedBody = cart.rb;
			Awake();
			if (cartRef.animal == this)
			{
				cartRef.animal = null;
			}
			cart.animal = this;
			cartRef = cart;
			cartRef.animal = this;
			cartRef.heartValue = GameData.ANIMAL_HEART_VALUE[typeIndex];
			cartRef.animalIndex = typeIndex;
			GameManager.getInstance().addAnimalScore(cartRef.heartValue, typeIndex, cart);
			extraHeartsGiven += cartRef.heartValue;
			RollerCoasterSounds.getInstance().playScreamSpecial(typeIndex);
			setExpression(2, true);
			int num3 = 180;
			Vector3 localEulerAngles2 = armTransform.localEulerAngles;
			float num4 = (localEulerAngles2.z = num3);
			Vector3 vector5 = (armTransform.localEulerAngles = localEulerAngles2);
			canBePickedUp = false;
			jumpEnabled = false;
		}
	}

	public virtual void jump()
	{
		if (!canBePickedUp)
		{
			return;
		}
		rb.freezeRotation = true;
		float z = 0f;
		Vector3 localEulerAngles = tr.localEulerAngles;
		float num = (localEulerAngles.z = z);
		Vector3 vector = (tr.localEulerAngles = localEulerAngles);
		if (cartRef.animalIndex != -1)
		{
			Cart cart = cartRef.trainRef.findEmptyCart();
			if (cart != null)
			{
				cartRef = cart;
			}
		}
		stayCounter = 0;
		float num2 = default(float);
		if (!(cartRef.tr.position.x <= head.transform.position.x))
		{
			float x = 1f;
			Vector3 localScale = tr.localScale;
			float num3 = (localScale.x = x);
			Vector3 vector3 = (tr.localScale = localScale);
			num2 = forwardForce;
		}
		else
		{
			float x2 = -1f;
			Vector3 localScale2 = tr.localScale;
			float num4 = (localScale2.x = x2);
			Vector3 vector5 = (tr.localScale = localScale2);
			num2 = 0f - forwardForce;
		}
		if (!(rb.velocity.sqrMagnitude >= 25f))
		{
			rb.AddForce(new Vector3(num2, jumpForce, 0f));
		}
	}

	public virtual void Update()
	{
		if (jumpEnabled)
		{
			raycastTrack();
			float z = 0f;
			Vector3 localEulerAngles = armTransform.localEulerAngles;
			float num = (localEulerAngles.z = z);
			Vector3 vector = (armTransform.localEulerAngles = localEulerAngles);
			float y = startArmPosition.y;
			Vector3 localPosition = armTransform.localPosition;
			float num2 = (localPosition.y = y);
			Vector3 vector3 = (armTransform.localPosition = localPosition);
			droppedTime += Time.deltaTime;
		}
		else if (joint == null)
		{
			raycastTrack();
			pickUpTest();
			droppedTime += Time.deltaTime;
		}
		else
		{
			if (trainRef.@lock)
			{
				setExpression(1, false);
			}
			else if (trainRef.goingFast || trainRef.isFlying)
			{
				setExpression(2, false);
			}
			else if (joint != null)
			{
				setExpression(1, false);
			}
			float num3 = Mathf.Min(1f, rb.velocity.magnitude / 20f);
			float z2 = 60f + 80f * num3;
			Vector3 localEulerAngles2 = armTransform.localEulerAngles;
			float num4 = (localEulerAngles2.z = z2);
			Vector3 vector5 = (armTransform.localEulerAngles = localEulerAngles2);
			if (!(num3 <= 0.5f))
			{
				float y2 = startArmPosition.y + (num3 - 0.5f) * 1f;
				Vector3 localPosition2 = armTransform.localPosition;
				float num5 = (localPosition2.y = y2);
				Vector3 vector7 = (armTransform.localPosition = localPosition2);
			}
		}
		controlHead();
	}

	public virtual void setExpression(int exp, bool force)
	{
		if (exp != nextExp && (force || !(Time.time - expressionSetTime <= MIN_EXPRESSION_DURATION)))
		{
			CancelInvoke("expressionInvoker");
			nextExp = exp;
			if (force)
			{
				expressionInvoker();
			}
			else
			{
				Invoke("expressionInvoker", 0.5f * UnityEngine.Random.value);
			}
		}
	}

	public virtual void expressionInvoker()
	{
		for (int i = 0; i < expression.Length; i++)
		{
			expression[i].active = nextExp == i;
		}
		expressionSetTime = Time.time;
	}

	public virtual void onCrash()
	{
		setExpression(3, true);
	}

	private void raycastTrack()
	{
		RaycastHit hitInfo = default(RaycastHit);
		if (!Physics.Raycast(tr.position, -Vector3.up, out hitInfo, 160f, LevelSpline.SPLINE_LAYER))
		{
			return;
		}
		MeshCollider meshCollider = hitInfo.collider as MeshCollider;
		if (meshCollider != null)
		{
			Mesh sharedMesh = meshCollider.sharedMesh;
			if (level.splineMeshVerticesCache.Count > 0)
			{
				Vector3[] array = level.splineMeshVerticesCache[sharedMesh];
				int num = hitInfo.triangleIndex & -2;
				float z = (array[num].z + array[num + 1].z + array[num + 2].z + array[num + 3].z) * 0.25f;
				Vector3 position = tr.position;
				float num2 = (position.z = z);
				Vector3 vector = (tr.position = position);
			}
		}
	}

	private void controlHead()
	{
		if (joint != null)
		{
			Vector3 velocity = cartRef.rb.velocity;
			Vehicle vehicle = cartRef.trainRef;
			if (!vehicle.@lock)
			{
				float num = vehicle.controller.throttle - vehicle.controller.brake;
				headAngle = headAngle * 0.9f + num * 0.1f * 0.1f;
			}
			float num2 = 0.98f;
			headDisplacer = (velocity.y - prevV.y) * (1f - num2) + headDisplacer * num2;
			headAngle = (velocity.x - prevV.x) * (1f - num2) + headAngle * num2;
			headDisplacer = Mathf.Max(Mathf.Min(0.1f, headDisplacer), -0.1f);
			headAngle = Mathf.Max(Mathf.Min(0.1f, headAngle), -0.1f);
			float y = headInitialPosition.y - headDisplacer * 4f * cartRef.cachedUpVector.y;
			Vector3 localPosition = head.localPosition;
			float num3 = (localPosition.y = y);
			Vector3 vector = (head.localPosition = localPosition);
			if (!(gaveExtraHeartsTime <= Time.time - 0.2f))
			{
				float y2 = head.localPosition.y + (0.2f - (Time.time - gaveExtraHeartsTime)) * 5f;
				Vector3 localPosition2 = head.localPosition;
				float num4 = (localPosition2.y = y2);
				Vector3 vector3 = (head.localPosition = localPosition2);
			}
			float z = headAngle * 250f;
			Vector3 localEulerAngles = head.localEulerAngles;
			float num5 = (localEulerAngles.z = z);
			Vector3 vector5 = (head.localEulerAngles = localEulerAngles);
			float z2 = headAngle * 250f + 15f;
			Vector3 localEulerAngles2 = body.localEulerAngles;
			float num6 = (localEulerAngles2.z = z2);
			Vector3 vector7 = (body.localEulerAngles = localEulerAngles2);
			prevV = velocity;
		}
	}

	public virtual bool isGivingExtraHearts()
	{
		return givingExtraHearts;
	}

	public virtual void giveSpecialHearts(int amount)
	{
		extraHeartsGiven += amount;
		givingExtraHearts = true;
		gaveExtraHeartsTime = Time.time;
		RollerCoasterSounds.getInstance().playScreamSpecial(typeIndex);
		givesExtraHeartsXPos = transform.position.x;
		GameManager.getInstance().addAnimalScore(amount, typeIndex);
		heartEmitInvoke();
		if (amount > 1)
		{
			for (int i = 1; i < amount; i++)
			{
				Invoke("heartEmitInvoke", (0.1f + UnityEngine.Random.value * 0.05f) * (float)i);
			}
		}
	}

	public virtual void heartEmitInvoke()
	{
		cartRef.emitHeartFromPosition(tr.position, typeIndex);
	}
}
