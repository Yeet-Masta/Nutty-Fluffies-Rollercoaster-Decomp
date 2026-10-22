using System;
using UnityEngine;

[Serializable]
public class HUDUVTools
{
	public static Vector2 fromHUDUVToScreenCoordinates(Vector2 from, Vector2 screenSize, bool clampToBorders)
	{
		if (clampToBorders)
		{
			if (!(from.x >= 0f))
			{
				from.x = 0f;
			}
			else if (!(from.x <= 1f))
			{
				from.x = 1f;
			}
			if (!(from.y >= 0f))
			{
				from.y = 0f;
			}
			else if (!(from.y <= 1f))
			{
				from.y = 1f;
			}
		}
		return new Vector2(from.x * screenSize.x, (1f - from.y) * screenSize.y);
	}

	public static Vector2 fromScreenCoordinatesToHUDUV(Vector2 from, Vector2 screenSize, bool clampToBorders)
	{
		if (clampToBorders)
		{
			if (!(from.x >= 0f))
			{
				from.x = 0f;
			}
			else if (!(from.x <= screenSize.x))
			{
				from.x = screenSize.x;
			}
			if (!(from.y >= 0f))
			{
				from.y = 0f;
			}
			else if (!(from.y <= screenSize.y))
			{
				from.y = screenSize.y;
			}
		}
		return new Vector2(from.x / screenSize.x, 1f - from.y / screenSize.y);
	}
}
