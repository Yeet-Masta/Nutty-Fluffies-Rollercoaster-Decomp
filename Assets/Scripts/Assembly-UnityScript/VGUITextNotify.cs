using System.Collections;
using UnityEngine;

public class VGUITextNotify : VGUINotify
{
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
		runningCoRoutinesAmount++;
		activate();
		yield return new WaitForSeconds(time);
		show(text);
		runningCoRoutinesAmount--;
	}
}
