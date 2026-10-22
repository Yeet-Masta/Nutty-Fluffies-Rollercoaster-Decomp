using System;
using UnityEngine;

[Serializable]
public class EditableObjectPart : MonoBehaviour, EditableGameObject
{
	public int partIndex;

	public virtual void Awake()
	{
		useGUILayout = false;
	}

	public virtual void notifyChanged()
	{
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

	public virtual string getDescription()
	{
		return "(object part index " + partIndex + ")";
	}
}
