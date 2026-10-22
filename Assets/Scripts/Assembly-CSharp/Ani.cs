using System;
using System.Collections;
using System.Reflection;
using UnityEngine;

public class Ani : MonoBehaviour
{
	public enum AniType
	{
		To = 0,
		From = 1,
		By = 2
	}

	public enum Animate
	{
		OneShot = 0,
		Loop = 1
	}

	public class AniValue
	{
		private static BindingFlags bFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

		private object obj;

		private string name;

		private Type objType;

		private FieldInfo fieldInfo;

		private PropertyInfo propertyInfo;

		public AniValue(object o, string n)
		{
			obj = o;
			name = n;
			objType = obj.GetType();
			fieldInfo = objType.GetField(n, bFlags);
			propertyInfo = objType.GetProperty(n, bFlags);
			if (fieldInfo == null && propertyInfo == null)
			{
				throw new MissingMethodException("Property or field '" + n + "' not found on " + obj);
			}
		}

		public Type ValueType()
		{
			if (propertyInfo != null)
			{
				return propertyInfo.PropertyType;
			}
			return fieldInfo.FieldType;
		}

		public bool Is(object checkObj)
		{
			return obj == checkObj;
		}

		public bool Is(object checkObject, string checkName)
		{
			return Is(checkObject) && checkName == name;
		}

		public object Get()
		{
			if (propertyInfo != null)
			{
				return propertyInfo.GetValue(obj, null);
			}
			return fieldInfo.GetValue(obj);
		}

		public void Set(object value)
		{
			if (propertyInfo != null)
			{
				propertyInfo.SetValue(obj, value, null);
			}
			else
			{
				fieldInfo.SetValue(obj, value);
			}
		}
	}

	public class AniMator
	{
		private object startValue;

		private object endValue;

		private object change;

		private float startTime;

		private float duration;

		private AnimationEasing easing;

		private EasingType easingType;

		private AnimationDrive drive;

		public AniMator(object sta, object end, object chg, float dur, float delay, AnimationEasingType eas, EasingType typ, AnimationDriveType d)
		{
			startValue = sta;
			endValue = end;
			change = chg;
			Setup(dur, delay, eas, typ, d);
		}

		private void Setup(float dur, float delay, AnimationEasingType eas, EasingType typ, AnimationDriveType d)
		{
			startTime = Time.time + delay;
			duration = dur;
			easingType = typ;
			easing = new AnimationEasing(eas);
			drive = new AnimationDrive(d);
		}

		public float GetEasing(float time)
		{
			if (easingType == EasingType.In)
			{
				return easing.In(time);
			}
			if (easingType == EasingType.Out)
			{
				return easing.Out(time);
			}
			if (easingType == EasingType.InOut)
			{
				return easing.InOut(time);
			}
			return 0f;
		}

		public float GetPosition()
		{
			return Mathf.Clamp01((Time.time - startTime) / duration);
		}

		public bool Running()
		{
			return startTime < Time.time;
		}

		public bool Finished()
		{
			return startTime + duration < Time.time;
		}

		public object GetValue()
		{
			float num = GetEasing(GetPosition());
			return drive.Animate(startValue, endValue, change, num * duration, duration);
		}
	}

	public enum AnimationDriveType
	{
		RegularDrive = 0,
		SlerpDrive = 1
	}

	public class AnimationDrive
	{
		private AnimationDriveType type;

		public AnimationDrive(AnimationDriveType type)
		{
			this.type = type;
		}

		public object Animate(object start, object end, object diff, float time, float duration)
		{
			switch (type)
			{
			case AnimationDriveType.RegularDrive:
				return Animate_RegularDrive(start, end, diff, time, duration);
			case AnimationDriveType.SlerpDrive:
				return Animate_SlerpDrive(start, end, diff, time, duration);
			default:
				return null;
			}
		}

		public bool CalculateDiff()
		{
			switch (type)
			{
			case AnimationDriveType.RegularDrive:
				return true;
			case AnimationDriveType.SlerpDrive:
				return false;
			default:
				return true;
			}
		}

