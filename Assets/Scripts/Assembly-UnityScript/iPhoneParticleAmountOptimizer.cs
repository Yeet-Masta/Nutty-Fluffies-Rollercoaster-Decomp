using System;
using UnityEngine;

[Serializable]
public class iPhoneParticleAmountOptimizer : MonoBehaviour
{
	[NonSerialized]
	public static int requiredGeneration = 3;

	[NonSerialized]
	private static float oppEmissionMultiply = 0.5f;

	[NonSerialized]
	private static float oppEnergyMultiply = 0.5f;

	public virtual void Main()
	{
	}
}
