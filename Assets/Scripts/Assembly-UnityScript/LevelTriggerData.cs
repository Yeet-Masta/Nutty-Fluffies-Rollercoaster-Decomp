using System;
using UnityEngine;

[Serializable]
public class LevelTriggerData : LevelEntityData
{
	public const int TRIGGER_SCRIPT_FINISH = 0;

	public const int TRIGGER_SCRIPT_CHECKPOINT = 1;

	public const int TRIGGER_SCRIPT_HIDE_OBJECT = 2;

	public const int TRIGGER_SCRIPT_SHOW_OBJECT = 3;

	public const int TRIGGER_SCRIPT_RESET = 4;

	public const int TRIGGER_SCRIPT_ENABLE_PHYSICS = 5;

	public const int TRIGGER_SCRIPT_BOOST = 6;

	public const int TRIGGER_SCRIPT_COIN = 7;

	public const int TRIGGER_SCRIPT_ACTIVATE_OBJECT = 8;

	public const int TRIGGER_SCRIPT_PARTICLES = 9;

	public static readonly string[] TRIGGER_SCRIPT_NAMES = new string[10] { "TriggerFinish", "TriggerCheckPoint", "TriggerHideObject", "TriggerShowObject", "TriggerReset", "TriggerEnablePhysics", "TriggerBoost", "TriggerCoin", "TriggerActivateObject", "TriggerParticles" };

	public static readonly ParamList[] TRIGGER_SCRIPT_PARAMS = new ParamList[10]
	{
		new ParamList(new ParamList.Param[0]),
		new ParamList(new ParamList.Param[1]
		{
			new ParamList.Param("kind", ParamList.ParamType.INT, "Type")
		}),
		new ParamList(new ParamList.Param[1]
		{
			new ParamList.Param("t", ParamList.ParamType.OBJECT_TARGETS, "Objects which will be hidden")
		}),
		new ParamList(new ParamList.Param[1]
		{
			new ParamList.Param("t", ParamList.ParamType.OBJECT_TARGETS, "Objects which will be shown")
		}),
		new ParamList(new ParamList.Param[0]),
		new ParamList(new ParamList.Param[3]
		{
			new ParamList.Param("t", ParamList.ParamType.OBJECT_TARGETS, "Objects which will have physics activated"),
			new ParamList.Param("c", ParamList.ParamType.BOOLEAN, "Disable colliders"),
			new ParamList.Param("act", ParamList.ParamType.BOOLEAN, "Active")
		}),
		new ParamList(new ParamList.Param[3]
		{
			new ParamList.Param("bx", ParamList.ParamType.FLOAT, "X Boost amount"),
			new ParamList.Param("by", ParamList.ParamType.FLOAT, "Y Boost amount"),
			new ParamList.Param("act", ParamList.ParamType.BOOLEAN, "Active")
		}),
		new ParamList(new ParamList.Param[1]
		{
			new ParamList.Param("amount", ParamList.ParamType.INT, "Point amount")
		}),
		new ParamList(new ParamList.Param[3]
		{
			new ParamList.Param("t", ParamList.ParamType.OBJECT_TARGETS, "Objects which will be activated"),
			new ParamList.Param("s", ParamList.ParamType.BOOLEAN, "New State (yes = activate, no = deactivate)"),
			new ParamList.Param("o", ParamList.ParamType.BOOLEAN, "Only once per run?")
		}),
		new ParamList(new ParamList.Param[11]
		{
			new ParamList.Param("pt", ParamList.ParamType.EMITTER, "Particle Type"),
			new ParamList.Param("ds", ParamList.ParamType.FLOAT, "Emit Duration (s)"),
			new ParamList.Param("xs", ParamList.ParamType.FLOAT, "Min X Speed"),
			new ParamList.Param("xm", ParamList.ParamType.FLOAT, "Max X Speed"),
			new ParamList.Param("ys", ParamList.ParamType.FLOAT, "Min Y Speed"),
			new ParamList.Param("ym", ParamList.ParamType.FLOAT, "Max Y Speed"),
			new ParamList.Param("ss", ParamList.ParamType.FLOAT, "Min Size"),
			new ParamList.Param("sm", ParamList.ParamType.FLOAT, "Max Size"),
			new ParamList.Param("act", ParamList.ParamType.BOOLEAN, "Active"),
			new ParamList.Param("ns", ParamList.ParamType.FLOAT, "Particle Amount Per Second"),
			new ParamList.Param("lf", ParamList.ParamType.FLOAT, "Energy in seconds")
		})
	};

	public const int TRIGGER_BOX = 1;

	public const int TRIGGER_SPHERE = 2;

	public int triggerType;

	public string triggerScript;

	public string triggerParam;

	public Vector2 size;

	public LevelTriggerData(int referenceId, int type)
		: base(referenceId, 1)
	{
		triggerParam = string.Empty;
		size = default(Vector2);
		triggerType = type;
	}

	public static int getTriggerActionIndex(LevelTrigger trigger)
	{
		int num = 0;
		int num2 = 0;
		string[] tRIGGER_SCRIPT_NAMES = TRIGGER_SCRIPT_NAMES;
		int length = tRIGGER_SCRIPT_NAMES.Length;
		while (num2 < length)
		{
			if (tRIGGER_SCRIPT_NAMES[num2] == trigger.GetType().Name)
			{
				return num;
			}
			num++;
			num2++;
		}
		Debug.LogError("NO SCRIPT EXIST: " + trigger.GetType().Name);
		return -1;
	}

	public virtual void setAction(string script, string param)
	{
		triggerScript = script;
		triggerParam = param;
	}
}
