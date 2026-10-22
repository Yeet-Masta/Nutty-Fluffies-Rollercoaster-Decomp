using System;
using UnityEngine;

[Serializable]
public class RemoveWithQuality : MonoBehaviour
{
	// NFPort: was the obsolete QualityLevel enum; saved scenes store the same index.
	public int fromLevel;

	public virtual void Awake()
	{
		if (QualitySettings.GetQualityLevel() <= fromLevel)
		{
			UnityEngine.Object.Destroy(gameObject);
		}
	}
}
