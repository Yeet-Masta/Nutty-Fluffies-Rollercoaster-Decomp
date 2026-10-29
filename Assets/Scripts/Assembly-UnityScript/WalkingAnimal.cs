using UnityEngine;

public class WalkingAnimal : MonoBehaviour
{
	public const int MOUSE = 0;

	public const int CAT = 1;

	public const int DOG = 2;

	public const int PIG = 3;

	public const int ELEPHANT = 4;

	public const int GOAT = 5;

	public const int RABBIT = 6;

	public static readonly float[] TILT_FACTOR = new float[7] { 8f, 9f, 8f, 6f, 5f, 5f, 15f };

	public static readonly float[] VERTICAL_FACTOR = new float[7] { 0.01f, 0.01f, 0.015f, 0.01f, 0.01f, 0.01f, 0.03f };

	public static readonly float[] TILT_SPEED = new float[7] { 80f, 30f, 28f, 27f, 25f, 40f, 35f };

	public const float X_LIMIT = 1f;

	private float tiltPhase;

	public Transform tr;

	public int typeIndex;

	public int clones = 4;

	private float y;

	private float speed = 0.3f;

	public virtual void Awake()
	{
		y = tr.localPosition.y;
		tiltPhase += 360f * UnityEngine.Random.value;
		float x = -1f + 2f * UnityEngine.Random.value;
		Vector3 localPosition = tr.localPosition;
		localPosition.x = x;
		tr.localPosition = localPosition;
		int count = clones;
		clones = 0;
		for (int i = 0; i < count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(this.gameObject, tr.position, Quaternion.identity);
			gameObject.transform.parent = tr.parent;
			gameObject.transform.localScale = Vector3.one;
		}
	}

	public virtual void Update()
	{
		Vector3 localPosition = tr.localPosition;
		float deltaTime = Time.deltaTime;
		tiltPhase += deltaTime * TILT_SPEED[typeIndex] * speed;
		float z = Mathf.Sin(tiltPhase) * TILT_FACTOR[typeIndex];
		Vector3 localEulerAngles = tr.localEulerAngles;
		localEulerAngles.z = z;
		tr.localEulerAngles = localEulerAngles;
		localPosition.x += 0.2f * speed * deltaTime;
		localPosition.y = y + Mathf.Sin(tiltPhase) * VERTICAL_FACTOR[typeIndex];
		if (localPosition.x > 1f || (!(speed >= 0f) && !(localPosition.x >= -1f)))
		{
			tiltPhase += 360f * UnityEngine.Random.value;
			speed = 0.3f + UnityEngine.Random.value * 0.3f;
			if (!(UnityEngine.Random.value >= 0.01f))
			{
				speed *= 2f;
			}
			if (!(UnityEngine.Random.value >= 0.01f))
			{
				Vector3 localScale = tr.localScale;
				localScale.x = -1f;
				tr.localScale = localScale;
				speed = 0f - speed;
				localPosition.x = 0.9f;
			}
			else
			{
				Vector3 localScale2 = tr.localScale;
				localScale2.x = 1f;
				tr.localScale = localScale2;
				localPosition.x = -1.3f + UnityEngine.Random.value * 0.4f;
			}
		}
		tr.localPosition = localPosition;
	}
}
