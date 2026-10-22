using System;
using System.Collections;
using Boo.Lang.Runtime;
using UnityScript.Lang;

[Serializable]
public class ArrayTools
{
	public static bool contains(UnityScript.Lang.Array a, object o)
	{
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(a);
		int result;
		while (true)
		{
			if (enumerator.MoveNext())
			{
				object current = enumerator.Current;
				if (RuntimeServices.EqualityOperator(current, o))
				{
					result = 1;
					break;
				}
				continue;
			}
			result = 0;
			break;
		}
		return (byte)result != 0;
	}
}
