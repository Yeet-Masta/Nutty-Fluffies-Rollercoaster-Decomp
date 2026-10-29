using UnityEngine;

public class ObjectEmitter : MonoBehaviour
{
	public GameObject @object;

	public int poolSize = 10;

	private GameObject[] objectPool;

	private float initialScale;

	public virtual void Awake()
	{
		objectPool = new GameObject[poolSize];
		int i = 0;
		GameObject[] array = objectPool;
		for (int length = array.Length; i < length; i++)
		{
			array[i] = UnityEngine.Object.Instantiate(@object, Vector3.zero, Quaternion.identity);
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
				position.x = x;
				array[i].transform.position = position;
				Vector3 position2 = array[i].transform.position;
				position2.y = y;
				array[i].transform.position = position2;
				float x2 = initialScale * scale;
				Vector3 localScale = array[i].transform.localScale;
				localScale.x = x2;
				array[i].transform.localScale = localScale;
				float y2 = initialScale * scale;
				Vector3 localScale2 = array[i].transform.localScale;
				localScale2.y = y2;
				array[i].transform.localScale = localScale2;
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
