using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TargetHandler
{
	[Serializable]
	internal class _0024removeTargetObject_0024locals_00241478
	{
		internal int _0024refId;
	}

	[Serializable]
	internal class _0024removeTargetObject_0024closure_0024265
	{
		internal _0024removeTargetObject_0024locals_00241478 _0024_0024locals_00241480;

		public _0024removeTargetObject_0024closure_0024265(_0024removeTargetObject_0024locals_00241478 _0024_0024locals_00241480)
		{
			this._0024_0024locals_00241480 = _0024_0024locals_00241480;
		}

		public bool Invoke(GameObject o)
		{
			return ((LevelEntity)o.GetComponent(typeof(LevelEntity))).referenceId == _0024_0024locals_00241480._0024refId;
		}
	}

	public GameObject[] targetObjects;

	public GameObject go;

	public TargetHandler(GameObject g)
	{
		targetObjects = new GameObject[0];
		go = g;
	}

	public virtual void setParamTargets(string param)
	{
		if (string.IsNullOrEmpty(param) || param.Length <= 0)
		{
			return;
		}
		string[] array = param.Split(","[0]);
		List<GameObject> list = new List<GameObject>();
		Level level = (Level)UnityEngine.Object.FindObjectOfType(typeof(Level));
		LevelLayer layerForNumber = level.getLayerForNumber(0);
		int i = 0;
		string[] array2 = array;
		for (int length = array2.Length; i < length; i++)
		{
			int uniqueID = Convert.ToInt32(array2[i]);
			LevelEntity levelEntity = level.findEntity(uniqueID, layerForNumber.gameObject);
			if ((bool)levelEntity && (bool)levelEntity.gameObject)
			{
				list.Add(levelEntity.gameObject);
			}
		}
		targetObjects = list.ToArray();
	}

	public virtual string getParamTargets()
	{
		string result;
		if (targetObjects != null && targetObjects.Length > 0)
		{
			string text = string.Empty;
			int i = 0;
			GameObject[] array = targetObjects;
			for (int length = array.Length; i < length; i++)
			{
				if (text.Length > 0)
				{
					text += ",";
				}
				text += ((LevelEntity)array[i].GetComponent(typeof(LevelEntity))).referenceId;
			}
			result = text;
		}
		else
		{
			result = string.Empty;
		}
		return result;
	}

	public virtual void removeTargetObject(int refId)
	{
		_0024removeTargetObject_0024locals_00241478 obj = new _0024removeTargetObject_0024locals_00241478();
		obj._0024refId = refId;
		List<GameObject> list = new List<GameObject>(targetObjects);
		list.RemoveAll(new _0024removeTargetObject_0024closure_0024265(obj).Invoke);
		targetObjects = list.ToArray();
	}
}
