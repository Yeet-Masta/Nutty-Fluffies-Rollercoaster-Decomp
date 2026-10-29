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
			int j = 0;
			Component[] array = componentsInChildren;
			for (int length2 = array.Length; j < length2; j++)
			{
				(array[j] as LevelTrigger).setManualActive(s);
			}
			componentsInChildren = targetObjects[i].gameObject.GetComponentsInChildren(typeof(ObjectScript));
			int k = 0;
			Component[] array2 = componentsInChildren;
			for (int length3 = array2.Length; k < length3; k++)
			{
				(array2[k] as ObjectScript).setManualActive(s);
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
		object result;
		switch (param)
		{
		case "t":
			result = targetHandler.getParamTargets();
			break;
		case "s":
			result = s ? "yes" : "no";
			break;
		case "o":
			result = o ? "yes" : "no";
			break;
		default:
			result = null;
			break;
		}
		return (string)result;
	}
}
