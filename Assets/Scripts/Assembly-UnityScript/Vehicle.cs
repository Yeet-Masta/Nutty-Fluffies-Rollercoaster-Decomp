using UnityEngine;

public class Vehicle : MonoBehaviour
{
	public static readonly int CART_NORMAL = -1;

	public const int CART_ROCKET = 0;

	public const int CART_MAGNET = 1;

	public const int CART_HOVER = 2;

	public const int MAX_PUSH_SPEED = 30;

	public float maxPushSpeedSliding = 30f;

	public float minimumSpeed = 1f;

	public Controller controller;

	[HideInInspector]
	public Cart[] carts;

	[HideInInspector]
	public Special special;

	public float engineForce;

	public float brakeForce;

	private float originalEngineForce;

	private float originalBrakeForce;

	public float chainPullForce = 1f;

	public Cart cartPrefab;

	public Cart driverCartPrefab;

	public Cart frontCartPrefab;

	private Rigidbody primaryRigidbody;

	public ObjectEmitter heartEmitter;

	public ObjectEmitter[] animalHeartEmitter;

	public SoundController soundController;

	public float soundControllerFactor = 0.02f;

	public float speedBonusTreshold;

	public float heightBonusTreshold;

	public float gBonusTreshold;

	public float screamSpeedTreshold = 20f;

	public float screamFallingTreshold = 24f;

	public float screamProbablility = 1f;

	public float screamFallingFromJumpTreshold = -1f;

	[HideInInspector]
	public bool upsideDown;

	[HideInInspector]
	public bool goingFast;

	[HideInInspector]
	public bool falling;

	[HideInInspector]
	public bool hasCrashed;

	[HideInInspector]
	public bool fallingFromJump;

	[HideInInspector]
	public bool chainPulling;

	[HideInInspector]
	public bool disableDefaultMagnet;

	private Transform primaryTr;

	private int ticker;

	private float age;

	private RollerCoasterStatistics st;

	public bool isFlying;

	private float trainSpeedSquared;

	private int cartCount;

	private bool reversing;

	private int cartsInAir;

	private int inLoopCounter;

	private float engineForceLevelMultiply = 15f;

	public bool applySeparatePowerToCarts;

	[HideInInspector]
	public bool @lock = true;

	private int ticker2;

	public virtual void prepare()
	{
		cartCount = carts.Length;
		st = RollerCoasterStatistics.getInstance();
		primaryRigidbody = getFirstCart().GetComponent<Rigidbody>();
		primaryTr = primaryRigidbody.transform;
		soundController.start();
		for (int i = 0; i < cartCount; i++)
		{
			carts[i].trainRef = this;
			carts[i].prepare();
		}
		isFlying = false;
		useGUILayout = false;
		disableDefaultMagnet = false;
		originalEngineForce = engineForce;
		originalBrakeForce = brakeForce;
		applySeparatePowerToCarts = false;
		engineForceLevelMultiply = 10f;
		if (GameData.getActiveLevelPackIndex() > 0 && GameData.getSelectedLevelIndex() < 5)
		{
			engineForceLevelMultiply += 5f;
		}
		else
		{
			engineForceLevelMultiply += GameData.getSelectedLevelIndex();
		}
		maxPushSpeedSliding = 30f;
	}

	public virtual void FixedUpdate()
	{
		age++;
		if (!hasCrashed)
		{
			handleUpdatePhysicsAndControls();
		}
		handleUpdateSounds();
	}

