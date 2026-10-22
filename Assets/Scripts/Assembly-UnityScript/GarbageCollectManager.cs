using System;
using UnityEngine;

[Serializable]
public class GarbageCollectManager : MonoBehaviour
{
	public int frameFreq;

	public GarbageCollectManager()
	{
		frameFreq = 30;
	}

	public virtual void Awake()
	{
		Debug.LogError("This is experimental component and should not be used in production!!");
	}

	public virtual void Update()
	{
		if (Time.frameCount % frameFreq == 0)
		{
			GC.Collect();
		}
	}

	public virtual void Main()
	{
	}
}
