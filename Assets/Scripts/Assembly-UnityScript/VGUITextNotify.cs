using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang;
using UnityEngine;

[Serializable]
public class VGUITextNotify : VGUINotify
{
	[Serializable]
	internal sealed class _0024showAfterCO_00241529 : GenericGenerator<WaitForSeconds>
	{
		[Serializable]
		internal sealed class _0024 : GenericGeneratorEnumerator<WaitForSeconds>, IEnumerator
		{
			internal float _0024time_00241530;

			internal string _0024text_00241531;

			internal VGUITextNotify _0024self__00241532;

			public _0024(float time, string text, VGUITextNotify self_)
			{
				_0024time_00241530 = time;
				_0024text_00241531 = text;
				_0024self__00241532 = self_;
			}

			public override bool MoveNext()
			{
				int result;
				switch (_state)
				{
				default:
					_0024self__00241532.runningCoRoutinesAmount++;
					_0024self__00241532.activate();
					result = (Yield(2, new WaitForSeconds(_0024time_00241530)) ? 1 : 0);
					break;
				case 2:
					_0024self__00241532.show(_0024text_00241531);
					_0024self__00241532.runningCoRoutinesAmount--;
					YieldDefault(1);
					goto case 1;
				case 1:
					result = 0;
					break;
				}
				return (byte)result != 0;
			}
		}

		internal float _0024time_00241533;

		internal string _0024text_00241534;

		internal VGUITextNotify _0024self__00241535;

		public _0024showAfterCO_00241529(float time, string text, VGUITextNotify self_)
		{
			_0024time_00241533 = time;
			_0024text_00241534 = text;
			_0024self__00241535 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new _0024(_0024time_00241533, _0024text_00241534, _0024self__00241535);
		}
	}

	public VGUIText textElement;

	public virtual void setText(string to)
	{
		textElement.setText(to);
	}

	public virtual void show(string withText)
	{
		show();
		setText(withText);
	}

	public virtual void showTemplated(string withText)
	{
		show();
		textElement.fillTemplate(withText);
	}

	public virtual void showAfter(float time, string text)
	{
		cancelHide();
		activate();
		StartCoroutine(showAfterCO(time, text));
	}

	public virtual IEnumerator showAfterCO(float time, string text)
	{
		return new _0024showAfterCO_00241529(time, text, this).GetEnumerator();
	}
}