	public virtual void handleUpdatePhysicsAndControls()
	{
		bool flag = controller.isCurrentControlTouchBased();
		ticker2++;
		if (flag)
		{
			engineForce = originalEngineForce * 6f;
			brakeForce = originalBrakeForce * 4f;
		}
		else
		{
			engineForce = originalEngineForce;
			brakeForce = originalBrakeForce;
		}
		Vector3 right = primaryTr.right;
		Vector3 velocity = primaryRigidbody.velocity;
		float num = Vector3.Dot(velocity, right);
		float num2 = 0f;
		reversing = num < 0f;
		trainSpeedSquared = velocity.sqrMagnitude;
		if (!(controller.brake <= 0.1f) && !@lock)
		{
			if ((num > 2f || !(controller.brake <= 1f)) && !flag)
			{
				applyBrake(brakeForce);
				if (carts[0].hasContact)
				{
					carts[0].wheelSparcCounter = 1;
				}
			}
			else
			{
				num2 = engineForce + engineForce * Mathf.Max(0f, 5f - Mathf.Max(0f, 0f - num));
				if (flag)
				{
					num2 = engineForce + engineForce * 6f;
				}
				applyTorque(0f - num2, num);
				if (flag && !(num <= 2f))
				{
					int i = 0;
					Cart[] array = carts;
					for (int length = array.Length; i < length; i++)
					{
						if (array[i].hasContact && !(UnityEngine.Random.value <= 0.5f))
						{
							array[i].wheelSparcCounter = 2;
						}
					}
				}
			}
		}
		bool flag2 = trainSpeedSquared < minimumSpeed * minimumSpeed;
		if (!flag)
		{
			if (!chainPulling)
			{
				if (carts[0].hasContact && !(controller.throttle <= 0.5f) && !(right.y <= 0.25f) && flag2)
				{
					applyForce(chainPullForce * right.y);
					chainPulling = true;
				}
			}
			else
			{
				if (flag2)
				{
					applyForce(chainPullForce);
				}
				if (controller.throttle < 0f || !(right.y >= 0.1f))
				{
					chainPulling = false;
				}
			}
		}
		if (!(controller.throttle <= 0f))
		{
			if (@lock)
			{
				carts[0].rb.isKinematic = false;
				@lock = false;
			}
			if (!chainPulling)
			{
				num2 = engineForce + engineForce * Mathf.Max(0f, 15f - Mathf.Max(0f, num));
				if (flag)
				{
					num2 = engineForce * engineForceLevelMultiply;
				}
				applyTorque(controller.throttle * num2, num);
				if (flag)
				{
					int j = 0;
					Cart[] array2 = carts;
					for (int length2 = array2.Length; j < length2; j++)
					{
						if (array2[j].hasContact && !(UnityEngine.Random.value <= 0.5f))
						{
							array2[j].wheelSparcCounter = 2;
						}
					}
				}
			}
		}
		if (@lock)
		{
			bool isKinematic = carts[0].rb.isKinematic;
			int k = 0;
			Cart[] array3 = carts;
			for (int length3 = array3.Length; k < length3; k++)
			{
				if (!isKinematic)
				{
					array3[k].rb.velocity = array3[k].rb.velocity * 0.96f;
				}
			}
			if (!(carts[0].rb.velocity.sqrMagnitude >= 0.1f) && !isKinematic && carts[0].hasContact)
			{
				carts[0].rb.isKinematic = true;
			}
		}
		if (GameManager.getInstance().isPlaying())
		{
			ticker++;
			if (ticker > cartCount >> 1)
			{
				ticker = 0;
			}
			cartsInAir = 0;
			int num4 = default(int);
			int l = 0;
			Cart[] array4 = carts;
			for (int length4 = array4.Length; l < length4; l++)
			{
				if (!array4[l].hasContact)
				{
					cartsInAir++;
				}
			}
			bool flag3 = false;
			for (num4 = 0; num4 < cartCount; num4++)
			{
				if (ticker == num4 >> 1)
				{
					carts[num4].updateCart();
					if (!(carts[num4].tr.up.y >= 0f) && carts[num4].hasContact)
					{
						flag3 = true;
					}
				}
				if (!carts[num4].hasContact && !disableDefaultMagnet && num4 < cartCount - 2 && !((float)num4 >= (float)cartCount * 0.5f - (float)cartsInAir * 0.6f))
				{
					float num7 = Vector3.Dot(carts[num4].tr.up, carts[num4 + 1].tr.right);
					if (!(num7 <= -0.2f))
					{
						carts[num4].applyDefaultMagnet();
					}
				}
			}
			if (flag3)
			{
				inLoopCounter = 20;
			}
			else if (inLoopCounter > 0)
			{
				inLoopCounter--;
			}
			if (applySeparatePowerToCarts)
			{
				applySeparatePower(carts[0], carts[cartCount - 1]);
				applySeparatePower(carts[0], carts[2]);
				applySeparatePower(carts[cartCount - 3], carts[cartCount - 1]);
			}
		}
		else
		{
			for (int num4 = 0; num4 < cartCount; num4++)
			{
				carts[num4].raycastTrack();
			}
		}
	}

	public virtual void applySeparatePower(Cart cart1, Cart cart2)
	{
		Vector3 vector = cart2.tr.position - cart1.tr.position;
		vector.Normalize();
		vector *= 60f;
		cart1.rb.AddForce(-vector);
		cart2.rb.AddForce(vector);
	}

