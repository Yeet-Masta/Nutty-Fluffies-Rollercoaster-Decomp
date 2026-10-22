using System;
using System.IO;
using UnityEngine;

[Serializable]
public class ObjectPartTextureData : ObjectPartData
{
	public string texture;

	public Color color;

	public Vector2 textureSourceSize;

	public bool allowStretch;

	public Vector2 stretchedScale;

	public ObjectPartTextureData(string texture, Vector2 origin, float rotation)
		: base(PartType.PART_TEXTURE, origin, rotation)
	{
		color = new Color(0.5f, 0.5f, 0.5f);
		textureSourceSize = Vector2.one;
		stretchedScale = Vector2.one;
		this.texture = texture;
	}

	public override string getDesc()
	{
		return base.getDesc() + " (" + Path.GetFileName(texture) + ")";
	}
}
