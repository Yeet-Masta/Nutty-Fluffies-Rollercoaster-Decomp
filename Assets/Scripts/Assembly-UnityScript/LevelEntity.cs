using System;
using Boo.Lang.Runtime;
using UnityEngine;

[Serializable]
public class LevelEntity : MonoBehaviour, EditableGameObject
{
	public int referenceId;

	public int flags;

	public int deviceList;

	public string scriptParams;

	public LevelEntity()
	{
		referenceId = -1;
		scriptParams = string.Empty;
	}

	public virtual void Awake()
	{
		useGUILayout = false;
	}

	public virtual bool checkDevice(int dev)
	{
		return (deviceList & dev) != 0;
	}

	public virtual void setDevice(int dev, bool state)
	{
		deviceList &= ~dev;
		if (state)
		{
			deviceList |= dev;
		}
	}

	public virtual string getDevicesDesc()
	{
		string text = string.Empty;
		for (int i = 0; i < Enum.GetValues(typeof(CommonData.DeviceGeneration)).Length; i++)
		{
			text = (checkDevice(1 << i) ? (text + "- ") : (text + (CommonData.deviceChars[i] + " ")));
		}
		return text;
	}

	public virtual void setFlag(int flag, bool state)
	{
		flags &= ~flag;
		if (state)
		{
			flags |= flag;
		}
	}

	public virtual bool checkFlag(int flag)
	{
		return (flags & flag) != 0;
	}

	public virtual int getRepeat(int repeatMask)
	{
		int maskShift = Bits.GetMaskShift(repeatMask);
		return (flags & repeatMask) >> maskShift;
	}

	public virtual void setRepeat(int repeatMask, int repeat)
	{
		int maskShift = Bits.GetMaskShift(repeatMask);
		flags &= ~repeatMask;
		flags |= repeat << maskShift;
	}

	public virtual void setPosition(Vector3 position)
	{
		transform.localPosition = position;
		transform.localScale = Vector3.one;
	}

	public virtual void notifyChanged()
	{
	}

	public virtual string getDescription()
	{
		return GetType().Name;
	}

	public virtual LevelLayer getLayer()
	{
		LevelLayer levelLayer = null;
		Transform parent = transform.parent;
		while (levelLayer == null && parent != null)
		{
			levelLayer = ((LevelLayer)parent.GetComponent(typeof(LevelLayer))) as LevelLayer;
			parent = parent.parent;
		}
		return levelLayer;
	}

	public virtual bool equalsTo(LevelEntity other)
	{
		return RuntimeServices.EqualityOperator(other.GetType(), GetType()) && Vector3.Distance(transform.position, other.transform.position) <= 0.1f && ((Mathf.Abs(transform.rotation.eulerAngles.z - other.transform.rotation.eulerAngles.z) <= 0.1f) ? true : false);
	}

	public virtual void deselect()
	{
	}

	public virtual void select()
	{
	}

	public virtual bool selectable()
	{
		return true;
	}
}
