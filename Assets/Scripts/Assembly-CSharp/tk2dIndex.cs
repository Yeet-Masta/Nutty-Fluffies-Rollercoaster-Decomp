using System.Collections.Generic;
using UnityEngine;

public class tk2dIndex : ScriptableObject
{
	[HideInInspector]
	public int version;

	[SerializeField]
	private List<tk2dSpriteAnimation> spriteAnimations = new List<tk2dSpriteAnimation>();

	[SerializeField]
	private List<tk2dFont> fonts = new List<tk2dFont>();

	[SerializeField]
	private List<tk2dSpriteCollectionIndex> spriteCollectionIndex = new List<tk2dSpriteCollectionIndex>();

	public tk2dSpriteCollectionIndex[] GetSpriteCollectionIndex()
	{
		spriteCollectionIndex.RemoveAll((tk2dSpriteCollectionIndex item) => item == null);
		return spriteCollectionIndex.ToArray();
	}

	public void AddSpriteCollectionData(tk2dSpriteCollectionData sc)
	{
	}

	public tk2dSpriteAnimation[] GetSpriteAnimations()
	{
		spriteAnimations.RemoveAll((tk2dSpriteAnimation item) => item == null);
		return spriteAnimations.ToArray();
	}

	public void AddSpriteAnimation(tk2dSpriteAnimation sc)
	{
		spriteAnimations.RemoveAll((tk2dSpriteAnimation item) => item == null);
		foreach (tk2dSpriteAnimation spriteAnimation in spriteAnimations)
		{
			if (spriteAnimation == sc)
			{
				return;
			}
		}
		spriteAnimations.Add(sc);
	}

	public tk2dFont[] GetFonts()
	{
		fonts.RemoveAll((tk2dFont item) => item == null);
		return fonts.ToArray();
	}

	public void AddFont(tk2dFont sc)
	{
		fonts.RemoveAll((tk2dFont item) => item == null);
		foreach (tk2dFont font in fonts)
		{
			if (font == sc)
			{
				return;
			}
		}
		fonts.Add(sc);
	}
}