		public object Animate_RegularDrive(object start, object end, object diff, float time, float duration)
		{
			float num = time / duration;
			Type type = start.GetType();
			if (type != diff.GetType())
			{
				return (float)start + num * (float)diff;
			}
			if (type == typeof(short))
			{
				return (float)(short)start + num * (float)(short)diff;
			}
			if (type == typeof(int))
			{
				return (float)(int)start + num * (float)(int)diff;
			}
			if (type == typeof(long))
			{
				return (float)(long)start + num * (float)(long)diff;
			}
			if (type == typeof(float))
			{
				return (float)start + num * (float)diff;
			}
			if (type == typeof(double))
			{
				return (double)start + (double)num * (double)diff;
			}
			if (type == typeof(decimal))
			{
				return (decimal)start + (decimal)num * (decimal)diff;
			}
			if (type == typeof(Vector2))
			{
				return (Vector2)start + num * (Vector2)diff;
			}
			if (type == typeof(Vector3))
			{
				return (Vector3)start + num * (Vector3)diff;
			}
			if (type == typeof(Vector4))
			{
				return (Vector4)start + num * (Vector4)diff;
			}
			if (type == typeof(Color))
			{
				return (Color)start + num * (Color)diff;
			}
			return (float)start + num * (float)diff;
		}

		public object Animate_SlerpDrive(object start, object end, object diff, float time, float duration)
		{
			return Quaternion.Slerp((Quaternion)start, (Quaternion)end, time / duration);
		}
	}

	public enum EasingType
	{
		In = 0,
		Out = 1,
		InOut = 2
	}

	public enum AnimationEasingType
	{
		LinearEasing = 0,
		QuadraticEasing = 1,
		CubicEasing = 2,
		QuarticEasing = 3,
		QuinticEasing = 4,
		SinusoidalEasing = 5,
		ExponentialEasing = 6,
		CircularEasing = 7
	}

	public class AnimationEasing
	{
		private AnimationEasingType type;

		public AnimationEasing(AnimationEasingType type)
		{
			this.type = type;
		}

		public float In(float time)
		{
			switch (type)
			{
			case AnimationEasingType.LinearEasing:
				return time;
			case AnimationEasingType.QuadraticEasing:
				return time * time;
			case AnimationEasingType.CubicEasing:
				return time * time * time;
			case AnimationEasingType.QuarticEasing:
				return Mathf.Pow(time, 4f);
			case AnimationEasingType.QuinticEasing:
				return Mathf.Pow(time, 5f);
			case AnimationEasingType.SinusoidalEasing:
				return Mathf.Sin((time - 1f) * ((float)Math.PI / 2f)) + 1f;
			case AnimationEasingType.ExponentialEasing:
				return Mathf.Pow(2f, 10f * (time - 1f));
			case AnimationEasingType.CircularEasing:
				return -1f * Mathf.Sqrt(1f - time * time) + 1f;
			default:
				return time;
			}
		}

		public float Out(float time)
		{
			switch (type)
			{
			case AnimationEasingType.LinearEasing:
				return time;
			case AnimationEasingType.QuadraticEasing:
				return time * (time - 2f) * -1f;
			case AnimationEasingType.CubicEasing:
				return Mathf.Pow(time - 1f, 3f) + 1f;
			case AnimationEasingType.QuarticEasing:
				return (Mathf.Pow(time - 1f, 4f) - 1f) * -1f;
			case AnimationEasingType.QuinticEasing:
				return Mathf.Pow(time - 1f, 5f) + 1f;
			case AnimationEasingType.SinusoidalEasing:
				return Mathf.Sin(time * ((float)Math.PI / 2f));
			case AnimationEasingType.ExponentialEasing:
				return -1f * Mathf.Pow(2f, -10f * time) + 1f;
			case AnimationEasingType.CircularEasing:
				return Mathf.Sqrt(1f - Mathf.Pow(time - 1f, 2f));
			default:
				return time;
			}
		}

		public float InOut(float time)
		{
			switch (type)
			{
			case AnimationEasingType.LinearEasing:
				return time;
			case AnimationEasingType.QuadraticEasing:
				return EasingHelper.InOut(this, time);
			case AnimationEasingType.CubicEasing:
				return EasingHelper.InOut(this, time);
			case AnimationEasingType.QuarticEasing:
				return EasingHelper.InOut(this, time);
			case AnimationEasingType.QuinticEasing:
				return EasingHelper.InOut(this, time);
			case AnimationEasingType.SinusoidalEasing:
				return EasingHelper.InOut(this, time);
			case AnimationEasingType.ExponentialEasing:
				return EasingHelper.InOut(this, time);
			case AnimationEasingType.CircularEasing:
				return EasingHelper.InOut(this, time);
			default:
				return time;
			}
		}
	}

