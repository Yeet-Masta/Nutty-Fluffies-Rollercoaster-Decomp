using System;
using UnityEngine;

[Serializable]
public class EffectManager : MonoBehaviour
{
	[NonSerialized]
	private static EffectManager instance;

	public ParticleEmitter dustEmitter;

	public ParticleEmitter sparkEmitter;

	public ParticleEmitter starEmitter;

	public static EffectManager getInstance()
	{
		return instance;
	}

	public virtual void Awake()
	{
		instance = this;
		useGUILayout = false;
		sparkEmitter.gameObject.GetComponent<ParticleRenderer>().materials[0].renderQueue = 3200;
	}

	public virtual void emitDust(Vector3 position, int count)
	{
		position.z--;
		for (int i = 0; i < count; i++)
		{
			Vector3 velocity = new Vector3(0f, UnityEngine.Random.value * 0.1f, 0f);
			float size = 2f + UnityEngine.Random.value;
			Vector3 pos = position;
			pos.x += UnityEngine.Random.value * 0.4f - 0.2f;
			pos.y += UnityEngine.Random.value * 0.4f - 0.2f;
			dustEmitter.Emit(pos, velocity, size, 1.5f, Color.white);
		}
	}

	public virtual void emitSmoke(Vector3 position, int count)
	{
		position.z--;
		for (int i = 0; i < count; i++)
		{
			Vector3 velocity = new Vector3(0f, UnityEngine.Random.value * 0.1f, 0f);
			float size = 1f + UnityEngine.Random.value;
			dustEmitter.Emit(position, velocity, size, 1.5f, Color.white);
		}
	}

	public virtual void emitSparks(Vector3 position, Vector3 speed, int count)
	{
		position.z--;
		sparkEmitter.transform.position = position;
		sparkEmitter.worldVelocity = Vector3.zero;
		sparkEmitter.Emit((int)((float)count * 1.5f));
	}

	public virtual void emitStars(Vector3 position, int count)
	{
		position.z--;
		starEmitter.transform.position = position;
		starEmitter.Emit(count);
	}

	public virtual void reset()
	{
		starEmitter.emit = false;
		dustEmitter.emit = false;
		sparkEmitter.emit = false;
	}
}
