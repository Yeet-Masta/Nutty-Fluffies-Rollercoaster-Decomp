using System.Collections.Generic;
using UnityEngine;

public class TriggerActivateObject : LevelTrigger
{
	public bool s;

	public bool o;

	private bool onceActivated;

	public override void handleAction(Collider other)
	{
		if (o && onceActivated)
		{
			return;
		}
		print("would activate " + string.Join(", ", targetNames()));
		int i = 0;
		GameObject[] targetObjects = targetHandler.targetObjects;
		for (int length = targetObjects.Length; i < length; i++)
		{
			Component[] componentsInChildren = targetObjects[i].gameObject.GetComponentsInChildren(typeof(LevelTrigger));
			foreach (Component component in componentsInChildren)
			{
				(component as LevelTrigger).setManualActive(s);
			}
			componentsInChildren = targetObjects[i].gameObject.GetComponentsInChildren(typeof(ObjectScript));
			foreach (Component component in componentsInChildren)
			{
				(component as ObjectScript).setManualActive(s);
			}
		}
		onceActivated = true;
	}

	private IEnumerable<string> targetNames()
	{
		foreach (GameObject target in targetHandler.targetObjects)
		{
			yield return target.ToString();
		}
	}

	public override void setParam(string param, string val)
	{
		switch (param)
		{
		case "t":
			targetHandler.setParamTargets(val);
			break;
		case "s":
			s = val == "yes";
			break;
		case "o":
			o = val == "yes";
			break;
		}
	}

	public override string getParam(string param)
	{
		switch (param)
		{
		case "t":
			return targetHandler.getParamTargets();
		case "s":
			return s ? "yes" : "no";
		case "o":
			return o ? "yes" : "no";
		default:
			return null;
		}
	}
}
