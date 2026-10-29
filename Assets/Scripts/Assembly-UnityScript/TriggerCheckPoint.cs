using UnityEngine;

public class TriggerCheckPoint : LevelTrigger
{
	public int kind;

	public override void Awake()
	{
		base.Awake();
		handler.playerTrigger = true;
		handler.onceTrigger = true;
	}

	public override void handleAction(Collider other)
	{
		GameManager.getInstance().onCheckPoint(kind);
	}

	public override void setParam(string param, string val)
	{
		if (param == "kind")
		{
			kind = int.Parse(val);
		}
		else
		{
			base.setParam(param, val);
		}
	}

	public override string getParam(string param)
	{
		return (!(param == "kind")) ? base.getParam(param) : (string.Empty + kind);
	}
}
