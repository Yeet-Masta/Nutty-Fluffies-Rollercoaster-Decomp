using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
	public ThemeData theme;

	public List<LevelLayerData> layerData = new List<LevelLayerData>();

	public Vector2 startPoint = new Vector2(0f, 0f);

	public Vector2 topLeftLimit = new Vector2(-10f, -10f);

	public Vector2 bottomRightLimit = new Vector2(10f, 10f);

	public string fileName = "Untitled";

	public List<ObjectPackData> usedObjectPacks = new List<ObjectPackData>();

	public CustomSettings customSettings = new CustomSettings();

	public string oldLeaderboardDoNotUse = string.Empty;

	public virtual void recalcUsedObjectPacks(ObjectListData odata)
	{
		usedObjectPacks = new List<ObjectPackData>();
		foreach (LevelLayerData levelLayerData in layerData)
		{
			foreach (LevelEntityData levelEntityData in levelLayerData.objectData)
			{
				if (levelEntityData is LevelObjectData)
				{
					LevelObjectData levelObjectData = levelEntityData as LevelObjectData;
					ObjectPackData packForObjectId = odata.getPackForObjectId(levelObjectData.objectId);
					if (packForObjectId != null && !usedObjectPacks.Contains(packForObjectId))
					{
						usedObjectPacks.Add(packForObjectId);
					}
				}
				else if (levelEntityData is LevelSplineData)
				{
					ObjectPackData packForSplineTexture = odata.getPackForSplineTexture((levelEntityData as LevelSplineData).texture);
					if (packForSplineTexture != null && !usedObjectPacks.Contains(packForSplineTexture))
					{
						usedObjectPacks.Add(packForSplineTexture);
					}
				}
			}
		}
	}
}
