using UnityEngine;

public class MaterialSwapper : MonoBehaviour
{
	public Material newMaterial;

	public virtual void Start()
	{
		object[] componentsInChildren = gameObject.GetComponentsInChildren(typeof(Renderer), true);
		foreach (object item in componentsInChildren)
		{
			Renderer renderer = item as Renderer;
			renderer.material = newMaterial;
		}
	}
}
