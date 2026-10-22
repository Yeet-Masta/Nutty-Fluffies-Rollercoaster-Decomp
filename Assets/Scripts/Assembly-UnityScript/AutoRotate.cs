using System;
using UnityEngine;

[Serializable]
public class AutoRotate : MonoBehaviour
{
	public virtual void Update()
	{
		Transform transform = this.transform.parent.transform;
		this.transform.rotation = Quaternion.identity;
		float num = ((transform.localScale.x >= transform.localScale.y) ? transform.localScale.y : transform.localScale.x);
		this.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
	}
}
