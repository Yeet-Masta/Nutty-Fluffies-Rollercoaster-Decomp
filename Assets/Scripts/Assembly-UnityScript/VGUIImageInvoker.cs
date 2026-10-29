using UnityEngine;

public class VGUIImageInvoker : VGUIImage
{
	public GameObject invokeObject;

	public string invokeMethod;

	public float invokeTime;

	public VGUIImageInvokerValueType valueType = VGUIImageInvokerValueType.none;

	public int intValue;

	public string stringValue;

	public bool booleanValue;

	public float floatValue;

	public virtual bool activateInvoke(bool blocking)
	{
		if (invokeObject == null || invokeMethod == null)
		{
			return false;
		}
		else
		{
			if (invokeTime == 0f)
			{
				executeInvoke();
			}
			else
			{
				Invoke("executeInvoke", invokeTime);
				VGUIElement.blockingButtonPressed = blocking;
			}
			return true;
		}
	}

	public virtual void cancelPendingInvoke()
	{
		CancelInvoke("executeInvoke");
	}

	public virtual void immediatePendingInvoke()
	{
		if (IsInvoking("executeInvoke"))
		{
			cancelPendingInvoke();
			executeInvoke();
		}
	}

	private void executeInvoke()
	{
		invokeObject.active = true;
		if (valueType == VGUIImageInvokerValueType.integer)
		{
			invokeObject.SendMessage(invokeMethod, intValue);
		}
		else if (valueType == VGUIImageInvokerValueType.@string)
		{
			invokeObject.SendMessage(invokeMethod, stringValue);
		}
		else if (valueType == VGUIImageInvokerValueType.boolean)
		{
			invokeObject.SendMessage(invokeMethod, booleanValue);
		}
		else if (valueType == VGUIImageInvokerValueType.@float)
		{
			invokeObject.SendMessage(invokeMethod, floatValue);
		}
		else
		{
			invokeObject.SendMessage(invokeMethod);
		}
		VGUIElement.blockingButtonPressed = false;
	}
}
