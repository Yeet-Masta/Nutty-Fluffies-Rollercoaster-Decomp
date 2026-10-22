using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class CollisionMultiAudioPlayer : CollisionAudioPlayer
{
	public PhysicMaterial[] targetMaterials;

	public int[] startIndices;

	public int[] amounts;

	public override void OnCollisionEnter(Collision collision)
	{
		float magnitude = collision.relativeVelocity.magnitude;
		if (!(Time.time >= lastPlayTime + CollisionAudioPlayer.replayMinimumWait) || !(magnitude >= CollisionAudioPlayer.minimumSpeed))
		{
			return;
		}
		int num = -1;
		PhysicMaterial sharedMaterial = collision.collider.sharedMaterial;
		if (sharedMaterial != null)
		{
			for (int i = 0; i < Extensions.get_length((System.Array)targetMaterials); i++)
			{
				if (sharedMaterial == targetMaterials[i])
				{
					num = i;
				}
			}
		}
		if (num != -1)
		{
			int num2 = startIndices[num];
			num = UnityEngine.Random.Range(num2, num2 + amounts[num]);
			GeneralSoundEffects.playSound(clips[num], getVolumeFromColPower(magnitude));
			lastPlayTime = Time.time;
		}
	}
}
