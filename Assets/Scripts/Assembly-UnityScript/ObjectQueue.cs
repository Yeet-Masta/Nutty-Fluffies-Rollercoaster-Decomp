using System;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class ObjectQueue : MonoBehaviour
{
	public GameObject pathObjectPrototype;

	public double minimumDistanceBetweenObjects;

	public int alphaArrowAmountAtEnd;

	public int alphaArrowAmountAtStart;

	private double lastObjectIndex;

	private Vector3 lastObjectPosition;

	private GameObject[] objects;

	private bool hiddenObjectsAvailable;

	public virtual void generateObjects(int amount)
	{
		objects = new GameObject[amount];
		for (int i = 0; i < amount; i++)
		{
			GameObject gameObject = (GameObject)UnityEngine.Object.Instantiate(pathObjectPrototype, new Vector3(0f, 0f, 0f), Quaternion.identity);
			gameObject.transform.parent = transform;
			gameObject.active = false;
			objects[i] = gameObject;
		}
		hiddenObjectsAvailable = true;
		lastObjectIndex = -1.0;
	}

	public virtual void setMaterialOfObjects(Material to)
	{
		for (int i = 0; i < Extensions.get_length((System.Array)objects); i++)
		{
			objects[i].GetComponent<Renderer>().material = to;
		}
	}

	public virtual bool placeObject(Vector3 to, Color color)
	{
		int result;
		if (!RuntimeServices.EqualityOperator(lastObjectPosition, null) && !((double)Vector3.Distance(lastObjectPosition, to) >= minimumDistanceBetweenObjects))
		{
			result = 0;
		}
		else
		{
			int num = (int)((lastObjectIndex + 1.0) % (double)Extensions.get_length((System.Array)objects));
			GameObject gameObject = objects[num];
			gameObject.transform.position = to;
			if (gameObject.active)
			{
				hiddenObjectsAvailable = false;
			}
			gameObject.active = true;
			if (!RuntimeServices.EqualityOperator(lastObjectPosition, null))
			{
				Transform transform = objects[(int)lastObjectIndex].transform;
				transform.LookAt(to);
				gameObject.transform.rotation = transform.rotation;
			}
			if (!RuntimeServices.EqualityOperator(color, null))
			{
				gameObject.GetComponent<Renderer>().material.color = color;
			}
			lastObjectIndex = num;
			lastObjectPosition = to;
			result = 1;
		}
		return (byte)result != 0;
	}

	public virtual GameObject getLastObject()
	{
		return objects[(int)lastObjectIndex];
	}

	public virtual void hideNextObject()
	{
		int num = (int)((lastObjectIndex + 1.0) % (double)Extensions.get_length((System.Array)objects));
		GameObject gameObject = objects[num];
		gameObject.active = false;
		lastObjectIndex = num;
	}

	public virtual bool isHiddenObjectsAvailable()
	{
		return hiddenObjectsAvailable;
	}

	public virtual void hideAll()
	{
		for (int i = 0; i < Extensions.get_length((System.Array)objects); i++)
		{
			objects[i].active = false;
		}
		hiddenObjectsAvailable = true;
		lastObjectIndex = -1.0;
	}

	public virtual void Main()
	{
	}
}
