using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Runtime;
using UnityEngine;
using UnityScript.Lang;

public class PowerMeter : MonoBehaviour
{
	private float barMax;

	public Transform bar;

	public Transform barBottom;

	private int currentValue;

	private float maxValue;

	public ScaleAnimator barAnimator;

	public AudioClip barLoop;

	public AudioClip bellSound;

	public VGUIImagesTK2D[] leftBarLights;

	public VGUIImagesTK2D[] rightBarLights;

	public VGUIImagesTK2D[] starLights1;

	public VGUIImagesTK2D[] starLights2;

	public VGUIImagesTK2D bell;

	public ResultEntry[] resultEntries;

	public Transform scaleMin;

	public Transform scaleMax;

	private int lightBarLevel;

	private int lightPhase;

	private int bellFrame;

	private int resultEntryCounter;

	private bool inited;

	public GameObject perfectText;

	private Vector3 resultScaleDelta;

	private Vector3 resultScaleStart;

	public virtual void prepare(int maxValue)
	{
		this.maxValue = maxValue;
		prepareFriendScores();
		perfectText.SetActiveRecursively(false);
	}

	public virtual void Awake()
	{
		if (barMax == 0f)
		{
			barMax = bar.localScale.y;
		}
	}

	public virtual void activate()
	{
		bar.GetComponent<Renderer>().enabled = false;
		lightBarLevel = 0;
		bellFrame = 0;
		Vector3 localScale = bar.localScale;
		localScale.y = 0.01f;
		bar.localScale = localScale;
		foreach (VGUIImagesTK2D leftBarLight in leftBarLights)
		{
			leftBarLight.setImage(0);
		}
		foreach (VGUIImagesTK2D rightBarLight in rightBarLights)
		{
			rightBarLight.setImage(0);
		}
		foreach (VGUIImagesTK2D starLight in starLights1)
		{
			starLight.setImage(0);
		}
		foreach (VGUIImagesTK2D starLight in starLights2)
		{
			starLight.setImage(0);
		}
		foreach (ResultEntry resultEntry in resultEntries)
		{
			resultEntry.gameObject.SetActiveRecursively(false);
		}
		bell.setImage(0);
		perfectText.SetActiveRecursively(false);
	}

	public virtual void deactivate()
	{
		if (barMax == 0f)
		{
			barMax = bar.localScale.y;
		}
		barAnimator.deactivate();
		CancelInvoke();
	}

	public virtual void setValue(int value)
	{
		currentValue = value;
		barAnimator.from.y = bar.localScale.y;
		float clamped = currentValue;
		if (!(clamped <= maxValue))
		{
			clamped = maxValue;
		}
		barAnimator.to.y = maxValue == 0f ? 0f : (clamped / maxValue * barMax);
		barAnimator.activate();
		bar.GetComponent<Renderer>().enabled = true;
	}

	public virtual void Update()
	{
		bell.setImage(bellFrame % 3);
		if (bellFrame > 0)
		{
			bellFrame--;
		}
	}

	public virtual void playBell()
	{
		GetComponent<AudioSource>().clip = bellSound;
		GetComponent<AudioSource>().Play();
		bellFrame = 60;
		perfectText.SetActiveRecursively(true);
	}

	public virtual void animateLights()
	{
		Invoke("lightInvoke", 0.3f);
	}

	public virtual void lightInvoke()
	{
		lightPhase++;
		int phase = lightPhase & 1;
		if (lightBarLevel >= 1)
		{
			foreach (VGUIImagesTK2D starLight in starLights1)
			{
				starLight.setImage(phase);
			}
			int image = (phase + 1) & 1;
			foreach (VGUIImagesTK2D starLight in starLights2)
			{
				starLight.setImage(image);
			}
		}
		for (int k = 0; k <= lightBarLevel; k++)
		{
			rightBarLights[k].setImage((lightPhase + k) & 1);
			leftBarLights[k].setImage((lightPhase + k) & 1);
		}
		Invoke("lightInvoke", 0.2f);
	}

	public virtual void onAnimationComplete()
	{
		if (leftBarLights.Length != 0 && maxValue != 0f)
		{
			float current = currentValue;
			int lightLevel = (int)(current * (float)leftBarLights.Length / maxValue);
			if (lightLevel >= leftBarLights.Length)
			{
				lightLevel = leftBarLights.Length - 1;
			}
			leftBarLights[lightLevel].setImage(1);
			rightBarLights[lightLevel].setImage(1);
			lightBarLevel = lightLevel;
		}
	}

