using System;
using UnityEngine;

[Serializable]
public class OSARotate : ObjectScript
{
	public float s;

	public OSARotate()
		: base(1)
	{
	}

	public override void doUpdate()
	{
		cachedTransform.Rotate(0f, 0f, s * Time.deltaTime);
	}

	public override void setParam(string param, string val)
	{
		if (param == "s")
		{
			s = float.Parse(val);
		}
		else
		{
			base.setParam(param, val);
		}
	}

	public override string getParam(string param)
	{
		return (!(param == "s")) ? base.getParam(param) : (string.Empty + s);
	}
}
