using System;
using UnityEngine;

public class TriggerParticles : LevelTriggerActivatable
{
	public ParticleEmitter pe;

	public int pt;

	public float ds;

	public float xs;

	public float xm;

	public float ys;

	public float ym;

	public float ss;

	public float sm;

	public float ns;

	public float lf;

	private float startTime;

	private int numSinceStart;

	private float emitAdvance;

	public override void Awake()
	{
		base.Awake();
		handler.playerTrigger = true;
		handler.onceTrigger = true;
		handler.hasTriggered = true;
	}

	public override void handleAction(Collider other)
	{
	}

	public override void setManualActive(bool yes)
	{
		if (triggerActive != yes && yes)
		{
			startTime = Time.realtimeSinceStartup;
			numSinceStart = 0;
			emitAdvance = 0f;
		}
		base.setManualActive(yes);
	}

	public override void prepare()
	{
		base.prepare();
		setAmitter();
	}

	private void setAmitter()
	{
		if ((bool)pe)
		{
			UnityEngine.Object.Destroy(pe.gameObject);
		}
		if (LevelObjectFactory.getInstance() != null && (bool)LevelObjectFactory.getInstance().getPrefabs() && (bool)LevelObjectFactory.getInstance().getPrefabs().emitters[pt])
		{
			pe = (ParticleEmitter)UnityEngine.Object.Instantiate(LevelObjectFactory.getInstance().getPrefabs().emitters[pt]);
			pe.transform.parent = transform;
			pe.transform.localPosition = Vector3.zero;
			pe.emit = false;
			if (lf == 0f)
			{
				lf = UnityEngine.Random.Range(1, 2);
			}
		}
	}

	public virtual void Update()
	{
		if (triggerActive)
		{
			emitAdvance += Time.deltaTime;
			while ((float)numSinceStart < emitAdvance * ns)
			{
				Vector3 velocity = new Vector3(xs + UnityEngine.Random.value * (xm - xs), ys + UnityEngine.Random.value * (ym - ys), 0f);
				float size = ss + UnityEngine.Random.value * (sm - ss);
				Vector3 position = transform.position;
				pe.Emit(position, velocity, size, lf, Color.white);
				numSinceStart++;
			}
			if (!(ds < 0f) && !(Mathf.Abs(Time.realtimeSinceStartup - startTime) <= ds))
			{
				triggerActive = false;
			}
		}
	}

	public override void setParam(string param, string val)
	{
		try
		{
			switch (param)
			{
			case "pt":
				pt = int.Parse(val);
				setAmitter();
				break;
			case "ns":
				ns = float.Parse(val);
				break;
			case "ds":
				ds = float.Parse(val);
				break;
			case "xs":
				xs = float.Parse(val);
				break;
			case "xm":
				xm = float.Parse(val);
				break;
			case "ys":
				ys = float.Parse(val);
				break;
			case "ym":
				ym = float.Parse(val);
				break;
			case "ss":
				ss = float.Parse(val);
				break;
			case "sm":
				sm = float.Parse(val);
				break;
			case "lf":
				lf = float.Parse(val);
				break;
			default:
				base.setParam(param, val);
				break;
			}
		}
		catch (Exception)
		{
			Debug.Log("INVALID FORMAT: '" + val + "' for param '" + param + "' in " + GetType());
		}
	}

	public override string getParam(string param)
	{
		switch (param)
		{
		case "pt":
			return string.Empty + pt;
		case "ds":
			return string.Empty + ds;
		case "ns":
			return string.Empty + ns;
		case "xs":
			return string.Empty + xs;
		case "xm":
			return string.Empty + xm;
		case "ys":
			return string.Empty + ys;
		case "ym":
			return string.Empty + ym;
		case "ss":
			return string.Empty + ss;
		case "sm":
			return string.Empty + sm;
		case "lf":
			return string.Empty + lf;
		default:
			return base.getParam(param);
		}
	}
}
