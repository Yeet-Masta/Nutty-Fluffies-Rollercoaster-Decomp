using UnityEngine;

public class SoundController : MonoBehaviour
{
	public float controlValue;

	public AudioSource soundSource;

	public AudioSource complementSource;

	public AudioSource brakeSource;

	public AudioClip brakeSound;

	public AudioClip chainPullSound;

	public AudioClip windSound;

	public Vector2[] pitchControlPoints;

	public Vector2[] volumeControlPoints;

	public Vector2[] masterVolumeControlPoints;

	public Vector2[] brakeVolumeControlPoints;

	public Vector2[] brakePitchControlPoints;

	private LinearCurve volumeCurve = new LinearCurve();

	private LinearCurve pitchCurve = new LinearCurve();

	private LinearCurve masterVolumeCurve = new LinearCurve();

	private LinearCurve brakeVolumeCurve = new LinearCurve();

	private LinearCurve brakePitchCurve = new LinearCurve();

	private float chainPullStart;

	private float chainPullStopDelay;

	public virtual void Awake()
	{
		int i = 0;
		Vector2[] array = volumeControlPoints;
		for (int length = array.Length; i < length; i++)
		{
			volumeCurve.addControlPoint(array[i]);
		}
		int j = 0;
		Vector2[] array2 = pitchControlPoints;
		for (int length2 = array2.Length; j < length2; j++)
		{
			pitchCurve.addControlPoint(array2[j]);
		}
		int k = 0;
		Vector2[] array3 = masterVolumeControlPoints;
		for (int length3 = array3.Length; k < length3; k++)
		{
			masterVolumeCurve.addControlPoint(array3[k]);
		}
		int l = 0;
		Vector2[] array4 = brakeVolumeControlPoints;
		for (int length4 = array4.Length; l < length4; l++)
		{
			brakeVolumeCurve.addControlPoint(array4[l]);
		}
		int m = 0;
		Vector2[] array5 = brakePitchControlPoints;
		for (int length5 = array5.Length; m < length5; m++)
		{
			brakePitchCurve.addControlPoint(array5[m]);
		}
	}

	public virtual void setValue(float value)
	{
		controlValue += (value - controlValue) * 0.1f;
		float value2 = masterVolumeCurve.getValue(controlValue);
		float value3 = volumeCurve.getValue(controlValue);
		float num = pitchCurve.getValue(controlValue);
		float num2 = value3 * value2;
		if (!(num2 >= 0f))
		{
			num2 = 0f;
		}
		else if (!(num2 <= 1f))
		{
			num2 = 1f;
		}
		soundSource.volume = num2;
		num2 = (1f - value3) * value2;
		if (!(num2 >= 0f))
		{
			num2 = 0f;
		}
		else if (!(num2 <= 1f))
		{
			num2 = 1f;
		}
		complementSource.volume = num2;
		if (!(num >= 0f))
		{
			num = 0f;
		}
		else if (!(num <= 1f))
		{
			num = 1f;
		}
		soundSource.pitch = num;
		complementSource.pitch = num;
	}

	public virtual void setBraking(float volume)
	{
	}

	public virtual void setFlying(bool active, float vol)
	{
		if (active)
		{
			if (!brakeSource.isPlaying || brakeSource.clip != windSound)
			{
				brakeSource.Stop();
				brakeSource.clip = windSound;
				brakeSource.loop = true;
				brakeSource.Play();
				brakeSource.pitch = 1f;
			}
			if (!(vol <= 1f))
			{
				vol = 1f;
			}
			else if (!(vol >= 0f))
			{
				vol = 0f;
			}
			brakeSource.volume = vol;
		}
		else if (brakeSource.clip == windSound && brakeSource.isPlaying)
		{
			brakeSource.Stop();
		}
	}

	public virtual void setChainPull(bool active)
	{
		switch (active)
		{
		case true:
			if (!brakeSource.isPlaying || brakeSource.clip != chainPullSound)
			{
				brakeSource.clip = chainPullSound;
				brakeSource.loop = true;
				brakeSource.Play();
				chainPullStart = Time.time;
				brakeSource.pitch = 1f;
				brakeSource.volume = 1f;
			}
			break;
		case false:
			if (!(Time.time - chainPullStart <= 1f) && brakeSource.isPlaying && brakeSource.clip == chainPullSound)
			{
				if (chainPullStopDelay == 0f)
				{
					chainPullStopDelay = Time.time;
				}
				else if (!(Time.time - chainPullStopDelay <= 1f))
				{
					brakeSource.Stop();
					chainPullStopDelay = 0f;
				}
			}
			break;
		}
	}

	public virtual void setSilence()
	{
		if (brakeSource.isPlaying)
		{
			brakeSource.Stop();
		}
	}

	public virtual void start()
	{
		setValue(controlValue);
		soundSource.Play();
		complementSource.Play();
		brakeSource.Play();
	}

	public virtual void stop()
	{
		soundSource.Stop();
		complementSource.Stop();
		brakeSource.Stop();
	}
}
