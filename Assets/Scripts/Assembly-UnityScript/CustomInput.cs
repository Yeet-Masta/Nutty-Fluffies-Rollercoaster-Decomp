using System;
using UnityEngine;

[Serializable]
public class CustomInput
{
	[NonSerialized]
	private static bool m_disableInput;

	public static void disableInput(bool disable)
	{
		m_disableInput = disable;
	}

	public static bool getKeyDown(int keyCode)
	{
		bool result;
		if (m_disableInput)
		{
			Input.ResetInputAxes();
			if (GameData.LOG)
			{
				Debug.Log("m_disableInput m_disableInput :: " + Input.GetKeyDown((KeyCode)keyCode));
			}
			result = !m_disableInput;
		}
		else
		{
			result = Input.GetKeyDown((KeyCode)keyCode);
		}
		return result;
	}
}
