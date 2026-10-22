using System;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class FastSphereTriggerManager : MonoBehaviour
{
	public FastSphereTrigger[] triggers;

	private Transform[] triggerTransforms;

	public Transform target;

	public Rigidbody targetPhysics;

	public FastSphereTriggerListener listener;

	public bool autoSearchForTriggers;

	private float targetSize;

	private Vector3 distVec;

	public virtual void init()
	{
		if (autoSearchForTriggers)
		{
			UnityScript.Lang.Array array = new UnityScript.Lang.Array();
			UnityScript.Lang.Array array2 = new UnityScript.Lang.Array();
			object[] array3 = (FastSphereTrigger[])UnityEngine.Object.FindObjectsOfType(typeof(FastSphereTrigger));
			int i = 0;
			object[] array4 = array3;
			for (int length = array4.Length; i < length; i++)
			{
				object obj = array4[i];
				if (!(obj is FastSphereTrigger))
				{
					obj = RuntimeServices.Coerce(obj, typeof(FastSphereTrigger));
				}
				if (!(((FastSphereTrigger)obj).GetComponent<Collider>() != null))
				{
					continue;
				}
				object obj2 = array4[i];
				if (!(obj2 is FastSphereTrigger))
				{
					obj2 = RuntimeServices.Coerce(obj2, typeof(FastSphereTrigger));
				}
				if (((FastSphereTrigger)obj2).GetComponent<Collider>().isTrigger)
				{
					object obj3 = array4[i];
					if (!(obj3 is FastSphereTrigger))
					{
						obj3 = RuntimeServices.Coerce(obj3, typeof(FastSphereTrigger));
					}
					array.Push((FastSphereTrigger)obj3);
					object obj4 = array4[i];
					if (!(obj4 is FastSphereTrigger))
					{
						obj4 = RuntimeServices.Coerce(obj4, typeof(FastSphereTrigger));
					}
					array2.Push(((FastSphereTrigger)obj4).transform);
				}
			}
			triggers = (FastSphereTrigger[])array.ToBuiltin(typeof(FastSphereTrigger));
			triggerTransforms = (Transform[])array2.ToBuiltin(typeof(Transform));
		}
		int j = 0;
		FastSphereTrigger[] array5 = triggers;
		for (int length2 = array5.Length; j < length2; j++)
		{
			array5[j].init();
		}
		targetSize = target.GetComponent<Collider>().bounds.extents.magnitude;
		targetSize *= targetSize;
	}

	public virtual void reactivateAll()
	{
		int i = 0;
		FastSphereTrigger[] array = triggers;
		for (int length = array.Length; i < length; i++)
		{
			array[i].reactivate();
		}
	}

	public virtual void FixedUpdate()
	{
		if (RuntimeServices.EqualityOperator(triggerTransforms, null))
		{
			return;
		}
		float num = default(float);
		float num2 = ((!(targetPhysics == null)) ? targetPhysics.velocity.magnitude : 0f);
		num2 *= num2;
		int num3 = 0;
		FastSphereTrigger fastSphereTrigger = null;
		num3 = 0;
		int i = 0;
		Transform[] array = triggerTransforms;
		for (int length = array.Length; i < length; i++)
		{
			fastSphereTrigger = triggers[num3];
			if (fastSphereTrigger.activated && fastSphereTrigger.gameObject.active)
			{
				distVec.x = array[i].position.x - target.position.x;
				distVec.y = array[i].position.y - target.position.y;
				distVec.z = array[i].position.z - target.position.z;
				if (!(distVec.sqrMagnitude >= targetSize + fastSphereTrigger.size + num2))
				{
					listener.OnFastTrigger(fastSphereTrigger);
					fastSphereTrigger.gameObject.SendMessage("OnFastTrigger", target.gameObject);
					fastSphereTrigger.trigger();
				}
			}
			else
			{
				fastSphereTrigger.checkForReactivate();
			}
			num3++;
		}
	}
}
