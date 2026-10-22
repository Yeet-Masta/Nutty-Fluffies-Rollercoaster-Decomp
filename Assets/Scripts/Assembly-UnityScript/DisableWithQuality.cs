using System;
using UnityEngine;

[Serializable]
public class DisableWithQuality : MonoBehaviour
{
	// NFPort: was the obsolete QualityLevel enum; saved scenes store the same index.
	public int fromLevel;

	public bool disableQualityCheck;

	public virtual void Awake()
	{
		if (!disableQualityCheck && QualitySettings.GetQualityLevel() <= fromLevel)
		{
			gameObject.SetActiveRecursively(false);
		}
	}

	public virtual void OnEnable()
	{
		if (!disableQualityCheck && QualitySettings.GetQualityLevel() <= fromLevel)
		{
			gameObject.SetActiveRecursively(false);
		}
	}
}
