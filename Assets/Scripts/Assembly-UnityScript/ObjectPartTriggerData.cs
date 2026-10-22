using System;
using UnityEngine;

[Serializable]
public class ObjectPartTriggerData : ObjectPartData
{
	public string script;

	public string @params;

	public ObjectPartTriggerData(int type, Vector2 origin, float rotation)
		: base((PartType)type, origin, rotation)
	{
		script = string.Empty;
		@params = string.Empty;
	}
}
