using UnityEngine;

public class OSATraveller : ObjectScript
{
	public float s;

	public float w;

	public bool o;

	public bool l;

	public Vector3 origPos;

	private int waypointIndex;

	private bool waiting;

	private float lastChangeTime;

	private Vector3 srcPos;

	private Vector3 dstPos;

	public OSATraveller()
		: base(5)
	{
	}

	public override void doInit()
	{
		origPos = cachedTransform.position;
		lastChangeTime = Time.time;
		waypointIndex = -1;
		startMove(-1, 0);
	}

	public override void setManualActive(bool yes)
	{
		base.setManualActive(yes);
		lastChangeTime = Time.time;
	}

	private void startMove(int fr, int to)
	{
		Debug.Log("FR: " + fr + ", TO: " + to);
		if (fr == -1)
		{
			srcPos = origPos;
		}
		else
		{
			srcPos = targetHandler.targetObjects[fr].transform.position;
		}
		dstPos = targetHandler.targetObjects[to].transform.position;
	}

	public override void doFixedUpdate()
	{
		if (waiting)
		{
			if (!(Mathf.Abs(Time.time - lastChangeTime) <= w))
			{
				waiting = false;
				lastChangeTime = Time.time;
			}
			return;
		}
		float num = (Time.time - lastChangeTime) / s;
		if (!(num <= 1f))
		{
			num = 1f;
		}
		float num2 = 1f - (0.5f + 0.5f * Mathf.Cos(num * 3.14f));
		cachedTransform.position = srcPos + (dstPos - srcPos) * num2;
		if (Mathf.Abs(Time.time - lastChangeTime) < s)
		{
			return;
		}
		waiting = true;
		waypointIndex++;
		if (waypointIndex >= targetHandler.targetObjects.Length)
		{
			if (!l && !CommonData.EDITOR_ACTIVE)
			{
				triggerActive = false;
			}
			waypointIndex = 0;
		}
		if (o && !CommonData.EDITOR_ACTIVE)
		{
			triggerActive = false;
		}
		startMove(waypointIndex, waypointIndex < targetHandler.targetObjects.Length - 1 ? (waypointIndex + 1) : 0);
		lastChangeTime = Time.time;
	}

	public override void setParam(string param, string val)
	{
		switch (param)
		{
		case "s":
			s = float.Parse(val);
			break;
		case "w":
			w = float.Parse(val);
			break;
		case "o":
			o = val == "yes";
			break;
		case "l":
			l = val == "yes";
			break;
		case "t":
			targetHandler.setParamTargets(val);
			break;
		default:
			base.setParam(param, val);
			break;
		}
	}

	public override string getParam(string param)
	{
		switch (param)
		{
		case "s":
			return string.Empty + s;
		case "w":
			return string.Empty + w;
		case "o":
			return o ? "yes" : "no";
		case "l":
			return l ? "yes" : "no";
		case "t":
			return targetHandler.getParamTargets();
		default:
			return base.getParam(param);
		}
	}
}
