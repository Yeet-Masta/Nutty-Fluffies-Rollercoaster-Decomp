using UnityEngine;

public class TriggerHideObject : LevelTrigger
{
	public override void handleAction(Collider other)
	{
		foreach (GameObject targetObject in targetHandler.targetObjects)
		{
			targetObject.SetActiveRecursively(false);
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
