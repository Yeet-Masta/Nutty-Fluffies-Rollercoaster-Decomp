using System;

[Serializable]
public class tk2dSpriteAnimationFrame
{
	public tk2dSpriteCollectionData spriteCollection;

	public int spriteId;

	public bool triggerEvent;

	public string eventInfo = string.Empty;

	public int eventInt;

	public float eventFloat;

	public void CopyFrom(tk2dSpriteAnimationFrame source)
	{
		spriteCollection = source.spriteCollection;
		spriteId = source.spriteId;
		triggerEvent = source.triggerEvent;
		eventInfo = source.eventInfo;
		eventInt = source.eventInt;
		eventFloat = source.eventFloat;
	}
}
