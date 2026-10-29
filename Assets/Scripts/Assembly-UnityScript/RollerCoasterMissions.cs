using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class RollerCoasterMissions : MissionManager
{
	public const int FINISH_WITH_N_CARTS = 1;

	public const int FINISH_WITH_N_MAGNET_CARTS = 2;

	public const int FINISH_WITH_N_ROCKET_CARTS = 3;

	public const int FINISH_WITH_N_HOVER_CARTS = 4;

	public const int JUMP_DISTANCE = 5;

	public const int JUMP_DURATION = 6;

	public const int EARN_HEARTS = 7;

	public const int RESULT_METER_PERCENT = 8;

	public const int USE_ROCKET_N_TIMES = 9;

	public const int CRASH_WITH_SPEED = 10;

	public const int DROP_TOYS = 11;

	public const int RIDES_WITHOUT_CRASH = 12;

	public const int FINISH_WITH_WOOD_CARTS = 13;

	public const int FINISH_WITH_METAL_CARTS = 14;

	public const int FINISH_WITH_LOOP_CARTS = 15;

	public const int FINISH_WITH_EXTREME_CARTS = 16;

	public const int FINISH_LAST_WOOD_TRACK_WITH_N_CARTS = 17;

	public const int FINISH_LAST_METAL_TRACK_WITH_N_CARTS = 18;

	public const int FINISH_LAST_LOOP_TRACK_WITH_N_CARTS = 19;

	public const int FINISH_LAST_EXTREME_TRACK_WITH_N_CARTS = 20;

	public const int UNLOCK_NTH_WOOD_TRACK = 21;

	public const int UNLOCK_NTH_METAL_TRACK = 22;

	public const int UNLOCK_NTH_LOOP_TRACK = 23;

	public const int UNLOCK_NTH_EXTREME_TRACK = 24;

	public const int COMPLETE_NTH_WOOD_TRACK = 25;

	public const int COMPLETE_NTH_METAL_TRACK = 26;

	public const int COMPLETE_NTH_LOOP_TRACK = 27;

	public const int COMPLETE_NTH_EXTREME_TRACK = 28;

	public const int FINISH_WITH_N_MOUSE = 29;

	public const int FINISH_WITH_N_CAT = 30;

	public const int FINISH_WITH_N_DOG = 31;

	public const int FINISH_WITH_N_PIG = 32;

	public const int FINISH_WITH_N_ELEPHANT = 33;

	public int completeCounter;

	public new static RollerCoasterMissions getInstance()
	{
		if (MissionManager.instance == null)
		{
			MissionManager.instance = new RollerCoasterMissions();
			string text = FileManager.getInstance().locateAnyFile("missions.json");
			if (text != null)
			{
				MissionManager.instance.load(text);
				if (GameData.TEST_BUILD)
				{
					MissionManager.instance.resetMissions();
				}
			}
			else
			{
				Debug.Log("missions file not found");
			}
		}
		return MissionManager.instance as RollerCoasterMissions;
	}

	public virtual bool checkMissions(int count)
	{
		bool flag = false;
		completeCounter = 0;
		List<Mission> missions = getInstance().getMissions(count, true);
		foreach (Mission mission in missions)
		{
			if (checkMission(mission))
			{
				flag = true;
				setMissionCompleted(mission);
			}
		}
		bool flag2 = checkHighPriorityMissions();
		return flag || flag2;
	}

	private bool checkHighPriorityMissions()
	{
		bool result = false;
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(missionList);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is Mission))
			{
				obj = RuntimeServices.Coerce(obj, typeof(Mission));
			}
			Mission mission = (Mission)obj;
			if (!mission.testFlag(2) && mission.featureUnlock != 0 && checkMission(mission))
			{
				result = true;
				setMissionCompleted(mission);
				UnityRuntimeServices.Update(enumerator, mission);
			}
		}
		return result;
	}

	private void setMissionCompleted(Mission mission)
	{
		mission.flags |= 1;
		if (mission.featureUnlock != 0)
		{
			if (GameData.LOG)
			{
				Debug.Log("UNLOCKING FEATURE: " + mission.featureUnlock);
			}
			GameData.unlockFeature(mission.featureUnlock);
		}
		if (mission.achievementKey != null)
		{
			if (GameData.LOG)
			{
				Debug.Log("UNLOCKING ACHIEVEMENT: " + mission.achievementKey);
			}
			OnlineServiceManager.getInstance().awardAchievement(mission.achievementKey);
		}
		saveMissionStatus(mission);
		completeCounter++;
	}

	private bool checkMission(Mission m)
	{
		if (m.testFlag(1) || GameData.LOCALIZATION_TEST)
		{
			return true;
		}
		else if (GameManager.getInstance() == null || GameManager.getInstance().getVehicle() == null)
		{
			return false;
		}
		else
		{
			RollerCoasterStatistics rollerCoasterStatistics = RollerCoasterStatistics.getInstance();
			GameManager gameManager = GameManager.getInstance();
			int passengerCartCount = gameManager.getVehicle().getPassengerCartCount();
			int num = m.intValue();
			gameManager.getTrainType();
			int activeLevelPackIndex = GameData.getActiveLevelPackIndex();
			if (!rollerCoasterStatistics.roundDidFinish && m.testFlag(Mission.FLAG_REQUIRE_FINISH))
			{
				return false;
			}
			else
			{
				bool flag = false;
				int specialCartType = GameManager.getInstance().getSpecialCartType();
				switch (m.type)
				{
				case 1:
					flag = passengerCartCount >= num;
					break;
				case 2:
				{
					flag = passengerCartCount >= num && specialCartType == 1;
					break;
				}
				case 3:
				{
					flag = passengerCartCount >= num && specialCartType == 0;
					break;
				}
				case 4:
				{
					flag = passengerCartCount >= num && specialCartType == 2;
					break;
				}
				case 5:
					flag = rollerCoasterStatistics.roundJumpLength > m.value;
					break;
				case 6:
					flag = rollerCoasterStatistics.roundJumpDuration > m.value;
					break;
				case 7:
					flag = gameManager.getScoreAmount() >= num;
					break;
				case 8:
					flag = rollerCoasterStatistics.roundPowerPercent > m.value;
					break;
				case 9:
				{
					flag = specialCartType == 0 && rollerCoasterStatistics.roundSpecialUseCount >= num;
					break;
				}
				case 10:
					flag = !(rollerCoasterStatistics.roundCrashSpeed < m.value);
					break;
				case 11:
					flag = rollerCoasterStatistics.roundDroppedItems >= num;
					break;
				case 12:
					flag = rollerCoasterStatistics.roundsWithoutFail >= num;
					break;
				case 13:
				{
					flag = activeLevelPackIndex == 0 && passengerCartCount >= num;
					break;
				}
				case 14:
				{
					flag = activeLevelPackIndex == 1 && passengerCartCount >= num;
					break;
				}
				case 15:
				{
					flag = activeLevelPackIndex == 2 && passengerCartCount >= num;
					break;
				}
				case 16:
				{
					flag = activeLevelPackIndex == 3 && passengerCartCount >= num;
					break;
				}
				case 17:
				{
					flag = activeLevelPackIndex == 0 && passengerCartCount >= num && isLastTrack();
					break;
				}
				case 18:
				{
					flag = activeLevelPackIndex == 1 && passengerCartCount >= num && isLastTrack();
					break;
				}
				case 19:
				{
					flag = activeLevelPackIndex == 2 && passengerCartCount >= num && isLastTrack();
					break;
				}
				case 20:
				{
					flag = activeLevelPackIndex == 3 && passengerCartCount >= num && isLastTrack();
					break;
				}
				case 21:
				{
					flag = passengerCartCount >= GameData.CARTS_TO_UNLOCK_NEXT_LEVEL && activeLevelPackIndex == 0 && GameData.SELECTED_LEVEL_INDEX + 2 == num;
					break;
				}
				case 22:
				{
					flag = passengerCartCount >= GameData.CARTS_TO_UNLOCK_NEXT_LEVEL && activeLevelPackIndex == 1 && GameData.SELECTED_LEVEL_INDEX + 2 == num;
					break;
				}
				case 23:
				{
					flag = passengerCartCount >= GameData.CARTS_TO_UNLOCK_NEXT_LEVEL && activeLevelPackIndex == 2 && GameData.SELECTED_LEVEL_INDEX + 2 == num;
					break;
				}
				case 24:
				{
					flag = passengerCartCount >= GameData.CARTS_TO_UNLOCK_NEXT_LEVEL && activeLevelPackIndex == 3 && GameData.SELECTED_LEVEL_INDEX + 2 == num;
					break;
				}
				case 25:
				{
					flag = activeLevelPackIndex == 0 && GameData.SELECTED_LEVEL_INDEX + 1 == num;
					break;
				}
				case 26:
				{
					flag = activeLevelPackIndex == 1 && GameData.SELECTED_LEVEL_INDEX + 1 == num;
					break;
				}
				case 27:
				{
					flag = activeLevelPackIndex == 2 && GameData.SELECTED_LEVEL_INDEX + 1 == num;
					break;
				}
				case 28:
				{
					flag = activeLevelPackIndex == 3 && GameData.SELECTED_LEVEL_INDEX + 1 == num;
					break;
				}
				case 29:
					flag = gameManager.getVehicle().getNumberOfToys(0) >= num;
					break;
				case 30:
					flag = gameManager.getVehicle().getNumberOfToys(1) >= num;
					break;
				case 31:
					flag = gameManager.getVehicle().getNumberOfToys(2) >= num;
					break;
				case 32:
					flag = gameManager.getVehicle().getNumberOfToys(3) >= num;
					break;
				case 33:
					flag = gameManager.getVehicle().getNumberOfToys(4) >= num;
					break;
				}
				return flag;
			}
		}
	}

	private bool isLastTrack()
	{
		LevelPack activeLevelPack = GameData.getActiveLevelPack();
		return GameData.SELECTED_LEVEL_INDEX >= activeLevelPack.levelList.Count - 1;
	}

	public virtual List<Mission> getMissionsForMenu(int count)
	{
		List<Mission> missions = getInstance().getMissions(count, true);
		List<Mission> list = new List<Mission>();
		List<Mission> list2 = new List<Mission>();
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(missionList);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is Mission))
			{
				obj = RuntimeServices.Coerce(obj, typeof(Mission));
			}
			Mission mission = (Mission)obj;
			if (!mission.testFlag(2) && mission.testFlag(1) && mission.featureUnlock != 0 && !missions.Contains(mission))
			{
				list.Add(mission);
				UnityRuntimeServices.Update(enumerator, mission);
			}
		}
		for (int i = 0; i < count - list.Count; i++)
		{
			if (i < missions.Count)
			{
				list2.Add(missions[i]);
			}
		}
		foreach (Mission mission2 in list)
		{
			if (list2.Count < count)
			{
				list2.Add(mission2);
			}
		}
		return list2;
	}
}
