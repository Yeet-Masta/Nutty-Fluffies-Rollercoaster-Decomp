using System;
using UnityEngine;

[Serializable]
public class FramesPerSecondVGUI : MonoBehaviour
{
	public float updateInterval;

	public VGUIText screenText;

	private double lastInterval;

	private int frames;

	private float fps;

	public FramesPerSecondVGUI()
	{
		updateInterval = 0.5f;
	}

	public virtual void Start()
	{
		lastInterval = Time.realtimeSinceStartup;
		frames = 0;
	}

	public virtual void Update()
	{
		frames++;
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		if (!((double)realtimeSinceStartup <= lastInterval + (double)updateInterval))
		{
			fps = (float)((double)frames / ((double)realtimeSinceStartup - lastInterval));
			frames = 0;
			lastInterval = realtimeSinceStartup;
			screenText.setText(string.Empty + fps.ToString("f2"));
		}
	}

	public virtual void Main()
	{
	}
}
