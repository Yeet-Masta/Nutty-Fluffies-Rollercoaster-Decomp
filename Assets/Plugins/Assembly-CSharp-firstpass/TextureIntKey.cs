using System;
using UnityEngine;

public class TextureIntKey : IEquatable<TextureIntKey>
{
	public Texture2D texture;

	public int val;

	public TextureIntKey(Texture2D _texture, int _val)
	{
		val = _val;
		texture = _texture;
	}

	public bool Equals(TextureIntKey other)
	{
		if (other == null)
		{
			return false;
		}
		return texture == other.texture && val == other.val;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return base.Equals(obj);
		}
		TextureIntKey textureIntKey = obj as TextureIntKey;
		if (textureIntKey == null)
		{
			return false;
		}
		return Equals(textureIntKey);
	}

	public override int GetHashCode()
	{
		return texture.GetHashCode() ^ val.GetHashCode();
	}

	public static bool operator ==(TextureIntKey a, TextureIntKey b)
	{
		if ((object)a == null || (object)b == null)
		{
			return object.Equals(a, b);
		}
		return a.Equals(b);
	}

	public static bool operator !=(TextureIntKey a, TextureIntKey b)
	{
		if (a == null || b == null)
		{
			return !object.Equals(a, b);
		}
		return !a.Equals(b);
	}
}
