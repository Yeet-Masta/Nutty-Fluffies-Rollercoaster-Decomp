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
			Component[] array = null;
			array = targetObjects[i].gameObject.GetComponentsInChildren(typeof(Rigidbody));
			int j = 0;
			Component[] array2 = array;
			for (int length2 = array2.Length; j < length2; j++)
			{
				Rigidbody rigidbody = array2[j] as Rigidbody;
				rigidbody.useGravity = true;
				rigidbody.isKinematic = false;
			}
			if (c)
			{
				Component[] componentsInChildren = targetObjects[i].gameObject.GetComponentsInChildren(typeof(BoxCollider));
				int k = 0;
				Component[] array3 = componentsInChildren;
				for (int length3 = array3.Length; k < length3; k++)
				{
					((BoxCollider)array3[k]).enabled = false;
				}
				Component[] componentsInChildren2 = targetObjects[i].gameObject.GetComponentsInChildren(typeof(SphereCollider));
				int l = 0;
				Component[] array4 = componentsInChildren2;
				for (int length4 = array4.Length; l < length4; l++)
				{
					((SphereCollider)array4[l]).enabled = false;
				}
			}
		}
	}

	public override void prepare()
	{
		base.prepare();
		handler.playerTrigger = true;
		int i = 0;
		GameObject[] targetObjects = targetHandler.targetObjects;
		for (int length = targetObjects.Length; i < length; i++)
		{
			Component[] array = null;
			array = targetObjects[i].gameObject.GetComponentsInChildren(typeof(Rigidbody));
			int j = 0;
			Component[] array2 = array;
			for (int length2 = array2.Length; j < length2; j++)
			{
				Rigidbody rigidbody = array2[j] as Rigidbody;
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
		string result;
		if (param == "t")
		{
			result = targetHandler.getParamTargets();
		}
		else if (param == "c")
		{
			result = ((!c) ? "no" : "yes");
		}
		else
		{
			result = base.getParam(param);
		}
		return result;
	}
}
