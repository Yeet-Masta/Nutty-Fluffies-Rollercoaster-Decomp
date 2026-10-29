using UnityEngine;

public class TriggerEnablePhysics : LevelTriggerActivatable
{
	public bool c;

	public override void handleAction(Collider other)
	{
		activateTargets();
	}

	public override void setManualActive(bool yes)
	{
		base.setManualActive(yes);
		if (yes)
		{
			activateTargets();
		}
	}

	private void activateTargets()
	{
		if (CommonData.EDITOR_ACTIVE)
		{
			return;
		}
		int i = 0;
		GameObject[] targetObjects = targetHandler.targetObjects;
		for (int length = targetObjects.Length; i < length; i++)
		{
			Component[] bodies = null;
			bodies = targetObjects[i].gameObject.GetComponentsInChildren(typeof(Rigidbody));
			foreach (Component component in bodies)
			{
				Rigidbody rigidbody = component as Rigidbody;
				rigidbody.useGravity = true;
				rigidbody.isKinematic = false;
			}
			if (c)
			{
				Component[] componentsInChildren = targetObjects[i].gameObject.GetComponentsInChildren(typeof(BoxCollider));
				foreach (Component component in componentsInChildren)
				{
					((BoxCollider)component).enabled = false;
				}
				Component[] componentsInChildren2 = targetObjects[i].gameObject.GetComponentsInChildren(typeof(SphereCollider));
				foreach (Component component in componentsInChildren2)
				{
					((SphereCollider)component).enabled = false;
				}
			}
		}
	}

	public override void prepare()
	{
		base.prepare();
		handler.playerTrigger = true;
		foreach (GameObject targetObject in targetHandler.targetObjects)
		{
			Component[] bodies = null;
			bodies = targetObject.gameObject.GetComponentsInChildren(typeof(Rigidbody));
			foreach (Component component in bodies)
			{
				Rigidbody rigidbody = component as Rigidbody;
				rigidbody.useGravity = false;
				rigidbody.isKinematic = true;
			}
		}
	}

	public override void setParam(string param, string val)
	{
		if (param == "t")
		{
			targetHandler.setParamTargets(val);
		}
		else if (param == "c")
		{
			c = val == "yes";
		}
		else
		{
			base.setParam(param, val);
		}
	}

	public override string getParam(string param)
	{
		if (param == "t")
		{
			return targetHandler.getParamTargets();
		}
		if (param == "c")
		{
			return c ? "yes" : "no";
		}
		return base.getParam(param);
	}
}
