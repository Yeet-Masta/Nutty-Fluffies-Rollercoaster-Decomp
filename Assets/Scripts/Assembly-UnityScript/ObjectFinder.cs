using System;
using System.Collections;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class ObjectFinder : MonoBehaviour
{
	public static GameObject[] findRootObjects()
	{
		object[] array = (GameObject[])UnityEngine.Object.FindObjectsOfType(typeof(GameObject));
		int num = default(int);
		ArrayList arrayList = new ArrayList();
		for (num = 0; num < Extensions.get_length((System.Array)array); num++)
		{
			if ((array[num] as GameObject).transform.root == (array[num] as GameObject).transform)
			{
				arrayList.Add(array[num]);
			}
		}
		return (GameObject[])arrayList.ToArray(typeof(GameObject));
	}

	public static UnityScript.Lang.Array findAllObjectsOfType(Type type)
	{
		UnityScript.Lang.Array array = findRootObjects();
		UnityScript.Lang.Array array2 = new UnityScript.Lang.Array();
		for (int i = 0; i < array.length; i++)
		{
			Component[] componentsInChildren = (array[i] as GameObject).GetComponentsInChildren(type, true);
			int j = 0;
			Component[] array3 = componentsInChildren;
			for (int length = array3.Length; j < length; j++)
			{
				array2.Push(array3[j]);
			}
		}
		return array2;
	}

	public static Material[] findAllMaterials(GameObject[] fromRoots)
	{
		int num = default(int);
		ArrayList arrayList = new ArrayList();
		for (num = 0; num < Extensions.get_length((System.Array)fromRoots); num++)
		{
			object[] componentsInChildren = fromRoots[num].GetComponentsInChildren(typeof(Renderer), true);
			int i = 0;
			object[] array = componentsInChildren;
			for (int length = array.Length; i < length; i++)
			{
				object obj = array[i];
				if (!(obj is Renderer))
				{
					obj = RuntimeServices.Coerce(obj, typeof(Renderer));
				}
				if (!(((Renderer)obj).material != null))
				{
					continue;
				}
				object obj2 = array[i];
				if (!(obj2 is Renderer))
				{
					obj2 = RuntimeServices.Coerce(obj2, typeof(Renderer));
				}
				if (!arrayList.Contains(((Renderer)obj2).material))
				{
					object obj3 = array[i];
					if (!(obj3 is Renderer))
					{
						obj3 = RuntimeServices.Coerce(obj3, typeof(Renderer));
					}
					arrayList.Add(((Renderer)obj3).material);
				}
			}
		}
		return (Material[])arrayList.ToArray(typeof(Material));
	}

	public virtual void Main()
	{
	}
}
