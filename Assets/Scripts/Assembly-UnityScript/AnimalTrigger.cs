using UnityEngine;

public class AnimalTrigger : MonoBehaviour
{
	public Cart cartRef;

	public BoxCollider trigger;

	public virtual void Awake()
	{
		deactivate();
	}

	public virtual void activate()
	{
		gameObject.active = true;
		trigger.enabled = true;
	}

	public virtual void deactivate()
	{
		trigger.enabled = false;
	}

	public virtual void OnTriggerEnter(Collider other)
	{
		Animal component = other.gameObject.GetComponent<Animal>();
		if (component != null && component.canBePickedUp)
		{
			component.reconnectToCart(cartRef);
			deactivate();
		}
	}
}
