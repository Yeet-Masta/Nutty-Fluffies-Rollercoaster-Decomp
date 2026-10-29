using UnityEngine;

public class TriggerBoost : LevelTriggerActivatable
{
	public float bx;

	public float by;

	public override void handleAction(Collider other)
	{
		if (!triggerActive)
		{
			return;
		}
		Rigidbody rigidbody = ((Rigidbody)other.gameObject.GetComponent(typeof(Rigidbody))) as Rigidbody;
		if (!rigidbody || !handler.isVehicle(other))
		{
			return;
		}
		rigidbody.AddForce(new Vector3(bx * 0.5f, by * 0.5f, 0f));
		if (!GameManager.getInstance().getVehicle().hasCrashed)
		{
			foreach (Cart cart in GameManager.getInstance().getVehicle().carts)
			{
				cart.rb.AddForce(new Vector3(bx * 0.1f, by * 0.1f, 0f));
			}
		}
	}

	public virtual void OnTriggerStay(Collider other)
	{
		handleAction(other);
	}

	public override void setParam(string param, string val)
	{
		if (param == "bx")
		{
			bx = float.Parse(val);
		}
		else if (param == "by")
		{
			by = float.Parse(val);
		}
		else
		{
			base.setParam(param, val);
		}
	}

	public override string getParam(string param)
	{
		string result;
		if (param == "bx")
		{
			result = string.Empty + bx;
		}
		else
		{
			result = param == "by" ? (string.Empty + by) : base.getParam(param);
		}
		return result;
	}
}
