using System;
using UnityEngine;

public class Hover : Special
{
	private float phase;

	public float phaseSpeed;

	public float phaseCartOffset;

	public float forwardPower;

	public float linearPowerDamping;

	public float constantPower;

	public GameObject flameGraphic;

	public int flameFrames;

	public int loopStart;

	public int loopEnd;

	public int frameTime;

	private int currentFlameFrame;

	private float frameStep;

	private Renderer[] flameRenderers;

	public Hover()
	{
		phaseSpeed = 0.1f;
		phaseCartOffset = 0.6f;
		forwardPower = 50f;
		frameTime = 2;
	}

	public override void Start()
	{
		base.Start();
		flameGraphic.GetComponent<Renderer>().enabled = false;
		frameStep = 1f / (float)flameFrames;
		flameRenderers = new Renderer[trainRef.carts.Length - 1];
		Vector3 localPosition = flameGraphic.transform.localPosition;
		int num = 0;
		for (int i = 1; i < trainRef.carts.Length; i++)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(flameGraphic, Vector3.zero, Quaternion.identity);
			gameObject.transform.parent = trainRef.carts[i].jumpRocketFlame;
			gameObject.transform.localPosition = localPosition;
			gameObject.GetComponent<Renderer>().material = new Material(gameObject.GetComponent<Renderer>().sharedMaterial);
			flameRenderers[num++] = gameObject.GetComponent<Renderer>();
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
			float num = 1f;
			float num2 = phase;
			for (int num3 = trainRef.carts.Length - 1; num3 >= 0; num3--)
			{
				Cart cart = trainRef.carts[num3];
				float num4 = 1f;
				if (!(num2 >= (float)Math.PI / 2f) && !(num2 <= 0f))
				{
					float num5 = power * Mathf.Cos(num2);
					cart.rb.AddForce(cart.tr.up * num * num5 * num4);
					cart.rb.AddForce(cart.tr.right * forwardPower * num4 * Mathf.Cos(num2));
				}
				cart.rb.AddForce(cart.tr.up * constantPower * num4 * num);
				num2 -= phaseCartOffset;
				num -= linearPowerDamping / (float)trainRef.carts.Length;
				float num6 = power * Mathf.Cos(num2);
				if (num3 > 0 && !(num6 <= 0f) && !(num2 >= (float)Math.PI / 2f))
				{
					float x = Mathf.Cos(num2);
					Vector3 localScale = cart.jumpRocketFlame.localScale;
					localScale.x = x;
					cart.jumpRocketFlame.localScale = localScale;
					float y = Mathf.Cos(num2);
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
		int num = currentFlameFrame / frameTime;
		for (int i = 0; i < flameRenderers.Length; i++)
		{
			int currentRenderQueue = trainRef.carts[i + 1].currentRenderQueue;
			flameRenderers[i].sharedMaterial.SetTextureOffset("_MainTex", new Vector2(frameStep * (float)num, 0f));
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
