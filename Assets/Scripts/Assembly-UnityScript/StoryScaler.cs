using UnityEngine;

public class StoryScaler : MonoBehaviour
{
	public virtual void Start()
	{
		float num = (float)Screen.width / ((float)Screen.height * 1f) / 1.5f;
		if (!(num >= 1f))
		{
			num = 1f;
		}
		float x = num;
		Vector3 localScale = transform.localScale;
		localScale.x = x;
		transform.localScale = localScale;
		float y = num;
		Vector3 localScale2 = transform.localScale;
		localScale2.y = y;
		transform.localScale = localScale2;
	}
}
