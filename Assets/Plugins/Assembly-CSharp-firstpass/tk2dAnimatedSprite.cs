using UnityEngine;

[AddComponentMenu("2D Toolkit/tk2dAnimatedSprite")]
public class tk2dAnimatedSprite : tk2dSprite
{
	public delegate void AnimationCompleteDelegate(tk2dAnimatedSprite sprite, int clipId);

	public delegate void AnimationEventDelegate(tk2dAnimatedSprite sprite, tk2dSpriteAnimationClip clip, tk2dSpriteAnimationFrame frame, int frameNum);

	public tk2dSpriteAnimation anim;

	public int clipId;

	public bool playAutomatically;

	public static bool g_paused;

	public bool paused;

	public bool createCollider;

	private tk2dSpriteAnimationClip currentClip;

	private float clipTime;

	private int previousFrame = -1;

	public AnimationCompleteDelegate animationCompleteDelegate;

	public AnimationEventDelegate animationEventDelegate;

	public tk2dSpriteAnimationClip CurrentClip
	{
		get
		{
			return currentClip;
		}
	}

	private new void Start()
	{
		base.Start();
		if (playAutomatically)
		{
			Play(clipId);
		}
	}

	public void Play()
	{
		Play(clipId);
	}

	public void Play(float clipStartTime)
	{
		Play(clipId, clipStartTime);
	}

	public void Play(string name)
	{
		int id = anim ? anim.GetClipIdByName(name) : -1;
		Play(id);
	}

	public void Play(string name, float clipStartTime)
	{
		int id = anim ? anim.GetClipIdByName(name) : -1;
		Play(id, clipStartTime);
	}

	public void Stop()
	{
		currentClip = null;
	}

	public bool isPlaying()
	{
		return currentClip != null;
	}

	protected override bool NeedBoxCollider()
	{
		return createCollider;
	}

	public int GetClipIdByName(string name)
	{
		return anim ? anim.GetClipIdByName(name) : -1;
	}

	public void Play(int id)
	{
		Play(id, 0f);
	}

	public void Play(int id, float clipStartTime)
	{
		clipId = id;
		if (id >= 0 && (bool)anim && id < anim.clips.Length)
		{
			currentClip = anim.clips[id];
			if (currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.Single || currentClip.frames == null)
			{
				SwitchCollectionAndSprite(currentClip.frames[0].spriteCollection, currentClip.frames[0].spriteId);
				if (currentClip.frames[0].triggerEvent && animationEventDelegate != null)
				{
					animationEventDelegate(this, currentClip, currentClip.frames[0], 0);
				}
				currentClip = null;
			}
			else if (currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.RandomFrame || currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.RandomLoop)
			{
				int num = Random.Range(0, currentClip.frames.Length - 1);
				tk2dSpriteAnimationFrame tk2dSpriteAnimationFrame2 = currentClip.frames[num];
				clipTime = (float)num * currentClip.fps;
				SwitchCollectionAndSprite(tk2dSpriteAnimationFrame2.spriteCollection, tk2dSpriteAnimationFrame2.spriteId);
				if (tk2dSpriteAnimationFrame2.triggerEvent && animationEventDelegate != null)
				{
					animationEventDelegate(this, currentClip, tk2dSpriteAnimationFrame2, 0);
				}
				if (currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.RandomFrame)
				{
					currentClip = null;
					previousFrame = -1;
				}
			}
			else
			{
				clipTime = clipStartTime;
				previousFrame = -1;
				if (currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.Once && clipTime >= currentClip.fps * (float)currentClip.frames.Length)
				{
					clipTime = currentClip.fps * ((float)currentClip.frames.Length - 0.1f);
				}
			}
		}
		else
		{
			OnCompleteAnimation();
			currentClip = null;
		}
	}

	public void Pause()
	{
		paused = true;
	}

	public void Resume()
	{
		paused = false;
	}

	private void OnCompleteAnimation()
	{
		previousFrame = -1;
		if (animationCompleteDelegate != null)
		{
			animationCompleteDelegate(this, clipId);
		}
	}

	private void SetFrame(int currFrame)
	{
		if (previousFrame != currFrame)
		{
			SwitchCollectionAndSprite(currentClip.frames[currFrame].spriteCollection, currentClip.frames[currFrame].spriteId);
			if (currentClip.frames[currFrame].triggerEvent && animationEventDelegate != null)
			{
				animationEventDelegate(this, currentClip, currentClip.frames[currFrame], currFrame);
			}
			previousFrame = currFrame;
		}
	}

	private void Update()
	{
		if (g_paused || paused || currentClip == null || currentClip.frames == null)
		{
			return;
		}
		clipTime += Time.deltaTime * currentClip.fps;
		if (currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.Loop || currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.RandomLoop)
		{
			int frame = (int)clipTime % currentClip.frames.Length;
			SetFrame(frame);
		}
		else if (currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.LoopSection)
		{
			int num = (int)clipTime;
			if (num >= currentClip.loopStart)
			{
				num = currentClip.loopStart + (num - currentClip.loopStart) % (currentClip.frames.Length - currentClip.loopStart);
			}
			SetFrame(num);
		}
		else if (currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.PingPong)
		{
			int num2 = (int)clipTime % (currentClip.frames.Length + currentClip.frames.Length - 2);
			if (num2 >= currentClip.frames.Length)
			{
				int num3 = num2 - currentClip.frames.Length;
				num2 = currentClip.frames.Length - 2 - num3;
			}
			SetFrame(num2);
		}
		else if (currentClip.wrapMode == tk2dSpriteAnimationClip.WrapMode.Once)
		{
			int num4 = (int)clipTime;
			if (num4 >= currentClip.frames.Length)
			{
				currentClip = null;
				OnCompleteAnimation();
			}
			else
			{
				SetFrame(num4);
			}
		}
	}
}
