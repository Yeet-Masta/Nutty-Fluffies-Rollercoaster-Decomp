using UnityEngine;

public class Magnet : Special
{
	public float distanceTreshold = 3f;

	public GameObject magnetGraphic;

	public int gridSize;

	private int currentFrame;

	private float frameStep;

	private Material effectMaterial;

	private Renderer[] effectRenderers;

	private int tick;

	public override void Start()
	{
		base.Start();
	}

	public override void prepareGraphcics()
	{
		magnetGraphic.GetComponent<Renderer>().enabled = false;
		frameStep = 1f / (float)gridSize;
		effectMaterial = magnetGraphic.GetComponent<Renderer>().material;
		effectRenderers = new Renderer[trainRef.carts.Length - 1];
		Vector3 localPosition = magnetGraphic.transform.localPosition;
		for (int i = 1; i < trainRef.carts.Length; i++)
		{
			GameObject gameObject = null;
			gameObject = i != 0 ? ((GameObject)UnityEngine.Object.Instantiate(magnetGraphic, Vector3.zero, Quaternion.identity)) : magnetGraphic;
			gameObject.transform.parent = trainRef.carts[i].transform;
			gameObject.transform.localPosition = localPosition;
			effectRenderers[i - 1] = gameObject.GetComponent<Renderer>();
			trainRef.carts[i].magnet.SetActiveRecursively(true);
		}
	}

	public override void activateSpecial()
	{
		base.activateSpecial();
		int i = 0;
		Renderer[] array = effectRenderers;
		for (int length = array.Length; i < length; i++)
		{
			array[i].enabled = true;
		}
		tick = 0;
	}

	public virtual void FixedUpdate()
	{
		int column = currentFrame % gridSize;
		int row = currentFrame / gridSize;
		effectMaterial.SetTextureOffset("_MainTex", new Vector2(frameStep * (float)column, frameStep * (float)row));
		currentFrame++;
		if (currentFrame >= gridSize * gridSize)
		{
			currentFrame = 0;
		}
		for (int i = 0; i < effectRenderers.Length; i++)
		{
			if (!(startTime + duration - Time.time >= 1.5f))
			{
				tick++;
				effectRenderers[i].enabled = (tick & 0x20) == 0;
			}
		}
		if (!(duration <= Time.time - startTime))
		{
			int j = 0;
			Cart[] carts = trainRef.carts;
			for (int length = carts.Length; j < length; j++)
			{
				if (Physics.Raycast(carts[j].tr.position, -carts[j].tr.up, distanceTreshold, LevelSpline.SPLINE_LAYER))
				{
					float pull = power;
					if (!(Vector3.Dot(carts[j].tr.up, carts[j].rb.velocity) >= 0f))
					{
						pull *= 3f;
					}
					if (!carts[j].rb.isKinematic && !(carts[j].rb.velocity.sqrMagnitude <= 600f))
					{
						carts[j].rb.velocity = carts[j].rb.velocity * 0.992f;
					}
					carts[j].wheels[0].rb.AddForce(-carts[j].tr.up * pull);
				}
			}
		}
		else
		{
			deactivateSpecial();
		}
	}

	public override void deactivateSpecial()
	{
		base.deactivateSpecial();
		int i = 0;
		Renderer[] array = effectRenderers;
		for (int length = array.Length; i < length; i++)
		{
			array[i].enabled = false;
		}
	}
}
