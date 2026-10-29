using UnityEngine;

public class LevelTriggerActivatable : LevelTrigger
{
	public bool triggerActive;

	public override void setManualActive(bool yes)
	{
		triggerActive = yes;
		if (yes)
		{
			(GetComponent(GetType()) as MonoBehaviour).enabled = true;
		}
	}

	public override void setParam(string param, string val)
	{
		if (param == "act")
		{
			setManualActive(val == "yes");
		}
		else
		{
			base.setParam(param, val);
		}
	}

	public override string getParam(string param)
	{
		if (param == "act")
		{
			return triggerActive ? "yes" : "no";
		}
		else
		{
			return base.getParam(param);
		}
	}
}
