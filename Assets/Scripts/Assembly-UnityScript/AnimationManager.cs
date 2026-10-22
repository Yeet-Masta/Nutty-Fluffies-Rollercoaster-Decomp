using System;
using UnityEngine;

[Serializable]
public class AnimationManager : MonoBehaviour
{
	public ScaleAnimator scaleAnimator;

	public AlphaAnimator alphaAnimator;

	[NonSerialized]
	private static AnimationManager instance;

	public static AnimationManager getInstance()
	{
		return instance;
	}

	public virtual void Awake()
	{
		instance = this;
		useGUILayout = false;
	}

	public virtual void scale(Transform target, float from, float to)
	{
		scaleAnimator.deactivate();
		scaleAnimator.target = target;
		scaleAnimator.from = new Vector2(from, from);
		scaleAnimator.to = new Vector2(to, to);
		scaleAnimator.activate();
	}

	public virtual void fadeOut(Transform target)
	{
		alpha(target, 1f, 0f);
	}

	public virtual void fadeIn(Transform target)
	{
		alpha(target, 0f, 1f);
	}

	public virtual void alpha(Transform target, float from, float to)
	{
		alphaAnimator.target = target;
		alphaAnimator.from = from;
		alphaAnimator.to = to;
		alphaAnimator.activate();
	}

	public virtual void onAnimationComplete()
	{
	}
}
