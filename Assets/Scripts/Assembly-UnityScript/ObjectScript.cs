using System;
using UnityEngine;

public class ObjectScript : MonoBehaviour, Parametrizable
{
	public const int OBJECT_SCRIPT_NONE = 0;

	public const int OBJECT_SCRIPT_ROTATE = 1;

	public const int OBJECT_SCRIPT_TRANSLATE = 2;

	public const int OBJECT_SCRIPT_PENDULUM = 3;

	public const int OBJECT_SCRIPT_WAVE_TRANSLATE = 4;

	public const int OBJECT_SCRIPT_TRAVELLER = 5;

	public static readonly Type[] OBJECT_SCRIPTS = new Type[6]
	{
		null,
		typeof(OSARotate),
		typeof(OSATranslate),
		typeof(OSAPendulum),
		typeof(OSAWaveTranslate),
		typeof(OSATraveller)
	};

	public static readonly ParamList[] OBJECT_SCRIPT_PARAMS = new ParamList[6]
	{
		null,
		new ParamList(new ParamList.Param[1]
		{
			new ParamList.Param("s", ParamList.ParamType.FLOAT, "Rotation speed")
		}),
		new ParamList(new ParamList.Param[4]
		{
			new ParamList.Param("x", ParamList.ParamType.FLOAT, "X-Speed"),
			new ParamList.Param("y", ParamList.ParamType.FLOAT, "Y-Speed"),
			new ParamList.Param("w", ParamList.ParamType.BOOLEAN, "Wrap on screen edges"),
			new ParamList.Param("m", ParamList.ParamType.FLOAT, "Wrap margin")
		}),
		new ParamList(new ParamList.Param[4]
		{
			new ParamList.Param("s", ParamList.ParamType.FLOAT, "Swing speed"),
			new ParamList.Param("a", ParamList.ParamType.FLOAT, "Swing angle"),
			new ParamList.Param("o", ParamList.ParamType.FLOAT, "Time offset"),
			new ParamList.Param("act", ParamList.ParamType.BOOLEAN, "Active?")
		}),
		new ParamList(new ParamList.Param[6]
		{
			new ParamList.Param("s", ParamList.ParamType.FLOAT, "Wave speed"),
			new ParamList.Param("xa", ParamList.ParamType.FLOAT, "X amplitude"),
			new ParamList.Param("ya", ParamList.ParamType.FLOAT, "Y amplitude"),
			new ParamList.Param("t", ParamList.ParamType.FLOAT, "unused"),
			new ParamList.Param("o", ParamList.ParamType.FLOAT, "Time offset"),
			new ParamList.Param("act", ParamList.ParamType.BOOLEAN, "Active?")
		}),
		new ParamList(new ParamList.Param[6]
		{
			new ParamList.Param("t", ParamList.ParamType.OBJECT_TARGETS, "Visited waypoints"),
			new ParamList.Param("s", ParamList.ParamType.FLOAT, "Speed"),
			new ParamList.Param("w", ParamList.ParamType.FLOAT, "Wait time"),
			new ParamList.Param("o", ParamList.ParamType.BOOLEAN, "Only one per activation"),
			new ParamList.Param("l", ParamList.ParamType.BOOLEAN, "Loop route"),
			new ParamList.Param("act", ParamList.ParamType.BOOLEAN, "Active?")
		})
	};

	public int type;

	public LevelEntity entity;

	public string sourceParams;

	public TargetHandler targetHandler;

	protected float originalAngleDegrees;

	protected Transform cachedTransform;

	public bool triggerActive;

	public ObjectScript(int _type)
	{
		triggerActive = true;
		type = _type;
	}

	public virtual void Awake()
	{
		targetHandler = new TargetHandler(gameObject);
	}

	public virtual void doInit()
	{
	}

	public virtual void doUpdate()
	{
	}

	public virtual void doFixedUpdate()
	{
	}

	public virtual void prepare()
	{
		if (!gameObject || !(LevelEntity)gameObject.GetComponent(typeof(LevelEntity)) || string.IsNullOrEmpty(((LevelEntity)gameObject.GetComponent(typeof(LevelEntity))).scriptParams))
		{
			return;
		}
		ParamList parameterList = getParameterList();
		int num = 0;
		string[] array = ((LevelEntity)gameObject.GetComponent(typeof(LevelEntity))).scriptParams.Split("|"[0]);
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

	public virtual void Start()
	{
		cachedTransform = gameObject.transform;
		originalAngleDegrees = cachedTransform.eulerAngles.z;
		if (!CommonData.EDITOR_ACTIVE || !CommonData.DISABLE_ANIMATIONS)
		{
			doInit();
		}
	}

	public virtual void Update()
	{
		if ((!CommonData.EDITOR_ACTIVE || !CommonData.DISABLE_ANIMATIONS) && triggerActive)
		{
			doUpdate();
		}
	}

	public virtual void FixedUpdate()
	{
		if ((!CommonData.EDITOR_ACTIVE || !CommonData.DISABLE_ANIMATIONS) && triggerActive)
		{
			doFixedUpdate();
		}
	}

	public virtual bool isBatchable()
	{
		return false;
	}

	public virtual void setManualActive(bool yes)
	{
		triggerActive = yes;
	}

	public virtual void setParam(string param, string val)
	{
		if (param == "act")
		{
			setManualActive(val == "yes");
		}
	}

	public virtual string getParam(string param)
	{
		string result;
		if (param == "act")
		{
			result = ((!triggerActive) ? "no" : "yes");
		}
		else
		{
			result = null;
		}
		return result;
	}

	public virtual ParamList getParameterList()
	{
		return OBJECT_SCRIPT_PARAMS[type];
	}
}
