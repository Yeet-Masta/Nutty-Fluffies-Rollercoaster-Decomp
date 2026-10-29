using UnityEngine;

public class Special : MonoBehaviour
{
	public float duration;

	public float power;

	public float rechargeTime;

	[HideInInspector]
	public float startTime;

	[HideInInspector]
	public Vehicle trainRef;

	[HideInInspector]
	public bool ready;

	public AudioSource loopSource;

	public Special()
	{
		duration = 1f;
		power = 500f;
		rechargeTime = 5f;
		ready = true;
	}

	public virtual void Start()
	{
		enabled = false;
		ready = true;
		useGUILayout = false;
		loopSource.Stop();
		readyInvoke();
	}

	public virtual void activateSpecial()
	{
		if (ready)
		{
			enabled = true;
			startTime = Time.time;
			RollerCoasterStatistics.getInstance().usedSpecial();
		}
		CancelInvoke();
		loopSource.Play();
		ready = false;
		trainRef.disableDefaultMagnet = true;
	}

	public virtual void deactivateSpecial()
	{
		if (enabled)
		{
			loopSource.Stop();
			enabled = false;
			prepare();
			GameManager.getInstance().onSpecialUsed();
			ready = false;
			trainRef.disableDefaultMagnet = false;
		}
	}

	public virtual void prepare()
	{
		if (!ready)
		{
			Invoke("readyInvoke", rechargeTime);
		}
	}

	public virtual void readyInvoke()
	{
		ready = true;
		GameManager.getInstance().onSpecialReady();
	}

	public virtual void prepareGraphcics()
	{
	}
}
