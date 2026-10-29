using UnityEngine;

public class StoryScaler : MonoBehaviour
{
	public virtual void Start()
	{
		float scale = (float)Screen.width / ((float)Screen.height * 1f) / 1.5f;
		if (!(scale >= 1f))
		{
			scale = 1f;
		}
		float x = scale;
		Vector3 localScale = transform.localScale;
		localScale.x = x;
		transform.localScale = localScale;
		float y = scale;
		Vector3 localScale2 = transform.localScale;
		localScale2.y = y;
		transform.localScale = localScale2;
	}
}
