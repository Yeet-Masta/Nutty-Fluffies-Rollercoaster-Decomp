using UnityEngine;

public class LevelTrigger : LevelEntity, Parametrizable
{
	public TriggerHandler handler = new TriggerHandler();

	public string sourceParams;

	public TargetHandler targetHandler;

	public virtual void OnTriggerEnter(Collider other)
	{
		if (handler.canTrigger(other))
		{
			handleAction(other);
			handler.hasTriggered = true;
		}
	}

	public virtual ParamList getParameterList()
	{
		int num = 0;
		int num2 = 0;
		string[] tRIGGER_SCRIPT_NAMES = LevelTriggerData.TRIGGER_SCRIPT_NAMES;
		int length = tRIGGER_SCRIPT_NAMES.Length;
		while (num2 < length)
		{
			if (tRIGGER_SCRIPT_NAMES[num2] == GetType().Name)
			{
				return LevelTriggerData.TRIGGER_SCRIPT_PARAMS[num];
			}
			num++;
			num2++;
		}
		return null;
	}

	public virtual void prepare()
	{
		handler.playerTrigger = true;
		handler.hasTriggered = false;
		if (string.IsNullOrEmpty(sourceParams) || sourceParams.Length <= 0)
		{
			return;
		}
		ParamList parameterList = getParameterList();
		int paramIndex = 0;
		string[] values = sourceParams.Split("|"[0]);
		if (values.Length > parameterList.@params.Length)
		{
			return;
		}
		foreach (ParamList.Param param in parameterList.@params)
		{
			if (paramIndex < values.Length)
			{
				setParam(param.tag, values[paramIndex++]);
			}
		}
	}

	public override void Awake()
	{
		targetHandler = new TargetHandler(gameObject);
	}

	public virtual void setParam(string param, string val)
	{
	}

	public virtual string getParam(string param)
	{
		return null;
	}

	public virtual void OnTriggerExit(Collider other)
	{
	}

	public virtual void handleAction(Collider other)
	{
	}

	public virtual void setManualActive(bool yes)
	{
	}
}
