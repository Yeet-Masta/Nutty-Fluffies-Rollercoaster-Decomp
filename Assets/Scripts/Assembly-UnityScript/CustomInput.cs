using System;
using UnityEngine;

[Serializable]
public class CustomInput
{
	private static bool m_disableInput;

	public static void disableInput(bool disable)
	{
		m_disableInput = disable;
	}

	public static bool getKeyDown(int keyCode)
	{
		if (m_disableInput)
		{
			Input.ResetInputAxes();
			return !m_disableInput;
		}
		return Input.GetKeyDown((KeyCode)keyCode);
	}
}