	private bool showResultEntry(string name, int value)
	{
		int result;
		if (resultEntryCounter < resultEntries.Length)
		{
			float score = value;
			float factor = score / maxValue;
			if (!(factor <= 1f))
			{
				factor = 1f;
			}
			if (GameData.LOG)
			{
				Debug.Log("FACTOR OF " + name + " = " + factor);
			}
			int entryIndex = 0;
			while (true)
			{
				if (entryIndex < resultEntryCounter)
				{
					if (!(Mathf.Abs(resultEntries[entryIndex].scale - factor) >= 0.1f))
					{
						result = 0;
						break;
					}
					entryIndex++;
					continue;
				}
				resultEntries[resultEntryCounter].playerName.setText(name);
				resultEntries[resultEntryCounter].scale = factor;
				resultEntries[resultEntryCounter].transform.localPosition = resultScaleStart + resultScaleDelta * factor;
				resultEntryCounter++;
				result = 1;
				break;
			}
		}
		else
		{
			return false;
		}
		return (byte)result != 0;
	}

	public virtual void startResultEntryAnimations()
	{
		int index = 0;
		int i = 0;
		ResultEntry[] array = resultEntries;
		for (int length = array.Length; i < length; i++)
		{
			if (index < resultEntryCounter)
			{
				array[i].gameObject.SetActiveRecursively(true);
				array[i].animator.activate();
			}
			else
			{
				array[i].gameObject.SetActiveRecursively(false);
			}
			index++;
		}
	}

	public virtual void prepareFriendScores()
	{
		if (!inited)
		{
			resultScaleDelta = scaleMax.localPosition - scaleMin.localPosition;
			resultScaleStart = scaleMin.localPosition;
			inited = true;
		}
		List<OnlineServiceManager.LeaderboardScore> list = new List<OnlineServiceManager.LeaderboardScore>();
		List<OnlineServiceManager.LeaderboardScore> list2 = new List<OnlineServiceManager.LeaderboardScore>();
		resultEntryCounter = 0;
		IEnumerator enumerator = UnityRuntimeServices.GetEnumerator(OnlineServiceManager.getInstance().getLoadedLeaderboardScores());
		while (enumerator.MoveNext())
		{
			object obj = enumerator.Current;
			if (!(obj is OnlineServiceManager.LeaderboardScore))
			{
				obj = RuntimeServices.Coerce(obj, typeof(OnlineServiceManager.LeaderboardScore));
			}
			OnlineServiceManager.LeaderboardScore leaderboardScore = (OnlineServiceManager.LeaderboardScore)obj;
			if (GameData.LOG)
			{
				Debug.Log("SCORE FOR " + leaderboardScore.name + " IS " + leaderboardScore.score);
				UnityRuntimeServices.Update(enumerator, leaderboardScore);
			}
			if (leaderboardScore.name != OnlineServiceManager.getInstance().getMyName())
			{
				if (leaderboardScore.score > currentValue)
				{
					list.Add(leaderboardScore);
					UnityRuntimeServices.Update(enumerator, leaderboardScore);
				}
				else
				{
					list2.Add(leaderboardScore);
					UnityRuntimeServices.Update(enumerator, leaderboardScore);
				}
			}
		}
		int better = 0;
		int worse = 0;
		List<OnlineServiceManager.LeaderboardScore> list3 = new List<OnlineServiceManager.LeaderboardScore>();
		foreach (OnlineServiceManager.LeaderboardScore leaderboardScore2 in list)
		{
			if (showResultEntry(leaderboardScore2.name, leaderboardScore2.score))
			{
				better++;
				list3.Add(leaderboardScore2);
			}
			if (better >= 2)
			{
				break;
			}
		}
		foreach (OnlineServiceManager.LeaderboardScore leaderboardScore3 in list2)
		{
			if (showResultEntry(leaderboardScore3.name, leaderboardScore3.score))
			{
				worse++;
				list3.Add(leaderboardScore3);
			}
			if (better + worse >= 3)
			{
				break;
			}
		}
		if (better + worse < 3)
		{
			foreach (OnlineServiceManager.LeaderboardScore leaderboardScore4 in list)
			{
				if (!list3.Contains(leaderboardScore4))
				{
					if (showResultEntry(leaderboardScore4.name, leaderboardScore4.score))
					{
						better++;
						list3.Add(leaderboardScore4);
					}
					if (better >= 3)
					{
						break;
					}
				}
			}
		}
		if (GameData.LOG)
		{
			Debug.Log("BETTER: " + better + ", WORSE: " + worse);
		}
	}
}
