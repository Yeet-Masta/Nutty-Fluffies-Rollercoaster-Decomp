using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class LevelData
{
	public ThemeData theme;

	public List<LevelLayerData> layerData;

	public Vector2 startPoint;

	public Vector2 topLeftLimit;

	public Vector2 bottomRightLimit;

	public string fileName;

	public List<ObjectPackData> usedObjectPacks;

	public CustomSettings customSettings;

	public string oldLeaderboardDoNotUse;

	public LevelData()
	{
		layerData = new List<LevelLayerData>();
		startPoint = new Vector2(0f, 0f);
		topLeftLimit = new Vector2(-10f, -10f);
		bottomRightLimit = new Vector2(10f, 10f);
		fileName = "Untitled";
		usedObjectPacks = new List<ObjectPackData>();
		customSettings = new CustomSettings();
		oldLeaderboardDoNotUse = string.Empty;
	}

	public virtual void recalcUsedObjectPacks(ObjectListData odata)
	{
		usedObjectPacks = new List<ObjectPackData>();
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(layerData);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is LevelLayerData))
			{
				obj = RuntimeServices.Coerce(obj, typeof(LevelLayerData));
			}
			LevelLayerData levelLayerData = (LevelLayerData)obj;
			IEnumerator enumerator2 = UnityRuntimeServices.GetEnumerator(levelLayerData.objectData);
			UnityRuntimeServices.Update(enumerator, levelLayerData);
			while (enumerator2.MoveNext())
			{
				object obj2 = enumerator2.Current;
				if (!(obj2 is LevelEntityData))
				{
					obj2 = RuntimeServices.Coerce(obj2, typeof(LevelEntityData));
				}
				LevelEntityData levelEntityData = (LevelEntityData)obj2;
				if (levelEntityData is LevelObjectData)
				{
					LevelObjectData levelObjectData = levelEntityData as LevelObjectData;
					UnityRuntimeServices.Update(enumerator2, levelEntityData);
					ObjectPackData packForObjectId = odata.getPackForObjectId(levelObjectData.objectId);
					if (!RuntimeServices.EqualityOperator(packForObjectId, null) && !usedObjectPacks.Contains(packForObjectId))
					{
						usedObjectPacks.Add(packForObjectId);
					}
				}
				else if (levelEntityData is LevelSplineData)
				{
					ObjectPackData packForSplineTexture = odata.getPackForSplineTexture((levelEntityData as LevelSplineData).texture);
					UnityRuntimeServices.Update(enumerator2, levelEntityData);
					if (!RuntimeServices.EqualityOperator(packForSplineTexture, null) && !usedObjectPacks.Contains(packForSplineTexture))
					{
						usedObjectPacks.Add(packForSplineTexture);
					}
				}
			}
		}
	}
}
