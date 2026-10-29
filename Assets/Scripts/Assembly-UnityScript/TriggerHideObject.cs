using UnityEngine;

public class TriggerHideObject : LevelTrigger
{
	public override void handleAction(Collider other)
	{
		int i = 0;
		GameObject[] targetObjects = targetHandler.targetObjects;
		for (int length = targetObjects.Length; i < length; i++)
		{
			targetObjects[i].SetActiveRecursively(false);
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
		return param == "t" ? targetHandler.getParamTargets() : null;
	}
}
