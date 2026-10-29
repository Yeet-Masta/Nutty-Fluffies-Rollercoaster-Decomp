using UnityEngine;

public class VGUISwitchObject : VGUIElement
{
	public GameObject firstActiveObject;

	private GameObject activeObject;

	public override void Awake()
	{
		base.Awake();
		if (activeObject != null)
		{
			switchTo(activeObject);
		}
		else if (firstActiveObject != null)
		{
			switchTo(firstActiveObject);
		}
		else
		{
			hideAll();
		}
	}

	public virtual void hideAll()
	{
		foreach (Transform transform in this.transform)
		{
			transform.gameObject.SetActiveRecursively(false);
		}
		activeObject = null;
	}

	public virtual void showAll()
	{
		foreach (Transform transform in this.transform)
		{
			transform.gameObject.SetActiveRecursively(true);
		}
	}

	public virtual void switchTo(GameObject to)
	{
		foreach (Transform transform in this.transform)
		{
			transform.gameObject.SetActiveRecursively(transform.gameObject == to);
		}
		activeObject = to;
	}

	public virtual void switchToTopLevelOnly(GameObject to)
	{
		foreach (Transform transform in this.transform)
		{
			if (transform.gameObject == to)
			{
				to.active = true;
				continue;
			}
			transform.gameObject.SetActiveRecursively(false);
		}
		activeObject = to;
	}

	public virtual void switchTo(int index)
	{
		int childIndex = 0;
		activeObject = null;
		foreach (Transform transform in this.transform)
		{
			transform.gameObject.SetActiveRecursively(childIndex == index);
			if (childIndex == index)
			{
				activeObject = transform.gameObject;
			}
			childIndex++;
		}
	}

	public virtual void show(GameObject go)
	{
		go.SetActiveRecursively(true);
	}

	public virtual void hide(GameObject go)
	{
		go.SetActiveRecursively(false);
	}

	public virtual void show(VGUIElement e)
	{
		e.gameObject.SetActiveRecursively(true);
	}

	public virtual void hide(VGUIElement e)
	{
		e.gameObject.SetActiveRecursively(false);
	}

	public virtual void switchNext()
	{
		bool flag = false;
		Transform transform = null;
		Transform transform2 = null;
		foreach (Transform transform3 in this.transform)
		{
			if (transform2 == null)
			{
				transform2 = transform3;
			}
			if (transform != null && transform.gameObject == activeObject)
			{
				flag = true;
				switchTo(transform3.gameObject);
			}
			transform = transform3;
		}
		if (!flag)
		{
			switchTo(transform2.gameObject);
		}
	}

	public virtual void switchPrevious()
	{
		bool flag = false;
		Transform transform = null;
		Transform transform2 = null;
		foreach (Transform transform3 in this.transform)
		{
			transform2 = transform3;
			if (transform != null && transform3.gameObject == activeObject)
			{
				flag = true;
				switchTo(transform.gameObject);
			}
			transform = transform3;
		}
		if (!flag)
		{
			switchTo(transform2.gameObject);
		}
	}
}
