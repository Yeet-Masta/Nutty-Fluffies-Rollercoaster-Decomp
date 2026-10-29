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
			float num = Mathf.Sin((Time.time + o) * s) * a;
			float z = originalAngleDegrees + num;
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
		string result;
		switch (param)
		{
		case "s":
			result = string.Empty + s;
			break;
		case "a":
			result = string.Empty + a;
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
