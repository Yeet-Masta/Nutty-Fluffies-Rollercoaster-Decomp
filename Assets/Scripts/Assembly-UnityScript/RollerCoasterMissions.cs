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
		if (RuntimeServices.EqualityOperator(MissionManager.instance, null))
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
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(missions);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is Mission))
			{
				obj = RuntimeServices.Coerce(obj, typeof(Mission));
			}
			Mission mission = (Mission)obj;
			if (checkMission(mission))
			{
				flag = true;
				setMissionCompleted(mission);
				UnityRuntimeServices.Update(enumerator, mission);
			}
		}
		bool flag2 = checkHighPriorityMissions();
		bool flag3 = flag;
		if (!flag3)
		{
			flag3 = flag2;
		}
		return flag3;
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
		int result;
		if (m.testFlag(1) || GameData.LOCALIZATION_TEST)
		{
			result = 1;
		}
		else if (GameManager.getInstance() == null || GameManager.getInstance().getVehicle() == null)
		{
			result = 0;
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
				result = 0;
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
					bool flag20 = passengerCartCount >= num;
					if (flag20)
					{
						flag20 = specialCartType == 1;
					}
					flag = flag20;
					break;
				}
				case 3:
				{
					bool flag12 = passengerCartCount >= num;
					if (flag12)
					{
						flag12 = specialCartType == 0;
					}
					flag = flag12;
					break;
				}
				case 4:
				{
					bool flag18 = passengerCartCount >= num;
					if (flag18)
					{
						flag18 = specialCartType == 2;
					}
					flag = flag18;
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
					bool flag4 = specialCartType == 0;
					if (flag4)
					{
						flag4 = rollerCoasterStatistics.roundSpecialUseCount >= num;
					}
					flag = flag4;
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
					bool flag15 = activeLevelPackIndex == 0;
					if (flag15)
					{
						flag15 = passengerCartCount >= num;
					}
					flag = flag15;
					break;
				}
				case 14:
				{
					bool flag14 = activeLevelPackIndex == 1;
					if (flag14)
					{
						flag14 = passengerCartCount >= num;
					}
					flag = flag14;
					break;
				}
				case 15:
				{
					bool flag8 = activeLevelPackIndex == 2;
					if (flag8)
					{
						flag8 = passengerCartCount >= num;
					}
					flag = flag8;
					break;
				}
				case 16:
				{
					bool flag9 = activeLevelPackIndex == 3;
					if (flag9)
					{
						flag9 = passengerCartCount >= num;
					}
					flag = flag9;
					break;
				}
				case 17:
				{
					bool flag19 = activeLevelPackIndex == 0;
					if (flag19)
					{
						flag19 = passengerCartCount >= num;
					}
					if (flag19)
					{
						flag19 = isLastTrack();
					}
					flag = flag19;
					break;
				}
				case 18:
				{
					bool flag16 = activeLevelPackIndex == 1;
					if (flag16)
					{
						flag16 = passengerCartCount >= num;
					}
					if (flag16)
					{
						flag16 = isLastTrack();
					}
					flag = flag16;
					break;
				}
				case 19:
				{
					bool flag11 = activeLevelPackIndex == 2;
					if (flag11)
					{
						flag11 = passengerCartCount >= num;
					}
					if (flag11)
					{
						flag11 = isLastTrack();
					}
					flag = flag11;
					break;
				}
				case 20:
				{
					bool flag6 = activeLevelPackIndex == 3;
					if (flag6)
					{
						flag6 = passengerCartCount >= num;
					}
					if (flag6)
					{
						flag6 = isLastTrack();
					}
					flag = flag6;
					break;
				}
				case 21:
				{
					bool flag21 = passengerCartCount >= GameData.CARTS_TO_UNLOCK_NEXT_LEVEL;
					if (flag21)
					{
						flag21 = activeLevelPackIndex == 0;
					}
					if (flag21)
					{
						flag21 = GameData.SELECTED_LEVEL_INDEX + 2 == num;
					}
					flag = flag21;
					break;
				}
				case 22:
				{
					bool flag17 = passengerCartCount >= GameData.CARTS_TO_UNLOCK_NEXT_LEVEL;
					if (flag17)
					{
						flag17 = activeLevelPackIndex == 1;
					}
					if (flag17)
					{
						flag17 = GameData.SELECTED_LEVEL_INDEX + 2 == num;
					}
					flag = flag17;
					break;
				}
				case 23:
				{
					bool flag13 = passengerCartCount >= GameData.CARTS_TO_UNLOCK_NEXT_LEVEL;
					if (flag13)
					{
						flag13 = activeLevelPackIndex == 2;
					}
					if (flag13)
					{
						flag13 = GameData.SELECTED_LEVEL_INDEX + 2 == num;
					}
					flag = flag13;
					break;
				}
				case 24:
				{
					bool flag10 = passengerCartCount >= GameData.CARTS_TO_UNLOCK_NEXT_LEVEL;
					if (flag10)
					{
						flag10 = activeLevelPackIndex == 3;
					}
					if (flag10)
					{
						flag10 = GameData.SELECTED_LEVEL_INDEX + 2 == num;
					}
					flag = flag10;
					break;
				}
				case 25:
				{
					bool flag7 = activeLevelPackIndex == 0;
					if (flag7)
					{
						flag7 = GameData.SELECTED_LEVEL_INDEX + 1 == num;
					}
					flag = flag7;
					break;
				}
				case 26:
				{
					bool flag5 = activeLevelPackIndex == 1;
					if (flag5)
					{
						flag5 = GameData.SELECTED_LEVEL_INDEX + 1 == num;
					}
					flag = flag5;
					break;
				}
				case 27:
				{
					bool flag3 = activeLevelPackIndex == 2;
					if (flag3)
					{
						flag3 = GameData.SELECTED_LEVEL_INDEX + 1 == num;
					}
					flag = flag3;
					break;
				}
				case 28:
				{
					bool flag2 = activeLevelPackIndex == 3;
					if (flag2)
					{
						flag2 = GameData.SELECTED_LEVEL_INDEX + 1 == num;
					}
					flag = flag2;
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
				result = (flag ? 1 : 0);
			}
		}
		return (byte)result != 0;
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
		IEnumerator enumerator2 = UnityRuntimeServices.GetEnumerator(list);
		while (enumerator2.MoveNext())
		{
			object obj2 = enumerator2.Current;
			if (!(obj2 is Mission))
			{
				obj2 = RuntimeServices.Coerce(obj2, typeof(Mission));
			}
			Mission mission2 = (Mission)obj2;
			if (list2.Count < count)
			{
				list2.Add(mission2);
				UnityRuntimeServices.Update(enumerator2, mission2);
			}
		}
		return list2;
	}
}
