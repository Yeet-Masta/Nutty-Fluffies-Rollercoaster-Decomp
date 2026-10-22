using System;
using UnityEngine;

[Serializable]
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
		float num2 = (localScale.x = x);
		Vector3 vector = (transform.localScale = localScale);
		float y = num;
		Vector3 localScale2 = transform.localScale;
		float num3 = (localScale2.y = y);
		Vector3 vector3 = (transform.localScale = localScale2);
	}

	public virtual void Main()
	{
	}
}
