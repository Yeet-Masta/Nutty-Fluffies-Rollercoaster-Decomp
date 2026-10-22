using System;
using UnityEngine;

[Serializable]
public class ObjectEmitter : MonoBehaviour
{
	public GameObject @object;

	public int poolSize;

	private GameObject[] objectPool;

	private int objectsToEmit;

	private Vector3 emitPosition;

	private float initialScale;

	public ObjectEmitter()
	{
		poolSize = 10;
	}

	public virtual void Awake()
	{
		objectPool = new GameObject[poolSize];
		int i = 0;
		GameObject[] array = objectPool;
		for (int length = array.Length; i < length; i++)
		{
			array[i] = (GameObject)UnityEngine.Object.Instantiate(@object, Vector3.zero, Quaternion.identity);
			array[i].transform.parent = transform;
			array[i].SetActiveRecursively(false);
		}
		initialScale = @object.transform.localScale.x;
		useGUILayout = false;
	}

	public virtual void emitAt(float x, float y, float scale, int scoreValue)
	{
		int num = 0;
		int i = 0;
		GameObject[] array = objectPool;
		for (int length = array.Length; i < length; i++)
		{
			if (!array[i].gameObject.active)
			{
				Vector3 position = array[i].transform.position;
				float num2 = (position.x = x);
				Vector3 vector = (array[i].transform.position = position);
				Vector3 position2 = array[i].transform.position;
				float num3 = (position2.y = y);
				Vector3 vector3 = (array[i].transform.position = position2);
				float x2 = initialScale * scale;
				Vector3 localScale = array[i].transform.localScale;
				float num4 = (localScale.x = x2);
				Vector3 vector5 = (array[i].transform.localScale = localScale);
				float y2 = initialScale * scale;
				Vector3 localScale2 = array[i].transform.localScale;
				float num5 = (localScale2.y = y2);
				Vector3 vector7 = (array[i].transform.localScale = localScale2);
				HeartAnimator component = array[i].GetComponent<HeartAnimator>();
				component.heartValue = scoreValue;
				component.activate();
				array[i].SetActiveRecursively(true);
				return;
			}
			num++;
		}
		GameManager.getInstance().gameUI.addScore(scoreValue);
	}
}
