using UnityEngine;

public class MaterialSwapper : MonoBehaviour
{
	public Material newMaterial;

	public virtual void Start()
	{
		object[] componentsInChildren = gameObject.GetComponentsInChildren(typeof(Renderer), true);
		int i = 0;
		object[] array = componentsInChildren;
		for (int length = array.Length; i < length; i++)
		{
			Renderer renderer = array[i] as Renderer;
			renderer.material = newMaterial;
		}
	}
}
