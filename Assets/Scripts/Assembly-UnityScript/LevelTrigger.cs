using UnityEngine;

public class LevelTrigger : LevelEntity, Parametrizable
{
	public TriggerHandler handler;

	public string sourceParams;

	public TargetHandler targetHandler;

	public LevelTrigger()
	{
		handler = new TriggerHandler();
	}

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
		object result;
		while (true)
		{
			if (num2 < length)
			{
				if (tRIGGER_SCRIPT_NAMES[num2] == GetType().Name)
				{
					result = LevelTriggerData.TRIGGER_SCRIPT_PARAMS[num];
					break;
				}
				num++;
				num2++;
				continue;
			}
			result = null;
			break;
		}
		return (ParamList)result;
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
		int num = 0;
		string[] array = sourceParams.Split("|"[0]);
		if (array.Length > parameterList.@params.Length)
		{
			return;
		}
		int i = 0;
		ParamList.Param[] array2 = parameterList.@params;
		for (int length = array2.Length; i < length; i++)
		{
			if (num < array.Length)
			{
				setParam(array2[i].tag, array[num++]);
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
