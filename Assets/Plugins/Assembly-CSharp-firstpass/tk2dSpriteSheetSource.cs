using System;
using UnityEngine;

[Serializable]
public class tk2dSpriteSheetSource
{
	public enum Anchor
	{
		UpperLeft = 0,
		UpperCenter = 1,
		UpperRight = 2,
		MiddleLeft = 3,
		MiddleCenter = 4,
		MiddleRight = 5,
		LowerLeft = 6,
		LowerCenter = 7,
		LowerRight = 8
	}

	public Texture2D texture;

	public int tilesX;

	public int tilesY;

	public int numTiles;

	public Anchor anchor = Anchor.MiddleCenter;

	public tk2dSpriteCollectionDefinition.Pad pad;

	public Vector3 scale = new Vector3(1f, 1f, 1f);

	public tk2dSpriteCollectionDefinition.ColliderType colliderType = tk2dSpriteCollectionDefinition.ColliderType.None;

	public void CopyFrom(tk2dSpriteSheetSource src)
	{
		texture = src.texture;
		tilesX = src.tilesX;
		tilesY = src.tilesY;
		numTiles = src.numTiles;
		anchor = src.anchor;
		pad = src.pad;
		scale = src.scale;
		colliderType = src.colliderType;
	}

	public bool CompareTo(tk2dSpriteSheetSource src)
	{
		if (texture != src.texture)
		{
			return false;
		}
		if (tilesX != src.tilesX)
		{
			return false;
		}
		if (tilesY != src.tilesY)
		{
			return false;
		}
		if (numTiles != src.numTiles)
		{
			return false;
		}
		if (anchor != src.anchor)
		{
			return false;
		}
		if (pad != src.pad)
		{
			return false;
		}
		if (scale != src.scale)
		{
			return false;
		}
		if (colliderType != src.colliderType)
		{
			return false;
		}
		return true;
	}
}
