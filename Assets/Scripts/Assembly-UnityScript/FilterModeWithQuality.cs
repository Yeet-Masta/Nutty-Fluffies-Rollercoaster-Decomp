using System;
using UnityEngine;

[Serializable]
public class FilterModeWithQuality : MonoBehaviour
{
	// NFPort: was the obsolete QualityLevel enum; saved scenes store the same index.
	public int fromLevel;

	public FilterMode filterMode;

	private FilterMode originalFilterMode;

	public virtual void Awake()
	{
		originalFilterMode = GetComponent<Renderer>().material.mainTexture.filterMode;
		if (QualitySettings.GetQualityLevel() <= fromLevel)
		{
			GetComponent<Renderer>().material.mainTexture.filterMode = filterMode;
		}
	}

	public virtual void OnDisable()
	{
		GetComponent<Renderer>().material.mainTexture.filterMode = originalFilterMode;
	}
}
