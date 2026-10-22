using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class AssetPreloader : MonoBehaviour
{
	public bool preloadAllAudioClips;

	public bool preloadAllTextures;

	public GameObject materialUploadModelPrototype;

	public GameObject[] preloadTextureRoots;

	public Material[] preloadMaterials;

	private UnityScript.Lang.Array preloadedTextures;

	public int autoPreloadSteps;

	public int autoPreloadStepsBias;

	private UnityScript.Lang.Array materialUploadModels;

	private int updates;

	private int preloadedMaterials;

	private int preloadedAudioClips;

	public AssetPreloader()
	{
		preloadedTextures = new UnityScript.Lang.Array();
		autoPreloadSteps = 3;
	}

	private void autoPreload(int step)
	{
		object[] array = null;
		step++;
		if (preloadAllTextures)
		{
			if (Extensions.get_length((System.Array)preloadTextureRoots) == 0)
			{
				preloadTextureRoots = ObjectFinder.findRootObjects();
			}
			preloadMaterials = ObjectFinder.findAllMaterials(preloadTextureRoots);
			int num = Extensions.get_length((System.Array)preloadMaterials) * step / autoPreloadSteps;
			for (int i = preloadedMaterials; i < num; i++)
			{
				preloadMaterial(preloadMaterials[i]);
			}
		}
	}

	public virtual void Update()
	{
		int num = updates - autoPreloadStepsBias;
		if (num >= 0 && num < autoPreloadSteps)
		{
			autoPreload(num);
		}
		else if (num > autoPreloadSteps)
		{
			UnityEngine.Object.Destroy(materialUploadModelPrototype);
			UnityEngine.Object.Destroy(gameObject);
			GC.Collect();
			Debug.Log("RKN System.GC.Collect");
		}
		updates++;
	}

	public virtual void preloadAudio(AudioClip clip)
	{
		OneShotAudio.preload(clip);
		preloadedAudioClips++;
	}

	public virtual void preloadMaterial(Material mat)
	{
		if (!(mat.mainTexture == null) && !ArrayTools.contains(preloadedTextures, mat.mainTexture))
		{
			GameObject gameObject = materialUploadModelPrototype;
			if (preloadedMaterials > 0)
			{
				gameObject = (GameObject)UnityEngine.Object.Instantiate(gameObject, gameObject.transform.position, gameObject.transform.rotation);
			}
			gameObject.GetComponent<Renderer>().material = mat;
			gameObject.transform.parent = transform;
			preloadedTextures.Push(mat.mainTexture);
			preloadedMaterials++;
		}
	}
}
