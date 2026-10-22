using System;
using UnityEngine;

[Serializable]
public class TriggerReset : LevelTrigger
{
	public override void Awake()
	{
		base.Awake();
		handler.playerTrigger = true;
	}

	public override void handleAction(Collider other)
	{
		if (GameManager.getInstance().isPlaying())
		{
			GameManager.getInstance().onOutOfLevel();
		}
	}
}
