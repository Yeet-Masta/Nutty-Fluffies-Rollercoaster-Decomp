using System;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;

public class TriggerActivateObject : LevelTrigger
{
	[Serializable]
	internal sealed class _0024handleAction_00241484 : GenericGenerator<string>
	{
		[Serializable]
		internal class Enumerator : IEnumerator<string>, IDisposable, ICloneable
		{
			internal TriggerActivateObject _0024this_00241485;

			protected IEnumerator<GameObject> _0024_0024enumerator;

			protected string _0024_0024current;

			object System.Collections.IEnumerator.Current
			{
				get
				{
					return Current;
				}
			}

			public string Current
			{
				get
				{
					return _0024_0024current;
				}
			}

			public Enumerator(TriggerActivateObject _0024this_00241485)
			{
				this._0024this_00241485 = _0024this_00241485;
				Reset();
			}

			public virtual void Reset()
			{
				_0024_0024enumerator = ((IEnumerable<GameObject>)_0024this_00241485.targetHandler.targetObjects).GetEnumerator();
			}

			public virtual object GetCurrentObject()
			{
				return _0024_0024current;
			}

			public virtual bool MoveNext()
			{
				int result;
				if (_0024_0024enumerator.MoveNext())
				{
					GameObject current = _0024_0024enumerator.Current;
					_0024_0024current = current.ToString();
					result = 1;
				}
				else
				{
					result = 0;
				}
				return (byte)result != 0;
			}

			public virtual object Clone()
			{
				return MemberwiseClone();
			}

			public virtual void Dispose()
			{
				_0024_0024enumerator.Dispose();
			}
		}

		internal TriggerActivateObject _0024this_00241486;

		public _0024handleAction_00241484(TriggerActivateObject _0024this_00241486)
		{
			this._0024this_00241486 = _0024this_00241486;
		}

		public override IEnumerator<string> GetEnumerator()
		{
			return new Enumerator(_0024this_00241486);
		}
	}

	public bool s;

	public bool o;

	private bool onceActivated;

	public override void handleAction(Collider other)
	{
		if (o && onceActivated)
		{
			return;
		}
		MonoBehaviour.print("would activate " + string.Join(", ", (string[])Builtins.array(typeof(string), new _0024handleAction_00241484(this))));
		int i = 0;
		GameObject[] targetObjects = targetHandler.targetObjects;
		for (int length = targetObjects.Length; i < length; i++)
		{
			Component[] componentsInChildren = targetObjects[i].gameObject.GetComponentsInChildren(typeof(LevelTrigger));
			int j = 0;
			Component[] array = componentsInChildren;
			for (int length2 = array.Length; j < length2; j++)
			{
				(array[j] as LevelTrigger).setManualActive(s);
			}
			componentsInChildren = targetObjects[i].gameObject.GetComponentsInChildren(typeof(ObjectScript));
			int k = 0;
			Component[] array2 = componentsInChildren;
			for (int length3 = array2.Length; k < length3; k++)
			{
				(array2[k] as ObjectScript).setManualActive(s);
			}
		}
		onceActivated = true;
	}

	public override void setParam(string param, string val)
	{
		switch (param)
		{
		case "t":
			targetHandler.setParamTargets(val);
			break;
		case "s":
			s = val == "yes";
			break;
		case "o":
			o = val == "yes";
			break;
		}
	}

	public override string getParam(string param)
	{
		object result;
		switch (param)
		{
		case "t":
			result = targetHandler.getParamTargets();
			break;
		case "s":
			result = ((!s) ? "no" : "yes");
			break;
		case "o":
			result = ((!o) ? "no" : "yes");
			break;
		default:
			result = null;
			break;
		}
		return (string)result;
	}
}
