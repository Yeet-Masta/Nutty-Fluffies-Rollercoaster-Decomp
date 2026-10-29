using UnityEngine;

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
				Vector3 position = balloons[i].position;
				position.y = -1f;
				balloons[i].position = position;
				float x = -0.5f + UnityEngine.Random.value;
				Vector3 position2 = balloons[i].position;
				position2.x = x;
				balloons[i].position = position2;
				balloonPhase[i] = 44f * UnityEngine.Random.value;
				float num3 = 0.25f + 0.75f * UnityEngine.Random.value;
				float x2 = num3;
				Vector3 localScale = balloons[i].localScale;
				localScale.x = x2;
				balloons[i].localScale = localScale;
				float y2 = num3;
				Vector3 localScale2 = balloons[i].localScale;
				localScale2.y = y2;
				balloons[i].localScale = localScale2;
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
			localEulerAngles.z = z;
			balloons[i].localEulerAngles = localEulerAngles;
			balloons[i].localPosition = balloons[i].localPosition + balloons[i].up * 0.2f * deltaTime;
			balloonPhase[i] += 4.5f * deltaTime;
		}
	}
}
