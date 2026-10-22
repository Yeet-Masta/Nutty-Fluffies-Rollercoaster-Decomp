using System;
using Boo.Lang.Runtime;
using UnityEngine;

[Serializable]
public class ShaderReplacer : MonoBehaviour
{
	// NFPort: was the obsolete QualityLevel enum; saved scenes store the same index.
	public int fromLevel;

	public Shader fromShader;

	public Shader toShader;

	public virtual void Awake()
	{
		if (QualitySettings.GetQualityLevel() <= fromLevel)
		{
			replaceShaders(fromShader, toShader);
		}
	}

	public static void replaceShaders(Shader ofType, Shader toType)
	{
		object[] array = (Material[])UnityEngine.Object.FindObjectsOfType(typeof(Material));
		int i = 0;
		object[] array2 = array;
		for (int length = array2.Length; i < length; i++)
		{
			object obj = array2[i];
			if (!(obj is Material))
			{
				obj = RuntimeServices.Coerce(obj, typeof(Material));
			}
			if (((Material)obj).shader == ofType)
			{
				object obj2 = array2[i];
				if (!(obj2 is Material))
				{
					obj2 = RuntimeServices.Coerce(obj2, typeof(Material));
				}
				((Material)obj2).shader = toType;
			}
		}
	}
}
