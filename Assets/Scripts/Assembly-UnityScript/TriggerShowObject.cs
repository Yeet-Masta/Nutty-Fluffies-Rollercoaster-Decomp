using System;
using UnityEngine;

[Serializable]
public class TriggerShowObject : LevelTrigger
{
	public override void handleAction(Collider other)
	{
		int i = 0;
		GameObject[] targetObjects = targetHandler.targetObjects;
		for (int length = targetObjects.Length; i < length; i++)
		{
			targetObjects[i].SetActiveRecursively(true);
		}
	}

	public override void setParam(string param, string val)
	{
		if (param == "t")
		{
			targetHandler.setParamTargets(val);
		}
	}

	public override string getParam(string param)
	{
		return (!(param == "t")) ? null : targetHandler.getParamTargets();
	}
}
