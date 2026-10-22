using System;
using UnityEngine;

[Serializable]
public class LevelDataPrefabs : MonoBehaviour
{
	[NonSerialized]
	public static LevelDataPrefabs instance;

	public Level levelPrefab;

	public LevelLayer layerPrefab;

	public GameObject triggerPrefab;

	public GameObject objectTexturePrefab;

	public LevelObject objectPrefab;

	public LevelObject objectWithTexturePrefab;

	public GameObject objectColliderPrefab;

	public LevelGroup groupPrefab;

	public GameObject coinPrefab;

	public LevelSpline splinePrefab;

	public GameObject splineSectionPrefab;

	public Texture2D placeholderTexture;

	public Shader alphaShader;

	public Shader nonAlphaShader;

	public ParticleEmitter[] emitters;

	public PhysicMaterial[] physicMaterials;

	public virtual void Awake()
	{
		instance = this;
		useGUILayout = false;
	}

	public static LevelDataPrefabs getInstance()
	{
		if (instance == null)
		{
			Debug.LogError("ADD LevelDataPrefabs prefab TO YOUR SCENE! :(\n");
		}
		return instance;
	}
}
