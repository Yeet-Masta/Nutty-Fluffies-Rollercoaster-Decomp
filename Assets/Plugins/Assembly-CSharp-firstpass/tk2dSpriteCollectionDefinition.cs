using System;
using UnityEngine;

[Serializable]
public class tk2dSpriteCollectionDefinition
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
		LowerRight = 8,
		Custom = 9
	}

	public enum Pad
	{
		Default = 0,
		BlackZeroAlpha = 1,
		Extend = 2
	}

	public enum ColliderType
	{
		Unset = 0,
		None = 1,
		BoxTrimmed = 2,
		BoxCustom = 3,
		Polygon = 4
	}

	public enum PolygonColliderCap
	{
		None = 0,
		FrontAndBack = 1,
		Front = 2,
		Back = 3
	}

	public enum ColliderColor
	{
		Default = 0,
		Red = 1,
		White = 2,
		Black = 3
	}

	public string name = string.Empty;

	public bool additive;

	public Vector3 scale = new Vector3(1f, 1f, 1f);

	[HideInInspector]
	public Texture2D texture;

	[NonSerialized]
	[HideInInspector]
	public Texture2D thumbnailTexture;

	public Anchor anchor = Anchor.MiddleCenter;

	public float anchorX;

	public float anchorY;

	public UnityEngine.Object overrideMesh;

	public bool dice;

	public int diceUnitX = 64;

	public int diceUnitY;

	public Pad pad;

	public int extraPadding;

	public bool fromSpriteSheet;

	public bool extractRegion;

	public int regionX;

	public int regionY;

	public int regionW;

	public int regionH;

	public int regionId;

	public ColliderType colliderType;

	public Vector2 boxColliderMin;

	public Vector2 boxColliderMax;

	public tk2dSpriteColliderIsland[] polyColliderIslands;

	public PolygonColliderCap polyColliderCap;

	public bool colliderConvex;

	public bool colliderSmoothSphereCollisions;

	public ColliderColor colliderColor;

	public void CopyFrom(tk2dSpriteCollectionDefinition src)
	{
		name = src.name;
		additive = src.additive;
		scale = src.scale;
		texture = src.texture;
		anchor = src.anchor;
		anchorX = src.anchorX;
		anchorY = src.anchorY;
		overrideMesh = src.overrideMesh;
		dice = src.dice;
		diceUnitX = src.diceUnitX;
		diceUnitY = src.diceUnitY;
		pad = src.pad;
		fromSpriteSheet = src.fromSpriteSheet;
		extractRegion = src.extractRegion;
		regionX = src.regionX;
		regionY = src.regionY;
		regionW = src.regionW;
		regionH = src.regionH;
		regionId = src.regionId;
		colliderType = src.colliderType;
		boxColliderMin = src.boxColliderMin;
		boxColliderMax = src.boxColliderMax;
		polyColliderCap = src.polyColliderCap;
		colliderColor = src.colliderColor;
		colliderConvex = src.colliderConvex;
		colliderSmoothSphereCollisions = src.colliderSmoothSphereCollisions;
		extraPadding = src.extraPadding;
		if (src.polyColliderIslands != null)
		{
			polyColliderIslands = new tk2dSpriteColliderIsland[src.polyColliderIslands.Length];
			for (int i = 0; i < polyColliderIslands.Length; i++)
			{
				polyColliderIslands[i] = new tk2dSpriteColliderIsland();
				polyColliderIslands[i].CopyFrom(src.polyColliderIslands[i]);
			}
		}
		else
		{
			polyColliderIslands = null;
		}
	}

	public bool CompareTo(tk2dSpriteCollectionDefinition src)
	{
		if (name != src.name)
		{
			return false;
		}
		if (additive != src.additive)
		{
			return false;
		}
		if (scale != src.scale)
		{
			return false;
		}
		if (texture != src.texture)
		{
			return false;
		}
		if (anchor != src.anchor)
		{
			return false;
		}
		if (anchorX != src.anchorX)
		{
			return false;
		}
		if (anchorY != src.anchorY)
		{
			return false;
		}
		if (overrideMesh != src.overrideMesh)
		{
			return false;
		}
		if (dice != src.dice)
		{
			return false;
		}
		if (diceUnitX != src.diceUnitX)
		{
			return false;
		}
		if (diceUnitY != src.diceUnitY)
		{
			return false;
		}
		if (pad != src.pad)
		{
			return false;
		}
		if (extraPadding != src.extraPadding)
		{
			return false;
		}
		if (fromSpriteSheet != src.fromSpriteSheet)
		{
			return false;
		}
		if (extractRegion != src.extractRegion)
		{
			return false;
		}
		if (regionX != src.regionX)
		{
			return false;
		}
		if (regionY != src.regionY)
		{
			return false;
		}
		if (regionW != src.regionW)
		{
			return false;
		}
		if (regionH != src.regionH)
		{
			return false;
		}
		if (regionId != src.regionId)
		{
			return false;
		}
		if (colliderType != src.colliderType)
		{
			return false;
		}
		if (boxColliderMin != src.boxColliderMin)
		{
			return false;
		}
		if (boxColliderMax != src.boxColliderMax)
		{
			return false;
		}
		if (polyColliderIslands != src.polyColliderIslands)
		{
			return false;
		}
		if (polyColliderIslands != null && src.polyColliderIslands != null)
		{
			if (polyColliderIslands.Length != src.polyColliderIslands.Length)
			{
				return false;
			}
			for (int i = 0; i < polyColliderIslands.Length; i++)
			{
				if (!polyColliderIslands[i].CompareTo(src.polyColliderIslands[i]))
				{
					return false;
				}
			}
		}
		if (polyColliderCap != src.polyColliderCap)
		{
			return false;
		}
		if (colliderColor != src.colliderColor)
		{
			return false;
		}
		if (colliderSmoothSphereCollisions != src.colliderSmoothSphereCollisions)
		{
			return false;
		}
		if (colliderConvex != src.colliderConvex)
		{
			return false;
		}
		return true;
	}
}
