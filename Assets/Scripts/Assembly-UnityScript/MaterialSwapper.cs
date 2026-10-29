using UnityEngine;

public class MaterialSwapper : MonoBehaviour
{
	public Material newMaterial;

	public virtual void Start()
	{
		object[] componentsInChildren = gameObject.GetComponentsInChildren(typeof(Renderer), true);
		foreach (object component in componentsInChildren)
		{
			Renderer renderer = component as Renderer;
			renderer.material = newMaterial;
		}
	}
}
