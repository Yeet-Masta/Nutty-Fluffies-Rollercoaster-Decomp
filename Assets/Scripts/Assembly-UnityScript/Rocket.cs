using System;
using UnityEngine;

[Serializable]
public class Rocket : Special
{
	public float downForce;

	public GameObject flameGraphic;

	public int flameFrames;

	public int loopStart;

	public int loopEnd;

	private int currentFlameFrame;

	private float frameStep;

	private Material flameMaterial;

	public int frameTime;

	public Transform smokeSpot;

	private int smokeCounter;

	public Rocket()
	{
		downForce = 0.1f;
		frameTime = 2;
	}

	public override void Start()
	{
		base.Start();
		flameGraphic.GetComponent<Renderer>().enabled = false;
		frameStep = 1f / (float)flameFrames;
		flameMaterial = flameGraphic.GetComponent<Renderer>().material;
		Vector3 localPosition = flameGraphic.transform.localPosition;
		DriverCart component = trainRef.carts[0].gameObject.GetComponent<DriverCart>();
		component.rocket.SetActiveRecursively(true);
		flameGraphic.transform.parent = component.rocketFlame;
		flameGraphic.transform.localPosition = localPosition;
	}

	public override void activateSpecial()
	{
		base.activateSpecial();
		currentFlameFrame = 0;
		flameGraphic.GetComponent<Renderer>().enabled = true;
		smokeCounter = 0;
		smokeInvoke();
	}

	public virtual void FixedUpdate()
	{
		if (!(duration <= Time.time - startTime))
		{
			Cart lastCart = trainRef.getLastCart();
			Vector3 velocity = lastCart.rb.velocity;
			float num = 1f;
			num += (20f - Mathf.Min(20f, velocity.magnitude)) * 0.5f;
			if (!(lastCart.transform.InverseTransformDirection(velocity).x >= 0f))
			{
				num = 10f;
			}
			int i = 0;
			Cart[] carts = trainRef.carts;
			for (int length = carts.Length; i < length; i++)
			{
				Vector3 force = carts[i].tr.right * power * num - carts[i].tr.up * downForce;
				carts[i].rb.AddForce(force);
			}
		}
		else
		{
			deactivateSpecial();
		}
		smokeCounter = 0;
		trainRef.controller.setAcceleratePressed(true);
	}

	public virtual void Update()
	{
		int num = currentFlameFrame / frameTime;
		flameMaterial.SetTextureOffset("_MainTex", new Vector2(frameStep * (float)num, 0f));
		flameMaterial.renderQueue = trainRef.carts[0].currentRenderQueue;
		currentFlameFrame++;
		if (currentFlameFrame >= loopEnd * frameTime)
		{
			currentFlameFrame = loopStart * frameTime;
		}
		smokeInvoke();
	}

	public override void prepare()
	{
		base.prepare();
		flameGraphic.GetComponent<Renderer>().enabled = false;
		currentFlameFrame = 0;
	}

	public override void deactivateSpecial()
	{
		base.deactivateSpecial();
		flameGraphic.GetComponent<Renderer>().enabled = false;
	}

	public virtual void smokeInvoke()
	{
		if (smokeCounter % 10 == 0)
		{
			EffectManager.getInstance().emitSmoke(smokeSpot.position, 1);
		}
		smokeCounter++;
	}
}