	public virtual void handleUpdateSounds()
	{
		float num = 0f;
		int num2 = 0;
		for (int i = 0; i < cartCount; i++)
		{
			if (carts[i].hasContact)
			{
				num += carts[i].rb.velocity.sqrMagnitude * 2f;
				num2++;
			}
		}
		if (num2 > 0)
		{
			num = Mathf.Sqrt(num);
			num = num / (float)num2 * soundControllerFactor;
			if (isFlying)
			{
				st.endJump(primaryTr.position);
				isFlying = false;
			}
		}
		else if (!isFlying)
		{
			st.startJump(primaryTr.position);
			isFlying = true;
		}
		if (hasCrashed)
		{
			isFlying = false;
		}
		float num3 = trainSpeedSquared;
		if (hasCrashed)
		{
			num3 = 0f;
		}
		if (hasCrashed)
		{
			num3 = 0f;
			num = 0f;
		}
		soundController.setValue(num);
		if (isFlying)
		{
			float num4 = primaryRigidbody.velocity.x * 0.1f;
			if (!(num4 >= 0f))
			{
				num4 = 0f - num4;
			}
			soundController.setFlying(true, num4);
		}
		else if (chainPulling)
		{
			soundController.setChainPull(chainPulling);
		}
		else
		{
			soundController.setSilence();
		}
	}

	public virtual void Update()
	{
		if (!hasCrashed && checkCrash())
		{
			GameManager.getInstance().crash();
			setCrashed();
		}
		checkState();
	}

	public virtual void setCrashed()
	{
		if (!hasCrashed)
		{
			disableSpecial();
			st.crash(primaryRigidbody.velocity.magnitude);
			int i = 0;
			Cart[] array = carts;
			for (int length = array.Length; i < length; i++)
			{
				UnityEngine.Object.Destroy(array[i].joint);
				array[i].setCrashed();
				hasCrashed = true;
			}
		}
	}

	public virtual void disableSpecial()
	{
		if (special != null)
		{
			special.deactivateSpecial();
		}
	}

	public virtual bool isSpecialActivated()
	{
		return !(special == null) && special.enabled;
	}

	private void checkState()
	{
		bool flag = false;
		float num = 0f;
		float num2 = screamSpeedTreshold * screamSpeedTreshold;
		if (!goingFast && !(trainSpeedSquared <= num2))
		{
			goingFast = true;
			flag = true;
			num += 0.8f;
		}
		else if (goingFast && !(trainSpeedSquared >= num2))
		{
			goingFast = false;
		}
		if (!upsideDown && !(primaryTr.up.y >= 0f))
		{
			upsideDown = true;
			flag = true;
			num += 0.8f;
		}
		else if (upsideDown && !(primaryTr.up.y <= 0f))
		{
			upsideDown = false;
		}
		if (!falling && !(primaryRigidbody.velocity.y >= 0f - screamFallingTreshold))
		{
			falling = true;
			flag = true;
			num += 0.8f;
		}
		else if (falling && !(primaryRigidbody.velocity.y <= 0f - screamFallingTreshold))
		{
			falling = false;
		}
		if (!fallingFromJump && !((float)cartsInAir <= (float)cartCount * 0.75f) && !(primaryRigidbody.velocity.y >= 0f - screamFallingFromJumpTreshold))
		{
			fallingFromJump = true;
			flag = true;
			num += 0.8f;
		}
		else if (fallingFromJump)
		{
			fallingFromJump = false;
		}
		if (!(age >= 180f))
		{
			flag = false;
		}
		if (!hasCrashed && GameManager.getInstance().isPlaying() && flag && !(UnityEngine.Random.value >= screamProbablility))
		{
			if (upsideDown)
			{
				RollerCoasterSounds.getInstance().playScream(num);
			}
			else
			{
				RollerCoasterSounds.getInstance().playScreamWithOneAnimal(0, num);
			}
		}
	}

	private void applyTorque(float amount, float generalSpeed)
	{
		controller.isCurrentControlTouchBased();
		int num = 0;
		float num2 = 0f;
		int i = 0;
		Cart[] array = carts;
		for (int length = array.Length; i < length; i++)
		{
			int num3 = (int)Vector3.Dot(array[i].transform.right, array[i].rb.velocity);
			num2 = amount * 50f;
			if ((!(amount <= 0f) && num3 < 30) || (!(amount >= 0f) && num3 > -30))
			{
				array[i].rb.AddForce(array[i].transform.right * num2);
			}
			else
			{
				array[i].rb.AddForce(array[i].transform.right * num2 * 0.05f);
			}
			num++;
		}
	}

