using System.Collections;
using UnityEngine;

public class Animator : MonoBehaviour
{
	private class AnimationObj
	{
		public string Cmd = string.Empty;

		public Object Obj;

		public float Duration;

		public GameObject CallbackObj;

		public string CallbackMsg;

		public object CallbackParams;

		public Hashtable Properties = new Hashtable();
	}

	private ArrayList mAnimate = new ArrayList();

	private bool mAnimating;

	private GameObject mCallbackObj;

	private string mCallbackMsg = string.Empty;

	private object mCallbackParams;

	private Ani.Animate mAnimateMode;

	public void AnimateTo(Object obj, float Duration, GameObject CallbackObj, object CallbackMsg, object CallbackParams, params object[] args)
	{
		int num = 0;
		AnimationObj animationObj = new AnimationObj();
		animationObj.Obj = obj;
		animationObj.Cmd = "To";
		animationObj.Duration = Duration;
		animationObj.CallbackObj = CallbackObj;
		animationObj.CallbackMsg = (string)CallbackMsg;
		animationObj.CallbackParams = CallbackParams;
		for (num = 0; num < args.Length; num += 2)
		{
			if (num + 2 <= args.Length)
			{
				object key = args[num];
				object value = args[num + 1];
				animationObj.Properties.Add(key, value);
			}
		}
		mAnimate.Add(animationObj);
	}

	public void AnimateFrom(Object obj, float Duration, GameObject CallbackObj, object CallbackMsg, object CallbackParams, params object[] args)
	{
		int num = 0;
		AnimationObj animationObj = new AnimationObj();
		animationObj.Obj = obj;
		animationObj.Cmd = "From";
		animationObj.Duration = Duration;
		animationObj.CallbackObj = CallbackObj;
		animationObj.CallbackMsg = (string)CallbackMsg;
		animationObj.CallbackParams = CallbackParams;
		for (num = 0; num < args.Length; num += 2)
		{
			if (num + 2 <= args.Length)
			{
				object key = args[num];
				object value = args[num + 1];
				animationObj.Properties.Add(key, value);
			}
		}
		mAnimate.Add(animationObj);
	}

	public void AnimateBy(Object obj, float Duration, GameObject CallbackObj, object CallbackMsg, object CallbackParams, params object[] args)
	{
		int num = 0;
		AnimationObj animationObj = new AnimationObj();
		animationObj.Obj = obj;
		animationObj.Cmd = "By";
		animationObj.Duration = Duration;
		animationObj.CallbackObj = CallbackObj;
		animationObj.CallbackMsg = (string)CallbackMsg;
		animationObj.CallbackParams = CallbackParams;
		for (num = 0; num < args.Length; num += 2)
		{
			if (num + 2 <= args.Length)
			{
				object key = args[num];
				object value = args[num + 1];
				animationObj.Properties.Add(key, value);
			}
		}
		mAnimate.Add(animationObj);
	}

	public bool StartAnimation(Ani.Animate Mode, GameObject CallbackObj, object CallbackMsg, object CallbackParams)
	{
		if (!mAnimating)
		{
			gameObject.active = true;
			mAnimating = true;
			mCallbackObj = CallbackObj;
			mCallbackMsg = (string)CallbackMsg;
			mCallbackParams = CallbackParams;
			mAnimateMode = Mode;
			StartCoroutine(AnimateEngine());
			return true;
		}
		return false;
	}

	public void StopAnimation()
	{
		mAnimating = false;
	}

	public void ClearAnimation()
	{
		mAnimate.Clear();
	}

	private IEnumerator AnimateEngine()
	{
		int Index = 0;
		while (mAnimate.Count > 0 && mAnimating)
		{
			if (Index >= mAnimate.Count)
			{
				continue;
			}
			AnimationObj item = (AnimationObj)mAnimate[Index];
			switch (item.Cmd)
			{
			case "To":
				yield return Ani.Mate.To(item.Obj, item.Duration, item.Properties);
				break;
			case "From":
				yield return Ani.Mate.From(item.Obj, item.Duration, item.Properties);
				break;
			case "By":
				yield return Ani.Mate.By(item.Obj, item.Duration, item.Properties);
				break;
			}
			if (item.CallbackObj != null)
			{
				item.CallbackObj.SendMessage(item.CallbackMsg, item.CallbackParams, SendMessageOptions.DontRequireReceiver);
			}
			if (mAnimateMode == Ani.Animate.OneShot)
			{
				if (mAnimate.Count > 0)
				{
					mAnimate.RemoveAt(Index);
				}
				continue;
			}
			Index++;
			if (Index >= mAnimate.Count)
			{
				Index = 0;
			}
		}
		mAnimating = false;
		if (mCallbackObj != null && mCallbackMsg != null)
		{
			mCallbackObj.SendMessage(mCallbackMsg, mCallbackParams, SendMessageOptions.DontRequireReceiver);
		}
	}
}
