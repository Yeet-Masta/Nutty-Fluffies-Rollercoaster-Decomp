public class VolumeButton : MenuButton
{
	public VGUIImagesTK2D volumeFrames;

	public override void OnEnable()
	{
		float level = GameData.SOUND_LEVEL;
		MasterVolume.setVolume(level * 0.5f);
		volumeFrames.setImage(GameData.SOUND_LEVEL);
	}

	public virtual void onAdjustVolume()
	{
		GameData.SOUND_LEVEL++;
		if (GameData.SOUND_LEVEL > 2)
		{
			GameData.SOUND_LEVEL = 0;
		}
		GameData.save();
		float level = GameData.SOUND_LEVEL;
		MasterVolume.setVolume(level * 0.5f);
		volumeFrames.setImage(GameData.SOUND_LEVEL);
	}
}