	public virtual void applyForce(float f)
	{
		int i = 0;
		Cart[] array = carts;
		for (int length = array.Length; i < length; i++)
		{
			array[i].rb.AddForce(array[i].transform.right * f * 200f);
		}
	}

	private void applyBrake(float amount)
	{
		bool flag = controller.isCurrentControlTouchBased();
		amount = 1f - amount;
		float num = 1f;
		int num2 = 0;
		int i = 0;
		Cart[] array = carts;
		for (int length = array.Length; i < length; i++)
		{
			if (!array[i].rb.isKinematic)
			{
				num = ((!flag) ? (1f - 0.01f * amount) : (1f - 0.05f * amount));
			}
			array[i].rb.velocity = array[i].rb.velocity * num;
			num2++;
		}
	}

	public virtual Cart getFirstCart()
	{
		return carts[cartCount - 1];
	}

	public virtual Cart getMiddleCart()
	{
		return carts[(int)((float)cartCount * 0.5f)];
	}

	public virtual Cart getLastCart()
	{
		return carts[0];
	}

	public virtual bool isReversing()
	{
		return reversing;
	}

	private bool checkCrash()
	{
		bool result = false;
		int num = default(int);
		int i = 0;
		Cart[] array = carts;
		for (int length = array.Length; i < length; i++)
		{
			if (array[i].crashed)
			{
				result = true;
				break;
			}
		}
		for (num = 1; num < cartCount; num++)
		{
			Vector3 up = carts[num - 1].tr.up;
			Vector3 up2 = carts[num].tr.up;
			if (!(Vector3.Dot(up, up2) >= -0.6f))
			{
				if (carts[num].joint != null)
				{
					UnityEngine.Object.Destroy(carts[num].joint);
				}
				result = true;
				carts[num].setCrashed();
				carts[num - 1].setCrashed();
			}
		}
		return result;
	}

	public virtual bool isInLoop()
	{
		return inLoopCounter > 0;
	}

	public virtual int getCartScoreInCheckPoint(Cart cart, int type)
	{
		int num = 0;
		float magnitude = cart.rb.velocity.magnitude;
		if (type == 0 && !(cart.height <= heightBonusTreshold))
		{
			num += 1 * cart.heartValue;
		}
		else if (type == 1 && !(magnitude <= speedBonusTreshold))
		{
			num += 1 * cart.heartValue;
		}
		return num;
	}

	public virtual void stopSounds()
	{
		soundController.stop();
	}

	public virtual void startSounds()
	{
		soundController.start();
	}

	public virtual int getCartCount()
	{
		return cartCount;
	}

	public virtual int getCartsInAir()
	{
		return cartsInAir;
	}

	public virtual int getPassengerCartCount()
	{
		return cartCount - 1;
	}

	public virtual int getMaxAnimalIndex()
	{
		int num = 0;
		for (int i = 1; i < cartCount; i++)
		{
			int animalIndex = carts[i].animalIndex;
			if (num < animalIndex)
			{
				num = animalIndex;
			}
		}
		return num;
	}

	public virtual bool activateSpecial()
	{
		int result;
		if (special != null && special.ready)
		{
			@lock = false;
			carts[0].rb.isKinematic = false;
			special.activateSpecial();
			result = 1;
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public virtual int getNumberOfToys(int toyType)
	{
		int num = 0;
		for (int i = 1; i < cartCount; i++)
		{
			if (carts[i].getAnimalType() == toyType)
			{
				num++;
			}
		}
		return num;
	}

	public virtual void lockMotion()
	{
		@lock = true;
	}

	public virtual Cart findEmptyCart()
	{
		int[] array = new int[cartCount];
		int num = 0;
		for (int i = 1; i < cartCount; i++)
		{
			if (!carts[i].hasAnimal())
			{
				array[num] = i;
				num++;
			}
		}
		int a = (int)(UnityEngine.Random.value * (float)num);
		return carts[Mathf.Max(a, num - 1)];
	}

	public virtual void onFinish()
	{
		if (!hasCrashed)
		{
			disableSpecial();
			lockMotion();
			int i = 0;
			Cart[] array = carts;
			for (int length = array.Length; i < length; i++)
			{
				array[i].onFinish();
			}
		}
	}

	public virtual void onStopGame()
	{
		disableSpecial();
		lockMotion();
	}
}
