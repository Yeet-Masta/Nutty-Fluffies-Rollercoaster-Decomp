using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class MissionManager
{
	protected static MissionManager instance;

	protected List<Mission> missionList;

	private List<string> DescriptionList;

	public static MissionManager getInstance()
	{
		if (RuntimeServices.EqualityOperator(instance, null))
		{
			instance = new MissionManager();
			instance.load("missions.json");
		}
		return instance;
	}

	public virtual void localizeDescriptions()
	{
		for (int i = 0; i < DescriptionList.Count; i++)
		{
			missionList[i].description = LocalizationManager.localize(DescriptionList[i]);
		}
	}

	public virtual List<Mission> load(string file)
	{
		List<Mission> list = new List<Mission>();
		List<string> list2 = new List<string>();
		string text = FileManager.getInstance().loadFileText(file);
		object result;
		if (text == null)
		{
			result = list;
		}
		else
		{
			JSONObject jSONObject = new JSONObject(text);
			JSONObject field = jSONObject.GetField("missions");
			float num = PlayerPrefs.GetFloat("DP_MISSION_SCORE_FACTOR", 1f);
			IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(field.list);
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				if (!(obj is JSONObject))
				{
					obj = RuntimeServices.Coerce(obj, typeof(JSONObject));
				}
				JSONObject jSONObject2 = (JSONObject)obj;
				string str = jSONObject2.GetFieldSafe("description", new JSONObject(string.Empty)).str;
				UnityRuntimeServices.Update(enumerator, jSONObject2);
				int num2 = PlayerPrefs.GetInt("MISSION_" + str + "_FLAGS", 0);
				if ((num2 & 2) == 0)
				{
					Mission mission = new Mission();
					mission.id = str;
					mission.flags = num2;
					mission.description = LocalizationManager.localize(jSONObject2.GetFieldSafe("description", new JSONObject(string.Empty)).str);
					UnityRuntimeServices.Update(enumerator, jSONObject2);
					mission.activeRounds = (int)jSONObject2.GetFieldSafe("activeRounds", new JSONObject(0f)).n;
					UnityRuntimeServices.Update(enumerator, jSONObject2);
					mission.time = (float)jSONObject2.GetFieldSafe("time", new JSONObject(0f)).n;
					UnityRuntimeServices.Update(enumerator, jSONObject2);
					mission.score = (int)(jSONObject2.GetFieldSafe("score", new JSONObject(0f)).n * (double)num);
					UnityRuntimeServices.Update(enumerator, jSONObject2);
					mission.type = (int)jSONObject2.GetFieldSafe("type", new JSONObject(0f)).n;
					UnityRuntimeServices.Update(enumerator, jSONObject2);
					mission.value = (float)jSONObject2.GetFieldSafe("value", new JSONObject(0f)).n;
					UnityRuntimeServices.Update(enumerator, jSONObject2);
					mission.flags = (int)jSONObject2.GetFieldSafe("flags", new JSONObject(0f)).n;
					UnityRuntimeServices.Update(enumerator, jSONObject2);
					mission.featureUnlock = (int)jSONObject2.GetFieldSafe("featureUnlock", new JSONObject(0f)).n;
					UnityRuntimeServices.Update(enumerator, jSONObject2);
					mission.achievementKey = jSONObject2.GetFieldSafe("achievementKey", new JSONObject(string.Empty)).str;
					UnityRuntimeServices.Update(enumerator, jSONObject2);
					mission.originalScore = (int)jSONObject2.GetFieldSafe("score", new JSONObject(0f)).n;
					UnityRuntimeServices.Update(enumerator, jSONObject2);
					mission.flags |= num2;
					mission.attempts = PlayerPrefs.GetInt("MISSION_" + str + "_ATTEMPTS", 0);
					list.Add(mission);
					list2.Add(jSONObject2.GetFieldSafe("description", new JSONObject(string.Empty)).str);
					UnityRuntimeServices.Update(enumerator, jSONObject2);
				}
			}
			if (GameData.LOG)
			{
				Debug.Log("loaded " + list.Count + " missions");
			}
			missionList = list;
			DescriptionList = list2;
			result = null;
		}
		return (List<Mission>)result;
	}

	public virtual void updateMissionScores()
	{
		float num = PlayerPrefs.GetFloat("DP_MISSION_SCORE_FACTOR", 1f);
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(missionList);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is Mission))
			{
				obj = RuntimeServices.Coerce(obj, typeof(Mission));
			}
			Mission mission = (Mission)obj;
			mission.score = (int)((float)mission.originalScore * num);
			UnityRuntimeServices.Update(enumerator, mission);
		}
	}

	public virtual void saveMissionStatus(Mission mission)
	{
		PlayerPrefs.SetInt("MISSION_" + mission.id + "_ATTEMPTS", mission.attempts);
		PlayerPrefs.SetInt("MISSION_" + mission.id + "_FLAGS", mission.flags);
	}

	public virtual List<Mission> getMissions(int count, bool increaseAttemptCounter)
	{
		localizeDescriptions();
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
			if (mission.testFlag(2))
			{
				continue;
			}
			if (increaseAttemptCounter)
			{
				mission.attempts++;
				UnityRuntimeServices.Update(enumerator, mission);
			}
			if (mission.attempts > mission.activeRounds)
			{
				list2.Add(mission);
				UnityRuntimeServices.Update(enumerator, mission);
				continue;
			}
			list.Add(mission);
			UnityRuntimeServices.Update(enumerator, mission);
			if (list.Count < count)
			{
				continue;
			}
			break;
		}
		foreach (Mission mission2 in list2)
		{
			missionList.Remove(mission2);
			missionList.Add(mission2);
		}
		return list;
	}

	public virtual void resetMissions()
	{
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(missionList);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is Mission))
			{
				obj = RuntimeServices.Coerce(obj, typeof(Mission));
			}
			Mission mission = (Mission)obj;
			mission.flags &= -4096;
			UnityRuntimeServices.Update(enumerator, mission);
			mission.attempts = 0;
			UnityRuntimeServices.Update(enumerator, mission);
			saveMissionStatus(mission);
			UnityRuntimeServices.Update(enumerator, mission);
		}
	}

	public virtual int getActiveCompletedMissionScore()
	{
		int num = 0;
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(missionList);
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is Mission))
			{
				obj = RuntimeServices.Coerce(obj, typeof(Mission));
			}
			Mission mission = (Mission)obj;
			if ((mission.flags & 1) != 0 && (mission.flags & 2) == 0)
			{
				num += mission.score;
				UnityRuntimeServices.Update(enumerator, mission);
			}
		}
		return num;
	}
}
