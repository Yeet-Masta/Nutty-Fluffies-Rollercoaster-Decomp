using System;
using UnityEngine;

[Serializable]
public class OSATranslate : ObjectScript
{
	public float x;

	public float y;

	public bool w;

	public float m;

	public Camera cameraRef;

	public Transform cameraTrans;

	public bool wasVis;

	public OSATranslate()
		: base(2)
	{
	}

	public override void doInit()
	{
		cameraRef = Camera.main;
		cameraTrans = cameraRef.transform;
	}

	public override void doUpdate()
	{
		float num = cachedTransform.lossyScale.x * Time.deltaTime;
		cachedTransform.position += new Vector3(x * num, y * num, 0f);
		if (w)
		{
			float orthographicSize = cameraRef.orthographicSize;
			float num2 = orthographicSize * cameraRef.aspect;
			if (!(x <= 0f) && !(cachedTransform.position.x <= cameraTrans.position.x + num2 + m * cachedTransform.lossyScale.x / 2f))
			{
				float num3 = cachedTransform.position.x - (num2 + m * cachedTransform.lossyScale.x / 2f) * 2f;
				Vector3 position = cachedTransform.position;
				float num4 = (position.x = num3);
				Vector3 vector = (cachedTransform.position = position);
			}
			if (!(x >= 0f) && !(cachedTransform.position.x >= cameraTrans.position.x - num2 - m * cachedTransform.lossyScale.x / 2f))
			{
				float num5 = cachedTransform.position.x + (num2 + m * cachedTransform.lossyScale.x / 2f) * 2f;
				Vector3 position2 = cachedTransform.position;
				float num6 = (position2.x = num5);
				Vector3 vector3 = (cachedTransform.position = position2);
			}
			if (!(y <= 0f) && !(cachedTransform.position.y <= cameraTrans.position.y + orthographicSize + m * cachedTransform.lossyScale.y / 2f))
			{
				float num7 = cachedTransform.position.y - (orthographicSize + m * cachedTransform.lossyScale.y / 2f) * 2f;
				Vector3 position3 = cachedTransform.position;
				float num8 = (position3.y = num7);
				Vector3 vector5 = (cachedTransform.position = position3);
			}
			if (!(y >= 0f) && !(cachedTransform.position.y >= cameraTrans.position.y - orthographicSize - m * cachedTransform.lossyScale.y / 2f))
			{
				float num9 = cachedTransform.position.y + (orthographicSize + m * cachedTransform.lossyScale.y / 2f) * 2f;
				Vector3 position4 = cachedTransform.position;
				float num10 = (position4.y = num9);
				Vector3 vector7 = (cachedTransform.position = position4);
			}
		}
	}

	public override void setParam(string param, string val)
	{
		switch (param)
		{
		case "x":
			x = float.Parse(val);
			break;
		case "y":
			y = float.Parse(val);
			break;
		case "w":
			w = val == "yes";
			break;
		case "m":
			m = float.Parse(val);
			break;
		}
	}

	public override string getParam(string param)
	{
		string result;
		switch (param)
		{
		case "x":
			result = string.Empty + x;
			break;
		case "y":
			result = string.Empty + y;
			break;
		case "w":
			result = ((!w) ? "no" : "yes");
			break;
		case "m":
			result = string.Empty + m;
			break;
		default:
			result = base.getParam(param);
			break;
		}
		return result;
	}
}
