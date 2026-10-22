using System;
using UnityEngine;

[Serializable]
public class MouseTools : MonoBehaviour
{
	public static void hideCursor()
	{
		if (!Application.isEditor)
		{
			Cursor.visible = false;
		}
	}

	public static void showCursor()
	{
		if (!Application.isEditor)
		{
			Cursor.visible = true;
		}
	}

	public virtual void Main()
	{
	}
}
