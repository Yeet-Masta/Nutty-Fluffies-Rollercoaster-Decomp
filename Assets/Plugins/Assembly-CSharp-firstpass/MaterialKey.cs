using UnityEngine;

public class MaterialKey
{
	private Texture2D texture;

	private int id;

	public MaterialKey(Texture2D _texture, int _id)
	{
		id = _id;
		texture = _texture;
	}

	public bool Equals(MaterialKey other)
	{
		if (other == null)
		{
			return false;
		}
		return texture == other.texture && id == other.id;
	}

	public override int GetHashCode()
	{
		return texture.GetHashCode() ^ id.GetHashCode();
	}

	public static bool operator ==(MaterialKey a, MaterialKey b)
	{
		if (a == null)
		{
			return b == null;
		}
		return a.Equals(b);
	}

	public static bool operator !=(MaterialKey a, MaterialKey b)
	{
		if (a == null)
		{
			return b != null;
		}
		return !a.Equals(b);
	}
}
