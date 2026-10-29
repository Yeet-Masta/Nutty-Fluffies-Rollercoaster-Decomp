using System.Collections.Generic;
using UnityEngine;

public class VGUIList : VGUIElement
{
	public GameObject entryPrefab;

	public float verticalSpacing = 0.2f;

	protected List<GameObject> entryRefs = new List<GameObject>();

	public virtual GameObject createEntry()
	{
		return createEntry(entryPrefab);
	}

	public virtual GameObject createEntry(GameObject prefab)
	{
		GameObject gameObject = ((GameObject)UnityEngine.Object.Instantiate(prefab, Vector3.zero, Quaternion.identity)) as GameObject;
		gameObject.transform.parent = transform;
		gameObject.transform.localPosition = calculatePosition();
		gameObject.transform.localScale = prefab.transform.localScale;
		entryRefs.Add(gameObject);
		return gameObject;
	}

	public virtual Vector3 calculatePosition()
	{
		return new Vector3(0f, (float)(-entryRefs.Count) * verticalSpacing, 0f);
	}

	public virtual void clearList()
	{
		foreach (GameObject gameObject in entryRefs)
		{
			UnityEngine.Object.Destroy(gameObject.gameObject);
		}
		entryRefs = new List<GameObject>();
	}

	public virtual void compress()
	{
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < entryRefs.Count; i++)
		{
			if (entryRefs[i] != null)
			{
				list.Add(entryRefs[i]);
			}
		}
		entryRefs = list;
	}

	public virtual List<GameObject> getEntryList()
	{
		return entryRefs;
	}
}
