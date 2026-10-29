using UnityEngine;

public class OSAPendulum : ObjectScript
{
	public float s;

	public float a;

	public float o;

	public OSAPendulum()
		: base(3)
	{
	}

	public override void doUpdate()
	{
		if (CommonData.getDeviceGeneration() != CommonData.DeviceGeneration.GEN_ANDROID_LOW)
		{
			float swing = Mathf.Sin((Time.time + o) * s) * a;
			float z = originalAngleDegrees + swing;
			Vector3 localEulerAngles = cachedTransform.localEulerAngles;
			localEulerAngles.z = z;
			cachedTransform.localEulerAngles = localEulerAngles;
		}
	}

	public override void setParam(string param, string val)
	{
		switch (param)
		{
		case "s":
			s = float.Parse(val);
			break;
		case "a":
			a = float.Parse(val);
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
		case "a":
			return string.Empty + a;
		case "o":
			return string.Empty + o;
		default:
			return base.getParam(param);
		}
	}
}
