using UnityEngine;

public class Special : MonoBehaviour
{
	public float duration = 1f;

	public float power = 500f;

	public float rechargeTime = 5f;

	[HideInInspector]
	public float startTime;

	[HideInInspector]
	public Vehicle trainRef;

	[HideInInspector]
	public bool ready = true;

	public AudioSource loopSource;

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
