using System;
using System.Collections;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
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
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(this.transform);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is Transform))
			{
				obj = RuntimeServices.Coerce(obj, typeof(Transform));
			}
			Transform transform = (Transform)obj;
			transform.gameObject.SetActiveRecursively(false);
			UnityRuntimeServices.Update(enumerator, transform);
		}
		activeObject = null;
	}

	public virtual void showAll()
	{
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(this.transform);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is Transform))
			{
				obj = RuntimeServices.Coerce(obj, typeof(Transform));
			}
			Transform transform = (Transform)obj;
			transform.gameObject.SetActiveRecursively(true);
			UnityRuntimeServices.Update(enumerator, transform);
		}
	}

	public virtual void switchTo(GameObject to)
	{
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(this.transform);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is Transform))
			{
				obj = RuntimeServices.Coerce(obj, typeof(Transform));
			}
			Transform transform = (Transform)obj;
			transform.gameObject.SetActiveRecursively(transform.gameObject == to);
			UnityRuntimeServices.Update(enumerator, transform);
		}
		activeObject = to;
	}

	public virtual void switchToTopLevelOnly(GameObject to)
	{
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(this.transform);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is Transform))
			{
				obj = RuntimeServices.Coerce(obj, typeof(Transform));
			}
			Transform transform = (Transform)obj;
			if (transform.gameObject == to)
			{
				to.active = true;
				continue;
			}
			transform.gameObject.SetActiveRecursively(false);
			UnityRuntimeServices.Update(enumerator, transform);
		}
		activeObject = to;
	}

	public virtual void switchTo(int index)
	{
		int num = 0;
		activeObject = null;
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(this.transform);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is Transform))
			{
				obj = RuntimeServices.Coerce(obj, typeof(Transform));
			}
			Transform transform = (Transform)obj;
			transform.gameObject.SetActiveRecursively(num == index);
			UnityRuntimeServices.Update(enumerator, transform);
			if (num == index)
			{
				activeObject = transform.gameObject;
				UnityRuntimeServices.Update(enumerator, transform);
			}
			num++;
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
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(this.transform);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is Transform))
			{
				obj = RuntimeServices.Coerce(obj, typeof(Transform));
			}
			Transform transform3 = (Transform)obj;
			if (transform2 == null)
			{
				transform2 = transform3;
				UnityRuntimeServices.Update(enumerator, transform3);
			}
			if (transform != null && transform.gameObject == activeObject)
			{
				flag = true;
				switchTo(transform3.gameObject);
				UnityRuntimeServices.Update(enumerator, transform3);
			}
			transform = transform3;
			UnityRuntimeServices.Update(enumerator, transform3);
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
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(this.transform);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is Transform))
			{
				obj = RuntimeServices.Coerce(obj, typeof(Transform));
			}
			Transform transform3 = (Transform)obj;
			transform2 = transform3;
			UnityRuntimeServices.Update(enumerator, transform3);
			if (transform != null && transform3.gameObject == activeObject)
			{
				flag = true;
				switchTo(transform.gameObject);
			}
			transform = transform3;
			UnityRuntimeServices.Update(enumerator, transform3);
		}
		if (!flag)
		{
			switchTo(transform2.gameObject);
		}
	}
}
