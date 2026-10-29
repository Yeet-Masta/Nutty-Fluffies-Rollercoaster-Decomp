using UnityEngine;

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
		float step = cachedTransform.lossyScale.x * Time.deltaTime;
		cachedTransform.position += new Vector3(x * step, y * step, 0f);
		if (w)
		{
			float orthographicSize = cameraRef.orthographicSize;
			float halfWidth = orthographicSize * cameraRef.aspect;
			if (!(x <= 0f) && !(cachedTransform.position.x <= cameraTrans.position.x + halfWidth + m * cachedTransform.lossyScale.x / 2f))
			{
				float wrappedX = cachedTransform.position.x - (halfWidth + m * cachedTransform.lossyScale.x / 2f) * 2f;
				Vector3 position = cachedTransform.position;
				position.x = wrappedX;
				cachedTransform.position = position;
			}
			if (!(x >= 0f) && !(cachedTransform.position.x >= cameraTrans.position.x - halfWidth - m * cachedTransform.lossyScale.x / 2f))
			{
				float wrappedX = cachedTransform.position.x + (halfWidth + m * cachedTransform.lossyScale.x / 2f) * 2f;
				Vector3 position2 = cachedTransform.position;
				position2.x = wrappedX;
				cachedTransform.position = position2;
			}
			if (!(y <= 0f) && !(cachedTransform.position.y <= cameraTrans.position.y + orthographicSize + m * cachedTransform.lossyScale.y / 2f))
			{
				float wrappedY = cachedTransform.position.y - (orthographicSize + m * cachedTransform.lossyScale.y / 2f) * 2f;
				Vector3 position3 = cachedTransform.position;
				position3.y = wrappedY;
				cachedTransform.position = position3;
			}
			if (!(y >= 0f) && !(cachedTransform.position.y >= cameraTrans.position.y - orthographicSize - m * cachedTransform.lossyScale.y / 2f))
			{
				float wrappedY = cachedTransform.position.y + (orthographicSize + m * cachedTransform.lossyScale.y / 2f) * 2f;
				Vector3 position4 = cachedTransform.position;
				position4.y = wrappedY;
				cachedTransform.position = position4;
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
		switch (param)
		{
		case "x":
			return string.Empty + x;
		case "y":
			return string.Empty + y;
		case "w":
			return w ? "yes" : "no";
		case "m":
			return string.Empty + m;
		default:
			return base.getParam(param);
		}
	}
}
