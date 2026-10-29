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
		float y = 0.01f;
		Vector3 localScale = bar.localScale;
		localScale.y = y;
		bar.localScale = localScale;
		int i = 0;
		VGUIImagesTK2D[] array = leftBarLights;
		for (int length = array.Length; i < length; i++)
		{
			array[i].setImage(0);
		}
		int j = 0;
		VGUIImagesTK2D[] array2 = rightBarLights;
		for (int length2 = array2.Length; j < length2; j++)
		{
			array2[j].setImage(0);
		}
		int k = 0;
		VGUIImagesTK2D[] array3 = starLights1;
		for (int length3 = array3.Length; k < length3; k++)
		{
			array3[k].setImage(0);
		}
		int l = 0;
		VGUIImagesTK2D[] array4 = starLights2;
		for (int length4 = array4.Length; l < length4; l++)
		{
			array4[l].setImage(0);
		}
		int m = 0;
		ResultEntry[] array5 = resultEntries;
		for (int length5 = array5.Length; m < length5; m++)
		{
			array5[m].gameObject.SetActiveRecursively(false);
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
		float num = currentValue;
		if (!(num <= maxValue))
		{
			num = maxValue;
		}
		barAnimator.to.y = ((maxValue == 0f) ? 0f : (num / maxValue * barMax));
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
		int num = lightPhase & 1;
		if (lightBarLevel >= 1)
		{
			int i = 0;
			VGUIImagesTK2D[] array = starLights1;
			for (int length = array.Length; i < length; i++)
			{
				array[i].setImage(num);
			}
			int image = (num + 1) & 1;
			int j = 0;
			VGUIImagesTK2D[] array2 = starLights2;
			for (int length2 = array2.Length; j < length2; j++)
			{
				array2[j].setImage(image);
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
			float num = currentValue;
			int num2 = (int)(num * (float)leftBarLights.Length / maxValue);
			if (num2 >= leftBarLights.Length)
			{
				num2 = leftBarLights.Length - 1;
			}
			leftBarLights[num2].setImage(1);
			rightBarLights[num2].setImage(1);
			lightBarLevel = num2;
		}
	}

	private bool showResultEntry(string name, int value)
	{
		int result;
		if (resultEntryCounter < resultEntries.Length)
		{
			float num = value;
			float num2 = num / maxValue;
			if (!(num2 <= 1f))
			{
				num2 = 1f;
			}
			if (GameData.LOG)
			{
				Debug.Log("FACTOR OF " + name + " = " + num2);
			}
			int num3 = 0;
			while (true)
			{
				if (num3 < resultEntryCounter)
				{
					if (!(Mathf.Abs(resultEntries[num3].scale - num2) >= 0.1f))
					{
						result = 0;
						break;
					}
					num3++;
					continue;
				}
				resultEntries[resultEntryCounter].playerName.setText(name);
				resultEntries[resultEntryCounter].scale = num2;
				resultEntries[resultEntryCounter].transform.localPosition = resultScaleStart + resultScaleDelta * num2;
				resultEntryCounter++;
				result = 1;
				break;
			}
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public virtual void startResultEntryAnimations()
	{
		int num = 0;
		int i = 0;
		ResultEntry[] array = resultEntries;
		for (int length = array.Length; i < length; i++)
		{
			if (num < resultEntryCounter)
			{
				array[i].gameObject.SetActiveRecursively(true);
				array[i].animator.activate();
			}
			else
			{
				array[i].gameObject.SetActiveRecursively(false);
			}
			num++;
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
		int num = 0;
		int num2 = 0;
		List<OnlineServiceManager.LeaderboardScore> list3 = new List<OnlineServiceManager.LeaderboardScore>();
		IEnumerator enumerator2 = UnityRuntimeServices.GetEnumerator(list);
		while (enumerator2.MoveNext())
		{
			object obj2 = enumerator2.Current;
			if (!(obj2 is OnlineServiceManager.LeaderboardScore))
			{
				obj2 = RuntimeServices.Coerce(obj2, typeof(OnlineServiceManager.LeaderboardScore));
			}
			OnlineServiceManager.LeaderboardScore leaderboardScore2 = (OnlineServiceManager.LeaderboardScore)obj2;
			if (showResultEntry(leaderboardScore2.name, leaderboardScore2.score))
			{
				num++;
				list3.Add(leaderboardScore2);
				UnityRuntimeServices.Update(enumerator2, leaderboardScore2);
			}
			if (num >= 2)
			{
				break;
			}
		}
		IEnumerator enumerator3 = UnityRuntimeServices.GetEnumerator(list2);
		while (enumerator3.MoveNext())
		{
			object obj3 = enumerator3.Current;
			if (!(obj3 is OnlineServiceManager.LeaderboardScore))
			{
				obj3 = RuntimeServices.Coerce(obj3, typeof(OnlineServiceManager.LeaderboardScore));
			}
			OnlineServiceManager.LeaderboardScore leaderboardScore3 = (OnlineServiceManager.LeaderboardScore)obj3;
			if (showResultEntry(leaderboardScore3.name, leaderboardScore3.score))
			{
				num2++;
				list3.Add(leaderboardScore3);
				UnityRuntimeServices.Update(enumerator3, leaderboardScore3);
			}
			if (num + num2 >= 3)
			{
				break;
			}
		}
		if (num + num2 < 3)
		{
			IEnumerator enumerator4 = UnityRuntimeServices.GetEnumerator(list);
			while (enumerator4.MoveNext())
			{
				object obj4 = enumerator4.Current;
				if (!(obj4 is OnlineServiceManager.LeaderboardScore))
				{
					obj4 = RuntimeServices.Coerce(obj4, typeof(OnlineServiceManager.LeaderboardScore));
				}
				OnlineServiceManager.LeaderboardScore leaderboardScore4 = (OnlineServiceManager.LeaderboardScore)obj4;
				if (!list3.Contains(leaderboardScore4))
				{
					if (showResultEntry(leaderboardScore4.name, leaderboardScore4.score))
					{
						num++;
						list3.Add(leaderboardScore4);
						UnityRuntimeServices.Update(enumerator4, leaderboardScore4);
					}
					if (num >= 3)
					{
						break;
					}
				}
			}
		}
		if (GameData.LOG)
		{
			Debug.Log("BETTER: " + num + ", WORSE: " + num2);
		}
	}
}
