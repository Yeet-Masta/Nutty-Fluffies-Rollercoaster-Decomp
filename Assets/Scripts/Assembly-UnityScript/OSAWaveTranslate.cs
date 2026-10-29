using UnityEngine;

public class OSAWaveTranslate : ObjectScript
{
	public float s;

	public float xa;

	public float ya;

	public float t;

	public float o;

	private Vector3 origPos;

	public OSAWaveTranslate()
		: base(4)
	{
	}

	public override void doInit()
	{
		origPos = cachedTransform.position;
	}

	public override void doFixedUpdate()
	{
		float num = Mathf.Sin((Time.time + o) * s);
		float x = origPos.x + num * xa;
		float y = origPos.y + num * ya;
		cachedTransform.position = new Vector3(x, y, origPos.z);
	}

	public override void setParam(string param, string val)
	{
		switch (param)
		{
		case "s":
			s = float.Parse(val);
			break;
		case "xa":
			xa = float.Parse(val);
			break;
		case "ya":
			ya = float.Parse(val);
			break;
		case "t":
			t = float.Parse(val);
			break;
		case "o":
			o = float.Parse(val);
			break;
		}
	}

	public override string getParam(string param)
	{
		string result;
		switch (param)
		{
		case "s":
			result = string.Empty + s;
			break;
		case "xa":
			result = string.Empty + xa;
			break;
		case "ya":
			result = string.Empty + ya;
			break;
		case "t":
			result = string.Empty + t;
			break;
		case "o":
			result = string.Empty + o;
			break;
		default:
			result = base.getParam(param);
			break;
		}
		return result;
	}
}
