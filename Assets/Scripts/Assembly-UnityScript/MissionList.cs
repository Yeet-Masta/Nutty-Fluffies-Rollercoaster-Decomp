using System.Collections.Generic;
using UnityEngine;

public class MissionList : VGUIList
{
	public ScaleAnimator appearAnimator;

	public SlideOutAnimator listEntryAnimator;

	public ListCompressAnimator listCompressAnimator;

	public ParticleEmitter[] paperEmitters;

	public GameObject missionsComplete;

	public GameObject continueButton;

	private int animatedRow = -1;

	private Mission animatingMission;

	private List<Mission> listedMissions;

	private bool sequenceReady;

	private bool wasActivated;

	private bool animatingEntry;

	public override void Awake()
	{
		disablePaperEmitters();
	}

	public override void activate()
	{
		clearList();
		base.activate();
		animatedRow = -1;
		animatingEntry = false;
		listedMissions = RollerCoasterMissions.getInstance().getMissionsForMenu(GameData.MAX_ACTIVE_MISSIONS);
		for (int i = 0; i < listedMissions.Count; i++)
		{
			MissionListEntry missionListEntry = createEntry().GetComponent<MissionListEntry>() as MissionListEntry;
			missionListEntry.setDescription(listedMissions[i].description);
			if (listedMissions[i].score > 0)
			{
				missionListEntry.score.setText(string.Empty + listedMissions[i].score);
			}
			else
			{
				missionListEntry.score.gameObject.SetActiveRecursively(false);
			}
			missionListEntry.completed.SetActiveRecursively(false);
		}
		missionsComplete.SetActiveRecursively(listedMissions.Count == 0);
		sequenceReady = false;
		if (needsAnimation())
		{
			continueButton.SetActiveRecursively(false);
			Invoke("animateNextCompleted", 0.25f);
		}
		wasActivated = true;
	}

	private bool needsAnimation()
	{
		int row = animatedRow;
		while (row < listedMissions.Count - 1)
		{
			row++;
			if (listedMissions[row].testFlag(1))
			{
				return true;
			}
		}
		return false;
	}

	private bool animateNextCompleted()
	{
		if (animatingEntry)
		{
			return true;
		}
		if (listedMissions != null)
		{
			while (animatedRow < listedMissions.Count - 1)
			{
				animatedRow++;
				if (listedMissions[animatedRow].testFlag(1))
				{
					MissionListEntry missionListEntry = entryRefs[animatedRow].GetComponent<MissionListEntry>() as MissionListEntry;
					appearAnimator.activate(missionListEntry.completed.transform);
					missionListEntry.completed.SetActiveRecursively(true);
					listedMissions[animatedRow].flags = listedMissions[animatedRow].flags | 2;
					animatingMission = listedMissions[animatedRow];
					continueButton.SetActiveRecursively(false);
					MissionManager.getInstance().saveMissionStatus(listedMissions[animatedRow]);
					animatingEntry = true;
					return true;
				}
			}
		}
		continueButton.SetActiveRecursively(true);
		return false;
	}

	public override void deactivate()
	{
		CancelInvoke("disablePaperEmitters");
		disablePaperEmitters();
		listEntryAnimator.deactivate();
		listCompressAnimator.deactivate();
		appearAnimator.deactivate();
		clearList();
		if (!wasActivated)
		{
			return;
		}
		List<Mission> missions = RollerCoasterMissions.getInstance().getMissions(GameData.MAX_ACTIVE_MISSIONS, false);
		int reward = 0;
		foreach (Mission mission in missions)
		{
			if (mission.testFlag(1))
			{
				reward += mission.score;
				mission.flags |= 2;
			}
		}
		if (reward > 0)
		{
			GameData.addTokens(reward);
			BaseMenuManager.getInstance().updateTokenAmount();
		}
		wasActivated = false;
	}

	public virtual void onPadgeVisible()
	{
		listEntryAnimator.activate(getEntry(animatedRow).transform);
		GameData.addTokens(animatingMission.score);
		int i = 0;
		ParticleEmitter[] array = paperEmitters;
		for (int length = array.Length; i < length; i++)
		{
			array[i].emit = true;
		}
		GeneralSoundEffects.playSound(BaseMenuManager.getInstance().fireworksSound);
		CancelInvoke("disablePaperEmitters");
		Invoke("disablePaperEmitters", 2f);
	}

	private void disablePaperEmitters()
	{
		int i = 0;
		ParticleEmitter[] array = paperEmitters;
		for (int length = array.Length; i < length; i++)
		{
			array[i].emit = false;
		}
	}

	public virtual void onAnimationComplete()
	{
		animatingEntry = false;
		if (animatedRow != -1)
		{
			UnityEngine.Object.Destroy(entryRefs[animatedRow]);
			entryRefs.RemoveAt(animatedRow);
			listedMissions.RemoveAt(animatedRow);
			animatedRow--;
			listCompressAnimator.activate();
			BaseMenuManager.getInstance().updateTokenAmount();
		}
	}

	public virtual void onCompressListAnimComplete()
	{
		if (!animateNextCompleted() && !sequenceReady)
		{
			int count = entryRefs.Count;
			activate();
			for (int i = count; i < entryRefs.Count; i++)
			{
				float y = entryRefs[i].transform.localPosition.y - 2f;
				Vector3 localPosition = entryRefs[i].transform.localPosition;
				localPosition.y = y;
				entryRefs[i].transform.localPosition = localPosition;
			}
			listCompressAnimator.activate();
			sequenceReady = true;
			continueButton.SetActiveRecursively(true);
		}
	}

	private MissionListEntry getEntry(int index)
	{
		return entryRefs[animatedRow].GetComponent<MissionListEntry>() as MissionListEntry;
	}
}
