using System;
using System.Collections;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class LayerTools : MonoBehaviour
{
	public static void setLayerRecursively(GameObject g, int to)
	{
		g.layer = to;
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(g.transform);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is Transform))
			{
				obj = RuntimeServices.Coerce(obj, typeof(Transform));
			}
			Transform transform = (Transform)obj;
			setLayerRecursively(transform.gameObject, to);
			UnityRuntimeServices.Update(enumerator, transform);
		}
	}

	public virtual void Main()
	{
	}
}
