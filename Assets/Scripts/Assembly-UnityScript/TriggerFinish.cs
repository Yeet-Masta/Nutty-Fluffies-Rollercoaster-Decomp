using UnityEngine;

public class TriggerFinish : LevelTrigger
{
	public override void Awake()
	{
		base.Awake();
		handler.playerTrigger = true;
		handler.onceTrigger = true;
	}

	public override void handleAction(Collider other)
	{
		GameManager.getInstance().finishGame();
	}
}
