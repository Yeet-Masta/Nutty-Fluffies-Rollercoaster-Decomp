using UnityEngine;

public class LevelCoin : MonoBehaviour
{
	public TriggerHandler handler = new TriggerHandler();

	public virtual void Awake()
	{
		handler.playerTrigger = true;
	}

	public virtual void OnTriggerEnter(Collider other)
	{
		if (handler.canTrigger(other))
		{
			MonoBehaviour.print("COIN TRIGGERED BY " + other.gameObject.name);
			Cart component = other.gameObject.GetComponent<Cart>();
			if (component != null && component.animal != null)
			{
				handler.hasTriggered = true;
				GameManager.getInstance().addAnimalScore(1, component.animalIndex, component);
				Debug.LogError("GIVING COIN");
				transform.parent.gameObject.SetActiveRecursively(false);
			}
		}
	}

	public virtual void OnTriggerExit(Collider other)
	{
	}
}
