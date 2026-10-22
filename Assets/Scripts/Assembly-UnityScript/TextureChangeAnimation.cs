using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class TextureChangeAnimation : MonoBehaviour
{
	public Material target;

	public Texture[] textures;

	public float randomStartTimeMin;

	public float randomStartTimeMax;

	public float frameTime;

	private float lastFrameTime;

	private float nextStartTime;

	private int runningFrame;

	public TextureChangeAnimation()
	{
		frameTime = 1f / 24f;
	}

	public virtual void Start()
	{
		nextStartTime = getNextTime();
	}

	public virtual void Update()
	{
		if (runningFrame > 0)
		{
			nextFrame();
			if (runningFrame == 0)
			{
				nextStartTime = getNextTime();
			}
		}
		else if (!(Time.time <= nextStartTime))
		{
			nextFrame();
		}
	}

	public virtual void nextFrame()
	{
		if (!(Time.time <= lastFrameTime + frameTime))
		{
			runningFrame = (runningFrame + 1) % Extensions.get_length((System.Array)textures);
			target.mainTexture = textures[runningFrame];
			lastFrameTime = Time.time;
		}
	}

	private float getNextTime()
	{
		return Time.time + UnityEngine.Random.Range(randomStartTimeMin, randomStartTimeMax);
	}

	public virtual void Main()
	{
	}
}
