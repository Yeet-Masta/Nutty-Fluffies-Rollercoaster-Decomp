using System;
using UnityEngine;

[Serializable]
public class MenuBackground : MonoBehaviour
{
	public Transform[] balloons;

	private float[] balloonPhase;

	public virtual void Awake()
	{
		Invoke("releaseBalloonInvoke", 0.5f + 8f * UnityEngine.Random.value);
		balloonPhase = new float[balloons.Length];
	}

	private void releaseBalloonInvoke()
	{
		for (int i = 0; i < balloons.Length; i++)
		{
			if (!(balloons[i].position.y <= 1f))
			{
				float y = -1f;
				Vector3 position = balloons[i].position;
				float num = (position.y = y);
				Vector3 vector = (balloons[i].position = position);
				float x = -0.5f + UnityEngine.Random.value;
				Vector3 position2 = balloons[i].position;
				float num2 = (position2.x = x);
				Vector3 vector3 = (balloons[i].position = position2);
				balloonPhase[i] = 44f * UnityEngine.Random.value;
				float num3 = 0.25f + 0.75f * UnityEngine.Random.value;
				float x2 = num3;
				Vector3 localScale = balloons[i].localScale;
				float num4 = (localScale.x = x2);
				Vector3 vector5 = (balloons[i].localScale = localScale);
				float y2 = num3;
				Vector3 localScale2 = balloons[i].localScale;
				float num5 = (localScale2.y = y2);
				Vector3 vector7 = (balloons[i].localScale = localScale2);
				break;
			}
		}
		Invoke("releaseBalloonInvoke", 0.5f + 8f * UnityEngine.Random.value);
	}

	public virtual void Update()
	{
		float deltaTime = Time.deltaTime;
		for (int i = 0; i < balloons.Length; i++)
		{
			float z = 30f * Mathf.Sin(balloonPhase[i]);
			Vector3 localEulerAngles = balloons[i].localEulerAngles;
			float num = (localEulerAngles.z = z);
			Vector3 vector = (balloons[i].localEulerAngles = localEulerAngles);
			balloons[i].localPosition = balloons[i].localPosition + balloons[i].up * 0.2f * deltaTime;
			balloonPhase[i] += 4.5f * deltaTime;
		}
	}
}
