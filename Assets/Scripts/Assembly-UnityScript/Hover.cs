using System;
using UnityEngine;

public class Hover : Special
{
	private float phase;

	public float phaseSpeed = 0.1f;

	public float phaseCartOffset = 0.6f;

	public float forwardPower = 50f;

	public float linearPowerDamping;

	public float constantPower;

	public GameObject flameGraphic;

	public int flameFrames;

	public int loopStart;

	public int loopEnd;

	public int frameTime = 2;

	private int currentFlameFrame;

	private float frameStep;

	private Renderer[] flameRenderers;

	public override void Start()
	{
		base.Start();
		flameGraphic.GetComponent<Renderer>().enabled = false;
		frameStep = 1f / (float)flameFrames;
		flameRenderers = new Renderer[trainRef.carts.Length - 1];
		Vector3 localPosition = flameGraphic.transform.localPosition;
		int index = 0;
		for (int i = 1; i < trainRef.carts.Length; i++)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(flameGraphic, Vector3.zero, Quaternion.identity);
			gameObject.transform.parent = trainRef.carts[i].jumpRocketFlame;
			gameObject.transform.localPosition = localPosition;
			gameObject.GetComponent<Renderer>().material = new Material(gameObject.GetComponent<Renderer>().sharedMaterial);
			flameRenderers[index++] = gameObject.GetComponent<Renderer>();
			trainRef.carts[i].jumpRocket.SetActiveRecursively(true);
		}
		UnityEngine.Object.Destroy(flameGraphic);
	}

	public override void activateSpecial()
	{
		base.activateSpecial();
		phase = 0f;
		int i = 0;
		Renderer[] array = flameRenderers;
		for (int length = array.Length; i < length; i++)
		{
			array[i].enabled = true;
		}
		trainRef.applySeparatePowerToCarts = true;
	}

	public virtual void FixedUpdate()
	{
		if (!(duration <= Time.time - startTime))
		{
			float liftFactor = 1f;
			float cartPhase = phase;
			for (int cartIndex = trainRef.carts.Length - 1; cartIndex >= 0; cartIndex--)
			{
				Cart cart = trainRef.carts[cartIndex];
				float powerScale = 1f;
				if (!(cartPhase >= (float)Math.PI / 2f) && !(cartPhase <= 0f))
				{
					float lift = power * Mathf.Cos(cartPhase);
					cart.rb.AddForce(cart.tr.up * liftFactor * lift * powerScale);
					cart.rb.AddForce(cart.tr.right * forwardPower * powerScale * Mathf.Cos(cartPhase));
				}
				cart.rb.AddForce(cart.tr.up * constantPower * powerScale * liftFactor);
				cartPhase -= phaseCartOffset;
				liftFactor -= linearPowerDamping / (float)trainRef.carts.Length;
				float nextLift = power * Mathf.Cos(cartPhase);
				if (cartIndex > 0 && !(nextLift <= 0f) && !(cartPhase >= (float)Math.PI / 2f))
				{
					float x = Mathf.Cos(cartPhase);
					Vector3 localScale = cart.jumpRocketFlame.localScale;
					localScale.x = x;
					cart.jumpRocketFlame.localScale = localScale;
					float y = Mathf.Cos(cartPhase);
					Vector3 localScale2 = cart.jumpRocketFlame.localScale;
					localScale2.y = y;
					cart.jumpRocketFlame.localScale = localScale2;
				}
			}
			phase += phaseSpeed;
		}
		else
		{
			deactivateSpecial();
		}
	}

	public virtual void Update()
	{
		int frame = currentFlameFrame / frameTime;
		for (int i = 0; i < flameRenderers.Length; i++)
		{
			int currentRenderQueue = trainRef.carts[i + 1].currentRenderQueue;
			flameRenderers[i].sharedMaterial.SetTextureOffset("_MainTex", new Vector2(frameStep * (float)frame, 0f));
			flameRenderers[i].sharedMaterial.renderQueue = currentRenderQueue;
		}
		currentFlameFrame++;
		if (currentFlameFrame >= loopEnd * frameTime)
		{
			currentFlameFrame = loopStart * frameTime;
		}
	}

	public override void deactivateSpecial()
	{
		base.deactivateSpecial();
		int i = 0;
		Renderer[] array = flameRenderers;
		for (int length = array.Length; i < length; i++)
		{
			array[i].enabled = false;
		}
		trainRef.applySeparatePowerToCarts = false;
	}
}