	public class EasingHelper
	{
		public static float InOut(AnimationEasing eas, float time)
		{
			if (time <= 0.5f)
			{
				return eas.In(time * 2f) / 2f;
			}
			return eas.Out((time - 0.5f) * 2f) / 2f + 0.5f;
		}
	}

	public delegate void DefaultCallback(object val);

	public float defaultDelay;

	public DefaultCallback defaultCallback;

	public AnimationEasingType defaultEasing;

	public EasingType defaultDirection;

	private ArrayList animations = new ArrayList();

	private ArrayList fixedAnimations = new ArrayList();

	private static Ani _mate;

	public static Ani Mate
	{
		get
		{
			if (_mate == null)
			{
				GameObject gameObject = new GameObject("AniMate");
				_mate = (Ani)gameObject.AddComponent<Ani>();
			}
			return _mate;
		}
	}

	public void Awake()
	{
		_mate = this;
	}

	public WaitForSeconds To(UnityEngine.Object obj, float duration, Hashtable _properties)
	{
		Hashtable options = (Hashtable)_properties.Clone();
		Hashtable options2 = ExtractOptions(ref options);
		CreateAnimations(obj, options, duration, options2, AniType.To);
		return new WaitForSeconds(duration);
	}

	public WaitForSeconds To(object obj, float duration, Hashtable _properties, Hashtable _options)
	{
		Hashtable properties = (Hashtable)_properties.Clone();
		Hashtable options = (Hashtable)_options.Clone();
		options = ExtractOptions(ref options);
		CreateAnimations(obj, properties, duration, options, AniType.To);
		return new WaitForSeconds(duration);
	}

	public WaitForSeconds From(object obj, float duration, Hashtable _properties)
	{
		Hashtable options = (Hashtable)_properties.Clone();
		Hashtable options2 = ExtractOptions(ref options);
		CreateAnimations(obj, options, duration, options2, AniType.From);
		return new WaitForSeconds(duration);
	}

	public WaitForSeconds From(object obj, float duration, Hashtable _properties, Hashtable _options)
	{
		Hashtable properties = (Hashtable)_properties.Clone();
		Hashtable options = (Hashtable)_options.Clone();
		options = ExtractOptions(ref options);
		CreateAnimations(obj, properties, duration, options, AniType.From);
		return new WaitForSeconds(duration);
	}

	public WaitForSeconds By(object obj, float duration, Hashtable _properties)
	{
		Hashtable options = (Hashtable)_properties.Clone();
		Hashtable options2 = ExtractOptions(ref options);
		CreateAnimations(obj, options, duration, options2, AniType.By);
		return new WaitForSeconds(duration);
	}

	public WaitForSeconds By(object obj, float duration, Hashtable _properties, Hashtable _options)
	{
		Hashtable properties = (Hashtable)_properties.Clone();
		Hashtable options = (Hashtable)_options.Clone();
		options = ExtractOptions(ref options);
		CreateAnimations(obj, properties, duration, options, AniType.By);
		return new WaitForSeconds(duration);
	}

	public void StopAll(object obj)
	{
		Remove(obj, null, ref animations);
		Remove(obj, null, ref fixedAnimations);
	}

	public void Stop(object obj, string name)
	{
		Remove(obj, name, ref animations);
		Remove(obj, name, ref fixedAnimations);
	}

	private void Remove(object obj, string name, ref ArrayList anims)
	{
		ArrayList arrayList = new ArrayList();
		foreach (object[] anim in anims)
		{
			AniValue aniValue = anim[0] as AniValue;
			if ((name == null && aniValue.Is(obj)) || (name != null && aniValue.Is(obj, name)))
			{
				arrayList.Add(anim);
			}
		}
		foreach (object[] item in arrayList)
		{
			animations.Remove(item);
		}
	}

	private void DoAnimation(ref ArrayList anims)
	{
		ArrayList arrayList = new ArrayList();
		foreach (object[] anim in anims)
		{
			AniValue aniValue = anim[0] as AniValue;
			AniMator aniMator = anim[1] as AniMator;
			DefaultCallback defaultCallback = anim[2] as DefaultCallback;
			if (aniMator.Running())
			{
				aniValue.Set(aniMator.GetValue());
				if (defaultCallback != null)
				{
					defaultCallback(aniMator.GetValue());
				}
				if (aniMator.Finished())
				{
					arrayList.Add(anim);
				}
			}
		}
		foreach (object[] item in arrayList)
		{
			anims.Remove(item);
		}
	}

	public void Update()
	{
		DoAnimation(ref animations);
	}

