using System;
using Boo.Lang.Runtime;
using UnityEngine;

[Serializable]
public class RollerCoasterStatistics : Statistics
{
	public int roundSpecialUseCount;

	public int roundDroppedItems;

	public float roundJumpLength;

	public float roundJumpDuration;

	public float roundCrashSpeed;

	public bool roundFellOutOfTrack;

	public float roundPowerPercent;

	public bool roundWasPerfect;

	public bool roundEarnedNewCart;

	public bool roundUnlockedNextTrack;

	public int[] roundAnimalHearts;

	private Vector3 jumpStartPos;

	private float jumpStartTime;

	protected RollerCoasterStatistics()
	{
	}

	public new static RollerCoasterStatistics getInstance()
	{
		if (Statistics.instance == null)
		{
			Statistics.instance = new RollerCoasterStatistics();
			Statistics.instance.loadStatistics();
		}
		return Statistics.instance as RollerCoasterStatistics;
	}

	public override void loadStatistics()
	{
		base.loadStatistics();
	}

	public override void startLevel()
	{
		base.startLevel();
		roundSpecialUseCount = 0;
		roundDroppedItems = 0;
		roundJumpLength = 0f;
		roundJumpDuration = 0f;
		roundCrashSpeed = 0f;
		roundPowerPercent = 0f;
		roundWasPerfect = false;
		roundFellOutOfTrack = false;
		roundEarnedNewCart = false;
		roundUnlockedNextTrack = false;
		roundAnimalHearts = new int[5];
	}

	public virtual void usedSpecial()
	{
		roundSpecialUseCount++;
	}

	public virtual void itemDropped()
	{
		roundDroppedItems++;
	}

	public virtual void startJump(Vector3 pos)
	{
		jumpStartPos = pos;
		jumpStartTime = Time.time;
	}

	public virtual void endJump(Vector3 pos)
	{
		float magnitude = (pos - jumpStartPos).magnitude;
		if (!(magnitude <= roundJumpLength))
		{
			roundJumpLength = magnitude;
		}
		magnitude = Time.time - jumpStartTime;
		if (!(magnitude <= roundJumpDuration))
		{
			roundJumpDuration = magnitude;
		}
	}

	public virtual void crash(float speed)
	{
		if (roundCrashSpeed == 0f)
		{
			roundCrashSpeed = speed;
		}
	}

	public virtual void fellOutOfTrack()
	{
		roundFellOutOfTrack = true;
	}

	public virtual void finish(float power, bool perfectRound)
	{
		roundPowerPercent = power;
		roundWasPerfect = perfectRound;
	}

	public virtual void animalHearts(int animalIndex, int amount)
	{
		if (animalIndex != GameData.ANIMAL_NONE)
		{
			roundAnimalHearts[animalIndex] += amount;
		}
	}
}
