using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TargetHandler
{
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
		foreach (string text in array)
		{
			int uniqueID = Convert.ToInt32(text);
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
			foreach (GameObject targetObject in targetObjects)
			{
				if (text.Length > 0)
				{
					text += ",";
				}
				text += ((LevelEntity)targetObject.GetComponent(typeof(LevelEntity))).referenceId;
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
		List<GameObject> list = new List<GameObject>(targetObjects);
		list.RemoveAll((GameObject o) => ((LevelEntity)o.GetComponent(typeof(LevelEntity))).referenceId == refId);
		targetObjects = list.ToArray();
	}
}