	private Hashtable ExtractOptions(ref Hashtable options)
	{
		Hashtable hashtable = new Hashtable();
		if (options["delay"] == null)
		{
			hashtable["delay"] = defaultDelay;
		}
		else
		{
			hashtable["delay"] = (float)options["delay"];
			options.Remove("delay");
		}
		if (options["callback"] == null)
		{
			hashtable["callback"] = defaultCallback;
		}
		else
		{
			hashtable["callback"] = (DefaultCallback)options["callback"];
			options.Remove("callback");
		}
		if (options["easing"] == null)
		{
			hashtable["easing"] = defaultEasing;
			hashtable["easing"] = 5;
		}
		else
		{
			hashtable["easing"] = (AnimationEasingType)(int)options["easing"];
			options.Remove("easing");
		}
		if (options["direction"] == null)
		{
			hashtable["direction"] = defaultDirection;
		}
		else
		{
			hashtable["direction"] = (EasingType)(int)options["direction"];
			options.Remove("direction");
		}
		if (options["drive"] == null)
		{
			hashtable["drive"] = AnimationDriveType.RegularDrive;
		}
		else
		{
			hashtable["drive"] = (AnimationDriveType)(int)options["drive"];
			options.Remove("drive");
		}
		if (options["rigidbody"] == null)
		{
			hashtable["rigidbody"] = null;
		}
		else
		{
			hashtable["rigidbody"] = (Rigidbody)options["rigidbody"];
			options.Remove("rigidbody");
		}
		return hashtable;
	}

	private void CreateAnimations(object obj, Hashtable properties, float duration, Hashtable options, AniType type)
	{
		foreach (DictionaryEntry property in properties)
		{
			name = (string)property.Key;
			object value = property.Value;
			AniValue aniValue = new AniValue(obj, name);
			object obj2 = aniValue.Get();
			object obj3 = null;
			object obj4 = null;
			object chg = null;
			switch (type)
			{
			case AniType.To:
				obj3 = obj2;
				obj4 = value;
				break;
			case AniType.From:
				obj3 = value;
				obj4 = obj2;
				break;
			case AniType.By:
				obj3 = obj2;
				chg = value;
				break;
			}
			if ((type == AniType.To || type == AniType.From) && DriveNeedsDiff((AnimationDriveType)(int)options["drive"]))
			{
				try
				{
					Type type2 = obj3.GetType();
					if (type2 != obj4.GetType())
					{
						chg = (float)obj4 - (float)obj3;
					}
					else if (type2 == typeof(short))
					{
						chg = (short)obj4 - (short)obj3;
					}
					else if (type2 == typeof(int))
					{
						chg = (int)obj4 - (int)obj3;
					}
					else if (type2 == typeof(long))
					{
						chg = (long)obj4 - (long)obj3;
					}
					else if (type2 == typeof(float))
					{
						chg = (float)obj4 - (float)obj3;
					}
					else if (type2 == typeof(double))
					{
						chg = (double)obj4 - (double)obj3;
					}
					else if (type2 == typeof(decimal))
					{
						chg = (decimal)obj4 - (decimal)obj3;
					}
					else if (type2 == typeof(Vector2))
					{
						chg = (Vector2)obj4 - (Vector2)obj3;
					}
					else if (type2 == typeof(Vector3))
					{
						chg = (Vector3)obj4 - (Vector3)obj3;
					}
					else if (type2 == typeof(Vector4))
					{
						chg = (Vector4)obj4 - (Vector4)obj3;
					}
					else
					{
						chg = ((type2 != typeof(Color)) ? ((object)((float)obj4 - (float)obj3)) : ((object)((Color)obj4 - (Color)obj3)));
					}
				}
				catch
				{
					throw new Exception(string.Concat("Cannot find diff between ", obj3.GetType(), " and ", obj4.GetType(), ": Operation +, - or * not supported."));
				}
			}
			AniMator aniMator = new AniMator(obj3, obj4, chg, duration, (float)options["delay"], (AnimationEasingType)(int)options["easing"], (EasingType)(int)options["direction"], (AnimationDriveType)(int)options["drive"]);
			animations.Add(new object[3]
			{
				aniValue,
				aniMator,
				options["callback"]
			});
			if (type == AniType.From)
			{
				aniValue.Set(obj3);
			}
		}
	}

	private bool DriveNeedsDiff(AnimationDriveType drive)
	{
		AnimationDrive animationDrive = new AnimationDrive(drive);
		return animationDrive.CalculateDiff();
	}
}
