using System;
using UnityEngine;

[Serializable]
public class CombineChildren : MonoBehaviour
{
	public virtual void Awake()
	{
		StaticBatchingUtility.Combine(gameObject);
	}

	public virtual void Main()
	{
	}
}
