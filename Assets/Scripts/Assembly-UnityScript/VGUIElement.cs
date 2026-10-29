using UnityEngine;

public class VGUIElement : MonoBehaviour
{
	protected static bool blockingButtonPressed;

	protected Transform t;

	protected int runningCoRoutinesAmount;

	public virtual void Awake()
	{
		useGUILayout = false;
		t = transform;
	}

	public virtual void activate()
	{
		gameObject.SetActiveRecursively(true);
	}

	public virtual void deactivate()
	{
		gameObject.SetActiveRecursively(false);
	}

	public virtual bool isActive()
	{
		return gameObject.active;
	}

	public virtual void setActive(bool to)
	{
		gameObject.SetActiveRecursively(to);
	}

	public virtual void setActiveIfNot(bool to)
	{
		if (gameObject.active != to)
		{
			gameObject.SetActiveRecursively(to);
		}
	}

	public virtual VGUIElement getChildElement(string name)
	{
		Component[] componentsInChildren = gameObject.GetComponentsInChildren(typeof(VGUIElement), true);
		int num = 0;
		Component[] array = componentsInChildren;
		int length = array.Length;
		object result;
		while (true)
		{
			if (num < length)
			{
				if (array[num].name == name)
				{
					result = array[num] as VGUIElement;
					break;
				}
				num++;
				continue;
			}
			result = null;
			break;
		}
		return (VGUIElement)result;
	}

	public static bool isBlockingButtonPressed()
	{
		return blockingButtonPressed;
	}

	public virtual void onBackButton()
	{
	}

	public virtual void Update()
	{
	}
}
