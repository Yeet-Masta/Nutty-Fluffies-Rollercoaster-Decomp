using System.Collections.Generic;
using UnityEngine;

public class PerformanceMeasure : MonoBehaviour
{
	public GameObject graphics;

	public Transform root;

	private bool ENABLE_FPS_DEBUG;

	private int NUM_SAMPLES;

	private List<GameObject> list;

	private float[] timing;

	private int timingIndex;

	private float totalTime;

	public PerformanceMeasure()
	{
		ENABLE_FPS_DEBUG = true;
		NUM_SAMPLES = 30;
		list = new List<GameObject>();
		timing = new float[NUM_SAMPLES];
	}

	public virtual void Awake()
	{
	}

	public virtual void Start()
	{
	}

	public virtual void OnDestroy()
	{
		if (GameData.LOG)
		{
			Debug.Log("PERFORMANCE: " + list.Count);
		}
		bool slow = list.Count < 30;
		PlayerPrefs.SetInt("ANDROID_LOW_DEF", slow ? 0 : 0);
		CommonData.reinit();
	}

	public virtual void Update()
	{
		if (!(Time.deltaTime <= 0.1f))
		{
			return;
		}
		totalTime -= timing[timingIndex];
		timing[timingIndex] = Time.deltaTime;
		totalTime += timing[timingIndex];
		timingIndex++;
		if (timingIndex >= NUM_SAMPLES)
		{
			timingIndex = 0;
			if (!(totalTime / (float)NUM_SAMPLES >= 0.02f))
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(graphics, Vector3.zero, Quaternion.identity);
				gameObject.GetComponent<Renderer>().material = new Material(gameObject.GetComponent<Renderer>().material);
				gameObject.transform.parent = root;
				gameObject.transform.position = new Vector3(graphics.transform.position.x, graphics.transform.position.y, graphics.transform.position.z + (float)list.Count / 50f);
				list.Add(gameObject);
			}
			else if (GameData.LOG)
			{
				Debug.Log("LIMIT REACHED, TOTALTIME = " + totalTime);
			}
		}
		if (ENABLE_FPS_DEBUG)
		{
			float frameTime = totalTime / (float)NUM_SAMPLES;
			float fps = 1f / frameTime;
			if (GameData.LOG)
			{
				Debug.Log("FPS: " + fps + " (" + totalTime + " s, frame: " + Time.deltaTime * 1000f + "ms)");
			}
		}
	}
}
