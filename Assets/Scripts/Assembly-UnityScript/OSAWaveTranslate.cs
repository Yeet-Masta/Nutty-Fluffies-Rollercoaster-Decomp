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
		float wave = Mathf.Sin((Time.time + o) * s);
		float x = origPos.x + wave * xa;
		float y = origPos.y + wave * ya;
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
		switch (param)
		{
		case "s":
			return string.Empty + s;
		case "xa":
			return string.Empty + xa;
		case "ya":
			return string.Empty + ya;
		case "t":
			return string.Empty + t;
		case "o":
			return string.Empty + o;
		default:
			return base.getParam(param);
		}
	}
}
