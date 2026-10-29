using UnityEngine;

public class TriggerCoin : LevelTrigger
{
	public int amount;

	public override void Awake()
	{
		base.Awake();
		handler.playerTrigger = true;
		handler.onceTrigger = true;
	}

	public override void prepare()
	{
		base.prepare();
		transform.parent.gameObject.SetActiveRecursively(true);
	}

	public override void handleAction(Collider other)
	{
		Cart component = other.gameObject.GetComponent<Cart>();
		if (component != null)
		{
			handler.hasTriggered = true;
			GameManager.getInstance().addAnimalScore(1, component.animalIndex, component);
			transform.parent.gameObject.SetActiveRecursively(false);
		}
	}

	public override void setParam(string param, string val)
	{
		if (param == "amount")
		{
			amount = int.Parse(val);
		}
		else
		{
			base.setParam(param, val);
		}
	}

	public override string getParam(string param)
	{
		return (!(param == "amount")) ? base.getParam(param) : (string.Empty + amount);
	}
}
